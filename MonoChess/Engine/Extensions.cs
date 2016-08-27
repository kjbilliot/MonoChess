using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonoChess.Engine
{
    public static class Extensions
    {
        public const string NumbersInOrder = "76543210";
        public const string LettersInOrder = "abcdefgh";

        public static string ToBoardLetter(this int i)
        {
            return LettersInOrder[i].ToString();
        }

        public static int ToNumber(this string s)
        {
            return LettersInOrder.IndexOf(s);
        }

        public static int Inverted(this int i)
        {
            int x;

            string s = NumbersInOrder[i].ToString();
            int.TryParse(s, out x);

            return x;
        }

        public static bool Matches(this string str, string pattern)
        {
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            return r.IsMatch(str);
        }

        public static string ToCoordinate(this Vector2D position)
        {
            return (position.X).ToBoardLetter() + "" + (position.Y.Inverted() + 1);
        }
    }
}
