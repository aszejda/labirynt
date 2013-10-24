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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        const int cellPx = 10;          //cell size in pixels
        const int bokX = 30;            //map width in celss
        const int bokY = 30;
        
        public MainWindow()
        {
            InitializeComponent();
            int[,] rectangle = new int[bokX, bokY];
            rectangle = createLabirynt();
        }

        private void drawMap ()
        {
            
        }
        
        private int [,] createLabirynt ()
        {            
            int[,] rectangle = new int[bokX, bokY]; //0 - null, can be changed on value 1 or 2; 1 - passage, can't be changed; 2 - wall, can't be changed;

            Random rand = new Random();
            int enterEdge;      // map edge, on which entrance will be placed;               
            
            //int enterCell;       edge cell, which will be entrance or exit;             
            
            int [] tEnter = new int [2];
            int [] tExit = new int [2];

            enterEdge = rand.Next(1, 5); //lottery edge: 1 - top, 2 - right, 3 - down, 4 - left;
            if(enterEdge == 1)
            {
                tEnter[0] = rand.Next(1, bokX - 2);
                tEnter[1] = 0;
            }
            else if(enterEdge == 2)
            {
                tEnter[0] = bokX -1;
                tEnter[1] = rand.Next(1, bokY - 2);
            }
            else if(enterEdge == 3)
            {
                tEnter[0] = rand.Next(1, bokX - 2);
                tEnter[1] = bokY - 1;
            }
            else 
            {
                tEnter[0] = 0;
                tEnter[1] = rand.Next(1, bokY - 2);
            }
           

            return rectangle;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            

            


        }
    }
}
