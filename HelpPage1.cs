using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class HelpPage1 : HelpPage
    {
        private UIMiniBoard pawnBoard;
        private UIMiniBoard bishopBoard;
        private UIMiniBoard rookBoard;

        public HelpPage1(GameResources resources, Game1 game) : base(resources, game)
        {
            pawnBoard = new UIMiniBoard(resources, 3, 3)
            {
                Position = new Vector2(Game.WindowSize.X / 4 - 70, 300),
                DrawMovesPiece = new Point(1, 1),
                Scale = new Vector2(2f, 2f)
            };
            pawnBoard.Data.SetAt(1, 1, new PieceData()
            {
                Type = PieceType.Pawn,
                IsPlayerOne = true,
            });

            bishopBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X * 2 / 4 - 60, 350),
                DrawMovesPiece = new Point(2, 2),
                Scale = new Vector2(2f, 2f)
            };
            bishopBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Bishop,
                IsPlayerOne = true,
            });

            rookBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X * 3 / 4, 350),
                DrawMovesPiece = new Point(2, 2),
                Scale = new Vector2(2f, 2f)
            };
            rookBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Rook,
                IsPlayerOne = true,
            });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var title = "The Pieces Part 1";
            spriteBatch.DrawString(Resources.PieceFont, title, new Vector2(Game.WindowSize.X / 2, 150) - Resources.PieceFont.MeasureString(title) / 2, Color.White);

            var pawnTitle = "Pawn";
            spriteBatch.DrawString(Resources.PieceFont, pawnTitle, new Vector2(Game.WindowSize.X / 4 - 70, 200) - Resources.PieceFont.MeasureString(pawnTitle) / 2, Color.White);
            pawnBoard.Draw(spriteBatch);
            var pawnDescription = "Pawns can move one tile forward";
            spriteBatch.DrawString(Resources.PieceFont, pawnDescription, new Vector2(Game.WindowSize.X / 4 - 70, 400) - Resources.PieceFont.MeasureString(pawnDescription) / 2, Color.White);

            var bishopTitle = "Bishop";
            spriteBatch.DrawString(Resources.PieceFont, bishopTitle, new Vector2(Game.WindowSize.X * 2 / 4 - 60, 200) - Resources.PieceFont.MeasureString(bishopTitle) / 2, Color.White);
            bishopBoard.Draw(spriteBatch);
            var bishopDescription = "Bishops can move as far as they can\nin any diagonal direction";
            spriteBatch.DrawString(Resources.PieceFont, bishopDescription, new Vector2(Game.WindowSize.X * 2 / 4 - 60, 515) - Resources.PieceFont.MeasureString(bishopDescription) / 2, Color.White);

            var rookTitle = "Rook";
            spriteBatch.DrawString(Resources.PieceFont, rookTitle, new Vector2(Game.WindowSize.X * 3 / 4, 200) - Resources.PieceFont.MeasureString(rookTitle) / 2, Color.White);
            rookBoard.Draw(spriteBatch);
            var rookDescription = "Same as bishop\nbut cardinal instead of diagonal";
            spriteBatch.DrawString(Resources.PieceFont, rookDescription, new Vector2(Game.WindowSize.X * 3 / 4, 515) - Resources.PieceFont.MeasureString(rookDescription) / 2, Color.White);
        }
    }
}