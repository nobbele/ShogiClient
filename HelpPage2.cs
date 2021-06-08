using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   Help Page 2
    /// </summary>
    public class HelpPage2 : HelpPage
    {
        private UIMiniBoard lanceBoard;
        private UIMiniBoard knightBoard;
        private UIMiniBoard silverBoard;

        public HelpPage2(GameResources resources, Game1 game) : base(resources, game)
        {
            lanceBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X / 4, 350),
                DrawMovesPiece = new Point(2, 2),
                Scale = new Vector2(2f, 2f)
            };
            lanceBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Lance,
                IsPlayerOne = true,
            });

            knightBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X * 2 / 4, 350),
                DrawMovesPiece = new Point(2, 2),
                Scale = new Vector2(2f, 2f)
            };
            knightBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Knight,
                IsPlayerOne = true,
            });
            knightBoard.Data.SetAt(1, 1, new PieceData()
            {
                Type = PieceType.Pawn,
                IsPlayerOne = false,
            });
            knightBoard.Data.SetAt(2, 1, new PieceData()
            {
                Type = PieceType.Pawn,
                IsPlayerOne = false,
            });
            knightBoard.Data.SetAt(3, 1, new PieceData()
            {
                Type = PieceType.Pawn,
                IsPlayerOne = false,
            });

            silverBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X * 3 / 4, 350),
                DrawMovesPiece = new Point(2, 2),
                Scale = new Vector2(2f, 2f)
            };
            silverBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Silver,
                IsPlayerOne = true,
            });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var title = "The Pieces Part 2";
            spriteBatch.DrawString(Resources.PieceFont, title, new Vector2(Game.WindowSize.X / 2, 150) - Resources.PieceFont.MeasureString(title) / 2, Color.White);

            var lanceTitle = "Lance";
            spriteBatch.DrawString(Resources.PieceFont, lanceTitle, new Vector2(Game.WindowSize.X / 4, 200) - Resources.PieceFont.MeasureString(lanceTitle) / 2, Color.White);
            lanceBoard.Draw(spriteBatch);
            var lanceDescription = "Similar to rook\nbut it can only move forward";
            spriteBatch.DrawString(Resources.PieceFont, lanceDescription, new Vector2(Game.WindowSize.X / 4, 515) - Resources.PieceFont.MeasureString(lanceDescription) / 2, Color.White);

            var knightTitle = "Knight";
            spriteBatch.DrawString(Resources.PieceFont, knightTitle, new Vector2(Game.WindowSize.X * 2 / 4, 200) - Resources.PieceFont.MeasureString(knightTitle) / 2, Color.White);
            knightBoard.Draw(spriteBatch);
            var knightDescription = "Knights move in an L-shape\nand they can jump\nover other pieces";
            spriteBatch.DrawString(Resources.PieceFont, knightDescription, new Vector2(Game.WindowSize.X * 2 / 4, 515) - Resources.PieceFont.MeasureString(knightDescription) / 2, Color.White);

            var silverTitle = "Silver General";
            spriteBatch.DrawString(Resources.PieceFont, silverTitle, new Vector2(Game.WindowSize.X * 3 / 4, 200) - Resources.PieceFont.MeasureString(silverTitle) / 2, Color.White);
            silverBoard.Draw(spriteBatch);
            var silverDescription = "Silvers can move in\nany diagonal direction and\nit can't move in any cardinal direction\nother than forward";
            spriteBatch.DrawString(Resources.PieceFont, silverDescription, new Vector2(Game.WindowSize.X * 3 / 4, 535) - Resources.PieceFont.MeasureString(silverDescription) / 2, Color.White);
        }
    }
}