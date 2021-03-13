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

        // How much to move a block per frame
        public const int SPEED = 1;
        // How much to move a block on keypress
        public const int KEYPRESS_SPEED = 2 * SPEED;
        // How much to move a block sideways on keypress
        public const int LATERAL_SPEED = 1;
    }
}
