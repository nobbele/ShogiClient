using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class DrawableBoard
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public Board State { get; set; }

        public Vector2 Size => GetTileOffsetFor(State.Data.Width - 1, State.Data.Height - 1) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        private GameResources resources;

        public DrawableBoard(GameResources resources, Board state)
        {
            this.resources = resources;
            State = state;
        }

        public Vector2 GetTileOffsetFor(int x, int y) => new Vector2((TileSize.X - 1) * x, (TileSize.Y - 1) * y);

        public (int X, int Y) GetTileForCoordinate(Vector2 position)
        {
            var topLeft = Position - Size / 2;
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
            for (int y = 0; y < State.Data.Height; y++)
            {
                for (int x = 0; x < State.Data.Width; x++)
                {
                    var tilePosition = Position - Size / 2 + GetTileOffsetFor(x, y);

                    spriteBatch.Draw(resources.Tile, tilePosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

                    var piece = State.Data.GetAt(x, y);
                    if (piece != null)
                    {
                        DrawPiece(spriteBatch, piece, tilePosition + TileSize / 2);
                    }

#pragma warning disable CS0162
                    if (Game1.DEBUG_DISPLAY)
                    {
                        spriteBatch.DrawString(resources.PieceFont, $"{x},{y}", tilePosition, Color.Snow);
                    }
#pragma warning restore CS0162
                }
            }

            if (State.HeldPiece != null)
            {
                List<Point> validMoves;
                if (State.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                {
                    validMoves = Utils.ValidMovesForPiece(State.HeldPiece, State.Data, new Point(pickUpPosition.X, pickUpPosition.Y));
                }
                else
                {
                    validMoves = Utils.ValidPositionsForPieceDrop(State.HeldPiece, State.Data);
                }

                foreach (var validMove in validMoves)
                {
                    var indicatorPosition = Position
                        - Size / 2
                        + GetTileOffsetFor(validMove.X, validMove.Y)
                        + TileSize / 2;
                    spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() * Scale / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }

                DrawPiece(spriteBatch, State.HeldPiece, State.HeldPiecePosition);
            }
        }
    }
}