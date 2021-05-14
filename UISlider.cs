using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class UISlider
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public float CurrentProgress { get; set; }
        public float CurrentValue { get; private set; }
        public float Min { get; set; } = 0;
        public float Max { get; set; } = 1;
        public event Action<float> OnChange;
        public event Action<float> OnRelease;

        private Rectangle RectOnScreen => new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint());

        private bool isBeingClicked = false;

        private GameResources resources;

        public UISlider(GameResources resources)
        {
            this.resources = resources;
        }

        public void SetCurrentValue(float value)
        {
            CurrentValue = value;
            CurrentProgress = (value - Min) / (Max - Min);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (isBeingClicked)
            {
                var progress = Math.Clamp((mouseState.Position.X - RectOnScreen.Left) / Size.X, 0, 1);
                var value = Min + (Max - Min) * progress;

                if (mouseState.LeftButton == ButtonState.Released)
                {
                    OnRelease?.Invoke(value);
                    isBeingClicked = false;
                }
                else if (progress != CurrentProgress)
                {
                    OnChange?.Invoke(value);
                }

                CurrentProgress = progress;
                CurrentValue = value;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && RectOnScreen.Contains(mouseState.Position))
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
            spriteBatch.Draw(resources.UIButton, new Rectangle(RectOnScreen.Location, new Point((int)(RectOnScreen.Size.X * CurrentProgress), RectOnScreen.Size.Y)), null, Color.Black);
        }
    }
}