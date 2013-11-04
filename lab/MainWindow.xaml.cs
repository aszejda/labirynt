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

            //      X/Y x0  x1  x2  ... ... ... x(edgeX-1)
            //      y0  ?   ?   ?  
            //      y1  ?   ?   ? 
            //      y2  ?   ?   ? 
            //      ...
            //      ...
            //      ...
            //      y(edgeY-1)

            Random rand = new Random();
            int enterEdge, exitEdge;      // edge map, on which entrance or exit will be placed;   


            int[] tEnter = new int[2];                          //entrance coordinates;  [0] - axis x coordinate, [1] - axis y coordinate 
            int[] tExit = new int[2];                           //exit coordinates  [0] - axis x coordinate, [1] - axis y coordinate 
            int[] actualPosition = new int[2];
            int[,] potentialPath = new int[2, edgeX * edgeY];   //table of coordinates leading to exit

            #region Entrance lottery
            //lottery entrance
            enterEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(enterEdge == 1)   
            {
                potentialPath[0, 0] = actualPosition[0] = tEnter[0] = rand.Next(1, edgeX - 1);      //drawing cell number on X axis; why -1? example: edgeX = 30 so table starts from 0 to 29. 29 is on top edge in right corner, so normaly it should be -2 but function Random.Next() returns a random number within a specified range(without last number).
                potentialPath[0, 1] = actualPosition[1] = tEnter[1] = 0;                            //top edge is on the X axis so y need to have value = 0
                rectangle[tEnter[0], tEnter[1]] = 1;                            //registration enterance; 1 - passage
            }
            else if(enterEdge == 2)
            {
                potentialPath[0, 0] = actualPosition[0] = tEnter[0] = edgeX - 1;
                potentialPath[0, 1] = actualPosition[1] = tEnter[1] = rand.Next(1, edgeY - 1);
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(enterEdge == 3)
            {
                potentialPath[0, 0] = actualPosition[0] = tEnter[0] = rand.Next(1, edgeX - 1);
                potentialPath[0, 1] = actualPosition[1] = tEnter[1] = edgeY - 1;
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            else 
            {
                potentialPath[0, 0] = actualPosition[0] = tEnter[0] = 0;
                potentialPath[0, 1] = actualPosition[1] = tEnter[1] = rand.Next(1, edgeY - 1);
                rectangle[tEnter[0], tEnter[1]] = 1;
            }
            #endregion
            
            #region Exit lottery
                        
            do            //lottery exit - can't be the same as enter
            {
                exitEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            } while (exitEdge == enterEdge);
                                   
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

            #endregion

            for (int i = 0; i < edgeX; i++)                     //transformation all border cells in to value 2 (wall), skipping entrance and exitt
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
            rectangle[tExit[0], tExit[1]] = 0;          //transformation exit from 1(passage) to 0(null), triggers end of path creation
            int[] cord = new int[2];

            actualPosition = NewPosition(rectangle, potentialPath, actualPosition);     //finding next null position
            rectangle[actualPosition[0],actualPosition[1]] = 1;                         //making a passage
            //potentialPath[]
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
            return 0;
        }

        private int[] NewPosition(int[,] rectangle, int[,] potentialPath, int[] actualPosition )
        {
            int[] newPosition = new int[2];             // new Pass position on map
            int[] potentialPosition = new int[2];       
            int[] isItNull = new int[5];                //[0]
            
            int potentialDirection;
            Random r = new Random();
            do
            {
                if (isItNull[1] != 0 & isItNull[2] != 0 & isItNull[3] != 0 & isItNull[4] != 0)
                {

                }
                potentialDirection = r.Next(1, 5);
                if(isItNull[potentialDirection] == 0)
                {
                    switch (potentialDirection)
                    {
                        case 1:                            
                            potentialPosition[0] = actualPosition[0];
                            potentialPosition[1] = actualPosition[1]--;
                            if(IsRectNull(potentialPosition, rectangle))
                            {
                                newPosition = potentialPosition;
                                isItNull[potentialDirection] = 1;
                            }
                            isItNull[potentialDirection] = 2;
                            break;

                        case 2:
                            break;

                        case 3:
                            break;

                        case 4:
                            break;

                        default:
                            break;
                    }
                }
            } while (isItNull[potentialDirection] != 1 );
            

            return newPosition;
        }

    }

}
