using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;

namespace Tetris_csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool DEBUG = false;

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
        int[,] field_;

        // 4*4 that holds the information of each piece in the tetromino
        tetromino_pos[,] position_;

        Tetromino current_;
        
        int current_shape_;

        System.Windows.Threading.DispatcherTimer timer;

        bool create_ = true;

        // Make gravity greater on key ´S´
        bool fast_ = false;

        // Randomizer for next tetromino
        System.Random random_;

        // The next tetromino to spawn
        int next_ = 0;

        // Graphics array, used for deleting old rectangles
        List<UIElement> graphics_ = new List<UIElement>();

        public MainWindow()
        {
            InitializeComponent();
            Init();
            DrawGrid();

            // Add a game timer
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new System.EventHandler(GameLoop);
            timer.Interval = new System.TimeSpan(0, 0, 1);
            timer.Start();

            //GameLoop();
        }

        void Init()
        {
            field_ = new int[Constants.COLUMNS,
                             Constants.ROWS];

            for (int x = 0; x < Constants.COLUMNS; ++x)
            {
                for (int y = 0; y < Constants.ROWS; ++y)
                {
                    field_[x,y] = 0;
                }
            }

            position_ = new tetromino_pos[4, 4];
            for (int x = 0; x < 4; ++x)
            {
                for (int y = 0; y < 4; ++y)
                {
                    position_[x, y] = new tetromino_pos { x = 0, y = 0 };
                }
            }

            random_ = new System.Random();
            next_ = random_.Next((int)
                Constants.TETROMINO_KIND.NUMBER_OF_TETROMINOS);
        }

        void GameLoop(object sender, System.EventArgs e)
        {  
            if (create_)
            {
                create_ = !create_;
                CreateBlock(next_);
            }
            
            Draw();
            MoveBlock((int)Constants.DIRECTIONS.DOWN);
        }

        void CreateBlock(int tetromino)
        {
            int start_x = 4;
            int start_y = 0;

            current_shape_ = tetromino;
            Tetromino t = new Tetromino(current_shape_);

            switch (tetromino)
            {
                case (int)Constants.TETROMINO_KIND.HORIZONTAL:
                    start_x = 4;
                    start_y = 16;
                    break;
                case (int)Constants.TETROMINO_KIND.SQUARE:
                    start_x = 4;
                    start_y = 16;
                    break;
                case (int)Constants.TETROMINO_KIND.STEP_UP_RIGHT:
                case (int)Constants.TETROMINO_KIND.STEP_UP_LEFT:
                case (int)Constants.TETROMINO_KIND.LEFT_CORNER:
                case (int)Constants.TETROMINO_KIND.RIGHT_CORNER:
                case (int)Constants.TETROMINO_KIND.PYRAMID:
                    start_x = 4;
                    start_y = 16;
                    break;
            }

            current_ = t;

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    position_[x, y] = 
                        new tetromino_pos { x = start_x + x, y = start_y + y };

                    // To allow the 4*4 to go over the top
                    if (start_y + y < 0) continue;

                    field_[start_x + x, start_y + y] = current_.GetShape()[x][y];
                }
            }
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (current_ == null) return;
            if (e.Key == Key.A)
            {
                MoveBlock((int)Constants.DIRECTIONS.LEFT);
            } 
            else if (e.Key == Key.S)
            {
                MoveBlock((int)Constants.DIRECTIONS.DOWN);
            }
            else if (e.Key == Key.D)
            {
                MoveBlock((int)Constants.DIRECTIONS.RIGHT);
            }
            else if (e.Key == Key.Space)
            {
                MoveToBottom();
            }
        }

        void MoveToBottom()
        {
            bool all_the_way = false;
            int delta_y = 0;

            List<int> piece_delta_y = new List<int>();
            bool[] piece_to_bottom = new bool[4]
                { false, false, false, false };
            int[] piece_to_bottom_y = new int[4]
                { Constants.ROWS + 1, Constants.ROWS + 1,
                  Constants.ROWS + 1, Constants.ROWS + 1};

            int i = 0;
            for (int px = 0; px < 4; ++px)
            {
                for (int py = 0; py < 4; ++py)
                {
                    if (current_.GetShape()[px][py] != 1) continue;

                    // Check if all pieces can go directly to floor
                    if (AllClearBelow(position_[px, py].x))
                    {
                        piece_to_bottom_y[i] = Constants.ROWS 
                            - position_[px, py].y - 1;
                        piece_to_bottom[i] = true;
                    }

                    // No need to continue loop if all
                    // can go to the floor
                    all_the_way = AllTrue(piece_to_bottom);
                    if (all_the_way)
                    {
                        break;
                    }

                    // Calculate the possible delta y for each piece
                    int j = 0;
                    for (int x = 0; x < Constants.COLUMNS; ++x)
                    {
                        for (int y = 0; y < Constants.ROWS; ++y)
                        {
                            if (x == position_[px, py].x &&
                                 field_[x, y] > 1)
                            {
                                int candidate_y = y - position_[px, py].y - 1;
                                piece_delta_y.Add(candidate_y);
                            }
                        }
                        ++j;
                    }
                    ++i;
                }
            }

            // Choose the smallest delta y from all pieces
            // to only move as little as possible
            delta_y = all_the_way ? piece_to_bottom_y.Min()
                                  : piece_delta_y.Min();

            Debug.WriteLine(delta_y);

            for (int h = 0; h < 4; h++)
            {
                for (int j = 0; j < 4; j++)
                {
                    SetAbsolutePosition(h, j, position_[h, j].x,
                                              position_[h, j].y + delta_y);
                }
            }
        }

        bool AllClearBelow(int col)
        {
            for (int y = 0; y < Constants.ROWS; ++y)
            {
                if (field_[col, y] > 1)
                {
                    return false;
                }
            }

            return true;
        }

        /* 
         * Helper to check if all elements
         * in vector are true
         */
        bool AllTrue(bool[] vec)
        {
            foreach (bool b in vec)
            {
                if (!b)
                {
                    return false;
                }
            }
            return true;
        }


        void MoveBlock(int d)
        {
            if (current_ == null) return;

            // Default delta x and delta y
            int dx = 0;
            int dy = 0;

            int r = 1;

            switch (d)
            {
                case (int)Constants.DIRECTIONS.LEFT:
                    dx = -Constants.LATERAL_SPEED;
                    break;
                case (int)Constants.DIRECTIONS.RIGHT:
                    dx = Constants.LATERAL_SPEED;
                    break;
                case (int)Constants.DIRECTIONS.DOWN:
                    // TODO: implement fast-down
                    dy = Constants.SPEED;
                    r = dy;
                    break;
            }

            int status = CheckSpace(d, r);
            switch (status)
            {
                case (int)Constants.OBSTACLE.TETROMINO:
                    if (d != (int)Constants.DIRECTIONS.DOWN)
                    {
                        return;
                    }
                    if (fast_)
                    {
                        MoveToBottom();
                    }
                    FinishTetromino();
                    return;
                case (int)Constants.OBSTACLE.WALL:
                    return;
                case (int)Constants.OBSTACLE.FLOOR:
                    if (fast_)
                    {
                        MoveToBottom();
                    }
                    FinishTetromino();
                    return;
            }

            // Move each piece's coordinates
            for (int px = 0; px < 4; ++px)
            {
                for (int py = 0; py < 4; ++py)
                {
                    SetAbsolutePosition(px, py, position_[px, py].x + dx,
                                                position_[px, py].y + dy);
                }
            }
        }

        void FinishTetromino()
        {
            for (int px = 0; px < 4; ++px)
            {
                for (int py = 0; py < 4; ++py)
                {
                    if (current_.GetShape()[px][py] == 0)
                    {
                        continue;
                    }

                    field_[position_[px, py].x,
                           position_[px, py].y] = current_shape_ + 2;
                }
            }

            // Randomize next tetromino and show
            // in the box next to game field
            current_ = null;
            CreateBlock(next_);
            next_ = random_.Next((int)
                Constants.TETROMINO_KIND.NUMBER_OF_TETROMINOS);
            /*drawNext();*/
            if (DEBUG) Debug.WriteLine("Tetromino finished");

            /* TODO: check rows
            for (int y = 0; y < ROWS; ++y)
            {
                if (checkRow(y))
                {
                    clearRow(y);
                }
            }*/
        }

        int CheckSpace(int d, int r = 1)
        {
            // Default delta x and delta y
            int dx = 0;
            int dy = 0;

            switch (d)
            {
                case (int)Constants.DIRECTIONS.LEFT:
                    dx = -r;
                    break;
                case (int)Constants.DIRECTIONS.RIGHT:
                    dx = r;
                    break;
                case (int)Constants.DIRECTIONS.DOWN:
                    dy = r;
                    break;
            }

            for (int px = 0; px < 4; ++px)
            {
                for (int py = 0; py < 4; ++py)
                {
                    if (current_.GetShape()[px][py] != 1) continue;

                    // Check for walls
                    if (position_[px, py].x + dx >= Constants.COLUMNS || // Right wall
                        position_[px, py].x + dx < 0)                    // Left wall
                    {
                        if (DEBUG) Debug.WriteLine("Movement blocked: wall");
                        return (int)Constants.OBSTACLE.WALL;
                    }

                    if (position_[px, py].y + dy >= Constants.ROWS)
                    {
                        if (DEBUG) Debug.WriteLine("Movement blocked: floor");
                        return (int)Constants.OBSTACLE.FLOOR;
                    }

                    // Check for other blocks
                    if ((position_[px, py].x + dx >= 0 &&
                         position_[px, py].x + dx < Constants.COLUMNS) &&

                         (position_[px, py].y + dy < Constants.ROWS) &&

                         (field_[position_[px, py].x + dx,
                                 position_[px, py].y + dy] > 1))
                    {
                        if (DEBUG) Debug.WriteLine("Movement blocked: tetromino");
                        return (int)Constants.OBSTACLE.TETROMINO;
                    }
                }
            }

            return (int)Constants.OBSTACLE.NONE;
        }

        void SetAbsolutePosition(int p_x, int p_y, int to_x, int to_y)
        {
            // Move piece's coordinates
            position_[p_x, p_y] = new tetromino_pos { x = to_x, y = to_y };

            // Clear the field
            for (int x = 0; x < Constants.COLUMNS; ++x)
            {
                for (int y = 0; y < Constants.ROWS; ++y)
                {
                    if (field_[x, y] != 1)
                    {
                        continue;
                    }
                    else if (field_[x, y] == 1)
                    {
                        field_[x, y] = 0;
                    }
                }
            }

            // Re-assign '1' to the new position
            for (int x = 0; x < Constants.COLUMNS; ++x)
            {
                for (int y = 0; y < Constants.ROWS; ++y)
                {
                    for (int px = 0; px < 4; ++px)
                    {
                        for (int py = 0; py < 4; ++py)
                        {
                            if (x == position_[px, py].x &&
                                y == position_[px, py].y &&
                                current_.GetShape()[px][py] == 1)
                            {
                                field_[x, y] = 1;
                            }
                        }
                    }
                }
            }

            // Redraw the field after movement
            Draw();
        }

        void Draw()
        {
            // Remove old graphics
            foreach (UIElement r in graphics_)
            {
                mainCanvas.Children.Remove(r);
            }

            for (int x = 0; x < Constants.COLUMNS; ++x)
            {
                for (int y = 0; y < Constants.ROWS; ++y)
                {
                    if (field_[x, y] == 1)
                    {
                        SolidColorBrush c = new SolidColorBrush(current_.GetColor());

                        Rectangle r = new Rectangle();
                        r.Stroke = c;
                        r.Fill = c;
                        r.StrokeThickness = 1;
                        r.Width = Constants.SQUARE_SIDE;
                        r.Height = Constants.SQUARE_SIDE;
                        Canvas.SetLeft(r, x * Constants.SQUARE_SIDE);
                        Canvas.SetTop(r, y * Constants.SQUARE_SIDE);

                        graphics_.Add(r);
                        mainCanvas.Children.Add(r);
                    }
                    else if (field_[x, y] >= 2)
                    {
                        SolidColorBrush c = new SolidColorBrush(
                            new Tetromino(field_[x, y] - 2).GetColor());

                        Rectangle r = new Rectangle();
                        r.Stroke = c;
                        r.Fill = c;
                        r.StrokeThickness = 1;
                        r.Width = Constants.SQUARE_SIDE;
                        r.Height = Constants.SQUARE_SIDE;
                        Canvas.SetLeft(r, x * Constants.SQUARE_SIDE);
                        Canvas.SetTop(r, y * Constants.SQUARE_SIDE);

                        graphics_.Add(r);
                        mainCanvas.Children.Add(r);
                    }

                    if (DEBUG)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = field_[x, y].ToString();
                        textBlock.Foreground = new SolidColorBrush(Colors.Black);
                        Canvas.SetLeft(textBlock, x * Constants.SQUARE_SIDE);
                        Canvas.SetTop(textBlock, y * Constants.SQUARE_SIDE);

                        graphics_.Add(textBlock);
                        mainCanvas.Children.Add(textBlock);
                    }
                }
            }
        }

        void DrawGrid()
        {
            SolidColorBrush black = new SolidColorBrush(Colors.DarkGray);

            for (int x = 0; x < Constants.COLUMNS; ++x)
            {
                for (int y = 0; y < Constants.ROWS; ++y)
                {
                    Rectangle r = new Rectangle();
                    r.Stroke = black;
                    r.StrokeThickness = 1;
                    r.Width = Constants.SQUARE_SIDE;
                    r.Height = Constants.SQUARE_SIDE;
                    Canvas.SetLeft(r, x * Constants.SQUARE_SIDE);
                    Canvas.SetTop(r, y * Constants.SQUARE_SIDE);

                    mainCanvas.Children.Add(r);
                }
            }
        }
    }
}
