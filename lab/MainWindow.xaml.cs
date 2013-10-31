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
        const int edgeX = 10;           //map width in celss
        const int edgeY = 10;           //map height in cells
        int[,] rectangle = new int[edgeX, edgeY];
        

        public MainWindow()
        {
            InitializeComponent();
            
            rectangle = CreateLabirynt();
            
        }

        
        private int [,] CreateLabirynt ()
        {            
            int[,] rectangle = new int[edgeX, edgeY]; //0 - null, can be changed on value 1 or 2; 1 - passage, can't be changed; 2 - wall, can't be changed;

            Random rand = new Random();
            int enterEdge, exitEdge;      // map edge, on which entrance or exit will be placed;   
            
            
            int [] tEnter = new int [2];        //entrance coordinates 
            int [] tExit = new int [2];         //exit coordinates

            
            
            //lottery entrance
            enterEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(enterEdge == 1)
            {
                tEnter[0] = rand.Next(1, edgeX - 2);
                tEnter[1] = 0;
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(enterEdge == 2)
            {
                tEnter[0] = edgeX -1;
                tEnter[1] = rand.Next(1, edgeY - 2);
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(enterEdge == 3)
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

            //lottery exit - can't be the same as enter
            do
            {
                exitEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            } while (exitEdge == enterEdge);
            
            
            //do
            //{
            if (exitEdge == 1)
            {
                    
                tExit[0] = rand.Next(1, edgeX - 2);
                tExit[1] = 0;
                rectangle[tExit[0], tExit[1]] = 1;
            }
            else if (exitEdge == 2)
            {
                tExit[0] = edgeX - 1;
                tExit[1] = rand.Next(1, edgeY - 2);
                rectangle[tExit[0], tExit[1]] = 1;
            }
            else if (exitEdge == 3)
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

            //} while (enterExit(tEnter, tExit)); //checking, whether the exit will not have coordinates, same as entrance

            for (int i = 0; i < edgeX; i++) //
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
            int[] cord = new int[2];
            
            
            DrawMap(rectangle);
            return rectangle;
        }

        private Visual RectNull (int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x,y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.Yellow, null, rect);
            }
            return drV;
        }

        private Visual RectPassage(int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x, y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.White, null, rect);
            }
            return drV;
        }

        private Visual RectWall(int x, int y)
        {
            DrawingVisual drV = new DrawingVisual();

            using (DrawingContext drC = drV.RenderOpen())
            {
                Rect rect = new Rect(new Point(x, y), new Size(cellPx, cellPx));
                drC.DrawRectangle(Brushes.Red, null, rect);
            }
            return drV;
        }


        private void DrawMap(int[,] rectangle)              //drawing labirynt
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap(500, 500, 100, 100, PixelFormats.Pbgra32); //create bitmap (height, width, dpi, dpi, format) 


            for (int i = 0; i < edgeX; i++)
            {
                for (int j = 0; j < edgeY; j++)     
                {
                    if (rectangle[i, j] == 0)                                   //if = 0 then draw empty cell
                    {
                        bmp.Render(RectNull(i * cellPx, j * cellPx));
                    }
                    else if (rectangle[i, j] == 1)                              //if = 0 then draw passage cell
                    {
                        bmp.Render(RectPassage(i * cellPx, j * cellPx));
                    }
                    else                                                        //if no one above, then draw wall cell
                    {
                        bmp.Render(RectWall(i * cellPx, j * cellPx));
                    }
                }
            }

            myImage.Source = bmp;       //display bit map with labirynt on the Image control
        }

        //private bool enterExit(int[] enter, int[] exit)     //checking, whether the exit will not have coordinates, same as entrance
        //{
        //    if ((exit[0] == 0 & enter[0] == 0) || (exit[0] == edgeX - 1 & enter[0] == edgeX - 1))       // if exit and entrance are on the same Y edge 
        //    {
        //        if (exit[1] == enter[1] || exit[1] - 1 == enter[1] || exit[1] + 1 == enter[1])          // if exit is near or on the same coordinate as enterance
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else if ((exit[1] == 0 & enter[1] == 0) || (exit[1] == edgeY - 1 & enter[1] == edgeY - 1))  // if exit and entrance are on the same X edge   
        //    {
        //        if (exit[0] == enter[0] || exit[0] - 1 == enter[0] || exit[0] + 1 == enter[0])          // if exit is near or on the same coordinate as enterance
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else return false;            
        //}

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            rectangle = CreateLabirynt();
        }

        private bool IsRectNull(int[] potentialCoords, int[,] rectangle)      
        {

            try 
            {
                if (rectangle[potentialCoords[0], potentialCoords[1]] == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
            
        }

        private int Distance(int[] myPosition, int[] exit)
        {

        }
    }
}
