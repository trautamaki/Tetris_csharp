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

namespace Tetris_csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            

            Rectangle Myline = new Rectangle();

            Myline.Stroke = new SolidColorBrush(Colors.Black);            Myline.StrokeThickness = 1;
            Canvas.SetLeft(Myline, 10);
            Canvas.SetTop(Myline, 100);
            Myline.Width = 50;
            Myline.Height = 50;


            mainCanvas.Children.Add(Myline);

        }
    }
}
