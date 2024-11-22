namespace LeituraHuffaman
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
            text_Huffaman = new RichTextBox();
            text_Caminho = new TextBox();
            button_anexar = new Button();
            button_Leitura = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(776, 456);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // text_Huffaman
            // 
            text_Huffaman.Location = new Point(93, 503);
            text_Huffaman.Name = "text_Huffaman";
            text_Huffaman.Size = new Size(695, 146);
            text_Huffaman.TabIndex = 1;
            text_Huffaman.Text = "";
            text_Huffaman.TextChanged += text_Huffaman_TextChanged;
            // 
            // text_Caminho
            // 
            text_Caminho.Enabled = false;
            text_Caminho.Location = new Point(93, 474);
            text_Caminho.Name = "text_Caminho";
            text_Caminho.Size = new Size(695, 23);
            text_Caminho.TabIndex = 2;
            // 
            // button_anexar
            // 
            button_anexar.Location = new Point(12, 474);
            button_anexar.Name = "button_anexar";
            button_anexar.Size = new Size(75, 23);
            button_anexar.TabIndex = 3;
            button_anexar.Text = "Anexar";
            button_anexar.UseVisualStyleBackColor = true;
            button_anexar.Click += button_anexar_Click;
            // 
            // button_Leitura
            // 
            button_Leitura.Location = new Point(12, 503);
            button_Leitura.Name = "button_Leitura";
            button_Leitura.Size = new Size(75, 23);
            button_Leitura.TabIndex = 4;
            button_Leitura.Text = "Leitura";
            button_Leitura.UseVisualStyleBackColor = true;
            button_Leitura.Click += button_Leitura_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 661);
            Controls.Add(button_Leitura);
            Controls.Add(button_anexar);
            Controls.Add(text_Caminho);
            Controls.Add(text_Huffaman);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Leitura Huffaman";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private RichTextBox text_Huffaman;
        private TextBox text_Caminho;
        private Button button_anexar;
        private Button button_Leitura;
    }
}
