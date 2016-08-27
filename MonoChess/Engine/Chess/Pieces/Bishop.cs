using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    class Bishop : ChessPiece
    {
        public Bishop(Vector2D position, PieceMaterial pieceType) : base(position, pieceType) { }

        public override bool IsValidMove(Vector2D toPosition)
        {
            bool validBishop = MovementValidation.IsValidBishopMovement(BoardPosition, toPosition);
            bool pathNotObstructed = MovementValidation.IsPathNotObstructed(BoardPosition, toPosition);
            return validBishop && pathNotObstructed;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "b" : "B");
        }
    }
}
