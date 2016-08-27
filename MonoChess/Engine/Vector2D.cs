using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine
{
    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2D() : this(0, 0) { }

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public string ToBoardCoord()
        {
            return X.ToBoardLetter() + "" + (Y + 1);
        }
    }
}
