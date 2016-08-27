using Microsoft.Xna.Framework.Content;
using MonoChess.Engine.Chess;
using MonoChess.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Pieces
{
    public enum PieceMaterial { L, D }
    public abstract class ChessPiece
    {
        public bool IsFirstMove = true;

        public Vector2D BoardPosition;
        public PieceMaterial PieceType;

        public static readonly int[] PossibleMovesXAdditionCycle = { 0, 1, 1, 1, 0, -1, -1, -1 };
        public static readonly int[] PossibleMovesYAdditionCycle = { -1, -1, 0, 1, 1, 1, 0, -1 };
        public const int SurroundingSpaceCount = 8;

        public ChessPiece(Vector2D position, PieceMaterial pieceType)
        {
            BoardPosition = position;
            PieceType = pieceType;
        }


        public bool IsCorrectTurn()
        {
            return ((PieceType == PieceMaterial.L && GameMaster.IsLightsTurn)
                || (PieceType == PieceMaterial.D && !GameMaster.IsLightsTurn));
        }

        public List<Vector2D> GetValidSurroundingTiles()
        {
            List<Vector2D> moves = new List<Vector2D>();
            for (int i = 0; i < SurroundingSpaceCount; i++)
            {
                int newX = BoardPosition.X + PossibleMovesXAdditionCycle[i];
                int newY = BoardPosition.Y + PossibleMovesYAdditionCycle[i];
                if (IsValidPosition(newX, newY))
                {
                    moves.Add(new Vector2D(newX, newY));
                }
            }
            return moves;
        }

        public bool IsValidPosition(int x, int y)
        {
            bool withinHorizontal = (x >= 0 && x < ChessBoard.Width);
            bool withinVertical = (y >= 0 && y < ChessBoard.Height);
            return withinHorizontal && withinVertical;
        }

        public abstract bool IsValidMove(Vector2D toPosition);

        public virtual List<Vector2D> GetPossibleMoves()
        {
            List<Vector2D> possibleMoves = new List<Vector2D>();
            
            for (int y = 0; y < ChessBoard.Height; y++)
            {
                for (int x = 0; x < ChessBoard.Width; x++)
                {
                    Vector2D currentCheck = new Vector2D(x, y);
                    ChessPiece pieceAtCheck = ChessGame.ChessBoard.Board[currentCheck.Y, currentCheck.X];

                    //if (currentCheck.ToCoordinate() == "g1") Debugger.Break();

                    bool isPieceKing = (pieceAtCheck != null && pieceAtCheck.GetType() == typeof(King));
                    bool isValidMove = IsValidMove(currentCheck);
                    bool pathNotObstructed = MovementValidation.IsPathNotObstructed(BoardPosition, currentCheck);
                    bool pieceIsValid = (pieceAtCheck == null || (pieceAtCheck != null && PieceType != pieceAtCheck.PieceType));

                    if (isValidMove && pathNotObstructed && pieceIsValid && !isPieceKing)
                    {
                        possibleMoves.Add(currentCheck);
                    }
                }
            }

            return possibleMoves;
        }

        public string GetCoordinate()
        {
            return (BoardPosition.X).ToBoardLetter() + "" + (BoardPosition.Y.Inverted() + 1);
        }
    }
}
