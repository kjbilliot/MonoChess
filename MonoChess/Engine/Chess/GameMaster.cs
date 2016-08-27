using MonoChess.Engine.Pieces;
using MonoChess.Engine.Rendering;
using MonoChess.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonoChess.Engine.Chess
{
    public static class GameMaster
    {
        public static bool IsLightsTurn = true;
        public static bool IsLightCheck = false;
        public static bool IsLightCheckmate = false;
        public static bool IsDarkCheck = false;
        public static bool IsDarkCheckmate = false;


        public static void AdvanceGame()
        {
            IsLightsTurn = !IsLightsTurn;
            CheckForCheckOrCheckmate();

            if (IsLightCheck) Notifications.Notify("Light King is in check!");
            if (IsDarkCheck) Notifications.Notify("Dark King is in check!");

            CheckForPawnPromotion();

            Notifications.Notify("It's " + (IsLightsTurn ? "Light" : "Dark") + "'s turn!");
        }

        private static void CheckForPawnPromotion()
        {
            List<Sprite> boardPieces = ((ChessGameScene)ChessGame.Instance.CurrentScene).Sprites
                                            .Where(m => m.GetType() == typeof(ChessBoardSprite)).ToList();

            List<ChessBoardSprite> boardSprites = new List<ChessBoardSprite>();
            boardPieces.ForEach(m => boardSprites.Add((ChessBoardSprite)m));

            boardSprites.ForEach(m => m.PromoteIfNecessary());
        }

        private static void CheckForCheckOrCheckmate()
        {
            ChessPiece lightKing, darkKing;
            lightKing = ChessGame.ChessBoard.FindPiece(typeof(King), PieceMaterial.L);
            darkKing = ChessGame.ChessBoard.FindPiece(typeof(King), PieceMaterial.D);

            List<ChessPiece> allDarkPieces = ChessGame.ChessBoard.FindByMaterial(PieceMaterial.D);
            List<ChessPiece> allLightPieces = ChessGame.ChessBoard.FindByMaterial(PieceMaterial.L);

            bool isLightKingCheckmate = IsInCheckmate(allDarkPieces, (King)lightKing);
            bool isDarkKingCheckmate = IsInCheckmate(allLightPieces, (King)darkKing);

            if (isLightKingCheckmate || isDarkKingCheckmate)
            {
                EndGame();
            }

            IsLightCheck = IsInCheck(allDarkPieces, (King)lightKing);
            IsDarkCheck = IsInCheck(allLightPieces, (King)darkKing);

        }

        private static bool IsInCheckmate(List<ChessPiece> enemies, King king)
        {
            bool isInCheckmate = false;

            // For checkmate, we can check if at least one of the king's possible moves is not a valid move for an enemy.
            List<Vector2D> possibleKingMoves = king.GetPossibleMoves();
            List<Vector2D> badKingMoves = new List<Vector2D>();

            if (possibleKingMoves.Count > 0)
            {
                foreach (Vector2D v in possibleKingMoves)
                {
                    foreach (ChessPiece piece in enemies)
                    {
                        if (IsValidPlay(piece, v))
                        {
                            if (!badKingMoves.Contains(v)) badKingMoves.Add(v);
                            break;
                        }
                    }
                }
                isInCheckmate = (badKingMoves.Count == possibleKingMoves.Count);
            }



            return isInCheckmate;
        }

        private static bool IsInCheck(List<ChessPiece> enemies, King king)
        {
            bool isInCheck = false;

            // For check, we can simply check if the king's position is a valid move for at least one enemy.
            foreach (ChessPiece piece in enemies)
            {
                if (IsValidPlay(piece, king.BoardPosition))
                {
                    isInCheck = true;
                    break;
                }
            }
            return isInCheck;
        }

        private static bool IsValidPlay(ChessPiece attacker, Vector2D position)
        {
            return attacker.IsValidMove(position)
                && MovementValidation.IsPathNotObstructed(attacker.BoardPosition, position);
        }

        private static void EndGame()
        {
            string winMessage = "Checkmate!\n" + (IsLightCheckmate ? "Dark" : "Light") + " player wins!";
            MessageBox.Show(winMessage, "Chess", MessageBoxButtons.OK);
            Environment.Exit(0);
        }
       
    }
}
