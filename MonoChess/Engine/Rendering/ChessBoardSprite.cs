using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChess.Engine.Chess;
using MonoChess.Engine.Chess.Pieces;
using MonoChess.Engine.Pieces;
using MonoChess.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonoChess.Engine.Rendering
{
    public class ChessBoardSprite : Sprite
    {
        public bool IsSelected = false;
        public static readonly Color ValidMoveColor = Color.Green, DefaultColor = Color.White;
        public Color SelectedColor = Color.Violet;
        public Color CurrentColor = Color.White;
        public Color HoverColor = Color.DarkGray;
        public ChessPiece PieceInThisPosition;
        public Texture2D ChessPieceTexture;
        private Vector2D OriginalPosition;
        public static StringBuilder NotationBuilder = new StringBuilder();
        private static readonly string[] CoordinateCheckCycleForPromoDarkPawn = { "a1", "b1", "c1", "d1", "e1", "f1", "g1" };
        private static readonly string[] CoordinateCheckCycleForPromoLightPawn = { "a8", "b8", "c8", "d8", "e8", "f8", "g8" };


        public ChessBoardSprite(ContentManager content, PieceMaterial mat, Vector2D renderPosition, Vector2D boardPosition, ChessPiece piece = null)
        {
            Texture = content.Load<Texture2D>((mat == PieceMaterial.L ? "chessboard_light.png" : "chessboard_dark.png"));
            BoardPosition = boardPosition;
            RenderPosition = renderPosition;
            OriginalPosition = new Vector2D(renderPosition.X, renderPosition.Y);
            MouseInteraction.LeftClick += OnLeftClick;
            MouseInteraction.RightClick += OnRightClick;
            PieceInThisPosition = piece;
            UpdateChessPieceTexture();
        }

        public void PromoteIfNecessary()
        {
            if (CanPromote())
            {
                ChessPiece newPiece = PromptForPromotion();
                PieceInThisPosition = newPiece;
                ChessGame.ChessBoard.Board[BoardPosition.Y, BoardPosition.X] = newPiece;
                UpdateChessPieceTexture();
            }
        }

        private ChessPiece PromptForPromotion()
        {
            ChessPiece newPiece = null;
            ChessPiece currentPiece = PieceInThisPosition;
            Console.WriteLine("What would you like to promote your pawn to?");
            Console.WriteLine("\t1.) Knight");
            Console.WriteLine("\t2.) Bishop");
            Console.WriteLine("\t3.) Queen");
            Console.WriteLine("\t4.) Rook");

            bool validInput = false;

            do
            {
                string rawInput = Console.ReadLine();
                int inputInt;
                if (rawInput.Length == 1 && int.TryParse(rawInput, out inputInt))
                {
                    switch (inputInt)
                    {
                        case 1:
                            newPiece = new Knight(currentPiece.BoardPosition, currentPiece.PieceType);
                            validInput = true;
                            break;
                        case 2:
                            newPiece = new Bishop(currentPiece.BoardPosition, currentPiece.PieceType);
                            validInput = true;
                            break;
                        case 3:
                            newPiece = new Queen(currentPiece.BoardPosition, currentPiece.PieceType);
                            validInput = true;
                            break;
                        case 4:
                            newPiece = new Rook(currentPiece.BoardPosition, currentPiece.PieceType);
                            validInput = true;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please input a number. :)");
                }
            } while (!validInput);

            return newPiece;
        }

        private bool CanPromote()
        {
            bool canPromote = false;
            string thisCoord = BoardPosition.ToCoordinate();
            if (CoordinateCheckCycleForPromoLightPawn.Contains(thisCoord))
            {
                canPromote = (PieceInThisPosition != null && 
                    PieceInThisPosition.GetType() == typeof(Pawn) && PieceInThisPosition.PieceType == PieceMaterial.L);
            }
            else if (CoordinateCheckCycleForPromoDarkPawn.Contains(thisCoord))
            {
                canPromote = (PieceInThisPosition != null && 
                    PieceInThisPosition.GetType() == typeof(Pawn) && PieceInThisPosition.PieceType == PieceMaterial.D);
            }
            return canPromote;
        }


        public override void Update()
        {
            //Tint = (IsSelected ? SelectedColor : CurrentColor);
            if (IsMouseOver())
            {
                if (!IsSelected)
                {
                    Tint = HoverColor;
                }
            }
            else
            {
                Tint = CurrentColor;
            }

            base.Update();
        }

        public void MovePiece(ChessBoardSprite movingTo)
        {

            UpdateChessPieceTexture();
        }

        public void UpdateChessPieceTexture()
        {
            //if (BoardPosition.ToCoordinate() == "a1") Debugger.Break();
            PieceInThisPosition = ChessGame.ChessBoard.Board[BoardPosition.Y, BoardPosition.X];
            if (PieceInThisPosition != null)
            {
                string textureName = PieceInThisPosition.GetType().Name + PieceInThisPosition.PieceType.ToString();
                TextureAtlas.PieceTextures.TryGetValue(textureName, out ChessPieceTexture);
            }
            else
            {
                TextureAtlas.PieceTextures.TryGetValue("transparent", out ChessPieceTexture);
            }
        }
        
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (ChessPieceTexture != null)
            {
                //RenderPosition.X += 1;
                //RenderPosition.Y -= 1;

                sb.Draw(ChessPieceTexture, BoundingBox, null, Color.White, RotationAngle, new Vector2(0, 0), SpriteEffects.None, 1.0f);
            }
        }

        public void MarkAsValidPosition()
        {
            CurrentColor = ValidMoveColor;
        }

        public void ResetColor()
        {
            CurrentColor = DefaultColor;
        }

        public bool IsMouseOver()
        {
            return BoundingBox.Contains(MouseInteraction.Position.ToPoint());
        }

        public void OnLeftClick()
        {
            if (IsMouseOver())
            {
                bool hasSelectedRoot = ((ChessGameScene)ChessGame.Instance.CurrentScene).HasSelectedRootNode;
                if (!hasSelectedRoot)
                {
                    if (PieceInThisPosition != null && PieceInThisPosition.IsCorrectTurn())
                    {
                        if (PieceInThisPosition.GetPossibleMoves().Count > 0)
                        {
                            NotationBuilder = new StringBuilder();
                            IsSelected = true;
                            ((ChessGameScene)ChessGame.Instance.CurrentScene).HasSelectedRootNode = true;
                            ((ChessGameScene)ChessGame.Instance.CurrentScene).RootNode = this;
                            List<Vector2D> possibleMoves = PieceInThisPosition.GetPossibleMoves();
                            List<ChessBoardSprite> possibleMovesSprites = new List<ChessBoardSprite>();
                            foreach (Vector2D move in possibleMoves)
                            {
                                if (PieceInThisPosition.IsValidMove(move) && 
                                    MovementValidation.IsPathNotObstructed(PieceInThisPosition.BoardPosition, move))
                                {
                                    possibleMovesSprites.Add(GetBoardPieceByCoord(move.ToCoordinate()));
                                }
                            }
                            //possibleMoves.ForEach(m => possibleMovesSprites.Add(GetBoardPieceByCoord(m.ToCoordinate())));
                            possibleMovesSprites.ForEach(m => m.MarkAsValidPosition());
                            NotationBuilder.Append(BoardPosition.ToCoordinate() + " ");
                        }
                        else
                        {
                            Notifications.Notify("That piece has no possible moves.");
                        }
                    }
                }
                else
                {
                    List<ChessBoardSprite> allChessBoardPieces = GetAllChessBoardPieces();
                    ChessBoardSprite movingPosition = ((ChessGameScene)ChessGame.Instance.CurrentScene).RootNode;
                    ChessPiece movingPiece = movingPosition.PieceInThisPosition;

                    if (movingPiece.IsValidMove(BoardPosition))
                    {
                        NotationBuilder.Append(BoardPosition.ToCoordinate());

                        if (PieceInThisPosition != null)
                        {
                            NotationBuilder.Append("*");
                        }

                        Console.WriteLine("Command: [" + NotationBuilder.ToString() + "]");

                        NotationParser.ParseString(NotationBuilder.ToString());
                        UpdateChessPieceTexture();
                        movingPosition.UpdateChessPieceTexture();
                        OnRightClick();
                    }
                }
            }
        }

        public ChessBoardSprite GetBoardPieceByCoord(string coord)
        {
            ChessBoardSprite foundSprite = null;

            List<ChessBoardSprite> allSprites = GetAllChessBoardPieces();

            foreach(ChessBoardSprite c in allSprites)
            {
                if (c.BoardPosition.ToCoordinate() == coord)
                {
                    foundSprite = c;
                    break;
                }
            }

            return foundSprite;
        }

        private List<ChessBoardSprite> GetAllChessBoardPieces()
        {
            List<ChessBoardSprite> sprites = new List<ChessBoardSprite>();

            List<Sprite> allSpritesFound = ChessGame.Instance.CurrentScene.Sprites
                                                .Where(s => s.GetType() == typeof(ChessBoardSprite)).ToList();

            allSpritesFound.ForEach(s => sprites.Add((ChessBoardSprite)s));

            return sprites;
        }

        public void OnRightClick()
        {
            IsSelected = false;
            ((ChessGameScene)ChessGame.Instance.CurrentScene).HasSelectedRootNode = false;
            GetAllChessBoardPieces().ForEach(m => m.ResetColor());
        }
    }
}
