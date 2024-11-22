using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LeituraHuffaman
{
    public partial class Form1 : Form
    {
        private Bitmap img;
        private byte[] JPGData;
        private string filePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_anexar_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
            openDialog.InitialDirectory = @"C:\Users\Guilherme\Desktop";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openDialog.FileName;
                text_Caminho.Text = filePath;
                pictureBox1.ImageLocation = filePath;

                // Carregar a imagem
                img = new Bitmap(filePath);
                JPGData = File.ReadAllBytes(filePath);
                MessageBox.Show("Imagem carregada com sucesso.");
            }
        }

        private void button_Leitura_Click(object sender, EventArgs e)
        {
            if (img == null)
            {
                MessageBox.Show("Por favor, anexe um arquivo primeiro.");
                return;
            }

            string temp = ReadHuffmanTables(JPGData);
            text_Huffaman.Text = temp;
            /*try
            {
                HuffmanTree huffmanTree = new HuffmanTree();
                string encodedText = huffmanTree.EncodeImage(img);

                // Exibe o texto codificado no TextBox
                text_Huffaman.Text = encodedText;

                // Decodificação
                Bitmap decodedImage = huffmanTree.DecodeImage(encodedText, img.Width, img.Height);

                // Salva a imagem decodificada
                string outputPath = Path.ChangeExtension(filePath, "_decodificado.jpg");
                decodedImage.Save(outputPath);

                MessageBox.Show("Imagem decodificada salva em: " + outputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar a imagem: " + ex.Message);
            }*/
        }

        static string ReadHuffmanTables(byte[] data)
        {
            string temp = "";

            for (int i = 0; i < data.Length - 1; i++)
            {
                if (data[i] == 0xFF && data[i + 1] == 0xC4)
                {
                    temp += "Encontrado o marcador DHT (Define Huffman Table) em posição: " + i + "\n";
                    int length = (data[i + 2] << 8) + data[i + 3];
                    temp += "Comprimento do segmento DHT: " + length + "\n";
                    i += length;
                }
            }
            return temp;
        }

        private void text_Huffaman_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Node : IComparable<Node>
    {
        public byte Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public int CompareTo(Node other)
        {
            return Frequency - other.Frequency;
        }
    }

    public class HuffmanTree
    {
        private Dictionary<byte, string> codes;
        private Dictionary<string, byte> reverseMapping;

        public HuffmanTree()
        {
            codes = new Dictionary<byte, string>();
            reverseMapping = new Dictionary<string, byte>();
        }

        public string EncodeImage(Bitmap img)
        {
            List<byte> imageBytes = new List<byte>();
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pixel = img.GetPixel(x, y);
                    imageBytes.Add(pixel.R);
                    imageBytes.Add(pixel.G);
                    imageBytes.Add(pixel.B);
                }
            }

            Dictionary<byte, int> frequency = BuildFrequencyDictionary(imageBytes.ToArray());
            Node root = BuildTree(frequency);
            GenerateCodes(root, "");

            string encodedText = "";
            foreach (byte b in imageBytes)
            {
                encodedText += codes[b];
            }

            return encodedText;
        }

        private Dictionary<byte, int> BuildFrequencyDictionary(byte[] fileBytes)
        {
            Dictionary<byte, int> frequency = new Dictionary<byte, int>();
            foreach (byte b in fileBytes)
            {
                if (frequency.ContainsKey(b))
                {
                    frequency[b]++;
                }
                else
                {
                    frequency[b] = 1;
                }
            }
            return frequency;
        }

        private Node BuildTree(Dictionary<byte, int> frequency)
        {
            PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();
            foreach (var kvp in frequency)
            {
                priorityQueue.Enqueue(new Node { Symbol = kvp.Key, Frequency = kvp.Value }, kvp.Value);
            }

            while (priorityQueue.Count > 1)
            {
                Node left = priorityQueue.Dequeue();
                Node right = priorityQueue.Dequeue();
                Node parent = new Node
                {
                    Symbol = 0,
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };
                priorityQueue.Enqueue(parent, parent.Frequency);
            }

            return priorityQueue.Dequeue();
        }

        private void GenerateCodes(Node node, string currentCode)
        {
            if (node == null)
                return;

            if (node.Left == null && node.Right == null)
            {
                codes[node.Symbol] = currentCode;
                reverseMapping[currentCode] = node.Symbol;
            }

            GenerateCodes(node.Left, currentCode + "0");
            GenerateCodes(node.Right, currentCode + "1");
        }

        public Bitmap DecodeImage(string encodedText, int width, int height)
        {
            List<byte> decodedBytes = new List<byte>();
            string currentCode = "";

            foreach (char bit in encodedText)
            {
                currentCode += bit;
                if (reverseMapping.ContainsKey(currentCode))
                {
                    decodedBytes.Add(reverseMapping[currentCode]);
                    currentCode = "";
                }
            }

            Bitmap img = new Bitmap(width, height);
            int byteIndex = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (byteIndex + 2 < decodedBytes.Count)
                    {
                        byte r = decodedBytes[byteIndex++];
                        byte g = decodedBytes[byteIndex++];
                        byte b = decodedBytes[byteIndex++];
                        img.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }

            return img;
        }
    }
}
