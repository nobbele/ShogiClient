using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   UI Object that renders a board with pieces and their allowed move targets.
    /// </summary>
    public class UIMiniBoard
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public Point DrawMovesPiece { get; set; }

        public Grid<PieceData> Data { get; private set; }

        public Vector2 Size => GetTileOffsetFor(new Point(Data.Width - 1, Data.Height - 1)) + TileSize;
        public Vector2 TileSize => resources.Tile.Bounds.Size.ToVector2() * Scale;

        private GameResources resources;

        // These don't matter for now, it's just required to check valid moves
        private PlayerData playerA = new PlayerData();
        private PlayerData playerB = new PlayerData();

        public UIMiniBoard(GameResources resources, int width, int height)
        {
            this.resources = resources;
            Data = new Grid<PieceData>(width, height);
        }

        public Vector2 GetTileOffsetFor(Point point) => new Vector2((TileSize.X - 1) * point.X, (TileSize.Y - 1) * point.Y);

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
            for (int y = 0; y < Data.Height; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    var tilePosition = Position - Size / 2 + GetTileOffsetFor(new Point(x, y));

                    spriteBatch.Draw(resources.Tile, tilePosition, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);

                    var piece = Data.GetAt(x, y).Data;
                    if (piece != null)
                    {
                        DrawPiece(spriteBatch, piece, tilePosition + TileSize / 2);
                    }
                }
            }

            var drawMovesPiece = Data.GetAt(DrawMovesPiece);
            List<Point> validMoves;
            validMoves = Utils.ValidMovesForPiece(drawMovesPiece, Data, playerA, playerB);

            foreach (var validMove in validMoves)
            {
                var indicatorPosition = Position
                    - Size / 2
                    + GetTileOffsetFor(validMove)
                    + TileSize / 2;
                spriteBatch.Draw(resources.MoveIndicator, indicatorPosition - resources.MoveIndicator.Bounds.Size.ToVector2() / 2, null, Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
        }
    }
}