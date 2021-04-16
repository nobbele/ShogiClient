using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class DrawableBoard : Board
    {
        public Vector2 Position { get; init; }
        public Vector2 Scale { get; init; } = Vector2.One;

        public Vector2 Size => GetTileOffsetFor(Data.Width, Data.Height);

        private GameResources resources;

        public DrawableBoard(GameResources resources, int width, int height) : base(width, height)
        {
            this.resources = resources;
        }

        public Vector2 GetTileOffsetFor(int x, int y) => new Vector2((32 * Scale.X - 1) * x, (26 * Scale.Y - 1) * y);

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Data.Height; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    // remove 1 from width(32) and height(25) so there isn't a double border between two tiles
                    // Base position, add the offset for the tile and then center it
                    var position = Position + GetTileOffsetFor(x, y) - Size / 2;

                    spriteBatch.Draw(resources.Tile, position, null, Data.GetAt(x, y)?.Type switch
                    {
                        PieceType.Pawn => Color.Red,
                        PieceType.Bishop => Color.Pink,
                        PieceType.Knight => Color.Teal,
                        PieceType.Rook => Color.Yellow,
                        PieceType.Lance => Color.Turquoise,
                        PieceType.Gold => Color.Gold,
                        PieceType.Silver => Color.Silver,
                        PieceType.King => Color.Green,
                        _ => Color.White
                    }, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}