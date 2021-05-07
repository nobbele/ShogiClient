using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class DrawableHand
    {
        public PlayerData PlayerData { get; init; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public Vector2 Size => GetTileOffsetFor(19) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        public Vector2 GetTileOffsetFor(int x) => new Vector2((TileSize.X + 1) * x, 0);

        private GameResources resources;

        public DrawableHand(GameResources resources)
        {
            this.resources = resources;
        }

        public (int X, int Y) GetIndexForCoordinate(Vector2 position)
        {
            var topLeftPlayerHand = Position - Size / 2 - TileSize / 2;
            var positionOnHand = position - topLeftPlayerHand;
            int x = (int)Math.Floor(positionOnHand.X / TileSize.X);
            int y = (int)Math.Floor(positionOnHand.Y / TileSize.Y);

            return (x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < 20; x++)
            {
                var tilePosition = Position - Size / 2 + GetTileOffsetFor(x);

                spriteBatch.Draw(resources.Tile, tilePosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                if (PlayerData.Hand.Count > x)
                {
                    PieceType type = PlayerData.Hand[x];
                    // The second parameter (isPlayerOne) is used to get the jewel king,
                    // since you can't capture the king it doesn't matter what the value is.
                    // The third parameter (promoted) is used to get promoted character.
                    // since captured pieces automatically demote, it should be set to false.
                    var piecePrint = Utils.PieceTypeToKanji(type, false, false);

                    // Can not scale the string as the character will become ugly
                    spriteBatch.DrawString(
                        resources.PieceFont,
                        piecePrint,
                        tilePosition - resources.PieceFont.MeasureString(piecePrint) / 2,
                        Color.Black
                    );
                }
            }
        }
    }
}