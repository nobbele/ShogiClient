using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class GameResources
    {
        public Texture2D Tile;

        public void LoadContent(ContentManager content)
        {
            Tile = content.Load<Texture2D>("Shogi Tile");
        }
    }
}