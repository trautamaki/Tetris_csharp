namespace Tetris_csharp
{
    public static class Constants
    {
        public const int BORDER_DOWN = 480;
        public const int BORDER_RIGHT = 240;
        public const int SQUARE_SIDE = 20;
        public const int COLUMNS = BORDER_RIGHT / SQUARE_SIDE;
        public const int ROWS = BORDER_DOWN / SQUARE_SIDE;

        // Constants for different tetrominos and the number of them
        public enum TETROMINO_KIND
        {
            HORIZONTAL,
            LEFT_CORNER,
            RIGHT_CORNER,
            SQUARE,
            STEP_UP_RIGHT,
            PYRAMID,
            STEP_UP_LEFT,
            NUMBER_OF_TETROMINOS
        };

        public enum DIRECTIONS { LEFT, RIGHT, DOWN };
        public enum OBSTACLE { NONE, WALL, FLOOR, TETROMINO };
    }
}
