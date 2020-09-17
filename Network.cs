using System.Drawing;

namespace perceptron
{
    internal static class Network
    {
        internal static Neuron[] neurons { get; set; } = { new Neuron("Крестик"), new Neuron("Кружок"), new Neuron("Треугольник") }; // выходные нейроны
        internal static int[] input { get; set; } = new int[100]; // нармализованный входной сигнал (входные нейроны 100 штук выдают либо 0 либо 1)
        internal static float speed { get; } = 0.2f; // скорость обучения, чем меньше темб дольше обучается нейрон (без ней обучение чрезвучайно грубое)
        internal static int curNeuron { get; set; } = 0; // текущий нейрон/активированный нейрон

        internal static string SetInput(Image image) // задать вход и превести его к нужному виду
        {
            Bitmap bitmap = new Bitmap(image);
            string returning = ""; // для отображения в текст боксе
            int x = 0, y = 0, xMax = 0, yMax = 0; // края

            for (int i = 0; i < bitmap.Width; i++) // обрезка 
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (x == 0 && bitmap.GetPixel(i, j).ToArgb() < -1) x = i; // -1 - это белый
                    if (bitmap.GetPixel(i, j).ToArgb() < -1) xMax = i;
                }

            for (int i = 0; i < bitmap.Height; i++)
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (y == 0 && bitmap.GetPixel(j, i).ToArgb() < -1) y = i;
                    if (bitmap.GetPixel(j, i).ToArgb() < -1) yMax = i;
                }

            bitmap = bitmap.Clone(new Rectangle(x, y, xMax - x, yMax - y), bitmap.PixelFormat);
            bitmap = new Bitmap(bitmap, 10, 10); // уменьшение
            int count = 0;

            for (int i = 0; i < bitmap.Width; i++) // задаём масивы инпут и ретёрнинг
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (bitmap.GetPixel(j, i).ToArgb() < -1)
                    {
                        input[count] = 1;
                        returning += "1\t";
                    }
                    else
                    {
                        input[count] = 0;
                        returning += "0\t";
                    }
                    count++;
                }
                returning += "\n\r";
                returning += "\n\r";
            }

            return returning;
        }
        internal static string GetReactedNeuron() // поиски активированного нейрона
        {
            float max = 0, tmp;

            for (int i = 0; i < neurons.Length; i++)
            {
                tmp = neurons[i].GetOutput();
                if (tmp > max)
                {
                    max = tmp;
                    curNeuron = i;
                }
            } 
            return neurons[curNeuron].GetName();
        }
    }

    internal class Neuron // нейрон
    {
        private string name; // имя
        private float[] weight = new float[100]; // веса

        internal Neuron(string name)
        {
            this.name = name;
        }

        internal string GetName()
        {
            return name;
        }

        internal string GetWeights() // для отображения в гуи
        {
            string returning = "";
            int count = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    returning += weight[count] + "\t";
                    count++;
                }
                returning += "\n\r";
                returning += "\n\r";
            }

            return returning;
        }

        internal float GetOutput() // реакция нейрона на инпут
        {
            float output = 0;
            
            for (int i=0; i < 100; i++)
            {
                output += Network.input[i] * weight[i];
            }

            return output;
        }

        internal void Teach() // обучение нейрона
        {
            for (int i = 0; i < 100; i++)
            {
                weight[i] += Network.speed * Network.input[i] * (1 - weight[i]);
            }
        }
    }
}
