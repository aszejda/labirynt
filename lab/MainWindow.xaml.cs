﻿using System;
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
        const int edgeX = 40;           //map width in celss
        const int edgeY = 40;           //map height in cells
        int[,] tRrectangle = new int[edgeX, edgeY];
        

        public MainWindow()
        {
            InitializeComponent();
            
            tRrectangle = CreateLabirynt();
            
        }

        
        private int [,] CreateLabirynt ()
        {            
            int[,] tRectangle = new int[edgeX, edgeY]; //0 - null, can be changed on value 1 or 2; 1 - passage, can't be changed; 2 - wall, can't be changed;

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
            int itr = 0;

            int[] tEnter = new int[2];                          //entrance coordinates;  [0] - axis x coordinate, [1] - axis y coordinate 
            int[] tExit = new int[2];                           //exit coordinates  [0] - axis x coordinate, [1] - axis y coordinate 
            int[] tNewPosition = new int[2];
            int[] tActualPosition = new int[2];
            int[,] tPotentialPath = new int[edgeX * edgeY, 2];   //table of coordinates leading to exit

            #region Entrance lottery
            //lottery entrance
            enterEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(enterEdge == 1)   
            {
                tPotentialPath[0, 0] = tActualPosition[0] = tEnter[0] = rand.Next(1, edgeX - 1);        //drawing cell number on X axis; example: edgeX = 30 so table starts from 0 to 29. 29 is on top edge in right corner, so normaly it should be -2 but function Random.Next() returns a random number within a specified range(without last number).
                tPotentialPath[0, 1] = tActualPosition[1] = tEnter[1] = 0;                              //top edge is on the X axis so y need to have value = 0
                tRectangle[tEnter[0], tEnter[1]] = 1;                                                   //registration enterance; 1 - passage
            }
            else if(enterEdge == 2)
            {
                tPotentialPath[0, 0] = tActualPosition[0] = tEnter[0] = edgeX - 1;
                tPotentialPath[0, 1] = tActualPosition[1] = tEnter[1] = rand.Next(1, edgeY - 1);
                tRectangle[tEnter[0], tEnter[1]] = 1;
            }
            else if(enterEdge == 3)
            {
                tPotentialPath[0, 0] = tActualPosition[0] = tEnter[0] = rand.Next(1, edgeX - 1);
                tPotentialPath[0, 1] = tActualPosition[1] = tEnter[1] = edgeY - 1;
                tRectangle[tEnter[0], tEnter[1]] = 1;
            }
            else 
            {
                tPotentialPath[0, 0] = tActualPosition[0] = tEnter[0] = 0;
                tPotentialPath[0, 1] = tActualPosition[1] = tEnter[1] = rand.Next(1, edgeY - 1);
                tRectangle[tEnter[0], tEnter[1]] = 1;
            }
            #endregion
            
            #region Exit lottery
                        
            do            //lottery exit - can't be the same as enter
            {
                exitEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            } while (exitEdge == enterEdge);
            do
            {
                if (exitEdge == 1)
                {
                    tExit[0] = rand.Next(1, edgeX - 2);
                    tExit[1] = 0;
                }
                else if (exitEdge == 2)
                {
                    tExit[0] = edgeX - 1;
                    tExit[1] = rand.Next(1, edgeY - 2);
                }
                else if (exitEdge == 3)
                {
                    tExit[0] = rand.Next(1, edgeX - 2);
                    tExit[1] = edgeY - 1;
                }
                else
                {
                    tExit[0] = 0;
                    tExit[1] = rand.Next(1, edgeY - 2);                    
                }
            } while (Distance(tEnter, tExit) <= 3);

            tRectangle[tExit[0], tExit[1]] = 1;
                                  
            

            #endregion

            for (int i = 0; i < edgeX; i++)                     //transformation all border cells in to value 2 (wall), skipping entrance and exitt
            {
                
                for (int j = 0; j < edgeY; j++)
                {
                    if(j == 0 || j == edgeY - 1)
                    {
                        if(tRectangle[i, j] != 1)
                            tRectangle[i, j] = 2;
                    }

                    if (i == 0 || i == edgeX - 1)
                    {
                        if (tRectangle[i, j] != 1)
                            tRectangle[i, j] = 2;
                    }
                }
            }
            //tRectangle[tExit[0], tExit[1]] = 0;          //transformation exit from 1(passage) to 0(null), triggers end of path creation
            int[] cord = new int[2];
            itr++;
            Random rd = new Random();
            do
            {
                
                tNewPosition = NewPosition(tRectangle, tActualPosition, rd);     //finding next null position
                if (tNewPosition[0] == 0 & tNewPosition[1] == 0)
                {
                    itr--;
                    tActualPosition[0] = tPotentialPath[itr, 0];
                    tActualPosition[1] = tPotentialPath[itr, 1];
                }
                else
                {
                    
                    tPotentialPath[itr, 0] = tActualPosition[0] = tNewPosition[0];
                    tPotentialPath[itr, 1] = tActualPosition[1] = tNewPosition[1];
                    tRectangle[tActualPosition[0], tActualPosition[1]] = 1;
                    itr++;
                }
                
            } while (itr != 0);
            
            if(exitEdge == 1)
            {
                tRectangle[tExit[0], tExit[1] + 1] = 1;
            }
            else if(exitEdge == 2)
            {
                tRectangle[tExit[0] - 1, tExit[1]] = 1;
            }
            else if (exitEdge == 3)
            {
                tRectangle[tExit[0], tExit[1] - 1] = 1;
            }
            else if (exitEdge == 4)
            {
                tRectangle[tExit[0] + 1, tExit[1]] = 1;
            }
            else
            {
                // cos nie tak
            }

            for (int i = 0; i < edgeX; i++)
            {
                for (int j = 0; j < edgeY; j++)
                {
                    if (tRectangle[i, j] == 0)
                        tRectangle[i, j] = 2;
                }
            }

                //Distance(tActualPosition, tExit);
                DrawMap(tRectangle);
            return tRectangle;
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
                drC.DrawRectangle(Brushes.Black, null, rect);
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
            tRrectangle = CreateLabirynt();
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

        private int Distance(int[] actualPosition, int[] exit)
        {
            int distance = Math.Abs(actualPosition[0] - exit[0]) + Math.Abs(actualPosition[1] - exit[1]);
            Label1.Content = distance;
            return distance;
        }

        private int[] NewPosition(int[,] rectangle, int[] actualPosition, Random r)
        {
            int[] tPotentialPosition = new int[2];       
            int[] isItNull = new int[5];
            int potentialDirection;                     
            
            do
            {
                tPotentialPosition[0] = actualPosition[0];                      
                tPotentialPosition[1] = actualPosition[1];
                
                if (isItNull[1] != 0 & isItNull[2] != 0 & isItNull[3] != 0 & isItNull[4] != 0)  //jezeli wszystke pozycje (procz indeksowej [0]) nie sa zerowe czyli sa przejsciamy lub tez scianami
                {
                    isItNull[0] = 1;                                                            //to indeks ustawiamy na wartość 0 dzieki czemu przerwiemy petle
                }
                potentialDirection = r.Next(1, 5);                                              //losowanie potencjalnego kierunku gdzie: 1 - polnoc, 2 - wschod, 3 - poludnie, 4 - zachod
                if(isItNull[potentialDirection] == 0)                                           //sprawdzamy czy dany kierunek nie zostal juz wykorzystany, jesli nie - mozemy wykonac instrukcje
                {
                    switch (potentialDirection)
                    {
                        //POLNOC
                        case 1: 
                            --tPotentialPosition[1];                                            //zmniejszamy wartosc indeksu na osi Y o jeden czyli przesowamy sie o jeden do gory
                            if(IsRectNull(tPotentialPosition, rectangle))                       //sprawdzamy czy pole jest puste (ma wartosc 0) i bedziemy mogli na nim wstawic przejscie
                            {
                                if(IsNeighborsArePassage(rectangle, tPotentialPosition, actualPosition))    //sprawdzamy czy sasiednie pola nie sa przypadkiem przejsciem 
                                    isItNull[potentialDirection] = 2;                           //oznaczamy iz nie moze byc przejsciem i nalezy szukac dalej
                                else
                                    isItNull[potentialDirection] = 1;                           //oznaczamy iz pole moze byc przejsciem
                            }                                
                            else                                                                
                                isItNull[potentialDirection] = 2;                               //oznaczamy iz nie moze byc przejsciem i nalezy szukac dalej
                            break;
                        //WSCHOD
                        case 2:
                            ++tPotentialPosition[0];
                            if(IsRectNull(tPotentialPosition, rectangle))
                            {
                                if (IsNeighborsArePassage(rectangle, tPotentialPosition, actualPosition))
                                    isItNull[potentialDirection] = 2;
                                else
                                    isItNull[potentialDirection] = 1;
                            }  
                            else
                                isItNull[potentialDirection] = 2;
                            break;
                        //POLUDNIE
                        case 3:
                            ++tPotentialPosition[1];
                            if(IsRectNull(tPotentialPosition, rectangle))
                            {
                                if (IsNeighborsArePassage(rectangle, tPotentialPosition, actualPosition))
                                    isItNull[potentialDirection] = 2;
                                else
                                    isItNull[potentialDirection] = 1;
                            }  
                            else
                                isItNull[potentialDirection] = 2;
                            break;
                        //ZACHOD
                        case 4:
                            --tPotentialPosition[0];
                            if(IsRectNull(tPotentialPosition, rectangle))
                            {
                                if (IsNeighborsArePassage(rectangle, tPotentialPosition, actualPosition))
                                    isItNull[potentialDirection] = 2;
                                else
                                    isItNull[potentialDirection] = 1;
                            }  
                            else
                                isItNull[potentialDirection] = 2;
                            break;

                        default:
                            break;
                    }
                }
            } while (isItNull[potentialDirection] != 1 & isItNull[0] == 0);        //petla ma sie wykonywac do poki ktores z pol nie zostanie oznaczone jako przejscie lub tez indeks[0] jest rowny 0 - czyli nie wszystkie kierunki zostaly jeszcze sprawdzone
            
            if(isItNull[0] == 1)                //jezeli = 1 -> zadne z pol nie moze byc przejsciem
            {
                tPotentialPosition[0] = 0;      //zerujemy obie pozycje
                tPotentialPosition[1] = 0;      //aby cofnac sie do tylu i tam szukac kolejnego przejscia
            }

            return tPotentialPosition;
        }

        private bool IsNeighborsArePassage(int[,] rectangle, int[] potentialPosition, int[] actualPosition)
        {
            bool trueFalse;

            //North neighbor checking
            if (rectangle[potentialPosition[0], --potentialPosition[1]] == 1)
            {
                if (CompareArray(actualPosition, potentialPosition))
                    trueFalse = false;
                else
                    return true;                
            }
            else
                trueFalse = false;
            ++potentialPosition[1];


            //East neighbor checking
            if (rectangle[++potentialPosition[0], potentialPosition[1]] == 1)
            {
                if (CompareArray(actualPosition, potentialPosition))
                    trueFalse = false;
                else
                    return true;                
            }
            else
                trueFalse = false;
            --potentialPosition[0];

            //South neighbor checking
            if (rectangle[potentialPosition[0], ++potentialPosition[1]] == 1)
            {
                if (CompareArray(actualPosition, potentialPosition))
                    trueFalse = false;
                else
                    return true;                
            }
            else
                trueFalse = false;
            --potentialPosition[1];

            //West neighbor checking
            if (rectangle[--potentialPosition[0], potentialPosition[1]] == 1)
            {
                if (CompareArray(actualPosition, potentialPosition))
                    trueFalse = false;
                else
                    return true;                
            }
            else
                trueFalse = false;
            ++potentialPosition[0];

            return trueFalse;
        }

        private bool IsNeighborAreExit(int[,] rectangle, int[] actualPosition, int[] exit)
        {            
            int[] potentialPosition = new int[2];

            potentialPosition[0] = actualPosition[0];
            potentialPosition[1] = actualPosition[1];
            --potentialPosition[1];                     //North
            if (CompareArray(potentialPosition, exit))
                return true;

            potentialPosition[0] = actualPosition[0];
            potentialPosition[1] = actualPosition[1];
            ++potentialPosition[0];                     //East
            if (CompareArray(potentialPosition, exit))
                return true;

            potentialPosition[0] = actualPosition[0];
            potentialPosition[1] = actualPosition[1];
            ++potentialPosition[1];                     //South
            if (CompareArray(potentialPosition, exit))
                return true;

            potentialPosition[0] = actualPosition[0];
            potentialPosition[1] = actualPosition[1];
            --potentialPosition[0];                     //West
            if (CompareArray(potentialPosition, exit))
                return true;

            return false;
        }

        private bool CompareArray(int[] tA, int[] tB)
        {
            int j = 0;
            foreach(int i in tA)
            {
                if (tB[j] != i)
                    return false;
                j++;
            }
            return true;
        }
    }

}
