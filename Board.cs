using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class Board
    {
        public Vector2 Position { get; init; }
        public int TileHorizontalCount { get; init; }
        public int TileVerticalCount { get; init; }
        public Vector2 Scale { get; init; } = Vector2.One;

        public Vector2 Size => GetTileOffsetFor(TileHorizontalCount, TileVerticalCount);

        private GameResources resources;

        public Board(GameResources resources)
        {
            this.resources = resources;
        }

        public Vector2 GetTileOffsetFor(int x, int y) => new Vector2((32 * Scale.X - 1) * x, (26 * Scale.Y - 1) * y);

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < TileVerticalCount; y++)
            {
                for (int x = 0; x < TileHorizontalCount; x++)
                {
                    // remove 1 from width(32) and height(25) so there isn't a double border between two tiles
                    // Base position, add the offset for the tile and then center it
                    var position = Position + GetTileOffsetFor(x, y) - Size / 2;

                    spriteBatch.Draw(resources.Tile, position, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}