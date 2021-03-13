using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tetris_csharp
{
    class Tetromino
    {
        public int type;

        public Tetromino(int t_type)
        {
            type = t_type;
        }

        public Color GetColor()
        {
            switch (type)
            {
                case (int)Constants.TETROMINO_KIND.HORIZONTAL:
                    return Colors.Cyan;
                case (int)Constants.TETROMINO_KIND.SQUARE:
                    return Colors.Blue;
                case (int)Constants.TETROMINO_KIND.STEP_UP_RIGHT:
                    return Colors.Orange;
                case (int)Constants.TETROMINO_KIND.STEP_UP_LEFT:
                    return Colors.Yellow;
                case (int)Constants.TETROMINO_KIND.LEFT_CORNER:
                    return Colors.Green;
                case (int)Constants.TETROMINO_KIND.RIGHT_CORNER:
                    return Colors.Magenta;
                case (int)Constants.TETROMINO_KIND.PYRAMID:
                    return Colors.Red;
                default:
                    return Colors.Black;
            }
        }
        
        public List<List<int>> GetShape()
        {
            List<List<int>> shape = new List<List<int>>();

            switch (type)
            {
                case (int)Constants.TETROMINO_KIND.HORIZONTAL:
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.SQUARE:
                    shape.Add(new List<int>() { 0, 0, 0, 0 }); 
                    shape.Add(new List<int>() { 1, 1, 0, 0 });
                    shape.Add(new List<int>() { 1, 1, 0, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.STEP_UP_RIGHT:
                    shape.Add(new List<int>() { 0, 1, 0, 0 });
                    shape.Add(new List<int>() { 0, 1, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.STEP_UP_LEFT:
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 1, 1, 0 });
                    shape.Add(new List<int>() { 0, 1, 0, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.LEFT_CORNER:
                    shape.Add(new List<int>() { 0, 1, 1, 0 });
                    shape.Add(new List<int>() { 0, 1, 0, 0 });
                    shape.Add(new List<int>() { 0, 1, 0, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.RIGHT_CORNER:
                    shape.Add(new List<int>() { 0, 1, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
                case (int)Constants.TETROMINO_KIND.PYRAMID:
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 1, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 1, 0 });
                    shape.Add(new List<int>() { 0, 0, 0, 0 });
                    break;
            }

            return shape;
        }

    }
}
