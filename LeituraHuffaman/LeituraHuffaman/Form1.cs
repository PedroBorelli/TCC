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

            try
            {
                HuffmanTree huffmanTree = new HuffmanTree();
                string encodedText = huffmanTree.EncodeImage(img);

                // Exibe o texto codificado (só para demonstração, normalmente não faria isso)
                MessageBox.Show("Texto codificado: " + encodedText.Substring(0, 100) + "...");

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
            }
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
