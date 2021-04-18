using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class DrawableBoard : Board
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public Vector2 Size => GetTileOffsetFor(Data.Width - 1, Data.Height - 1);
        public Vector2 TileSize => new Vector2(32 * Scale.X, 26 * Scale.Y);

        public Vector2 HeldPiecePosition { get; set; } = new Vector2(0, 0);
        public (int X, int Y) HeldPiecePickUpPosition { get; set; } = (0, 0);

        private GameResources resources;

        public DrawableBoard(GameResources resources, int width, int height) : base(width, height)
        {
            this.resources = resources;
        }

        public Vector2 GetTileOffsetFor(int x, int y) => new Vector2((TileSize.X - 1) * x, (TileSize.Y - 1) * y);

        private void DrawPiece(SpriteBatch spriteBatch, PieceData piece, Vector2 tilePosition)
        {
            spriteBatch.Draw(
                resources.Piece,
                tilePosition - resources.Piece.Bounds.Size.ToVector2() * Scale / 2,
                null,
                Color.White,
                0,
                Vector2.Zero,
                Scale,
                piece.IsPlayerOne ? SpriteEffects.None : SpriteEffects.FlipVertically,
                0
            );

            var piecePrint = Utils.PieceTypeToKanji(piece.Type, piece.IsPlayerOne, piece.Promoted);

            var piecePrintPosition = tilePosition + (piece.IsPlayerOne ? new Vector2(0, 2) : new Vector2(0, -4)) * Scale;

            // Can not scale the string as the character will become ugly
            spriteBatch.DrawString(
                resources.PieceFont,
                piecePrint,
                piecePrintPosition - resources.PieceFont.MeasureString(piecePrint) / 2,
                piece.Promoted ? Color.Red : Color.Black
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < Data.Height; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    var tilePosition = Position - Size / 2 + GetTileOffsetFor(x, y);

                    spriteBatch.Draw(resources.Tile, tilePosition - resources.Tile.Bounds.Size.ToVector2() * Scale / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

                    var piece = Data.GetAt(x, y);
                    if (piece != null)
                    {
                        DrawPiece(spriteBatch, piece, tilePosition);
                    }
                }
            }

            if (HeldPiece != null)
            {
                var validMoves = Utils.ValidMovesForPiece(HeldPiece, Data, HeldPiecePickUpPosition.X, HeldPiecePickUpPosition.Y, HeldPiece.IsPlayerOne);

                foreach (var validMove in validMoves)
                {
                    var indicatorPosition = Position
                        - Size / 2
                        + GetTileOffsetFor(validMove.X, validMove.Y);
                    spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }

                DrawPiece(spriteBatch, HeldPiece, HeldPiecePosition);
            }
        }
    }
}