using System;
using System.Drawing;
using System.Windows.Forms;

namespace perceptron
{
    public partial class Form1 : Form
    {
        private bool isLeftMouseKeyDown = false;

        private Graphics graphics;
        private Color backcolor = Color.FromArgb(255, Color.White); // цвет фона
        private Pen pen; // карандаш

        private Point CurPix; // текущая координата 
        private Point OldPix; // захваченая координата

        public Form1()
        {
            InitializeComponent();
        }       

        private void Form1_Load(object sender, EventArgs e)
        {
            pen = new Pen(Color.Black, 5);
            pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);

            /* Создаём новый бмп */

            Bitmap bmp = new Bitmap(200, 200);

            /* Закрашиваем пиксели белым */
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++)
                    bmp.SetPixel(i, j, Color.White);

            pictureBox1.Image = bmp;
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.ScaleTransform(1, 1);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isLeftMouseKeyDown = true;
            OldPix.X = e.X;
            OldPix.Y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftMouseKeyDown && e.X > 0 && e.Y > 0 && e.X < pictureBox1.Width && e.Y < pictureBox1.Height)
            {
                CurPix.X = e.X;
                CurPix.Y = e.Y;
                graphics.DrawLine(pen, OldPix, CurPix);
                OldPix = CurPix;


                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isLeftMouseKeyDown)
                isLeftMouseKeyDown = false;
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            neuronListBox.SetSelected(Network.curNeuron, false);
            inputTextBox.Text = Network.SetInput(pictureBox1.Image);
            weightBox.Text = Network.neurons[Network.curNeuron].GetWeights();
            answerLabel.Text = "Вы нарисовали " + Network.GetReactedNeuron();
            weightBox.Text = Network.neurons[Network.curNeuron].GetWeights();
            neuronBox.Text = "NEURON: " + Network.curNeuron;
            neuronListBox.SetSelected(Network.curNeuron, true);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        private void neuronListBox_MouseCaptureChanged(object sender, EventArgs e)
        {
            Network.curNeuron = neuronListBox.SelectedIndex;
            weightBox.Text = Network.neurons[Network.curNeuron].GetWeights();
            neuronBox.Text = "NEURON: " + Network.curNeuron;
        }

        private void teachButton_Click(object sender, EventArgs e)
        {
            Network.neurons[Network.curNeuron].Teach();
            weightBox.Text = Network.neurons[Network.curNeuron].GetWeights();
        }
    }
}
