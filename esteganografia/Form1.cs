using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace esteganografia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
            openDialog.InitialDirectory = @"C:\Users\Guilherme\Desktop";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openDialog.FileName;
                pictureBox1.ImageLocation = textBoxFilePath.Text;
            }
        }

        private void buttonEncode_Click(object sender, EventArgs e)
        {
            string filePath = textBoxFilePath.Text;
            string fileExtension = Path.GetExtension(filePath).ToLower();

            if (fileExtension == ".png")
            {

                // Carregar a imagem
                Bitmap img = new Bitmap(textBoxFilePath.Text);
                while (true)
                {
                    // Selecionar o arquivo para embutir na imagem
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "Todos os Arquivos (*.*)|*.*";
                    fileDialog.InitialDirectory = @"C:\Users\Guilherme\Desktop";

                    if (fileDialog.ShowDialog() != DialogResult.OK)
                    {
                        // Ação cancelada
                        return;
                    }

                    // Converte o arquivo em uma matriz de bytes
                    byte[] fileBytes = File.ReadAllBytes(fileDialog.FileName);
                    int fileLength = fileBytes.Length;

                    // Obter a extensão do arquivo
                    fileExtension = Path.GetExtension(fileDialog.FileName).TrimStart('.');
                    byte[] extensionBytes = Encoding.UTF8.GetBytes(fileExtension);

                    // Certifique-se de que a extensão tenha no máximo 4 caracteres
                    if (extensionBytes.Length > 4)
                    {
                        MessageBox.Show("Extensão muito longa. Use extensões de até 4 caracteres.");
                        continue;
                    }

                    // Combinar os bytes do comprimento, extensão e conteúdo do arquivo em uma única matriz
                    byte[] dataBytes = new byte[4 + 4 + fileLength];
                    Array.Copy(BitConverter.GetBytes(fileLength), 0, dataBytes, 0, 4);
                    Array.Copy(extensionBytes, 0, dataBytes, 4, extensionBytes.Length);
                    Array.Copy(fileBytes, 0, dataBytes, 8, fileLength);

                    // Verificar o tamanho do arquivo em relação à capacidade da imagem
                    int maxDataBits = img.Width * img.Height * 3; // Cada pixel pode armazenar 3 bits de dados (R, G, B)
                    if (dataBytes.Length * 8 > maxDataBits)
                    {
                        MessageBox.Show("O arquivo é grande demais para ser embutido na imagem. Selecione outro arquivo.");
                        continue;
                    }

                    // Se permitir fazer a embução da imagem
                    int dataIndex = 0;

                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            if (dataIndex < dataBytes.Length * 8)
                            {
                                Color pixel = img.GetPixel(i, j);

                                // Inserir o bit do arquivo nos menores bits significativos dos componentes R, G e B
                                int bitIndex = dataIndex % 8;
                                int byteIndex = dataIndex / 8;
                                byte bit = (byte)((dataBytes[byteIndex] >> (7 - bitIndex)) & 1);

                                int r = (pixel.R & 0xFE) | ((bit >> 2) & 1);
                                int g = (pixel.G & 0xFE) | ((bit >> 1) & 1);
                                int b = (pixel.B & 0xFE) | (bit & 1);

                                img.SetPixel(i, j, Color.FromArgb(r, g, b));
                                dataIndex++;
                            }
                        }
                    }

                    // Salva a imagem com o arquivo embutido
                    SaveFileDialog saveFile = new SaveFileDialog
                    {
                        Filter = "PNG Image (*.png)|*.png",
                        InitialDirectory = @"C:\Users\Guilherme\Desktop"
                    };

                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        string savePath = saveFile.FileName;
                        img.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Embução realizada com sucesso");
                    }

                    break; // Sai do loop
                }
            }
            else
            {
                MessageBox.Show("Arquivo em jpg");
            }
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {

            string filePath = textBoxFilePath.Text;
            string fileExtension = Path.GetExtension(filePath).ToLower();

            if (fileExtension == ".png")
            {
                Bitmap img = new Bitmap(textBoxFilePath.Text);

                // Recuperar os bits
                int dataLength = img.Width * img.Height * 3 / 8; // Total que pode ser recuperado
                byte[] dataBytes = new byte[dataLength];
                int dataIndex = 0;

                for (int i = 0; i < img.Width; i++)
                {
                    for (int j = 0; j < img.Height; j++)
                    {
                        if (dataIndex < dataBytes.Length * 8)
                        {
                            Color pixel = img.GetPixel(i, j);

                            // Extrair os bits do arquivo dos menores bits significativos dos componentes R, G e B
                            int bitIndex = dataIndex % 8;
                            int byteIndex = dataIndex / 8;

                            byte bitR = (byte)((pixel.R & 1) << 2); // Bit 2
                            byte bitG = (byte)((pixel.G & 1) << 1); // Bit 1
                            byte bitB = (byte)(pixel.B & 1);        // Bit 0

                            byte bit = (byte)(bitR | bitG | bitB);

                            dataBytes[byteIndex] |= (byte)(bit << (7 - bitIndex));
                            dataIndex++;
                        }
                    }
                }

                // Recuperar o comprimento do arquivo dos primeiros 4 bytes
                int fileLength = BitConverter.ToInt32(dataBytes, 0);

                // Verificar se temos bytes suficientes para a extensão e o conteúdo do arquivo
                if (dataBytes.Length < 8 + fileLength)
                {
                    MessageBox.Show("Erro ao recuperar o arquivo: o comprimento dos dados não é suficiente.");
                    return;
                }

                // Recuperar a extensão do arquivo dos próximos 4 bytes
                fileExtension = Encoding.UTF8.GetString(dataBytes, 4, 4).Trim('\0');

                // Recuperar o conteúdo do arquivo
                byte[] fileBytes = new byte[fileLength];
                Array.Copy(dataBytes, 8, fileBytes, 0, fileLength);

                // Salvar o arquivo extraído
                SaveFileDialog saveFile = new SaveFileDialog
                {
                    Filter = $"Arquivo Recuperado (*.{fileExtension})|*.{fileExtension}",
                    DefaultExt = fileExtension,
                    InitialDirectory = @"C:\Users\Guilherme\Desktop"
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFile.FileName, fileBytes);
                    MessageBox.Show("Arquivo extraído com sucesso!");
                }
            }
            else
            {
                MessageBox.Show("Falta codigo");
            }
        }
    }
}
