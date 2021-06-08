using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   UI Object that renders Text, horizontally and vertically centered.
    /// </summary>
    public class UIText
    {
        public Vector2 Position { get; set; }
        public string Text { get; set; }

        private GameResources resources;

        public UIText(GameResources resources)
        {
            this.resources = resources;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(resources.PieceFont, Text, Position - resources.PieceFont.MeasureString(Text) / 2, Color.White);
        }
    }
}