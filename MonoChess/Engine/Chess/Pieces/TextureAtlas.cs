using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Chess.Pieces
{
    public static class TextureAtlas
    {
        public static Dictionary<string, Texture2D> PieceTextures = new Dictionary<string, Texture2D>
        {
            {"BishopL", ChessGame.Instance.Content.Load<Texture2D>("BishopW.png") },
            {"BishopD", ChessGame.Instance.Content.Load<Texture2D>("BishopB.png") },
            {"KingL", ChessGame.Instance.Content.Load<Texture2D>("KingW.png") },
            {"KingD", ChessGame.Instance.Content.Load<Texture2D>("KingB.png") },
            {"KnightL", ChessGame.Instance.Content.Load<Texture2D>("KnightW.png") },
            {"KnightD", ChessGame.Instance.Content.Load<Texture2D>("KnightB.png") },
            {"PawnL", ChessGame.Instance.Content.Load<Texture2D>("PawnW.png") },
            {"PawnD", ChessGame.Instance.Content.Load<Texture2D>("PawnB.png") },
            {"QueenL", ChessGame.Instance.Content.Load<Texture2D>("QueenW.png") },
            {"QueenD", ChessGame.Instance.Content.Load<Texture2D>("QueenB.png") },
            {"RookL", ChessGame.Instance.Content.Load<Texture2D>("RookW.png") },
            {"RookD", ChessGame.Instance.Content.Load<Texture2D>("RookB.png") },
            {"transparent", ChessGame.Instance.Content.Load<Texture2D>("transparent.png") }
        };
    }
}
