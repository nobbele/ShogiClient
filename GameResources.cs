using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class GameResources
    {
        public Texture2D Tile;
        public Texture2D Piece;
        public SpriteFont PieceFont;
        public Texture2D MoveIndicator;

        public void LoadContent(ContentManager content)
        {
            Tile = content.Load<Texture2D>("Shogi Tile");
            Piece = content.Load<Texture2D>("Shogi Piece");
            PieceFont = content.Load<SpriteFont>("Piece Font");
            MoveIndicator = content.Load<Texture2D>("Shogi Move Indicator");
        }
    }
}