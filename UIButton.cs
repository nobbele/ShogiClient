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
        public string Text { get; set; }
        public event Action OnClick;

        private bool isBeingClicked = false;

        private Rectangle RectOnScreen => new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint());

        private GameResources resources;

        public UIButton(GameResources resources)
        {
            this.resources = resources;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (isBeingClicked)
            {
                if (!RectOnScreen.Contains(mouseState.Position))
                {
                    isBeingClicked = false;
                }

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    OnClick.Invoke();
                    isBeingClicked = false;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (prevMouseState.LeftButton == ButtonState.Released)
                {
                    isBeingClicked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(resources.UIButton, RectOnScreen, null, Color.White);
            spriteBatch.DrawString(resources.PieceFont, Text, RectOnScreen.Center.ToVector2() - resources.PieceFont.MeasureString(Text) / 2, Color.White);
        }
    }
}