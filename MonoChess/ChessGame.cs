using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoChess.Engine;
using MonoChess.Engine.Pieces;
using System.Collections.Generic;
using System;
using MonoChess.Engine.Rendering;
using MonoChess.Engine.Scenes;
using System.Linq;

namespace MonoChess
{
    public class ChessGame : Game
    {
        public static volatile ChessBoard ChessBoard = new ChessBoard();

        public SpriteBatch SpriteBatch;
        public GraphicsDeviceManager Graphics;

        public const int WindowDimension = 640;
        private static ChessGame _instance = null;

        public Scene CurrentScene;

        public static ChessGame Instance
        {   
            get
            {
                if (_instance == null) _instance = new ChessGame();
                return _instance;
            }
        }

        public ChessGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = WindowDimension;
            Graphics.PreferredBackBufferHeight = WindowDimension;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            CurrentScene = new ChessGameScene(Content);
            CurrentScene.Play();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            MouseInteraction.Update();
            CurrentScene.Update();

            base.Update(gameTime);
        }

        private const string rawIntToBoardLetter = "abcdefgh";

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CurrentScene.Draw(SpriteBatch);

            base.Draw(gameTime);
        }

    }
}
