using System.Diagnostics;
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
        bool DEBUG = true;
        struct tetromino_pos
        {
            public int x;
            public int y;
        };

        // Whole game field, initialized with size COLUMNS*ROWS
        // values:
        //  0: empty
        //  1: active tetromino
        //  2..9: finished tetromino
        List<List<int>> field_;

        // 4*4 that holds the information of each piece in the tetromino
        List<List<tetromino_pos>> position_;

        Tetromino current_;
        
        int current_shape_;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            DrawGrid();
            GameLoop();
        }

        void Init()
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

            position_ = new List<List<tetromino_pos>>();
            for (int x = 0; x < 4; ++x)
            {
                position_.Add(new List<tetromino_pos>());
                for (int y = 0; y < 4; ++y)
                {
                    position_[x].Add(new tetromino_pos { x = 0, y = 0 });
                }
            }
        }

        void GameLoop()
        {
            CreateBlock(0);
            Draw();
        }

        void CreateBlock(int tetromino)
        {
            Debug.WriteLine("Create a shape");
            int start_x = 4;
            int start_y = 0;

            current_shape_ = tetromino;
            Tetromino t = new Tetromino(current_shape_);

            switch (tetromino)
            {
                case (int) Constants.TETROMINO_KIND.HORIZONTAL:
                    start_x = 4;
                    start_y = 0;
                    break;
                case (int)Constants.TETROMINO_KIND.SQUARE:
                    start_x = 4;
                    start_y = 0;
                    break;
                case (int)Constants.TETROMINO_KIND.STEP_UP_RIGHT:
                case (int)Constants.TETROMINO_KIND.STEP_UP_LEFT:
                case (int)Constants.TETROMINO_KIND.LEFT_CORNER:
                case (int)Constants.TETROMINO_KIND.RIGHT_CORNER:
                case (int)Constants.TETROMINO_KIND.PYRAMID:
                    start_x = 4;
                    start_y = 0;
                    break;
            }

            current_ = t;

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    position_[x][y] = 
                        new tetromino_pos { x = start_x + x, y = start_y + y };

                    // To allow the 4*4 to go over the top
                    if (start_y + y < 0) continue;

                    field_[start_x + x][start_y + y] = current_.GetShape()[x][y];
                }
            }
        }

        void Draw()
        {
            for (int x = 0; x < Constants.ROWS; ++x)
            {
                for (int y = 0; y < Constants.COLUMNS; ++y)
                {
                    if (field_[x][y] == 0)
                    {
                        Debug.WriteLine("empty");
                        // Skip empty spots
                        continue;
                    }
                    else if (field_[x][y] == 1)
                    {
                        SolidColorBrush c = new SolidColorBrush(current_.GetColor());

                        Rectangle r = new Rectangle();
                        r.Stroke = c;
                        r.Fill = c;
                        r.StrokeThickness = 1;
                        r.Width = Constants.SQUARE_SIDE;
                        r.Height = Constants.SQUARE_SIDE;
                        Canvas.SetLeft(r, y * Constants.SQUARE_SIDE);
                        Canvas.SetTop(r, x * Constants.SQUARE_SIDE);

                        mainCanvas.Children.Add(r);
                    }
                    else if (field_[x][y] >= 2)
                    {
                        SolidColorBrush c = new SolidColorBrush(current_.GetColor());

                        Rectangle r = new Rectangle();
                        r.Stroke = c;
                        r.Fill = c;
                        r.StrokeThickness = 1;
                        r.Width = Constants.SQUARE_SIDE;
                        r.Height = Constants.SQUARE_SIDE;
                        Canvas.SetLeft(r, y * Constants.SQUARE_SIDE);
                        Canvas.SetTop(r, x * Constants.SQUARE_SIDE);

                        mainCanvas.Children.Add(r);
                    }

                    if (DEBUG)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = field_[x][y].ToString();
                        textBlock.Foreground = new SolidColorBrush(Colors.Black);
                        Canvas.SetLeft(textBlock, y * Constants.SQUARE_SIDE);
                        Canvas.SetTop(textBlock, x * Constants.SQUARE_SIDE);

                        mainCanvas.Children.Add(textBlock);
                    }
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
