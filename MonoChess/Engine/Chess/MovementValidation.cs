using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    public static class MovementValidation
    {
        public enum Direction { North, South, West, East, NorthWest, NorthEast, SouthWest, SouthEast, Unknown };
        public static bool PiecesAreSeparateTeams(Vector2D from, Vector2D to)
        {
            var board = ChessGame.ChessBoard.Board;

            bool isValid = false;

            ChessPiece fromPiece = board[from.Y, from.X];
            ChessPiece toPiece = board[to.Y, to.X];

            if (fromPiece != null && toPiece != null)
            {
                isValid = (fromPiece.PieceType != toPiece.PieceType);
            }

            return isValid;
        }

        public static bool IsPathNotObstructed(Vector2D from, Vector2D to)
        {
            bool result = true;
            Direction directionGoingTo = GetDirection(from, to);
            var board = ChessGame.ChessBoard.Board;
            int xTmp;
            try
            {
                switch (directionGoingTo)
                {
                    case Direction.North:
                        for (int i = from.Y - 1; i >= to.Y + 1; i--)
                        {
                            if (board[i, from.X] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.South:
                        for (int i = from.Y + 1; i < to.Y + 1; i++)
                        {
                            if (board[i, from.X] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.West:
                        for (int i = from.X - 1; i >= to.X + 1; i--)
                        {
                            if (board[from.Y, i] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.East:
                        for (int i = from.X + 1; i < to.X; i++)
                        {
                            if (board[from.Y, i] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.NorthWest:
                        xTmp = from.X - 1;
                        for (int i = from.Y - 1; i >= to.Y + 1; i--)
                        {
                            if (board[i, xTmp--] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.NorthEast:
                        xTmp = from.X + 1;
                        for (int i = from.Y - 1; i >= to.Y + 1; i--)
                        {
                            if (board[i, xTmp++] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.SouthWest:
                        xTmp = from.X - 1;
                        for (int i = from.Y + 1; i <= to.Y - 1; i++)
                        {
                            if (board[i, xTmp--] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                    case Direction.SouthEast:
                        xTmp = from.X + 1;
                        for (int i = from.Y + 1; i <= to.Y + 1; i++)
                        {
                            if (board[i, xTmp++] != null)
                            {
                                result = false;
                                break;
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        public static Direction GetDirection(Vector2D from, Vector2D to)
        {
            Direction d = Direction.Unknown;

            int xDiff = from.X - to.X;
            int yDiff = from.Y - to.Y;

            if (from.Y == to.Y)
            {
                // Going horizontal.
                d = (xDiff < 0 ? Direction.East : Direction.West);
            }
            else if (from.X == to.X)
            {
                // Going vertical.
                d = (yDiff < 0 ? Direction.South : Direction.North);
            }
            else
            {
                // Going diagonal somewhere.
                if (Math.Abs(xDiff) == Math.Abs(yDiff))
                {
                    if (to.Y >= from.Y)
                    {
                        d = (to.X >= from.X ? Direction.SouthEast : Direction.SouthWest);
                    }
                    else
                    {
                        d = (to.X >= from.X ? Direction.NorthEast : Direction.NorthWest);
                    }
                }
            }

            return d;
        }

        public static bool IsValidBishopMovement(Vector2D from, Vector2D to)
        {
            bool isValid = false;

            int xDiff = Math.Abs(from.X - to.X);
            int yDiff = Math.Abs(from.Y - to.Y);

            isValid = (xDiff == yDiff);

            return isValid;
        }

        public static bool IsValidRookMovement(Vector2D from, Vector2D to)
        {
            bool isValid = false;

            isValid = (from.X == to.X || from.Y == to.Y);

            return isValid;
        }
    }
}
