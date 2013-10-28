using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace lab
{
    
    public partial class MainWindow : Window
    {
        const int cellPx = 10;          //cell size in pixels
        const int edgeX = 30;           //map width in celss
        const int edgeY = 40;           //map height in cells

        

        public MainWindow()
        {
            InitializeComponent();
            int[,] rectangle = new int[edgeX, edgeY];
            rectangle = createLabirynt();
            
        }

        
        private int [,] createLabirynt ()
        {            
            int[,] rectangle = new int[edgeX, edgeY]; //0 - null, can be changed on value 1 or 2; 1 - passage, can't be changed; 2 - wall, can't be changed;

            Random rand = new Random();
            int edge;      // map edge, on which entrance or exit will be placed;   
            
            
            int [] tEnter = new int [2];
            int [] tExit = new int [2];

            edge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(edge == 1)
            {
                tEnter[0] = rand.Next(1, edgeX - 2);
                tEnter[1] = 0;
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(edge == 2)
            {
                tEnter[0] = edgeX -1;
                tEnter[1] = rand.Next(1, edgeY - 2);
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(edge == 3)
            {
                tEnter[0] = rand.Next(1, edgeX - 2);
                tEnter[1] = edgeY - 1;
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else 
            {
                tEnter[0] = 0;
                tEnter[1] = rand.Next(1, edgeY - 2);
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            
            edge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            do
            {
                if (edge == 1)
                {
                    tExit[0] = rand.Next(1, edgeX - 2);
                    tExit[1] = 0;
                    rectangle[tExit[0], tExit[1]] = 1;
                }
                else if (edge == 2)
                {
                    tExit[0] = edgeX - 1;
                    tExit[1] = rand.Next(1, edgeY - 2);
                    rectangle[tExit[0], tExit[1]] = 1;
                }
                else if (edge == 3)
                {
                    tExit[0] = rand.Next(1, edgeX - 2);
                    tExit[1] = edgeY - 1;
                    rectangle[tExit[0], tExit[1]] = 1;
                }
                else
                {
                    tExit[0] = 0;
                    tExit[1] = rand.Next(1, edgeY - 2);
                    rectangle[tExit[0], tExit[1]] = 1;
                }
                
            } while (tEnter[0] == tExit[0] & tEnter[1] == tExit[1]);

            for (int i = 0; i < edgeX; i++)
            {
                
                for (int j = 0; j < edgeY; j++)
                {
                    if(j == 0 || j == edgeY - 1)
                    {
                        if(rectangle[i, j] != 1)
                            rectangle[i, j] = 2;
                    }

                    if (i == 0 || i == edgeX - 1)
                    {
                        if (rectangle[i, j] != 1)
                            rectangle[i, j] = 2;
                    }
                }
            }
                drawMap(rectangle);
            return rectangle;
        }

        private Visual rectNull (int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x,y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.Yellow, null, rect);
            }
            return drV;
        }

        private Visual rectPassage(int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x, y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.Green, null, rect);
            }
            return drV;
        }

        private Visual rectWall(int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x, y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.Red, null, rect);
            }
            return drV;
        }


        private void drawMap(int[,] rectangle)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(500, 500, 100, 100, PixelFormats.Pbgra32);


            for (int i = 0; i < edgeX; i++)
            {
                for (int j = 0; j < edgeY; j++)
                {
                    if (rectangle[i, j] == 0)
                    {
                        bmp.Render(rectNull(i * cellPx, j * cellPx));
                    }
                    else if (rectangle[i, j] == 1)
                    {
                        bmp.Render(rectPassage(i * cellPx, j * cellPx));
                    }
                    else
                    {
                        bmp.Render(rectWall(i * cellPx, j * cellPx));
                    }
                }
            }

            myImage.Source = bmp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
               
        }
    }
}
