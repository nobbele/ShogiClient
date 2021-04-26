using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class UIButton
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public event Action OnClick;

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (prevMouseState.LeftButton == ButtonState.Released)
                {

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}