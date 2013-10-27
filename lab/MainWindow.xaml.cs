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
        const int edgeY = 30;           //map height in cells
        
        public MainWindow()
        {
            InitializeComponent();
            int[,] rectangle = new int[edgeX, edgeY];
            rectangle = createLabirynt();
            
        }

        private void drawMap (int[,] rectangle)
        {
            List<Rectangle> _rectangles = new List<Rectangle>();
            Canvas cPan = new Canvas();

            Rectangle rectNull = new Rectangle();
            Rectangle rectPassage = new Rectangle();
            Rectangle rectWall = new Rectangle();
            
            rectNull.Width = cellPx;
            rectNull.Height = cellPx;
            rectNull.StrokeThickness = 1;
            rectNull.Stroke = Brushes.White;
            rectNull.Fill = Brushes.Yellow;

            rectPassage.Width = cellPx;
            rectPassage.Height = cellPx;
            rectPassage.StrokeThickness = 1;
            rectPassage.Stroke = Brushes.White;
            rectPassage.Fill = Brushes.Green;

            rectWall.Width = cellPx;
            rectWall.Height = cellPx;
            rectWall.StrokeThickness = 1;
            rectWall.Stroke = Brushes.White;
            rectWall.Fill = Brushes.Red;

            
            
            //cPan.Margin = new Thickness(0);
            //for (int i = 0; i < edgeX; i++)
            //{
            //    for (int j = 0; i < edgeY; j++)
            //    {
            //        if (rectangle[i, j] == 0)
            //        {
            //            rectNull.Margin = new Thickness(i * cellPx, j * cellPx, 0, 0);
            //            cPan.Children.Add(rectNull);

            //        }
            //        else if (rectangle[i, j] == 1)
            //        {
            //            rectPassage.Margin = new Thickness(i * cellPx, j * cellPx, 0, 0);
            //            cPan.Children.Add(rectPassage);

            //        }
            //        else
            //        {
            //            rectWall.Margin = new Thickness(i * cellPx, j * cellPx, 0, 0);
            //            cPan.Children.Add(rectWall);

            //        }
            //    }
            //}

            //rectNull.Margin = new Thickness(0, 0, 0, 0); //pierwsza pozycja pustego kwadratu (kolor żółty)
            //cPan.Children.Add(rectNull);

            //rectNull.Margin = new Thickness(10, 0, 0, 0); //druga pozycja pustego kwadratu - próba ustawinie go obok pierwszego
            //cPan.Children.Add(rectNull); //w tym miejscu pojawia się błąd

                        
            this.Content = cPan;
        }
        
        private int [,] createLabirynt ()
        {            
            int[,] rectangle = new int[edgeX, edgeY]; //0 - null, can be changed on value 1 or 2; 1 - passage, can't be changed; 2 - wall, can't be changed;

            Random rand = new Random();
            int enterEdge;      // map edge, on which entrance will be placed;   
            Pen blackPen = new Pen(Brushes.Black, 1);
            
            int [] tEnter = new int [2];
            int [] tExit = new int [2];

            enterEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(enterEdge == 1)
            {
                tEnter[0] = rand.Next(1, edgeX - 2);
                tEnter[1] = 0;
            }
            else if(enterEdge == 2)
            {
                tEnter[0] = edgeX -1;
                tEnter[1] = rand.Next(1, edgeY - 2);
            }
            else if(enterEdge == 3)
            {
                tEnter[0] = rand.Next(1, edgeX - 2);
                tEnter[1] = edgeY - 1;
            }
            else 
            {
                tEnter[0] = 0;
                tEnter[1] = rand.Next(1, edgeY - 2);
            }
            drawMap(rectangle);
            return rectangle;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
               
        }
    }
}
