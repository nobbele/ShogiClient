using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   An object that contains properties and methods used to be able to draw board used for gameplay.
    /// </summary>
    public class DrawableBoard
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public Board State { get; set; }

        public Vector2 Size => GetTileOffsetFor(new Point(State.Data.Width - 1, State.Data.Height - 1)) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        private GameResources resources;

        public DrawableBoard(GameResources resources, Board state)
        {
            this.resources = resources;
            State = state;
        }

        public Vector2 GetTileOffsetFor(Point point) => new Vector2((TileSize.X - 1) * point.X, (TileSize.Y - 1) * point.Y);

        public Point GetTileForCoordinate(Vector2 position)
        {
            var topLeft = Position - Size / 2;
            var positionOnBoard = position - topLeft;
            int tileX = (int)Math.Floor(positionOnBoard.X / TileSize.X);
            int tileY = (int)Math.Floor(positionOnBoard.Y / TileSize.Y);

            return new Point(tileX, tileY);
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

            var piecePrint = Utils.PieceToKanji(piece);

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
                    var tilePosition = Position - Size / 2 + GetTileOffsetFor(new Point(x, y));

                    spriteBatch.Draw(resources.Tile, tilePosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

                    var piece = State.Data.GetAt(x, y).Data;
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
                if (State.HeldPiecePickUpPosition is Point pickUpPosition)
                {
                    validMoves = Utils.ValidMovesForPiece(new GridRef<PieceData> { Data = State.HeldPiece, Position = pickUpPosition }, State.Data, State.GameplayState.CurrentPlayer, State.GameplayState.OpponentPlayer);
                }
                else
                {
                    validMoves = Utils.ValidPositionsForPieceDrop(State.HeldPiece, State.Data, State.GameplayState.CurrentPlayer, State.GameplayState.OpponentPlayer);
                }

                foreach (var validMove in validMoves)
                {
                    var indicatorPosition = Position
                        - Size / 2
                        + GetTileOffsetFor(validMove)
                        + TileSize / 2;
                    spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() * Scale / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }

                DrawPiece(spriteBatch, State.HeldPiece, State.HeldPiecePosition);
            }
        }
    }
}