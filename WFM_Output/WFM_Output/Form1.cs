using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFM_Output
{
    //TODO: read files from iso directly instead of needing exported WFM files (I have a c# iso file extractor+decompression, just need unLDAR iirc)
    //TODO: wfm1/2 support + pre8 support (not started, need sample files, also pre8 isn't even in wfm format so have fun with that)
    //pre8 support also supports the messages on the map, the status screen, and item names
    //TODO: Add 16 word(2byte) per row dialog byte display on front end (mostly done, could use human readable text explaining what control characters do)
    //TODO: Add support for FreeMono instead of Courier font, test for backup/missing font too https://en.wikipedia.org/wiki/GNU_FreeFont (not started)
    //TODO: full font export, pooling all glyphs together into one sheet (working on pulling all wfm glyph data together still)
    //TODO: XML Logging of glyph-typed character relationship for editing and plaintext output (there are remnants of debug messages that can be used as a base for this, but hasnt been worked on yet)
    //TODO: Recoding the bitmap generation of the dialog so it can properly draw transparency where glyphs stop if there is whitespace
    //TODO: drawing method only uses the list now, rename the list(currently palettedata or something) to be moe fitting and remove the textpixel array
    //TODO: proper dialog drawing routine for the bitmap object, where it is done by glyph instead of scanning the bitmap by x,y

    public partial class Form1 : Form
    {
        GlyphData glyphData;
        GlyphImage[] glyphImage;
        DialogData[] dialogData;
        List<PaletteData> paletteData;
        int currentBubble = 0;

        public Form1()
        {
            InitializeComponent();
            lbl_FilePath.Text = Properties.Settings.Default.settingFilePath;
        }

        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            //showdialog to select file
            DialogResult resultFilepath = new DialogResult();
            resultFilepath = ofd_WFM3.ShowDialog();
            if (resultFilepath == DialogResult.OK)
            {
                //store file in setting
                Properties.Settings.Default.settingFilePath = ofd_WFM3.FileName;
                Properties.Settings.Default.Save();
                //show filepath as text in lbl_FileSelect.text
                lbl_FilePath.Text = ofd_WFM3.FileName;
                //enable output button
                btn_Output.Enabled = true;
                UIReset();
                
            }
            else
            {
                //do nothing? idk yet
            }
        }


        private void btn_Output_Click(object sender, EventArgs e)
        {
            //load file from file setting into binaryreader
            BinaryReader wfmFile = new BinaryReader(File.OpenRead(Properties.Settings.Default.settingFilePath));
            glyphData = new GlyphData();
            FillGlyphData(wfmFile);
            FillGlyphImage(wfmFile);
            //fill glyph relative pointer table
            FillDialogData(wfmFile);
            lsbx_GlyphIndex.Items.Clear();
            for (int i = 0; i < glyphData.glyphAmount; i++)
            {
                lsbx_GlyphIndex.Items.Add(i.ToString("X4"));
            }
            lsbx_DialogIndex.Items.Clear();
            for (int i = 0; i < dialogData.Length; i++)
            {
                lsbx_DialogIndex.Items.Add(i.ToString("X4"));
            }
        }
        
        private void lsbx_GlyphIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isEvent = false;
            lsbx_GlyphAsciiOutput.Items.Clear();
            string output = "";
            int a = lsbx_GlyphIndex.SelectedIndex;
            lbl_GlyphHeight.Text = "Glyph Height: " + glyphData.glyphHeight[a].ToString();
            lbl_GlyphWidth.Text = "Glyph Width: " + glyphData.glyphWidth[a].ToString();
            if (glyphData.glyphHeight[a] == 24)
            {
                isEvent = true;
            }
            Bitmap ogGlyph = glyphImage[a].GenerateBitmapProper(glyphImage[a], glyphData, a, isEvent);
            Bitmap glyph = glyphImage[a].GenerateBitmapScaled(glyphImage[a], glyphData, a, isEvent);
            pcbx_GlyphImage.Width = glyphData.glyphWidth[a];
            pcbx_GlyphImage.Height = glyphData.glyphHeight[a];
            pcbx_GlyphImage.Image = ogGlyph;
            ScaleImage(isEvent);
            for (int i = 0; i < glyphImage[a].character.Length; i++) //character logging
            {
                byte leftNybble = (byte)(glyphImage[a].character[i] >> 4);
                byte rightNybble = (byte)(glyphImage[a].character[i] & 0x0F);
                //TODO: modify this into a data set table instead of an ascii view in a listbox
                switch (rightNybble)
                {
                    case 0x00:
                        output += @".";
                        break;
                    case 0x0E:
                        output += @",";
                        break;
                    case 0x0F:
                        output += @"~";
                        break;
                    //handle 0, E, and F as special characters, otherwise print the number.
                    default:
                        output += rightNybble.ToString("X1");
                        break;
                }
                switch (leftNybble)
                {
                    case 0x00:
                        output += @".";
                        break;
                    case 0x0E:
                        output += @",";
                        break;
                    case 0x0F:
                        output += @"~";
                        break;
                    //handle 0, E, and F as #, otherwise print the number.
                    default:
                        output += leftNybble.ToString("X1");
                        break;
                }
            }
            for (int i = 0; i < output.Length; i += glyphData.glyphWidth[a])
            {
                //Console.WriteLine(output.Substring(i, glyphData.glyphWidth[a]));
                lsbx_GlyphAsciiOutput.Items.Add(output.Substring(i, glyphData.glyphWidth[a]));
            }
            output = "";
        }

        private void lsbx_DialogSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: fast scrolling throws errors, need to async and make it only update upon a completed one
            rtb_DialogByteDisplay.Clear();
            bool isEvent = false;
            int index = lsbx_DialogIndex.SelectedIndex;
            //TODO: previous behavior not finished, needs more work, IE not go back after 1 and only enable when on 2 or more
            if (dialogData[index].bubbleCount > 1)
            {
                btn_Dialog_Next.Enabled = true;
                btn_Dialog_Previous.Enabled = false;
            }
            else
            {
                btn_Dialog_Next.Enabled = false;
                btn_Dialog_Previous.Enabled = false;
            }
            DialogByteTextDisplay(index, dialogData);
            byte[,] textPixels;
            currentBubble = 0;
            GenerateTextBubble(index, currentBubble, out textPixels);
            if (glyphData.glyphHeight[0] == 24)
            {
                isEvent = true;
            }
            Bitmap textbox = dialogData[0].GenerateBitmapTextbox(dialogData[0], paletteData, textPixels, glyphData, index, isEvent);
            pcbx_TextBubble.Width = dialogData[0].textboxX;
            pcbx_TextBubble.Height = dialogData[0].textboxY;
            pcbx_TextBubble.Image = textbox;
        }

        private void btn_Dialog_Next_Click(object sender, EventArgs e)
        {
            bool isEvent = false;
            if (glyphData.glyphHeight[0] == 24)
            {
                isEvent = true;
            }
            currentBubble++;
            int index = lsbx_DialogIndex.SelectedIndex;
            btn_Dialog_Previous.Enabled = true;
            if (currentBubble > dialogData[index].bubbleCount)
            {
                currentBubble = dialogData[index].bubbleCount;
                btn_Dialog_Next.Enabled = false;
            }
            byte[,] textPixels;
            GenerateTextBubble(index, currentBubble, out textPixels);
            Bitmap textbox = dialogData[0].GenerateBitmapTextbox(dialogData[0], paletteData, textPixels, glyphData, index, isEvent);
            pcbx_TextBubble.Width = dialogData[0].textboxX;
            pcbx_TextBubble.Height = dialogData[0].textboxY;
            pcbx_TextBubble.Image = textbox;
        }

        private void btn_Dialog_Previous_Click(object sender, EventArgs e)
        {
            bool isEvent = false;
            if (glyphData.glyphHeight[0] == 24)
            {
                isEvent = true;
            }
            currentBubble--;
            btn_Dialog_Next.Enabled = true;
            if (currentBubble < 0)
            {
                currentBubble = 0;
                btn_Dialog_Previous.Enabled = false;
            }
            int index = lsbx_DialogIndex.SelectedIndex;
            byte[,] textPixels;
            GenerateTextBubble(index, currentBubble, out textPixels);
            Bitmap textbox = dialogData[0].GenerateBitmapTextbox(dialogData[0], paletteData, textPixels, glyphData, index, isEvent);
            pcbx_TextBubble.Width = dialogData[0].textboxX;
            pcbx_TextBubble.Height = dialogData[0].textboxY;
            pcbx_TextBubble.Image = textbox;
        }

        private void FillGlyphData(BinaryReader wfmFile)
        {
            glyphData.header = Encoding.Default.GetString(wfmFile.ReadBytes(4));
            if (glyphData.header != "WFM3")
            {
                //break on non WFM3 entry
                return;
            }
            //check next 4 bytes as uint for 0 entry, half if nonzeron
            glyphData.headerPadding = BitConverter.ToUInt32(wfmFile.ReadBytes(4), 0);
            if (glyphData.headerPadding != 0)
            {
                //break on non zero
                return;
            }
            //check next 4 bytes as uint, store in LE for relative dialogue table pointer
            glyphData.relativeDialogTablePointer = wfmFile.ReadUInt32();
            //check next two bytes at 16bit unsigned int type, dialog amount
            glyphData.dialogAmount = wfmFile.ReadUInt16();
            //check next 2 bytes as 16bit unsigned int type, glyph amount
            glyphData.glyphAmount = wfmFile.ReadUInt16();

            //next 128 bytes are just logged as unknown datablock.
            glyphData.unknownBytes = new byte[128];
            glyphData.unknownBytes = wfmFile.ReadBytes(128);

            //GLYPH POINTER TABLE
            //bytecount for glyph pointer table size is glyph amount * 2.
            ////Hey so it turns out with how things work I only needed to know how many were there, neat
            //store every 2byte uint LE as a glyph pointer.
            glyphData.glyphPointers = new UInt16[glyphData.glyphAmount];
            for (int i = 0; i < glyphData.glyphAmount; i++)
            {
                glyphData.glyphPointers[i] = wfmFile.ReadUInt16();
            }

            //GLYPH DATA
            glyphData.glyphPalette = new UInt16[glyphData.glyphAmount];
            glyphData.glyphHeight = new UInt16[glyphData.glyphAmount];
            glyphData.glyphWidth = new UInt16[glyphData.glyphAmount];
            glyphData.glyphHandaKuten = new UInt16[glyphData.glyphAmount];
        }

        private void FillGlyphImage(BinaryReader wfmFile)
        {
            glyphImage = new GlyphImage[glyphData.glyphAmount];
            int a = 0;
            while (a < glyphData.glyphAmount)
            {
                //Console.WriteLine("POINTER CURRENT: 0x" + wfmFile.BaseStream.Position.ToString("X4") + " | POINTER IN TABLE: 0x" + glyphData.glyphPointers[a].ToString("X4"));
                if (wfmFile.BaseStream.Position != glyphData.glyphPointers[a])
                {
                    Console.WriteLine("ERROR, MISMATCH IN POINTERS");
                }
                wfmFile.BaseStream.Position = glyphData.glyphPointers[a];
                glyphImage[a] = new GlyphImage();
                glyphData.glyphPalette[a] = wfmFile.ReadUInt16();
                glyphData.glyphHeight[a] = wfmFile.ReadUInt16();
                glyphData.glyphWidth[a] = wfmFile.ReadUInt16();
                //re evaluated, width/2 needs to be checked to be an even number, if it is not then it needs to be rounded up
                if ((glyphData.glyphWidth[a] / 2) % 2 != 0)
                {
                    Console.WriteLine("Uneven character width detected");
                    //TOOD: Add note to width to store new width, then add to writeline report that it was adjusted
                    while ((glyphData.glyphWidth[a] / 2) % 2 != 0)
                    {
                        glyphData.glyphWidth[a]++;
                    }
                }
                glyphData.glyphHandaKuten[a] = wfmFile.ReadUInt16();
                int characterSizeTotal = (glyphData.glyphWidth[a] / 2) * glyphData.glyphHeight[a];
                glyphImage[a].character = new byte[characterSizeTotal];
                glyphImage[a].character = wfmFile.ReadBytes(characterSizeTotal);
                /*Console.WriteLine(" | PALETTE: 0x" + glyphData.glyphPalette[a].ToString("X2") + "(" + glyphData.glyphPalette[a].ToString() +
                                  ") | HEIGHT: 0x" + glyphData.glyphHeight[a].ToString("X2") + "(" + glyphData.glyphHeight[a].ToString() +
                                  ") | WIDTH: 0x" + glyphData.glyphWidth[a].ToString("X2") + "(" + glyphData.glyphWidth[a].ToString() +
                                  ") | HANDAKUTEN: 0x" + glyphData.glyphHandaKuten[a].ToString("X2") + "(" + glyphData.glyphHandaKuten[a].ToString() +
                                  ") | SIZE: 0x" + characterSizeTotal.ToString("X2") + "(" + characterSizeTotal.ToString() + ")");*/
                //Console.WriteLine("----------------------------");
                string output = "";
                byte[] byteOutput = new byte[glyphData.glyphWidth[a] / 2];
                a++;
            }
        }

        private void FillDialogData(BinaryReader wfmFile)
        {
            dialogData = new DialogData[glyphData.dialogAmount];
            wfmFile.BaseStream.Position = glyphData.relativeDialogTablePointer;
            for (int i = 0; i < dialogData.Length; i++)
            {
                dialogData[i] = new DialogData();
                dialogData[i].dialogRelativePointer = wfmFile.ReadUInt16();
            }
            for (int i = 0; i < dialogData.Length; i++)
            {
                int dialogEntryLength = 0;
                wfmFile.BaseStream.Position = glyphData.relativeDialogTablePointer + dialogData[i].dialogRelativePointer;
                if (i != dialogData.Length - 1) //finding length of dialog entry via basic math, last entry needs to work against the wfmfile length
                {
                    dialogEntryLength = dialogData[i + 1].dialogRelativePointer - dialogData[i].dialogRelativePointer;
                }
                else
                {
                    dialogEntryLength = (int)(wfmFile.BaseStream.Length - (dialogData[i].dialogRelativePointer+glyphData.relativeDialogTablePointer));
                }
                //write bytes to array in dialogData object
                dialogData[i].dialogBytes = new UInt16[dialogEntryLength / 2]; //set size of array in object
                for (int j = 0; j < dialogData[i].dialogBytes.Length; j++)
                {
                    UInt16 testword = wfmFile.ReadUInt16();
                    if (testword == 0xFFFC)
                    {
                        dialogData[i].bubbleCount++;
                    }
                    dialogData[i].dialogBytes[j] = testword;
                }
            }
        }


        private void btn_Shitpost_Click(object sender, EventArgs e)
        {
            byte[,] textPixels;
            bool isEvent = false; 
            //dialogData[0].dialogBytes = new UInt16[19] { 0x800A, 0x8015, 0x800D, 0x8013, 0x8024, 0x801E, 0x801F, 0x800C, 0x8016, 0x8022, 0x800E, 0x8014, 0x8017, 0x8014, 0x8015, 0x8015, 0x8014, 0x800C, 0xFFFF };
            dialogData[0].dialogBytes = new UInt16[72] { 0xFFFA,0x00F8,0x0035,
                                                        0x8068, 0x8043, 0x8043, 0x8067,0x8048, //good
                                                        0xFFF7, 0x0001,
                                                        0x8067, 0x8050, 0x805D,0x8048, //day
                                                        0xFFF7,0x0002,
                                                        0x8054,0x8060,0x807A,0x8042,0x8074, //TCRF,
                                                        0xFFFD,
                                                        0xFFF7,0x0003,
                                                        0x804C,0x8043,0x8043,0x8071, 0x8048,//look
                                                        0xFFF7,0x0004,
                                                        0x8050,0x8047,0x8048, //at
                                                        0xFFF7,0x0005,
                                                        0x8065,0x805D, //my
                                                        0xFFFD,
                                                        0xFFF7, 0x0006,
                                                        0x805F,0x8043,0x804C,0x8043,0x8044,0x8049,0x8063,0x804C, 0x8048,//colorful
                                                        0xFFF7,0x0007,
                                                        0x8067,0x8061,0x8050,0x804C,0x8043,0x8069,0x8048, //dialog
                                                        0xFFF7,0x0008,
                                                        0x805F,0x805E,0x8043,0x8061,0x805F,0x8045,0x8046,0x806A, //selection!
                                                        0xFFFC,0xFFFB,0xFFFF };
            int index = 0;
            GenerateTextBubble(index, currentBubble, out textPixels);
            Bitmap textbox = dialogData[0].GenerateBitmapTextbox(dialogData[0], paletteData, textPixels, glyphData, index, isEvent);
            pcbx_TextBubble.Width = dialogData[0].textboxX;
            pcbx_TextBubble.Height = dialogData[0].textboxY;
            pcbx_TextBubble.Image = textbox;

        }


        private void GenerateTextBubble(int selectedIndex, int currentBubble, out byte[,] textPixels)
        {
            paletteData = new List<PaletteData>();
            int dialogIndex = 0;
            int textBubbleMaxX = 0;
            int textBubbleMaxY = 0;
            int textBubbleCurX = 0;
            int textBubbleCurY = 0;
            textPixels = new byte[900,900]; //PH value, should never be this if everything works right
            int arrayX = 0;
            int arrayY = 0;
            int tallestCharacter = 0;
            
            //for multi bubble
            int fffcCheck = 0;

            //for palette
            int paletteIndex = 0;
            //for list tracking
            int listIndex = -1;
            //TODO: try catch for exceptions
            while (dialogIndex < dialogData[selectedIndex].dialogBytes.Length)
            {
                UInt16 currentIndex = dialogData[selectedIndex].dialogBytes[dialogIndex];
                if (currentIndex >= 0xFFF0) //control character
                {
                    //paletteIndex = 0;
                    switch (currentIndex)
                    {
                        case 0xFFF0: //unused, takes 2 arguments?
                            dialogIndex += 3;
                            break;

                        case 0xFFF1: //unused
                            MessageBox.Show("Error: 0xFFF1 detected /r/n This is supposed to be an unused control word, if this message comes up please submit a bug report and attach the wfm file used.");
                            break;

                        case 0xFFF2: //not sure, proactive NPCs use this, 1 arg
                            dialogIndex += 2;
                            break;

                        case 0xFFF3: //not sure, used once
                            dialogIndex += 2;
                            break;

                        case 0xFFF4: //unused
                            MessageBox.Show("Error: 0xFFF4 detected /r/n This is supposed to be an unused control word, if this message comes up please submit a bug report and attach the wfm file used.");
                            break;

                        case 0xFFF5: //prompt, no arguments, used for SAVE DATA
                            dialogIndex++;
                            break;

                        case 0xFFF6: //TODO: dialog box initial placement, apparently? 
                            //two int16 (X,Y)
                            //0,0 is top left of viewport
                            dialogIndex += 3;
                            break;

                        case 0xFFF7: //change color of all following text, argument determines color change
                            paletteIndex = dialogData[selectedIndex].dialogBytes[dialogIndex + 1];
                            dialogIndex += 2;
                            break;

                        case 0xFFF8: //murmur sound/tail type of textbox, arguments determine what sound plays and what speech tail, 2 arguments
                            //TODO: Will implement graphics for textbox later
                            dialogIndex += 3;
                            break;

                        case 0xFFF9: //wait, 1 argument, how many frames to wait
                            //TODO: Implement varying things for this, accurate textbox rep + diagnostic data display that shows this
                            dialogIndex += 2;
                            break;

                        case 0xFFFA: //Make speec bubble, 2 arguments, width/height
                            textBubbleMaxX = dialogData[selectedIndex].dialogBytes[dialogIndex + 1];
                            textBubbleMaxY = dialogData[selectedIndex].dialogBytes[dialogIndex + 2];
                            dialogData[0].textboxY = textBubbleMaxY;
                            dialogData[0].textboxX = textBubbleMaxX;
                            textPixels = new byte[textBubbleMaxX, textBubbleMaxY];
                            dialogIndex += 3;
                            break;

                        case 0xFFFB: //clear feed + line return. Clears speech bubble and resets to beginning, no arguments
                            //TODO: implement this to clear speech bubble properly once manual textbox scrolling is in.
                            textBubbleCurY += tallestCharacter;
                            textBubbleCurX = 0;
                            tallestCharacter = 0;
                            dialogIndex++;
                            break;

                        case 0xFFFC: //wait for input, dialog freeze until button is pressed, no arguments
                            fffcCheck++;
                            dialogIndex++;
                            break;

                        case 0xFFFD: //new line, no arguments
                            //TODO: This will push array Y down to the next line
                            textBubbleCurY += tallestCharacter;
                            textBubbleCurX = 0;
                            tallestCharacter = 0;
                            dialogIndex++;
                            break;

                        case 0xFFFE: //End dialog
                            dialogIndex++;
                            break;

                        case 0xFFFF: //end and close dialog
                            dialogIndex = dialogData[selectedIndex].dialogBytes.Length;
                            break;

                    }
                }
                if (currentIndex >= 0x0799 && currentIndex <= 0xEFFF) //glyph character
                {
                    listIndex += 2;
                    if (fffcCheck == currentBubble)
                    {
                        int glyphPointer = (byte)(currentIndex & 0x0FFF);
                        int currentGlyphWidth = glyphData.glyphWidth[glyphPointer] / 2;
                        if (glyphData.glyphHeight[glyphPointer] > tallestCharacter) //trying to feel out how to track the tallest character in text box
                        {
                            tallestCharacter = glyphData.glyphHeight[glyphPointer];
                        }
                        arrayY = textBubbleCurY;
                        for (int widthSegment = 0;
                              widthSegment < glyphImage[glyphPointer].character.Length;
                              widthSegment += currentGlyphWidth)
                        {
                            arrayX = textBubbleCurX;
                            for (int pixels = 0; pixels < currentGlyphWidth; pixels++)
                            {
                                byte rightNybble = (byte)(glyphImage[glyphPointer].character[widthSegment + pixels] & 0x0F);
                                byte leftNybble = (byte)(glyphImage[glyphPointer].character[widthSegment + pixels] & 0xF0);
                                leftNybble = (byte)(leftNybble >> 4);
                                
                                var paletteDataAdd1 = new PaletteData() { posX = arrayX, posY = arrayY, colorIndex = paletteIndex, colorPointer = rightNybble };
                                var paletteDataAdd2 = new PaletteData() { posX = arrayX + 1, posY = arrayY, colorIndex = paletteIndex, colorPointer = leftNybble };
                                paletteData.Add(paletteDataAdd1);
                                paletteData.Add(paletteDataAdd2);
                                //The attempt to 1 line the list.add led to outofbounds exceptions, I dont get why
                                //textPixels[arrayX, arrayY] = rightNybble; //add soon-to-be pixel 1
                                //paletteData.Add(new PaletteData() { posX = arrayX, posY = arrayY, colorIndex = paletteIndex, colorPointer = rightNybble });
                                //textPixels[arrayX + 1, arrayY] = leftNybble; //add soon-to-be pixel 2
                                //paletteData.Add(new PaletteData() { posX = arrayX + 1, posY = arrayY, colorIndex = paletteIndex, colorPointer = leftNybble });
                                arrayX += 2;
                            }
                            arrayY++;
                        }
                        dialogIndex++;
                        textBubbleCurX += currentGlyphWidth * 2; //set glyph width at end of glyph character to arrayX for later placement
                    }
                    else
                    {
                        dialogIndex++;
                    }
                }
                if (currentIndex == 0x0000) //back up in case FFFF fails for whatever reason to escape loops/etc
                {
                    dialogIndex++;
                }
                //dialogData[selectedIndex].paletteColorIndex.Add(paletteIndex);
            }
        }

        private void ScaleImage(bool isEvent)
        {
            if (pcbx_GlyphImage.Image != null) //https://stackoverflow.com/questions/49554126/image-getting-blurred-when-enlarging-picture-box
            {
                //float x = tb_GlyphScaleX.Value;
                //float y = tb_GlyphScaleY.Value;
                float x = 25;
                float y = 25;
                int selectedIndex = lsbx_GlyphIndex.SelectedIndex;
                Bitmap bmp = glyphImage[selectedIndex].GenerateBitmapScaled(glyphImage[selectedIndex], glyphData, selectedIndex, isEvent);
                Size sz = bmp.Size;
                Bitmap zoomed = (Bitmap)pcbx_GlyphScaled.Image;
                if (zoomed != null) zoomed.Dispose();

                float zoomx = (float)(x / 4f + 1);
                float zoomy = (float)(y / 4f + 1);
                //TODO better formula or better understanding of what this is doing to square up pixels and maintain aspect ratio
                zoomed = new Bitmap((int)(sz.Width * zoomx), (int)(sz.Height * zoomy));

                using (Graphics g = Graphics.FromImage(zoomed))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
                }
                pcbx_GlyphScaled.Width = zoomed.Width;
                pcbx_GlyphScaled.Height = zoomed.Height;
                pcbx_GlyphScaled.Image = zoomed;
                //pcbx_GlyphScaled.Anchor = AnchorStyles.Top;
            }
        }

        private void DialogByteTextDisplay(int index, DialogData[] dialogData)
        {
            int wordIndex = 0;
            string controlString = "";
            while (wordIndex < dialogData[index].dialogBytes.Length)
            {
                UInt16 caseWord = dialogData[index].dialogBytes[wordIndex];
                if (controlString.Contains("\n") || controlString.Contains("\r") || controlString.Contains("\r\n") || controlString.Contains("FF"))
                {
                    controlString = "";
                }
                if (caseWord >= 0xFFF0)
                {
                    if (controlString != "")
                    {
                        rtb_DialogByteDisplay.Text += controlString + "\r\n";
                        controlString = "";
                    }
                    switch (caseWord)
                    {
                        case 0xFFF0: //unused, takes 2 arguments?
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 2].ToString("X4"); 
                            wordIndex += 3;
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            break;

                        case 0xFFF1: //unused
                            MessageBox.Show("Error: 0xFFF1 detected /r/n This is supposed to be an unused control word, if this message comes up please submit a bug report and attach the wfm file used.");
                            break;

                        case 0xFFF2: //not sure, proactive NPCs use this, 1 arg
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 2;
                            break;

                        case 0xFFF3: //not sure, used once
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 2;
                            break;

                        case 0xFFF4: //unused
                            MessageBox.Show("Error: 0xFFF4 detected /r/n This is supposed to be an unused control word, if this message comes up please submit a bug report and attach the wfm file used.");
                            break;

                        case 0xFFF5: //prompt, no arguments, used for SAVE DATA
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;

                        case 0xFFF6: //unknown, 2 arguments
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 2].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 3;
                            break;

                        case 0xFFF7: //change color of all following text, argument determines color change
                            //TODO: Implement palette change
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 2;
                            break;

                        case 0xFFF8: //murmur sound/tail type of textbox, arguments determine what sound plays and what speech tail, 2 arguments
                            //TODO: Will implement graphics for textbox later
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 2].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 3;
                            break;

                        case 0xFFF9: //wait, 1 argument, how many frames to wait
                            //TODO: Implement varying things for this, accurate textbox rep + diagnostic data display that shows this
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 2;
                            break;

                        case 0xFFFA: //Make speec bubble, 2 arguments, width/height
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 1].ToString("X4") + " " + dialogData[index].dialogBytes[wordIndex + 2].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex += 3;
                            break;

                        case 0xFFFB: //clear feed + line return. Clears speech bubble and resets to beginning, no arguments
                            //TODO: implement this to clear speech bubble properly once manual textbox scrolling is in.
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;

                        case 0xFFFC: //wait for input, dialog freeze until button is pressed, no arguments
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;

                        case 0xFFFD: //new line, no arguments
                            //TODO: This will push array Y down to the next line
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;

                        case 0xFFFE: //End dialog
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;

                        case 0xFFFF: //end and close dialog
                            controlString = dialogData[index].dialogBytes[wordIndex].ToString("X4");
                            rtb_DialogByteDisplay.Text += controlString + "\r\n";
                            wordIndex++;
                            break;
                    }
                }
                else if (caseWord <= 0xEFFF)
                {
                    //rtb_DialogByteDisplay.Text += dialogData[index].dialogBytes[wordIndex].ToString("X4") + " ";
                    controlString += dialogData[index].dialogBytes[wordIndex].ToString("X4") + " ";
                    wordIndex++;
                }
                //rtb_DialogByteDisplay.Text += controlString + "\r\n";
            }
        }

        private void UIReset()
        {
            pcbx_GlyphImage.Image = null;
            pcbx_GlyphScaled.Image = null;
            pcbx_TextBubble.Image = null;
            lsbx_DialogIndex.Items.Clear();
            lsbx_GlyphIndex.Items.Clear();
            lsbx_GlyphAsciiOutput.Items.Clear();
            rtb_DialogByteDisplay.Text = "";
            btn_Dialog_Next.Enabled = false;
            btn_Dialog_Previous.Enabled = false;
        }

        private void TEST_btn_findWFMFiles_Click(object sender, EventArgs e)
        {
            //TODO: Make setting if this method is a feature in final
            bool finalCompare = true;
            string wfmDirectory = @"C:\Users\Michael\Documents\ProjectSamples\WFMOutput\WFMs";
            List<string> wfmListCurated = new List<string>();
            List<byte[]> glyphImageBytes = new List<byte[]>();

            string[] wfmListFull = Directory.GetFiles(wfmDirectory, "*.wfm", SearchOption.AllDirectories);

            for (int i = 0; i < wfmListFull.Length; i++)
            {
                BinaryReader wfmFile = new BinaryReader(File.OpenRead(wfmListFull[i]));
                wfmFile.BaseStream.Position = wfmFile.BaseStream.Length - 6;
                UInt16 testWord = wfmFile.ReadUInt16();
                testWord = (UInt16)(testWord & 0xFFFF);
                UInt16 testWord2 = wfmFile.ReadUInt16();
                testWord2 = (UInt16)(testWord2 & 0xFFFF);
                UInt16 testWord3 = wfmFile.ReadUInt16();
                testWord3 = (UInt16)(testWord3 & 0xFFFF);
                //check control words for non-FFFF entries, but still above 0x8000 entries
                if ((testWord >= 0xFFF0 && testWord < 0xFFFF) || (testWord2 >= 0xFFF0 && testWord2 < 0xFFFF) || (testWord3 >= 0xFFF0 && testWord3 < 0xFFFF))
                {
                    //Console.WriteLine(wfmListFull[i] + "is a dialog file");
                    //add to curated list
                    wfmListCurated.Add(wfmListFull[i]);
                }
            }
            for (int i = 0; i < wfmListCurated.Count; i++)
            {
                glyphData = new GlyphData();
                BinaryReader wfmFile = new BinaryReader(File.OpenRead(wfmListCurated[i]));
                FillGlyphData(wfmFile);
                FillGlyphImage(wfmFile);
                for (int j = 0; j < glyphImage.Length; j++)
                {
                    if (glyphImageBytes.Count > 0)
                    {
                        byte[] charTest = glyphImage[j].character;
                        CompareItemsForList(glyphImageBytes, charTest, out finalCompare);
                        if (finalCompare == false)
                        {
                            glyphImageBytes.Add(glyphImage[j].character);
                        }
                    }
                    else
                    {
                        glyphImageBytes.Add(glyphImage[j].character);
                    }
                }
            }
            //at this point, glyphImageBytes contains a full character array, now it needs to be printed

        }

        private void CompareItemsForList(List<byte[]> currentList, byte[] newArray, out bool compareResult)
        {
            bool refCompare = true;
            bool lenCompare = true;
            bool byteCompare = true;
            bool nullCheck = true;
            for (int a = 0; a < currentList.Count; a++)
            {
                if (currentList[a] != null || newArray != null)
                {
                    nullCheck = false;
                }
                if (!ReferenceEquals(currentList[a], newArray))
                {
                    refCompare = false;
                }
                if (currentList[a].Length != newArray.Length)
                {
                    lenCompare = false;
                }
                if (currentList[a] == newArray)
                {
                    byteCompare = true;
                }
            }
            if (refCompare == false || lenCompare == false || byteCompare == false || nullCheck == false)
            {
                compareResult = false;
            }
            else
            {
                compareResult = true;
            }
        }
    }

    class PaletteData
    {
        public int posX;
        public int posY;
        public int colorPointer;
        public int colorIndex;
    }

    class GlyphData
    {
        //HEADER
        public string header;
        public UInt32 headerPadding;
        public UInt32 relativeDialogTablePointer;
        public UInt16 dialogAmount;
        public UInt16 glyphAmount;
        public byte[] unknownBytes; //initialize to 128 on property call

        //GLYPH POINTER TABLE
        public UInt16[] glyphPointers;

        //GLYPH DATA
        public UInt16[] glyphPalette;
        public UInt16[] glyphHeight;
        public UInt16[] glyphWidth;
        public UInt16[] glyphHandaKuten;

    }

    class GlyphImage
    {
        //IMAGE
        Color[] colorListNew;

        public byte[] character;
        //original palette, working out some things, big todo, etc
        public byte[] dialogPalette1 = new byte[32] { 0x00, 0x00, 0x00, 0x04, 0x73, 0x4E, 0x29, 0x25,
                                           0xAD, 0x35, 0x10, 0x42, 0xA5, 0x14, 0x4D, 0x7E,
                                           0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53,
                                           0x74, 0x46, 0x11, 0x3A, 0x00, 0x00, 0x00, 0x00};
        public byte[] eventPalette1 = new byte[32] { 0xFF, 0x01, 0x00, 0x84, 0xFF, 0x7F, 0xEF, 0x3D,
                                           0x29, 0x25, 0xB5, 0x56, 0xF0, 0x00, 0x98, 0x01,
                                           0x39, 0x67, 0x34, 0x01, 0xFF, 0x01, 0x00, 0x7C,
                                           0x00, 0x7C, 0x00, 0x7C, 0x00, 0x7C, 0x00, 0x7C};
        //This is fine, it's just used to get the palette, should really scrape the output and then pull the output and put it in its own array of color objects instead
        private void GeneratePalette(byte[] paletteBytes)
        {
            colorListNew = new Color[16];
            byte[] lePaletteBytes = new byte[paletteBytes.Length];
            byte alpha8bit;
            int alpha;
            byte red5Bit;
            byte green5Bit;
            byte blue5Bit;
            int red8Bit;
            int green8Bit;
            int blue8Bit;
            byte tempByte1;
            byte tempByte2;
            int colorBytesCounter = 0;
            //TODO: maybe just pull them in the right order from the start instead of having to shuffle them in here
            for (int i = 0; i < paletteBytes.Length; i += 2)
            {
                lePaletteBytes[i] = paletteBytes[i + 1];
                lePaletteBytes[i + 1] = paletteBytes[i];
            }
            for (int a = 0; a < lePaletteBytes.Length; a += 2)
            {
                byte byte1 = lePaletteBytes[a];
                byte byte2 = lePaletteBytes[a + 1];
                alpha8bit = (byte)((byte1 & 0x80) >> 7); //isolate first bit, and move it to last position
                /*if (alpha8bit == 0)
                {
                    alpha = 100;
                }
                else
                {
                    alpha = 0;
                }*/
                alpha = alpha8bit * 100;
                tempByte1 = (byte)(byte1 << 1);
                tempByte1 = (byte)(tempByte1 >> 3);
                red5Bit = tempByte1;

                tempByte1 = (byte)(byte1 << 6);
                tempByte1 = (byte)(tempByte1 >> 3);
                tempByte2 = (byte)(byte2 >> 5);
                green5Bit = (byte)(tempByte1 | tempByte2);

                tempByte1 = (byte)(byte2 << 3);
                tempByte1 = (byte)(tempByte1 >> 3);
                blue5Bit = tempByte1;

                red8Bit = (red5Bit * 255) / 31;
                green8Bit = (green5Bit * 255) / 31;
                blue8Bit = (blue5Bit * 255) / 31;
                Color color = new Color();
                //color = Color.FromArgb(alpha, blue8Bit, green8Bit, red8Bit);
                if (blue8Bit == 0 && red8Bit == 0 && green8Bit == 0)
                {
                    color = Color.FromArgb(alpha, blue8Bit, green8Bit, red8Bit);
                }
                else
                {
                    color = Color.FromArgb(blue8Bit, green8Bit, red8Bit);
                }
                colorListNew[colorBytesCounter] = color;
                colorBytesCounter++;
            }
        }

        public Bitmap GenerateBitmapProper(GlyphImage glyphImage, GlyphData glyphData, int arrayPos, bool isEvent)
        { //For use in the 1:1 export of the image
            if (isEvent == true)
            {
                GeneratePalette(glyphImage.eventPalette1);
            }
            else
            {
                GeneratePalette(glyphImage.dialogPalette1); //this should populate the glyphImage colorListNew property
            }
            Bitmap glyphFrame = new Bitmap(glyphData.glyphWidth[arrayPos], glyphData.glyphHeight[arrayPos]); //make a bitmap object with the proper size
            //current bytes are stored in character byte array
            //loop should iterate across the width (2 nybbles per byte)
            int y = 0; //start y row at 0
            for (int i = 0; i < glyphImage.character.Length; i+=(glyphData.glyphWidth[arrayPos]/2)) //iterate over character array length, by width/2 (2 pixels per byte)
            {
                int x = 0; //start x at zero
                for (int j = 0; j < (glyphData.glyphWidth[arrayPos]/2); j++)
                {
                    byte leftNybble = (byte)(glyphImage.character[i + j] & 0xF0); //mask left half
                    leftNybble = (byte)(leftNybble >> 4); //bitshift
                    byte rightNybble = (byte)(glyphImage.character[i + j] & 0x0F); //mask right half
                    glyphFrame.SetPixel(x, y, colorListNew[rightNybble]); //set right
                    glyphFrame.SetPixel(x + 1, y, colorListNew[leftNybble]); //set left
                    x += 2; //increment x by 2 pixels
                }
                y++; //increment y after done with row fill to go to next row coordinate
            }
            return glyphFrame;
        }

        public Bitmap GenerateBitmapScaled(GlyphImage glyphImage, GlyphData glyphData, int arrayPos, bool isEvent)
        { //For use in scaled bitmaps to reduce cutoff on sides/top
            int xOffset = 1;
            int yOFfset = 1;
            if (isEvent == true)
            {
                GeneratePalette(glyphImage.eventPalette1);
            }
            else
            {
                GeneratePalette(glyphImage.dialogPalette1); //this should populate the glyphImage colorListNew property
            }
            Bitmap glyphFrame = new Bitmap(glyphData.glyphWidth[arrayPos] + xOffset, glyphData.glyphHeight[arrayPos] + yOFfset); //make a bitmap object with the proper size
            //current bytes are stored in character byte array
            //loop should iterate across the width (2 nybbles per byte)
            int y = 0 + yOFfset; //start y row at 0
            for (int i = 0; i < glyphImage.character.Length; i += (glyphData.glyphWidth[arrayPos] / 2)) //iterate over character array length, by width/2 (2 pixels per byte)
            {
                int x = 0 + xOffset; //start x at zero
                for (int j = 0; j < (glyphData.glyphWidth[arrayPos] / 2); j++)
                {
                    byte leftNybble = (byte)(glyphImage.character[i + j] & 0xF0); //mask left half
                    leftNybble = (byte)(leftNybble >> 4); //bitshift
                    byte rightNybble = (byte)(glyphImage.character[i + j] & 0x0F); //mask right half
                    glyphFrame.SetPixel(x, y, colorListNew[rightNybble]); //set right
                    glyphFrame.SetPixel(x + 1, y, colorListNew[leftNybble]); //set left
                    x += 2; //increment x by 2 pixels
                }
                y++; //increment y after done with row fill to go to next row coordinate
            }
            return glyphFrame;
        }

        
    }

    class DialogData
    {
        public List<int> paletteColorIndex;
        public UInt16 dialogRelativePointer;
        public UInt16[] dialogBytes;
        public int textboxX;
        public int textboxY;
        Color[] colorListNew;
        public int bubbleCount;
        //public int dialogPaletteIndex = 0;
        //TODO: the rest of the dialogPalettes
        //TODO: CLUT visualization?
        public byte[] dialogPalette0 = new byte[32] { 0x00, 0x00, 0x00, 0x04, 0x73, 0x4E, 0x29, 0x25,
                                           0xAD, 0x35, 0x10, 0x42, 0xA5, 0x14, 0x4D, 0x7E,
                                           0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53,
                                           0x74, 0x46, 0x11, 0x3A, 0x00, 0x00, 0x00, 0x00}; //default
        public byte[] dialogPalette1 = new byte[32] {0x00, 0x00, 0x16, 0x00, 0x9F, 0x31, 0x19, 0x00, 0x18, 0x00, 0x3B, 0x29,
                                                0xA5, 0x14, 0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53, 0x74,
                                                0x46, 0x11, 0x3A, 0x00, 0x00, 0x00, 0x00 }; //red palette
        public byte[] dialogPalette2 = new byte[32] { 0x00, 0x00, 0x20, 0x67, 0xF0, 0x7F, 0x63, 0x6F, 0x30, 0x67, 0x75, 0x6F, 0xA5,
                                                      0x14, 0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53, 0x74, 0x46,
                                                      0x11, 0x3A, 0x00, 0x00, 0x00, 0x00 }; //teal
        public byte[] dialogPalette3 = new byte[32] { 0x00, 0x00, 0x16, 0x58, 0x36, 0x5A, 0x18, 0x60, 0xDA, 0x68, 0xBA, 0x69, 0xA5, 0x14,
                                                        0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53, 0x74, 0x46, 0x11, 0x3A,
                                                        0x00, 0x00, 0x00, 0x00 }; //purple?
        public byte[] dialogPalette4 = new byte[32] { 0x00, 0x00, 0x00, 0x03, 0xB4, 0x53, 0x40, 0x03,
                                                    0x8B, 0x2F, 0xB2, 0x4B, 0xA5, 0x14, 0x4D, 0x7E, 0xE0,
                                                    0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53, 0x74, 0x46, 0x11,
                                                    0x3A, 0x00, 0x00, 0x00, 0x00 };
        public byte[] dialogPalette5 = new byte[32] { 0x00, 0x00, 0x00, 0x60, 0x73, 0x6A, 0x00, 0x70, 0x29, 0x71, 0x10, 0x6A, 0xA5, 0x14,
                                                      0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53, 0x74, 0x46,
                                                        0x11, 0x3A, 0x00, 0x00, 0x00, 0x00 };
        public byte[] dialogPalette6 = new byte[32] { 0x00, 0x00, 0xBF, 0x01, 0xFF, 0x4A, 0x1F, 0x02, 0x3F, 0x0E, 0xBF,
                                                      0x2E, 0xA5, 0x14, 0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53,
                                                      0x74, 0x46, 0x11, 0x3A, 0x00, 0x00 , 0x00, 0x00 };
        public byte[] dialogPalette7 = new byte[32] { 0x00, 0x00, 0x1F, 0x55, 0x7F, 0x5E, 0xFF, 0x64, 0x9F, 0x69, 0x1F, 0x5E,
                                                      0xA5, 0x14, 0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42, 0x7F, 0x29, 0x19, 0x53,
                                                      0x74, 0x46, 0x11, 0x3A, 0x00, 0x00, 0x00, 0x00 };
        public byte[] dialogPalette8 = new byte[32] { 0x00, 0x00, 0xDD, 0x02, 0xDF, 0x43, 0xFF, 0x16, 0x3F, 0x27,
                                                      0x7F, 0x4F, 0xA5, 0x14, 0x4D, 0x7E, 0xE0, 0x03, 0x1F, 0x42,
                                                      0x7F, 0x29, 0x19, 0x53, 0x74, 0x46, 0x11, 0x3A, 0x00, 0x00, 0x00, 0x00 };
        public byte[] eventPalette1 = new byte[32] { 0xFF, 0x01, 0x00, 0x84, 0xFF, 0x7F, 0xEF, 0x3D,
                                           0x29, 0x25, 0xB5, 0x56, 0xF0, 0x00, 0x98, 0x01,
                                           0x39, 0x67, 0x34, 0x01, 0xFF, 0x01, 0x00, 0x7C,
                                           0x00, 0x7C, 0x00, 0x7C, 0x00, 0x7C, 0x00, 0x7C};

        //This is fine, it's just used to get the palette, should really scrape the output and put it in its own array of color objects instead
        private void GeneratePalette(byte[] paletteBytes)
        {
            colorListNew = new Color[16];
            byte[] lePaletteBytes = new byte[paletteBytes.Length];
            byte alpha8bit;
            int alpha;
            byte red5Bit;
            byte green5Bit;
            byte blue5Bit;
            int red8Bit;
            int green8Bit;
            int blue8Bit;
            byte tempByte1;
            byte tempByte2;
            int colorBytesCounter = 0;
            //TODO: maybe just pull them in the right order from the start instead of having to shuffle them in here
            for (int i = 0; i < paletteBytes.Length; i += 2)
            {
                lePaletteBytes[i] = paletteBytes[i + 1];
                lePaletteBytes[i + 1] = paletteBytes[i];
            }
            for (int a = 0; a < lePaletteBytes.Length; a += 2)
            {
                byte byte1 = lePaletteBytes[a];
                byte byte2 = lePaletteBytes[a + 1];
                alpha8bit = (byte)((byte1 & 0x80) >> 7); //isolate first bit, and move it to last position
                /*if (alpha8bit == 0)
                {
                    //alpha = 100;
                    alpha = 0;
                }
                else
                {
                    alpha = 100;
                }*/
                alpha = alpha8bit * 100;
                tempByte1 = (byte)(byte1 << 1);
                tempByte1 = (byte)(tempByte1 >> 3);
                red5Bit = tempByte1;

                tempByte1 = (byte)(byte1 << 6);
                tempByte1 = (byte)(tempByte1 >> 3);
                tempByte2 = (byte)(byte2 >> 5);
                green5Bit = (byte)(tempByte1 | tempByte2);

                tempByte1 = (byte)(byte2 << 3);
                tempByte1 = (byte)(tempByte1 >> 3);
                blue5Bit = tempByte1;

                red8Bit = (red5Bit * 255) / 31;
                green8Bit = (green5Bit * 255) / 31;
                blue8Bit = (blue5Bit * 255) / 31;
                Color color = new Color();
                //color = Color.FromArgb(alpha, blue8Bit, green8Bit, red8Bit); //you'd think this would work, and it should, but its wrong and I dont know why alpha isnt defined properly despite the bit existing
                if (blue8Bit == 0 && red8Bit == 0 && green8Bit == 0)
                {
                    color = Color.FromArgb(alpha, blue8Bit, green8Bit, red8Bit);
                }
                else
                {
                    color = Color.FromArgb(blue8Bit, green8Bit, red8Bit);
                }
                colorListNew[colorBytesCounter] = color;
                colorBytesCounter++;
            }
        }

        public Bitmap GenerateBitmapTextbox(DialogData dialogData, List<PaletteData> paletteData, byte[,] textPixels, GlyphData glyphData, int index, bool isEvent)
        {
            //List<Order> SortedList = objListOrder.OrderBy(o=>o.OrderDate).ToList();
            List<PaletteData> sortedpaletteData = paletteData.OrderBy(y => y.posY).ToList();
            int paletteSwapCheck = 0;
            if (isEvent == true)
            {
                GeneratePalette(eventPalette1);
            }
            else
            {
                GeneratePalette(dialogPalette0);
                //this should populate the glyphImage colorListNew property
            }
            //todo figure out a way to calculate the width of the event boxes/current X/Y totals
            //this is for event box handling
            if (textboxX == 0)
            {
                textboxX = 900;
            }
            if (textboxY == 0)
            {
                textboxY = 64;
            }
            Bitmap textbox = new Bitmap(textboxX, textboxY);
            for (int i = 0; i < sortedpaletteData.Count; i++)
            {
                int paletteIndexCheck = sortedpaletteData[i].colorIndex;
                if (paletteIndexCheck != paletteSwapCheck)
                {
                    if (paletteIndexCheck != paletteSwapCheck)
                    {
                        paletteSwapCheck = paletteIndexCheck;
                        switch (paletteSwapCheck)
                        {
                            case 0:
                                GeneratePalette(dialogPalette0);
                                break;
                            case 1:
                                GeneratePalette(dialogPalette1);
                                break;
                            case 2:
                                GeneratePalette(dialogPalette2);
                                break;
                            case 3:
                                GeneratePalette(dialogPalette3);
                                break;
                            case 4:
                                GeneratePalette(dialogPalette4);
                                break;
                            case 5:
                                GeneratePalette(dialogPalette5);
                                break;
                            case 6:
                                GeneratePalette(dialogPalette6);
                                break;
                            case 7:
                                GeneratePalette(dialogPalette7);
                                break;
                            case 8:
                                GeneratePalette(dialogPalette8);
                                break;
                            default:
                                GeneratePalette(dialogPalette0);
                                break;
                        }
                    }
                }
                if (sortedpaletteData[i].posX < textboxX && sortedpaletteData[i].posY < textboxY)
                {
                    textbox.SetPixel(sortedpaletteData[i].posX, sortedpaletteData[i].posY, colorListNew[sortedpaletteData[i].colorPointer]);
                }
            }
            return textbox;
        }
    }
}
