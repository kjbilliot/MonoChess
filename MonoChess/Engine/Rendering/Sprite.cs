using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Rendering
{
    public class Sprite
    {
        public Texture2D Texture;
        public Vector2D RenderPosition, BoardPosition;
        public Color Tint = Color.White;
        public Rectangle BoundingBox;
        public ContentManager Content;
        public float RotationAngle;

        public Sprite() { }

        public Sprite(ContentManager content, string textureName, Vector2D position)
        {
            Content = content;
            Texture = Content.Load<Texture2D>(textureName);
            RenderPosition = position;
            BoundingBox = new Rectangle(RenderPosition.X, RenderPosition.Y, RenderPosition.X + Texture.Width, RenderPosition.Y + Texture.Height);

        }

        public virtual void Update()
        {
            BoundingBox = new Rectangle(RenderPosition.X, RenderPosition.Y, Texture.Width, Texture.Height);
            
        }

        public virtual void Draw(SpriteBatch sb)
        {
            //RotationAngle += 0.1f;
            sb.Draw(Texture, BoundingBox, null, Tint, RotationAngle, new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }

    }
}
