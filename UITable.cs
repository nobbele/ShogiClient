using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class UITable
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color PanelColor { get; set; } = new Color(Color.Black, 0.6f);
        public Color BorderColor { get; set; } = Color.White;
        public Grid<string> Data { get; set; }
        public int EntryHeight;

        private int entryWidth => (int)(Size.X / Data.Width);

        private Texture2D tex;

        private GameResources resources;

        public UITable(Game game, GameResources resources)
        {
            this.resources = resources;

            var color = Color.White;
            tex = new Texture2D(game.GraphicsDevice, 1, 1);
            tex.SetData(new Color[] { color });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle((Position - Size / 2).ToPoint(), Size.ToPoint()), null, PanelColor);
            for (int y = 0; y < Data.Height; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    var entryData = Data.GetAt(x, y);
                    if (entryData != null)
                    {
                        var entryPosition = new Vector2(Position.X + entryWidth * x, Position.Y + EntryHeight * y)
                            + new Vector2(entryWidth / 2, EntryHeight / 2)
                            - resources.PieceFont.MeasureString(entryData) / 2
                            - Size / 2;
                        spriteBatch.DrawString(resources.PieceFont, entryData, entryPosition, Color.White);
                    }
                }
            }

            for (int y = 0; y < (Size.Y / EntryHeight); y++)
            {
                var horLinePosition = new Vector2(Position.X, Position.Y + EntryHeight * y)
                    - Size / 2;
                spriteBatch.Draw(tex, new Rectangle(horLinePosition.ToPoint(), new Point(entryWidth * 2, 1)), null, BorderColor);
            }

            var verLinePosition = new Vector2(Position.X + entryWidth, Position.Y)
                - Size / 2;
            spriteBatch.Draw(tex, new Rectangle(verLinePosition.ToPoint(), new Point(1, (int)Size.Y)), null, BorderColor);
        }
    }
}