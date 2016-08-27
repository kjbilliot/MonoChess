using Microsoft.Xna.Framework.Graphics;
using MonoChess.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine
{
    public abstract class Scene
    {
        public List<Sprite> Sprites;
        public Scene()
        {
            Sprites = new List<Sprite>();
        }

        public abstract void Play();

        public virtual void Update()
        {
            foreach (Sprite s in Sprites)
            {
                s.Update();
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Sprite s in Sprites)
            {
                spriteBatch.Draw(s.Texture, s.BoundingBox, s.Tint);
                s.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

    }
}
