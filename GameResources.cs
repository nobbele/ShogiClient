using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class GameResources
    {
        public Texture2D Tile;
        public Texture2D Piece;

        public void LoadContent(ContentManager content)
        {
            Tile = content.Load<Texture2D>("Shogi Tile");
            Piece = content.Load<Texture2D>("Shogi Piece");
        }
    }
}