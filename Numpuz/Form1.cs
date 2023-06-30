using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Numpuz
{
    public partial class Form1 : Form
    {

        private Bitmap offScreenBitmap1;
        private Graphics offScreenGraphics;
        private int centralHorizontalLine;
        private int centralVerticalLine;
        private int clicks = 0;
        int brushSize = 1;
        int selectedSquareSize = 5;
        int squareSize = 150;
        Color gridColor = Color.White;
        Color selectedSquareColor = Color.White;
        Color emptySquareColor = Color.Green;
        Color occupedSquareColor = Color.Red;
        string selectedItem = string.Empty;
        //string selectedItem_ = string.Empty;
        Point selectedPoint;
        List<string> itensList = new List<string>();
        List<Point> pointsList = new List<Point>();
        List<Color> colorList = new List<Color>();
        List<int[]> indexAddress = new List<int[]>();
        string[,] items;
        Point[,] points;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            centralHorizontalLine = (this.Width / 2);
            centralVerticalLine = (this.Height / 2);
            //
            points = new Point[4, 3] {
                { new Point(centralHorizontalLine - 225, centralVerticalLine - 300), new Point(centralHorizontalLine - 75, centralVerticalLine - 300), new Point(centralHorizontalLine + 75, centralVerticalLine - 300) },
                { new Point(centralHorizontalLine - 225, centralVerticalLine - 150), new Point(centralHorizontalLine - 75, centralVerticalLine - 150), new Point(centralHorizontalLine + 75, centralVerticalLine - 150) },
                { new Point(centralHorizontalLine - 225, centralVerticalLine), new Point(centralHorizontalLine - 75, centralVerticalLine), new Point(centralHorizontalLine + 75, centralVerticalLine) },
                { new Point(centralHorizontalLine - 225, centralVerticalLine + 150), new Point(centralHorizontalLine - 75, centralVerticalLine + 150), new Point(centralHorizontalLine + 75, centralVerticalLine + 150) }
            };
            List<string> randomNumbers = new List<string>();
            while (randomNumbers.Count <= 9)
            {
                var random = new Random();
                int number = random.Next(0, 10);
                if (!randomNumbers.Contains(number.ToString()))
                {
                    randomNumbers.Add(number.ToString());
                }
            }
            //
            items = new string[4, 3] {
                { randomNumbers[0], randomNumbers[1], randomNumbers[2] },
                { randomNumbers[3], randomNumbers[4], randomNumbers[5] },
                { randomNumbers[6], randomNumbers[7], randomNumbers[8] },
                { "*", randomNumbers[9],"" }
            };
            //
            offScreenBitmap1 = new Bitmap(this.Width, this.Height);
            offScreenGraphics = Graphics.FromImage(offScreenBitmap1);
            //
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine);//X
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine + 75);//X|II
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine + 225);//X|III
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine - 75);//X|IV
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine - 225);//X|V
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine - 300);//X|VI
            //DrawHorizontalLine(Color.Red, 0, this.Width, centralVerticalLine + 300);//X|VII

            //DrawVerticalLine(Color.Red, centralHorizontalLine, 0, this.Height);//Y
            //DrawVerticalLine(Color.Red, centralHorizontalLine - 150, 0, this.Height);//Y|II
            //DrawVerticalLine(Color.Red, centralHorizontalLine + 150, 0, this.Height);//Y|III
            //DrawVerticalLine(Color.Red, centralHorizontalLine - 225, 0, this.Height);//Y|IV
            //DrawVerticalLine(Color.Red, centralHorizontalLine + 225, 0, this.Height);//Y|V
            //
            DrawGrid();
            DrawNumbers();
            UpdateImage();
        }

        private void UpdateImage()
        {
            using (var graphics = this.CreateGraphics())
            {
                graphics.DrawImage(offScreenBitmap1, 0, 0);
            }
        }

        private void DrawGrid()
        {
            DrawRectangle(gridColor, centralHorizontalLine - 225, centralVerticalLine - 300, 450, 600, brushSize);
            DrawRectangle(gridColor, centralHorizontalLine - 75, centralVerticalLine - 300, 150, 600, brushSize);
            DrawRectangle(gridColor, centralHorizontalLine - 225, (centralVerticalLine - 75) - 225, 450, 150, brushSize);//01
            DrawRectangle(gridColor, centralHorizontalLine - 225, (centralVerticalLine - 75) - 75, 450, 150, brushSize);//02
            DrawRectangle(gridColor, centralHorizontalLine - 225, (centralVerticalLine - 75) + 75, 450, 150, brushSize);//03
        }

        private void DrawNumbers()
        {
            DrawString(centralHorizontalLine - 203, centralVerticalLine - 288, items[0, 0]);
            DrawString(centralHorizontalLine - 53, centralVerticalLine - 288, items[0, 1]);
            DrawString(centralHorizontalLine + 97, centralVerticalLine - 288, items[0, 2]);
            DrawString(centralHorizontalLine - 203, centralVerticalLine - 138, items[1, 0]);
            DrawString(centralHorizontalLine - 53, centralVerticalLine - 138, items[1, 1]);
            DrawString(centralHorizontalLine + 97, centralVerticalLine - 138, items[1, 2]);
            DrawString(centralHorizontalLine - 203, centralVerticalLine + 12, items[2, 0]);
            DrawString(centralHorizontalLine - 53, centralVerticalLine + 12, items[2, 1]);
            DrawString(centralHorizontalLine + 97, centralVerticalLine + 12, items[2, 2]);
            DrawString(centralHorizontalLine - 203, centralVerticalLine + 162, items[3, 0]);
            DrawString(centralHorizontalLine - 53, centralVerticalLine + 162, items[3, 1]);
            DrawString(centralHorizontalLine + 97, centralVerticalLine + 162, items[3, 2]);
        }

        private void DrawHorizontalLine(Color color, int x1, int x2, int y, int size = 1)
        {
            offScreenGraphics.DrawLine(new Pen(new SolidBrush(color), size), x1, y, x2, y);
        }

        private void DrawVerticalLine(Color color, int x, int y1, int y2, int size = 1)
        {
            offScreenGraphics.DrawLine(new Pen(new SolidBrush(color), size), x, y1, x, y2);
        }

        private void DrawRectangle(Color color, int x, int y, int width, int heigth, int brushSize = 1)
        {
            Rectangle rectangle = new Rectangle(x, y, width, heigth);
            offScreenGraphics.DrawRectangle(new Pen(new SolidBrush(color), brushSize), rectangle);
        }

        private void DrawRectangle(Color color, int x, int y, int brushSize = 1)
        {
            Rectangle rectangle = new Rectangle(x, y, squareSize, squareSize);
            offScreenGraphics.DrawRectangle(new Pen(new SolidBrush(color), brushSize), rectangle);
        }

        private void DrawString(int x, int y, string text)
        {
            Font font = new Font("Helvetica Narrow", 88, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);
            offScreenGraphics.DrawString(text, font, brush, x, y);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            offScreenGraphics.Clear(Color.Black);
            DrawGrid();
            Point point = new Point(e.X, e.Y);
            if (point.X >= centralHorizontalLine - 225 &&
                point.X <= centralHorizontalLine + 225 &&
                point.Y >= centralVerticalLine - 300 &&
                point.Y <= centralVerticalLine + 300)
            {
                if (point.Y >= centralVerticalLine - 300 && point.Y <= centralVerticalLine - 150)
                {
                    if (point.X >= centralHorizontalLine - 225 && point.X <= centralHorizontalLine - 75)
                    {
                        if (items[0, 0].Equals(string.Empty) || items[0, 1].Equals(string.Empty) || items[1, 0].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[0, 0].X, points[0, 0].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[0, 0].X, points[0, 0].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[0, 0];
                        selectedItem = items[0, 0];
                        indexAddress.Add(new int[2] { 0, 0 });
                    }
                    if (point.X >= centralHorizontalLine - 75 && point.X <= centralHorizontalLine + 75)
                    {
                        if (items[0, 1].Equals(string.Empty) || items[0, 0].Equals(string.Empty) || items[0, 2].Equals(string.Empty) || items[1, 1].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[0, 1].X, points[0, 1].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[0, 1].X, points[0, 1].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[0, 1];
                        selectedItem = items[0, 1];
                        indexAddress.Add(new int[2] { 0, 1 });
                    }
                    if (point.X >= centralHorizontalLine + 75 && point.X <= centralHorizontalLine + 225)
                    {
                        if (items[0, 2].Equals(string.Empty) || items[0, 1].Equals(string.Empty) || items[1, 2].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[0, 2].X, points[0, 2].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[0, 2].X, points[0, 2].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[0, 2];
                        selectedItem = items[0, 2];
                        indexAddress.Add(new int[2] { 0, 2 });
                    }
                }
                if (point.Y >= centralVerticalLine - 150 && point.Y <= centralVerticalLine)
                {
                    if (point.X >= centralHorizontalLine - 225 && point.X <= centralHorizontalLine - 75)
                    {
                        if (items[1, 0].Equals(string.Empty) || items[0, 0].Equals(string.Empty) || items[1, 1].Equals(string.Empty) || items[2, 0].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[1, 0].X, points[1, 0].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[1, 0].X, points[1, 0].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[1, 0];
                        selectedItem = items[1, 0];
                        indexAddress.Add(new int[2] { 1, 0 });
                    }
                    if (point.X >= centralHorizontalLine - 75 && point.X <= centralHorizontalLine + 75)
                    {
                        if (items[1, 1].Equals(string.Empty) || items[1, 0].Equals(string.Empty) || items[0, 1].Equals(string.Empty) || items[1, 2].Equals(string.Empty) || items[2, 1].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[1, 1].X, points[1, 1].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[1, 1].X, points[1, 1].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[1, 1];
                        selectedItem = items[1, 1];
                        indexAddress.Add(new int[2] { 1, 1 });
                    }
                    if (point.X >= centralHorizontalLine + 75 && point.X <= centralHorizontalLine + 225)
                    {
                        if (items[1, 2].Equals(string.Empty) || items[0, 2].Equals(string.Empty) || items[1, 1].Equals(string.Empty) || items[2, 2].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[1, 2].X, points[1, 2].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[1, 2].X, points[1, 2].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[1, 2];
                        selectedItem = items[1, 2];
                        indexAddress.Add(new int[2] { 1, 2 });
                    }
                }
                if (point.Y >= centralVerticalLine && point.Y <= centralVerticalLine + 150)
                {
                    if (point.X >= centralHorizontalLine - 225 && point.X <= centralHorizontalLine - 75)
                    {
                        if (items[2, 0].Equals(string.Empty) || items[1, 0].Equals(string.Empty) || items[2, 1].Equals(string.Empty) || items[3, 0].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[2, 0].X, points[2, 0].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[2, 0].X, points[2, 0].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[2, 0];
                        selectedItem = items[2, 0];
                        indexAddress.Add(new int[2] { 2, 0 });
                    }
                    if (point.X >= centralHorizontalLine - 75 && point.X <= centralHorizontalLine + 75)
                    {
                        if (items[2, 1].Equals(string.Empty) || items[2, 0].Equals(string.Empty) || items[2, 2].Equals(string.Empty) || items[3, 1].Equals(string.Empty) || items[1, 1].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[2, 1].X, points[2, 1].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[2, 1].X, points[2, 1].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[2, 1];
                        selectedItem = items[2, 1];
                        indexAddress.Add(new int[2] { 2, 1 });
                    }
                    if (point.X >= centralHorizontalLine + 75 && point.X <= centralHorizontalLine + 225)
                    {
                        if (items[2, 2].Equals(string.Empty) || items[3, 2].Equals(string.Empty) || items[2, 1].Equals(string.Empty) || items[1, 2].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[2, 2].X, points[2, 2].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[2, 2].X, points[2, 2].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[2, 2];
                        selectedItem = items[2, 2];
                        indexAddress.Add(new int[2] { 2, 2 });
                    }
                }
                if (point.Y >= centralVerticalLine + 150 && point.Y <= centralVerticalLine + 300)
                {
                    if (point.X >= centralHorizontalLine - 225 && point.X <= centralHorizontalLine - 75)
                    {
                        if (items[3, 0].Equals(string.Empty) || items[2, 0].Equals(string.Empty) || items[3, 1].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[3, 0].X, points[3, 0].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[3, 0].X, points[3, 0].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[3, 0];
                        selectedItem = items[3, 0];
                        indexAddress.Add(new int[2] { 3, 0 });
                    }
                    if (point.X >= centralHorizontalLine - 75 && point.X <= centralHorizontalLine + 75)
                    {
                        if (items[3, 1].Equals(string.Empty) || items[3, 0].Equals(string.Empty) || items[3, 2].Equals(string.Empty) || items[2, 1].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[3, 1].X, points[3, 1].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[3, 1].X, points[3, 1].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[3, 1];
                        selectedItem = items[3, 1];
                        indexAddress.Add(new int[2] { 3, 1 });
                    }
                    if (point.X >= centralHorizontalLine + 75 && point.X <= centralHorizontalLine + 225)
                    {
                        if (items[3, 2].Equals(string.Empty) || items[3, 1].Equals(string.Empty) || items[2, 2].Equals(string.Empty))
                        {
                            DrawRectangle(emptySquareColor, points[3, 2].X, points[3, 2].Y, selectedSquareSize);
                            colorList.Add(emptySquareColor);
                        }
                        else
                        {
                            DrawRectangle(occupedSquareColor, points[3, 2].X, points[3, 2].Y, selectedSquareSize);
                            colorList.Add(occupedSquareColor);
                        }
                        selectedPoint = points[3, 2];
                        selectedItem = items[3, 2];
                        indexAddress.Add(new int[2] { 3, 2 });
                    }
                }
                //
                itensList.Add(selectedItem);
                //
                if (itensList.Count == 2)
                {
                    if (!colorList.Contains(occupedSquareColor))
                    {
                        if (!itensList[0].Equals(string.Empty) && itensList[1].Equals(string.Empty))
                        {
                            items[indexAddress[1][0], indexAddress[1][1]] = itensList[0];
                            items[indexAddress[0][0], indexAddress[0][1]] = string.Empty;
                        }
                    }
                    itensList.Clear();
                    indexAddress.Clear();
                    colorList.Clear();
                }
                //
                DrawNumbers();
                UpdateImage();
            }
            else
            {
                //this.Text = $"FORA";
                DrawGrid();
                DrawNumbers();
                //
                UpdateImage();
            }
        }
    }
}
