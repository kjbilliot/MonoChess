using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoChess.Engine.Pieces;
using MonoChess.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Scenes
{
    class ChessGameScene : Scene
    {
        private const int TextureDimension = 80;
        private ContentManager content;
        public bool HasSelectedRootNode = false;
        public ChessBoardSprite RootNode;

        public ChessGameScene(ContentManager cm)
        {
            content = cm;
        }

        public override void Play()
        {
            AddChessBoard();
            AddChessPieces();
        }

        private void AddChessPieces()
        {

        }

        private void AddChessBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    bool isDark = ((x + y) % 2 != 0);
                    PieceMaterial material = (isDark ? PieceMaterial.D : PieceMaterial.L);
                    string textureFilename = "chessboard_" + (isDark ? "dark" : "light") + ".png";

                    ChessBoardSprite boardSprite = new ChessBoardSprite(content, material,
                        new Vector2D(x * TextureDimension, y * TextureDimension), new Vector2D(x, y));

                    Sprites.Add(boardSprite);
                }
            }
        }

        public ChessBoardSprite FindTileByBoardCoord(string boardCoord)
        {
            ChessBoardSprite foundSprites = null;

            foreach (Sprite s in Sprites)
            {
                if (s.GetType() == typeof(ChessBoardSprite))
                {
                    if (s.BoardPosition.ToCoordinate() == boardCoord)
                    {
                        foundSprites = (ChessBoardSprite)s;
                    }
                }
            }

            return foundSprites;
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
