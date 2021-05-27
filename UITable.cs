using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class UITable
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color PanelColor { get; set; } = new Color(Color.Black, 0.6f);
        public Color BorderColor { get; set; } = Color.White;
        public int EntryHeight;

        private int scrolledAmount = 0;
        public int ScrolledAmount { get => ClampScroll(scrolledAmount); set => scrolledAmount = ClampScroll(value); }
        private int ClampScroll(int v)
        {
            return MathHelper.Clamp(v, 0, MathHelper.Max(0, ((int)MathF.Ceiling(Data.Count / 2.0f)) - entryVerticalCount));
        }

        public int TableWidth { get; set; }
        public List<string> Data { get; } = new List<string>();

        private int entryHorizontalCount => (int)(Size.X / TableWidth);
        private int entryVerticalCount => (int)(Size.Y / EntryHeight);

        private Texture2D tex;

        private GameResources resources;

        private int oldScrollValue = 0;

        public UITable(Game game, GameResources resources)
        {
            this.resources = resources;

            var color = Color.White;
            tex = new Texture2D(game.GraphicsDevice, 1, 1);
            tex.SetData(new Color[] { color });
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            var scrollDelta = mouseState.ScrollWheelValue - oldScrollValue;
            if (scrollDelta != 0)
            {
                ScrolledAmount -= Math.Sign(scrollDelta);
                oldScrollValue = mouseState.ScrollWheelValue;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint()), null, PanelColor);
            for (int i = 0; i < Math.Min(entryVerticalCount * 2, Data.Count - (ScrolledAmount * 2)); i++)
            {
                var entryData = Data[(ScrolledAmount * 2) + i];
                if (entryData != null)
                {
                    var entryPosition = new Vector2(Position.X + entryHorizontalCount * (i % TableWidth), Position.Y + EntryHeight * (i / TableWidth))
                        + new Vector2(entryHorizontalCount / 2, EntryHeight / 2)
                        - resources.PieceFont.MeasureString(entryData) / 2
                        - Size / 2;
                    spriteBatch.DrawString(resources.PieceFont, entryData, entryPosition, Color.White);
                }
            }

            for (int y = 0; y < entryVerticalCount; y++)
            {
                var horLinePosition = new Vector2(Position.X, Position.Y + EntryHeight * y)
                    - Size / 2;
                spriteBatch.Draw(tex, new Rectangle(horLinePosition.ToPoint(), new Point(entryHorizontalCount * 2, 1)), null, BorderColor);
            }

            var verLinePosition = new Vector2(Position.X + entryHorizontalCount, Position.Y)
                - Size / 2;
            spriteBatch.Draw(tex, new Rectangle(verLinePosition.ToPoint(), new Point(1, (int)Size.Y)), null, BorderColor);
        }
    }
}