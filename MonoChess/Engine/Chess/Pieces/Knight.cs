using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    class Knight : ChessPiece
    {
        private const int KnightTraversalLength = 2;
        private static readonly int[] KnightPossibleMovementCycleX = { -2, -2, -1, -1, 1, 1, 2, 2 };
        private static readonly int[] KnightPossibleMovementCycleY = { -1, 1, -2, 2, -2, 2, -1, 1 };

        public Knight(Vector2D position, PieceMaterial pieceType) : base(position, pieceType) { }

        public override bool IsValidMove(Vector2D toPosition)
        {
            bool isValid = false;

            int xDiff = Math.Abs(BoardPosition.X - toPosition.X);
            int yDiff = Math.Abs(BoardPosition.Y - toPosition.Y);

            var pieceAtPosition = ChessGame.ChessBoard.Board[toPosition.Y, toPosition.X];

            if (!(pieceAtPosition != null && pieceAtPosition.GetType() == typeof(King)))
            {
                isValid = (xDiff == KnightTraversalLength && yDiff == 1)
                    || (yDiff == KnightTraversalLength && xDiff == 1);
            }

            return isValid;
        }


        //public override List<Vector2D> GetPossibleMoves()
        //{
        //    List<Vector2D> possibleMoves = new List<Vector2D>();

        //    int curX = Position.X;
        //    int curY = Position.Y;

        //    for (int i = 0; i < KnightPossibleMovementCycleX.Length; i++)
        //    {
        //        int movX = curX + KnightPossibleMovementCycleX[i];
        //        int movY = curY + KnightPossibleMovementCycleY[i];
        //        if (IsPositionValid(movX, movY))
        //        {
        //            ChessPiece pieceAtPosition = ChessGame.ChessBoard.Board[movY, movX];
        //            if (pieceAtPosition == null || pieceAtPosition.PieceType != PieceType)
        //            {
        //                Vector2D movePosition = new Vector2D(movX, movY);
        //                if (IsValidMove(movePosition))
        //                {
        //                    possibleMoves.Add(movePosition);
        //                }
        //            }
        //        }
        //    }

        //    return possibleMoves;
        //}

        private bool IsPositionValid(int x, int y)
        {
            bool withinHorizontal = (x >= 0 && x < ChessBoard.Width);
            bool withinVertical = (y >= 0 && y < ChessBoard.Height);
            return withinHorizontal && withinVertical;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "n" : "N");
        }
    }
}
