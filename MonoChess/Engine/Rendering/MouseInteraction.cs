using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChess.Engine.Rendering
{
    public static class MouseInteraction
    {
        public static Vector2D Position;
        public static bool IsLeftMouseDown;
        public static bool IsRightMouseDown;
        private static MouseState oldState;

        public delegate void MouseLeftClick();
        public delegate void MouseRightClick();
        public static event MouseLeftClick LeftClick;
        public static event MouseRightClick RightClick;

        public static void Update()
        {
            var newState = Mouse.GetState();
            Position = new Vector2D(newState.X, newState.Y);

            if (oldState.LeftButton == ButtonState.Released && newState.LeftButton == ButtonState.Pressed)
            {
                OnLeftClick();
            }

            if (oldState.RightButton == ButtonState.Released && newState.RightButton == ButtonState.Pressed)
            {
                OnRightClick();
            }

            oldState = newState;
        }

        public static void OnLeftClick()
        {
            if (LeftClick != null)
            {
                LeftClick.Invoke();
            }
        }

        public static void OnRightClick()
        {
            if (RightClick != null)
            {
                RightClick.Invoke();
            }
        }
    }
}
