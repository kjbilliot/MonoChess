using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    public class Queen : ChessPiece
    {
        public Queen(Vector2D position, PieceMaterial pieceType) : base(position, pieceType) { }

        public override bool IsValidMove(Vector2D toPosition)
        {
            bool isValidRook = MovementValidation.IsValidRookMovement(BoardPosition, toPosition);
            bool isValidBishop = MovementValidation.IsValidBishopMovement(BoardPosition, toPosition);
            bool isNotObstructed = MovementValidation.IsPathNotObstructed(BoardPosition, toPosition);
            return (isValidRook || isValidBishop) && isNotObstructed;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "q" : "Q");
        }
    }
}
