using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class HelpPage3 : HelpPage
    {
        private UIMiniBoard goldBoard;
        private UIMiniBoard kingBoard;

        public HelpPage3(GameResources resources, Game1 game) : base(resources, game)
        {
            goldBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X / 3, 350),
                DrawMovesPiece = (2, 2),
                Scale = new Vector2(2f, 2f)
            };
            goldBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.Gold,
                IsPlayerOne = true,
            });

            kingBoard = new UIMiniBoard(resources, 5, 5)
            {
                Position = new Vector2(Game.WindowSize.X * 2 / 3, 350),
                DrawMovesPiece = (2, 2),
                Scale = new Vector2(2f, 2f)
            };
            kingBoard.Data.SetAt(2, 2, new PieceData()
            {
                Type = PieceType.King,
                IsPlayerOne = true,
            });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var title = "The Pieces Part 3";
            spriteBatch.DrawString(Resources.PieceFont, title, new Vector2(Game.WindowSize.X / 2, 150) - Resources.PieceFont.MeasureString(title) / 2, Color.White);

            var goldTitle = "Gold";
            spriteBatch.DrawString(Resources.PieceFont, goldTitle, new Vector2(Game.WindowSize.X / 3, 200) - Resources.PieceFont.MeasureString(goldTitle) / 2, Color.White);
            goldBoard.Draw(spriteBatch);
            var goldDescription = "Similar to silver but can move cardinal non-forward\nand can't move diagonally back";
            spriteBatch.DrawString(Resources.PieceFont, goldDescription, new Vector2(Game.WindowSize.X / 3, 515) - Resources.PieceFont.MeasureString(goldDescription) / 2, Color.White);

            var kingTitle = "King";
            spriteBatch.DrawString(Resources.PieceFont, kingTitle, new Vector2(Game.WindowSize.X * 2 / 3, 200) - Resources.PieceFont.MeasureString(kingTitle) / 2, Color.White);
            kingBoard.Draw(spriteBatch);
            var kingDescription = "The king can move one step in any direction.\nMust be protected at all cost.";
            spriteBatch.DrawString(Resources.PieceFont, kingDescription, new Vector2(Game.WindowSize.X * 2 / 3, 515) - Resources.PieceFont.MeasureString(kingDescription) / 2, Color.White);
        }
    }
}