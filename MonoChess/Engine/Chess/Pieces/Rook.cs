using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    class Rook : ChessPiece
    {
        public Rook(Vector2D position, PieceMaterial pieceType) : base(position, pieceType) { }

        public override bool IsValidMove(Vector2D toPosition)
        {
            bool isValidRook = MovementValidation.IsValidRookMovement(BoardPosition, toPosition);
            //if (toPosition.ToCoordinate() == "g1") Debugger.Break();
            bool isNotObstructed = MovementValidation.IsPathNotObstructed(BoardPosition, toPosition);
            return isValidRook && isNotObstructed;
        }

        public override List<Vector2D> GetPossibleMoves()
        {
            List<Vector2D> baseMoves = base.GetPossibleMoves();
            List<Vector2D> validMoves = new List<Vector2D>();

            foreach (Vector2D move in baseMoves)
            {
                bool validMove = IsValidMove(move);
                bool pathNotObstructed = MovementValidation.IsPathNotObstructed(BoardPosition, move);
                if (validMove && pathNotObstructed)
                {
                    validMoves.Add(move);
                }
            }

            return validMoves;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "r" : "R");
        }
    }
}
