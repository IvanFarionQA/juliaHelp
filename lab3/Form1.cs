using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        List<int[]> study = new List<int[]>();
        List<int[]> recognition = new List<int[]>();

        private int[,] sumMatrix = new int[16, 16];
        private int[,] resultMatrix = new int[16, 1];

        public Form1()
        {
            InitializeComponent();
            for (int i = 1; i <= 32; i++)//Цей код додає обробник події Click до всіх кнопок на формі
            {
                Button button = this.Controls.Find("button" + i.ToString(), true).FirstOrDefault() as Button;

                if (button != null)
                {
                    button.Click += new EventHandler(ColorChange);
                }
            }
        }
        private void ColorChange(object sender, EventArgs e)// зміна кольору кнопок
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                if (clickedButton.BackColor != Color.Black)
                {
                    clickedButton.BackColor = Color.Black;
                }
                else
                {
                    clickedButton.BackColor = SystemColors.Control;
                }
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private int[] CalculateValuesVector()//обчислює вектор для навчання
        {
            int[] values = new int[16];

            for (int i = 1; i <= 16; i++)
            {
                Button button = this.Controls.Find("button" + i.ToString(), true).FirstOrDefault() as Button;
                if (button != null && button.BackColor == Color.Black)
                {
                    values[i - 1] = 1;
                } 
                else
                {
                    values[i - 1] = -1;
                }
            }

            return values;
        }
        private void button51_Click(object sender, EventArgs e)
        {
            int[] values = CalculateValuesVector();
            study.Add(values);

            ClearButtonTeach();
        }

        private int[,] CreateMatrix(int[] values)//створює матрицю з векторів значень вага матриць
        {
            int[,] matrix = new int[16, 16];
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    matrix[i, j] = values[i] * values[j];
                }
            }
            return matrix;
        }

        private void button52_Click(object sender, EventArgs e)//кнопка для навчання
        {
            for (int k = 0; k < study.Count; k++)
            {
                int[,] matrix = CreateMatrix(study[k]);
                for (int i = 0; i < 16; i++)
                {

                    for (int j = 0; j < 16; j++)
                    {
                        sumMatrix[i, j] += matrix[i, j];
                    }
                }
            }

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (i == j)
                    {
                        if (sumMatrix[i, j] == sumMatrix[j, i])
                        {
                            sumMatrix[i, j] = 0;
                        }
                    }
                }
            }
        }


        private void button53_Click(object sender, EventArgs e)
        {
            ClearButtonTeach();
        }

        private int[] CalculateVectorRecognition()// обчислює вектор значень для розпізнавання
        {
            int[] values = new int[16];

            for (int i = 17; i <= 32; i++)
            {
                Button button = this.Controls.Find("button" + i.ToString(), true).FirstOrDefault() as Button;
                if (button != null && button.BackColor == Color.Black)
                {
                    values[i - 17] = 1;
                }
                else
                {
                    values[i - 17] = -1;
                }
            }
            return values;
        }

        private void button55_Click(object sender, EventArgs e)// кнопка для розпізнавання
        {
            int[] values = CalculateVectorRecognition();
            recognition.Add(values);
            int[,] transposedValues = new int[16, 1];
            for (int i = 0; i < 16; i++)
            {
                transposedValues[i, 0] = values[i];
            }

            for (int i = 0; i < 16; i++)
            {
                int sum = 0;
                for (int j = 0; j < 16; j++)
                {
                    sum += sumMatrix[i, j] * transposedValues[j, 0];
                }
                resultMatrix[i, 0] = sum;
            }

            for (int i = 0; i < 16; i++)
            {
                if (resultMatrix[i, 0] < 0)
                {
                    resultMatrix[i, 0] = -1;
                }
                if (resultMatrix[i, 0] > 0)
                {
                    resultMatrix[i, 0] = 1;
                }
                else if (resultMatrix[i, 0] == 0)
                {
                    resultMatrix[i, 0] = 1;
                }
            }
        }

        private void button54_Click(object sender, EventArgs e)
        {
            ClearButtonRecognition();
        }

        private void button49_Click(object sender, EventArgs e)// кнопка для виведення розпізнаного образу
        {
            for (int i = 0; i < 16; i++)
            {
                int buttonNumber = i + 33;
                Button button = this.Controls.Find("button" + buttonNumber.ToString(), true).FirstOrDefault() as Button;
                if (button != null)
                {
                    if (resultMatrix[i, 0] == 1)
                    {
                        button.BackColor = Color.Black;
                    }
                    else if (resultMatrix[i, 0] == -1)
                    {
                        button.BackColor = SystemColors.Control;
                    }
                }
            }
        }

        private void ClearButtonTeach() // очищає колір кнопок з 1 по 16
        {
            for (int i = 1; i <= 16; i++)
            {
                Button button = this.Controls.Find("button" + i.ToString(), true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.BackColor = SystemColors.Control;
                }
            }
        }
        private void ClearButtonRecognition() // очищає колір кнопок з 17 по 32
        {
            for (int i = 17; i <= 32; i++)
            {
                Button button = this.Controls.Find("button" + i.ToString(), true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.BackColor = SystemColors.Control;
                }
            }
        }
    }
}
