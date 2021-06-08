using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    /// <summary>
    ///   UI Object that renders a table with a specified width and height.
    /// </summary>
    public class UITable
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color PanelColor { get; set; } = new Color(Color.Black, 0.6f);
        public Color BorderColor { get; set; } = Color.White;
        /// <summary>
        ///   The height of a single table entry
        /// </summary>
        public int EntryHeight;

        private int scrolledAmount = 0;
        /// <summary>
        ///   How many entries the table has been scrolled by
        /// </summary>
        public int ScrolledAmount
        {
            get => ClampScroll(scrolledAmount);
            set => scrolledAmount = ClampScroll(value);
        }
        /// <summary>
        ///   Computes the lower and upper bound of the amount of entries you can scroll and sets v to be within that.
        /// </summary>
        private int ClampScroll(int v)
        {
            return MathHelper.Clamp(v, 0, MathHelper.Max(0, ((int)MathF.Ceiling(Data.Count / 2.0f)) - entryVerticalCount));
        }

        /// <summary>
        ///   Width of the entire table
        /// </summary>
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
            // Draws the border
            spriteBatch.Draw(tex, new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint()), null, PanelColor);
            // i will iterate from 0 to either the max amount of entries or to the amount of entries currently visible by the scrolled amount
            for (int i = 0; i < Math.Min(entryVerticalCount * entryHorizontalCount, Data.Count - (ScrolledAmount * entryHorizontalCount)); i++)
            {
                // Adds i to the index to the first row that has been scrolled to
                var entryData = Data[(ScrolledAmount * entryHorizontalCount) + i];
                if (entryData != null)
                {
                    // Calculate the position of the entry by taking the position of the board added with the index times the dimensions
                    // and then adds centering with respective to different componenets.
                    var entryPosition = new Vector2(Position.X + entryHorizontalCount * (i % TableWidth), Position.Y + EntryHeight * (i / TableWidth))
                        + new Vector2(entryHorizontalCount / 2, EntryHeight / 2)
                        - resources.PieceFont.MeasureString(entryData) / 2
                        - Size / 2;
                    spriteBatch.DrawString(resources.PieceFont, entryData, entryPosition, Color.White);
                }
            }

            // Draws the horizontal lines
            for (int y = 0; y < entryVerticalCount; y++)
            {
                var horLinePosition = new Vector2(Position.X, Position.Y + EntryHeight * y)
                    - Size / 2;
                spriteBatch.Draw(tex, new Rectangle(horLinePosition.ToPoint(), new Point(entryHorizontalCount * 2, 1)), null, BorderColor);
            }

            // Draws the vertical line (assumed to have 2 horizontal items, it's more complicated to solve the general case).
            var verLinePosition = new Vector2(Position.X + entryHorizontalCount, Position.Y)
                - Size / 2;
            spriteBatch.Draw(tex, new Rectangle(verLinePosition.ToPoint(), new Point(1, (int)Size.Y)), null, BorderColor);
        }
    }
}