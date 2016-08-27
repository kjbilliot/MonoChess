using MonoChess.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    class King : ChessPiece
    {
        public King(Vector2D position, PieceMaterial pieceType) : base(position, pieceType) { }



        public override bool IsValidMove(Vector2D toPosition)
        {
            bool isValid = false;

            var pieceAtToPosition = ChessGame.ChessBoard.Board[toPosition.Y, toPosition.X];

            if ((MovementValidation.IsValidBishopMovement(BoardPosition, toPosition)
                || MovementValidation.IsValidRookMovement(BoardPosition, toPosition))
                && !(pieceAtToPosition != null && pieceAtToPosition.GetType() == typeof(King)))
            {
                int xDiff = Math.Abs(BoardPosition.X - toPosition.X);
                int yDiff = Math.Abs(BoardPosition.Y - toPosition.Y);

                isValid = ((xDiff == 0 || xDiff == 1) && (yDiff == 0 || yDiff == 1));
            }

            return isValid;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "k" : "K");
        }
    }
}
