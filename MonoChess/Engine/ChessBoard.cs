using MonoChess.Engine.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine
{
    public class ChessBoard
    {
        public const int Width = 8;
        public const int Height = 8;

        private const string Separator = " | ";

        public ChessPiece[,] Board;
        
        public ChessBoard()
        {
            Board = new ChessPiece[Height, Width];
        }

        public bool IsSpotFree(int x, int y)
        {
            return (Board[y, x] == null);
        }
        
        public void WriteBoard()
        {
            StringBuilder board = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ChessPiece p = Board[y, x];

                    board.Append((p != null ? p.ToString() : " ") + Separator);
                }
                board.Length -= Separator.Length;
                board.Append("\n");
            }
            Console.WriteLine(board);
        }

        public List<ChessPiece> FindByMaterial(PieceMaterial mat)
        {
            List<ChessPiece> pieces = new List<ChessPiece>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var p = Board[y, x];
                    if (p != null && p.PieceType == mat)
                    {
                        pieces.Add(p);
                    }
                }
            }

            return pieces;
        }

        public ChessPiece FindPiece(Type pieceType, PieceMaterial mat)
        {
            ChessPiece found = null;

            for (int y = 0; y < Height && found == null; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ChessPiece p = Board[y, x]; 
                    if (p != null && p.GetType() == pieceType && p.PieceType == mat)
                    {
                        found = p;
                        break;
                    }
                }
            }

            return found;
        }

    }
}
