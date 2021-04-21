using System;
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

        public Vector2 Size => GetTileOffsetFor(Data.Width - 1, Data.Height - 1) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        private GameResources resources;

        public DrawableBoard(GameResources resources, int width, int height) : base(width, height)
        {
            this.resources = resources;
        }

        public Vector2 GetTileOffsetFor(int x, int y) => new Vector2((TileSize.X - 1) * x, (TileSize.Y - 1) * y);

        public (int X, int Y) GetTileForCoordinate(Vector2 position)
        {
            var topLeft = Position - Size / 2 - TileSize / 2;
            var positionOnBoard = position - topLeft;
            int tileX = (int)Math.Floor(positionOnBoard.X / TileSize.X);
            int tileY = (int)Math.Floor(positionOnBoard.Y / TileSize.Y);

            return (tileX, tileY);
        }

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

#pragma warning disable CS0162
                    if (Game1.DEBUG_DISPLAY)
                    {
                        spriteBatch.DrawString(resources.PieceFont, $"{x},{y}", tilePosition - resources.Tile.Bounds.Size.ToVector2() * Scale / 2, Color.Snow);
                    }
#pragma warning restore CS0162
                }
            }

            if (HeldPiece != null)
            {
                if (HeldPiecePickUpPosition is (int, int) pickUpPosition)
                {
                    var validMoves = Utils.ValidMovesForPiece(HeldPiece, Data, pickUpPosition.X, pickUpPosition.Y, HeldPiece.IsPlayerOne);

                    foreach (var validMove in validMoves)
                    {
                        var indicatorPosition = Position
                            - Size / 2
                            + GetTileOffsetFor(validMove.X, validMove.Y);
                        spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    }
                }

                DrawPiece(spriteBatch, HeldPiece, HeldPiecePosition);
            }
        }
    }
}