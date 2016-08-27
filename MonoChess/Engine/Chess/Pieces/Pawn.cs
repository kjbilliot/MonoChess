using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonoChess.Engine.Pieces.MovementValidation;

namespace MonoChess.Engine.Pieces
{
    class Pawn : ChessPiece
    {

        private static readonly Direction[] acceptableLightDirections = { Direction.North, Direction.NorthEast, Direction.NorthWest };
        private static readonly Direction[] acceptableDarkDirections = { Direction.South, Direction.SouthEast, Direction.SouthWest };


        private const int EnPassantMoveLength = 2;

        public Pawn(Vector2D position, PieceMaterial pieceType) : base(position, pieceType)
        {

        }


        public override bool IsValidMove(Vector2D toPosition)
        {
            bool isValid = false;

            if (IsValidBishopMovement(BoardPosition, toPosition)
            || IsValidRookMovement(BoardPosition, toPosition))
            {
                Direction heading = GetDirection(BoardPosition, toPosition);
                PieceMaterial color = PieceType;

                if ((color == PieceMaterial.L && acceptableLightDirections.Contains(heading))
                    || (color == PieceMaterial.D && acceptableDarkDirections.Contains(heading)))
                {
                    bool passesCoordTest = ((toPosition.Y >= 0 && toPosition.Y < ChessBoard.Height) && (toPosition.X >= 0 && toPosition.X < ChessBoard.Width));
                    if (passesCoordTest)
                    {
                        int yDiff = Math.Abs(BoardPosition.Y - toPosition.Y);
                        if (yDiff == 1 || (IsFirstMove && yDiff == EnPassantMoveLength))
                        {
                            int xDiff = Math.Abs(BoardPosition.X - toPosition.X);
                            ChessPiece pieceAtPosition = ChessGame.ChessBoard.Board[toPosition.Y, toPosition.X];
                            if (!(pieceAtPosition != null && pieceAtPosition.GetType() == typeof(King)))
                            {
                                if (xDiff == 0)
                                {
                                    isValid = (pieceAtPosition == null);
                                }
                                else if (IsValidBishopMovement(BoardPosition, toPosition) && xDiff == 1)
                                {
                                    if (pieceAtPosition != null && PiecesAreSeparateTeams(BoardPosition, toPosition))
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        public override List<Vector2D> GetPossibleMoves()
        {
            List<Vector2D> moves = new List<Vector2D>();

            //if (Position.GetCoordinate() == "e7") Debugger.Break();

            if (IsFirstMove)
            {
                Vector2D enPassantMovement = new Vector2D(BoardPosition.X, BoardPosition.Y + (PieceType == PieceMaterial.L ? -2 : 2));
                bool isValidMove = IsValidMove(enPassantMovement);
                bool pathNotObstructed = IsPathNotObstructed(BoardPosition, enPassantMovement);
                if (isValidMove && pathNotObstructed)
                {
                    moves.Add(enPassantMovement);
                }
            }

            Vector2D regularMovement = new Vector2D(BoardPosition.X, BoardPosition.Y + (PieceType == PieceMaterial.L ? -1 : 1));
            if (IsValidMove(regularMovement) && IsPathNotObstructed(BoardPosition, regularMovement))
            {
                moves.Add(regularMovement);
            }

            List<Vector2D> diagonalMovements = new List<Vector2D>
            {
                new Vector2D(BoardPosition.X + 1, BoardPosition.Y + (PieceType == PieceMaterial.L ? -1 : 1)),
                new Vector2D(BoardPosition.X - 1, BoardPosition.Y + (PieceType == PieceMaterial.L ? -1 : 1)),
            };

            diagonalMovements.Where(m => IsValidMove(m)).ToList().ForEach(m => moves.Add(m));

            return moves;
        }

        public override string ToString()
        {
            return (PieceType == PieceMaterial.L ? "p" : "P");
        }
    }
}
