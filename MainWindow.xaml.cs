using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Tetris_csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<List<int>> field_;

        public MainWindow()
        {
            InitializeComponent();
            DrawGrid();
            StartGame();
        }

        void StartGame()
        {
            field_ = new List<List<int>>();
            for (int x = 0; x < Constants.ROWS; ++x)
            {
                field_.Add(new List<int>());
                for (int y = 0; y < Constants.COLUMNS; ++y)
                {
                    field_[x].Add(0);
                }
            }
        }

        void DrawGrid()
        {
            SolidColorBrush black = new SolidColorBrush(Colors.DarkGray);

            for (int x = 0; x < Constants.ROWS; ++x)
            {
                for (int y = 0; y < Constants.COLUMNS; ++y)
                {
                    Rectangle r = new Rectangle();
                    r.Stroke = black;
                    r.StrokeThickness = 1;
                    r.Width = Constants.SQUARE_SIDE;
                    r.Height = Constants.SQUARE_SIDE;
                    Canvas.SetLeft(r, y * Constants.SQUARE_SIDE);
                    Canvas.SetTop(r, x * Constants.SQUARE_SIDE);

                    mainCanvas.Children.Add(r);
                }
            }
        }
    }
}
