namespace esteganografia
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            buttonOpenFile = new Button();
            buttonEncode = new Button();
            buttonDecode = new Button();
            textBoxFilePath = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(960, 360);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // buttonOpenFile
            // 
            buttonOpenFile.Location = new Point(12, 378);
            buttonOpenFile.Name = "buttonOpenFile";
            buttonOpenFile.Size = new Size(91, 23);
            buttonOpenFile.TabIndex = 1;
            buttonOpenFile.Text = "Abrir arquivo";
            buttonOpenFile.UseVisualStyleBackColor = true;
            buttonOpenFile.Click += buttonOpenFile_Click;
            // 
            // buttonEncode
            // 
            buttonEncode.Location = new Point(12, 407);
            buttonEncode.Name = "buttonEncode";
            buttonEncode.Size = new Size(80, 23);
            buttonEncode.TabIndex = 2;
            buttonEncode.Text = "Codificar";
            buttonEncode.UseVisualStyleBackColor = true;
            buttonEncode.Click += buttonEncode_Click;
            // 
            // buttonDecode
            // 
            buttonDecode.Location = new Point(109, 407);
            buttonDecode.Name = "buttonDecode";
            buttonDecode.Size = new Size(80, 23);
            buttonDecode.TabIndex = 3;
            buttonDecode.Text = "Decodificar";
            buttonDecode.UseVisualStyleBackColor = true;
            buttonDecode.Click += buttonDecode_Click;
            // 
            // textBoxFilePath
            // 
            textBoxFilePath.Location = new Point(109, 378);
            textBoxFilePath.Name = "textBoxFilePath";
            textBoxFilePath.Size = new Size(325, 23);
            textBoxFilePath.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 441);
            Controls.Add(textBoxFilePath);
            Controls.Add(buttonDecode);
            Controls.Add(buttonEncode);
            Controls.Add(buttonOpenFile);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Estenografia";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button buttonOpenFile;
        private Button buttonEncode;
        private Button buttonDecode;
        private TextBox textBoxFilePath;
    }
}
