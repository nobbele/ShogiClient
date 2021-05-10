using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class UIMiniBoard
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public (int X, int Y) DrawMovesPiece { get; set; }

        public Grid<PieceData> Data { get; private set; }

        public Vector2 Size => GetTileOffsetFor(Data.Width - 1, Data.Height - 1) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        private GameResources resources;

        public UIMiniBoard(GameResources resources, int width, int height)
        {
            this.resources = resources;
            Data = new Grid<PieceData>(width, height);
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

                    spriteBatch.Draw(resources.Tile, tilePosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

                    var piece = Data.GetAt(x, y);
                    if (piece != null)
                    {
                        DrawPiece(spriteBatch, piece, tilePosition + TileSize / 2);
                    }
                }
            }

            var drawMovesPiece = Data.GetAt(DrawMovesPiece.X, DrawMovesPiece.Y);
            List<Point> validMoves;
            validMoves = Utils.ValidMovesForPiece(drawMovesPiece, Data, new Point(DrawMovesPiece.X, DrawMovesPiece.Y));
            //validMoves = Utils.ValidPositionsForPieceDrop(State.HeldPiece, State.Data);

            foreach (var validMove in validMoves)
            {
                var indicatorPosition = Position
                    - Size / 2
                    + GetTileOffsetFor(validMove.X, validMove.Y)
                    + TileSize / 2;
                spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
        }
    }
}