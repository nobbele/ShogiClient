using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   UI Object that renders a rectangle with a color.
    /// </summary>
    public class UIPanel
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color Color { get; set; } = new Color(Color.Black, 0.6f);

        private Texture2D tex;

        public UIPanel(Game game)
        {
            var color = Color.White;
            tex = new Texture2D(game.GraphicsDevice, 1, 1);
            tex.SetData(new Color[] { color });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, new Rectangle(Position.ToPoint(), Size.ToPoint()), null, Color);
        }
    }
}