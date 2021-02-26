namespace WFM_Output
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_FilePath = new System.Windows.Forms.Label();
            this.btn_SelectFile = new System.Windows.Forms.Button();
            this.lbl_desc_FilePath = new System.Windows.Forms.Label();
            this.pcbx_GlyphImage = new System.Windows.Forms.PictureBox();
            this.lsbx_GlyphIndex = new System.Windows.Forms.ListBox();
            this.btn_Output = new System.Windows.Forms.Button();
            this.ofd_WFM3 = new System.Windows.Forms.OpenFileDialog();
            this.lsbx_GlyphAsciiOutput = new System.Windows.Forms.ListBox();
            this.pcbx_GlyphScaled = new System.Windows.Forms.PictureBox();
            this.lsbx_DialogIndex = new System.Windows.Forms.ListBox();
            this.lbl_desc_GlyphIndex = new System.Windows.Forms.Label();
            this.lbl_desc_AsciiOutput = new System.Windows.Forms.Label();
            this.lbl_desc_BitmapOutput = new System.Windows.Forms.Label();
            this.lbl_desc_DialogIndex = new System.Windows.Forms.Label();
            this.lbl_GlyphWidth = new System.Windows.Forms.Label();
            this.lbl_GlyphHeight = new System.Windows.Forms.Label();
            this.pcbx_TextBubble = new System.Windows.Forms.PictureBox();
            this.btn_Dialog_Previous = new System.Windows.Forms.Button();
            this.btn_Dialog_Next = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_Shitpost = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtb_DialogByteDisplay = new System.Windows.Forms.RichTextBox();
            this.TEST_btn_findWFMFiles = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_GlyphImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_GlyphScaled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_TextBubble)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_FilePath
            // 
            this.lbl_FilePath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FilePath.Location = new System.Drawing.Point(106, 21);
            this.lbl_FilePath.Name = "lbl_FilePath";
            this.lbl_FilePath.Size = new System.Drawing.Size(597, 61);
            this.lbl_FilePath.TabIndex = 0;
            this.lbl_FilePath.Text = "FilePath Here";
            // 
            // btn_SelectFile
            // 
            this.btn_SelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectFile.Location = new System.Drawing.Point(12, 93);
            this.btn_SelectFile.Name = "btn_SelectFile";
            this.btn_SelectFile.Size = new System.Drawing.Size(96, 39);
            this.btn_SelectFile.TabIndex = 1;
            this.btn_SelectFile.Text = "Select File";
            this.btn_SelectFile.UseVisualStyleBackColor = true;
            this.btn_SelectFile.Click += new System.EventHandler(this.btn_SelectFile_Click);
            // 
            // lbl_desc_FilePath
            // 
            this.lbl_desc_FilePath.AutoSize = true;
            this.lbl_desc_FilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc_FilePath.Location = new System.Drawing.Point(12, 21);
            this.lbl_desc_FilePath.Name = "lbl_desc_FilePath";
            this.lbl_desc_FilePath.Size = new System.Drawing.Size(88, 20);
            this.lbl_desc_FilePath.TabIndex = 2;
            this.lbl_desc_FilePath.Text = "Path to file:";
            // 
            // pcbx_GlyphImage
            // 
            this.pcbx_GlyphImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcbx_GlyphImage.Location = new System.Drawing.Point(303, 50);
            this.pcbx_GlyphImage.Name = "pcbx_GlyphImage";
            this.pcbx_GlyphImage.Size = new System.Drawing.Size(59, 46);
            this.pcbx_GlyphImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbx_GlyphImage.TabIndex = 3;
            this.pcbx_GlyphImage.TabStop = false;
            // 
            // lsbx_GlyphIndex
            // 
            this.lsbx_GlyphIndex.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbx_GlyphIndex.FormattingEnabled = true;
            this.lsbx_GlyphIndex.ItemHeight = 14;
            this.lsbx_GlyphIndex.Location = new System.Drawing.Point(6, 36);
            this.lsbx_GlyphIndex.Name = "lsbx_GlyphIndex";
            this.lsbx_GlyphIndex.Size = new System.Drawing.Size(84, 242);
            this.lsbx_GlyphIndex.TabIndex = 4;
            this.lsbx_GlyphIndex.SelectedIndexChanged += new System.EventHandler(this.lsbx_GlyphIndex_SelectedIndexChanged);
            // 
            // btn_Output
            // 
            this.btn_Output.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Output.Location = new System.Drawing.Point(125, 93);
            this.btn_Output.Name = "btn_Output";
            this.btn_Output.Size = new System.Drawing.Size(96, 39);
            this.btn_Output.TabIndex = 5;
            this.btn_Output.Text = "Output";
            this.btn_Output.UseVisualStyleBackColor = true;
            this.btn_Output.Click += new System.EventHandler(this.btn_Output_Click);
            // 
            // ofd_WFM3
            // 
            this.ofd_WFM3.FileName = "openFileDialog1";
            // 
            // lsbx_GlyphAsciiOutput
            // 
            this.lsbx_GlyphAsciiOutput.FormattingEnabled = true;
            this.lsbx_GlyphAsciiOutput.Location = new System.Drawing.Point(734, 187);
            this.lsbx_GlyphAsciiOutput.Name = "lsbx_GlyphAsciiOutput";
            this.lsbx_GlyphAsciiOutput.Size = new System.Drawing.Size(120, 238);
            this.lsbx_GlyphAsciiOutput.TabIndex = 6;
            this.lsbx_GlyphAsciiOutput.Visible = false;
            // 
            // pcbx_GlyphScaled
            // 
            this.pcbx_GlyphScaled.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pcbx_GlyphScaled.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcbx_GlyphScaled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbx_GlyphScaled.Location = new System.Drawing.Point(303, 149);
            this.pcbx_GlyphScaled.Name = "pcbx_GlyphScaled";
            this.pcbx_GlyphScaled.Size = new System.Drawing.Size(59, 69);
            this.pcbx_GlyphScaled.TabIndex = 8;
            this.pcbx_GlyphScaled.TabStop = false;
            // 
            // lsbx_DialogIndex
            // 
            this.lsbx_DialogIndex.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsbx_DialogIndex.FormattingEnabled = true;
            this.lsbx_DialogIndex.ItemHeight = 14;
            this.lsbx_DialogIndex.Location = new System.Drawing.Point(17, 40);
            this.lsbx_DialogIndex.Name = "lsbx_DialogIndex";
            this.lsbx_DialogIndex.Size = new System.Drawing.Size(84, 144);
            this.lsbx_DialogIndex.TabIndex = 18;
            this.lsbx_DialogIndex.SelectedIndexChanged += new System.EventHandler(this.lsbx_DialogSelect_SelectedIndexChanged);
            // 
            // lbl_desc_GlyphIndex
            // 
            this.lbl_desc_GlyphIndex.AutoSize = true;
            this.lbl_desc_GlyphIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc_GlyphIndex.Location = new System.Drawing.Point(3, 17);
            this.lbl_desc_GlyphIndex.Name = "lbl_desc_GlyphIndex";
            this.lbl_desc_GlyphIndex.Size = new System.Drawing.Size(78, 16);
            this.lbl_desc_GlyphIndex.TabIndex = 19;
            this.lbl_desc_GlyphIndex.Text = "Glyph Index";
            // 
            // lbl_desc_AsciiOutput
            // 
            this.lbl_desc_AsciiOutput.AutoSize = true;
            this.lbl_desc_AsciiOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc_AsciiOutput.Location = new System.Drawing.Point(734, 168);
            this.lbl_desc_AsciiOutput.Name = "lbl_desc_AsciiOutput";
            this.lbl_desc_AsciiOutput.Size = new System.Drawing.Size(82, 16);
            this.lbl_desc_AsciiOutput.TabIndex = 20;
            this.lbl_desc_AsciiOutput.Text = "ASCII Output";
            this.lbl_desc_AsciiOutput.Visible = false;
            // 
            // lbl_desc_BitmapOutput
            // 
            this.lbl_desc_BitmapOutput.AutoSize = true;
            this.lbl_desc_BitmapOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc_BitmapOutput.Location = new System.Drawing.Point(300, 21);
            this.lbl_desc_BitmapOutput.Name = "lbl_desc_BitmapOutput";
            this.lbl_desc_BitmapOutput.Size = new System.Drawing.Size(98, 16);
            this.lbl_desc_BitmapOutput.TabIndex = 21;
            this.lbl_desc_BitmapOutput.Text = "Bitmap Outputs";
            // 
            // lbl_desc_DialogIndex
            // 
            this.lbl_desc_DialogIndex.AutoSize = true;
            this.lbl_desc_DialogIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc_DialogIndex.Location = new System.Drawing.Point(14, 21);
            this.lbl_desc_DialogIndex.Name = "lbl_desc_DialogIndex";
            this.lbl_desc_DialogIndex.Size = new System.Drawing.Size(83, 16);
            this.lbl_desc_DialogIndex.TabIndex = 22;
            this.lbl_desc_DialogIndex.Text = "Dialog Index";
            // 
            // lbl_GlyphWidth
            // 
            this.lbl_GlyphWidth.AutoSize = true;
            this.lbl_GlyphWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_GlyphWidth.Location = new System.Drawing.Point(106, 36);
            this.lbl_GlyphWidth.Name = "lbl_GlyphWidth";
            this.lbl_GlyphWidth.Size = new System.Drawing.Size(83, 16);
            this.lbl_GlyphWidth.TabIndex = 25;
            this.lbl_GlyphWidth.Text = "Glyph Width:";
            // 
            // lbl_GlyphHeight
            // 
            this.lbl_GlyphHeight.AutoSize = true;
            this.lbl_GlyphHeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_GlyphHeight.Location = new System.Drawing.Point(106, 66);
            this.lbl_GlyphHeight.Name = "lbl_GlyphHeight";
            this.lbl_GlyphHeight.Size = new System.Drawing.Size(88, 16);
            this.lbl_GlyphHeight.TabIndex = 26;
            this.lbl_GlyphHeight.Text = "Glyph Height:";
            // 
            // pcbx_TextBubble
            // 
            this.pcbx_TextBubble.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pcbx_TextBubble.BackColor = System.Drawing.Color.White;
            this.pcbx_TextBubble.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcbx_TextBubble.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbx_TextBubble.Location = new System.Drawing.Point(122, 40);
            this.pcbx_TextBubble.Name = "pcbx_TextBubble";
            this.pcbx_TextBubble.Size = new System.Drawing.Size(988, 86);
            this.pcbx_TextBubble.TabIndex = 27;
            this.pcbx_TextBubble.TabStop = false;
            // 
            // btn_Dialog_Previous
            // 
            this.btn_Dialog_Previous.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Dialog_Previous.Location = new System.Drawing.Point(122, 132);
            this.btn_Dialog_Previous.Name = "btn_Dialog_Previous";
            this.btn_Dialog_Previous.Size = new System.Drawing.Size(64, 43);
            this.btn_Dialog_Previous.TabIndex = 28;
            this.btn_Dialog_Previous.Text = "Previous";
            this.btn_Dialog_Previous.UseVisualStyleBackColor = true;
            this.btn_Dialog_Previous.Click += new System.EventHandler(this.btn_Dialog_Previous_Click);
            // 
            // btn_Dialog_Next
            // 
            this.btn_Dialog_Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Dialog_Next.Location = new System.Drawing.Point(318, 132);
            this.btn_Dialog_Next.Name = "btn_Dialog_Next";
            this.btn_Dialog_Next.Size = new System.Drawing.Size(64, 43);
            this.btn_Dialog_Next.TabIndex = 29;
            this.btn_Dialog_Next.Text = "Next";
            this.btn_Dialog_Next.UseVisualStyleBackColor = true;
            this.btn_Dialog_Next.Click += new System.EventHandler(this.btn_Dialog_Next_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(43, 164);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1139, 392);
            this.tabControl1.TabIndex = 30;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lsbx_GlyphIndex);
            this.tabPage1.Controls.Add(this.lbl_desc_GlyphIndex);
            this.tabPage1.Controls.Add(this.lbl_GlyphHeight);
            this.tabPage1.Controls.Add(this.lbl_GlyphWidth);
            this.tabPage1.Controls.Add(this.pcbx_GlyphImage);
            this.tabPage1.Controls.Add(this.lbl_desc_BitmapOutput);
            this.tabPage1.Controls.Add(this.pcbx_GlyphScaled);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1131, 365);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Character";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_Shitpost);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.rtb_DialogByteDisplay);
            this.tabPage2.Controls.Add(this.pcbx_TextBubble);
            this.tabPage2.Controls.Add(this.btn_Dialog_Next);
            this.tabPage2.Controls.Add(this.lsbx_DialogIndex);
            this.tabPage2.Controls.Add(this.btn_Dialog_Previous);
            this.tabPage2.Controls.Add(this.lbl_desc_DialogIndex);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1131, 365);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Dialog";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_Shitpost
            // 
            this.btn_Shitpost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Shitpost.Location = new System.Drawing.Point(475, 132);
            this.btn_Shitpost.Name = "btn_Shitpost";
            this.btn_Shitpost.Size = new System.Drawing.Size(123, 65);
            this.btn_Shitpost.TabIndex = 32;
            this.btn_Shitpost.Text = "Debug: Shitpost";
            this.btn_Shitpost.UseVisualStyleBackColor = true;
            this.btn_Shitpost.Click += new System.EventHandler(this.btn_Shitpost_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(122, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 16);
            this.label4.TabIndex = 32;
            this.label4.Text = "In-Game Display";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 31;
            this.label3.Text = "Dialog Bytes (UInt16)";
            // 
            // rtb_DialogByteDisplay
            // 
            this.rtb_DialogByteDisplay.Location = new System.Drawing.Point(17, 223);
            this.rtb_DialogByteDisplay.Name = "rtb_DialogByteDisplay";
            this.rtb_DialogByteDisplay.Size = new System.Drawing.Size(581, 136);
            this.rtb_DialogByteDisplay.TabIndex = 30;
            this.rtb_DialogByteDisplay.Text = "";
            // 
            // TEST_btn_findWFMFiles
            // 
            this.TEST_btn_findWFMFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TEST_btn_findWFMFiles.Location = new System.Drawing.Point(264, 93);
            this.TEST_btn_findWFMFiles.Name = "TEST_btn_findWFMFiles";
            this.TEST_btn_findWFMFiles.Size = new System.Drawing.Size(123, 65);
            this.TEST_btn_findWFMFiles.TabIndex = 31;
            this.TEST_btn_findWFMFiles.Text = "Debug: Find all wfm files";
            this.TEST_btn_findWFMFiles.UseVisualStyleBackColor = true;
            this.TEST_btn_findWFMFiles.Click += new System.EventHandler(this.TEST_btn_findWFMFiles_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1131, 365);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Compare Dialog";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 566);
            this.Controls.Add(this.TEST_btn_findWFMFiles);
            this.Controls.Add(this.lsbx_GlyphAsciiOutput);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btn_Output);
            this.Controls.Add(this.lbl_desc_AsciiOutput);
            this.Controls.Add(this.lbl_desc_FilePath);
            this.Controls.Add(this.btn_SelectFile);
            this.Controls.Add(this.lbl_FilePath);
            this.Name = "Form1";
            this.Text = "The Literature Pig Bag V0.5";
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_GlyphImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_GlyphScaled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbx_TextBubble)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_FilePath;
        private System.Windows.Forms.Button btn_SelectFile;
        private System.Windows.Forms.Label lbl_desc_FilePath;
        private System.Windows.Forms.PictureBox pcbx_GlyphImage;
        private System.Windows.Forms.ListBox lsbx_GlyphIndex;
        private System.Windows.Forms.Button btn_Output;
        private System.Windows.Forms.OpenFileDialog ofd_WFM3;
        private System.Windows.Forms.ListBox lsbx_GlyphAsciiOutput;
        private System.Windows.Forms.PictureBox pcbx_GlyphScaled;
        private System.Windows.Forms.ListBox lsbx_DialogIndex;
        private System.Windows.Forms.Label lbl_desc_GlyphIndex;
        private System.Windows.Forms.Label lbl_desc_AsciiOutput;
        private System.Windows.Forms.Label lbl_desc_BitmapOutput;
        private System.Windows.Forms.Label lbl_desc_DialogIndex;
        private System.Windows.Forms.Label lbl_GlyphWidth;
        private System.Windows.Forms.Label lbl_GlyphHeight;
        private System.Windows.Forms.PictureBox pcbx_TextBubble;
        private System.Windows.Forms.Button btn_Dialog_Previous;
        private System.Windows.Forms.Button btn_Dialog_Next;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox rtb_DialogByteDisplay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button TEST_btn_findWFMFiles;
        private System.Windows.Forms.Button btn_Shitpost;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

