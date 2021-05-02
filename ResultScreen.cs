using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class ResultScreen : Screen
    {
        // Have yet to implement a screen stack so this will have to do for now
        private GameplayScreenState gameplayState;
        private Texture2D background;

        private UIPanel panel;

        public ResultScreen(Game1 game, GameplayScreenState gameplayState, Texture2D background) : base(game)
        {
            this.gameplayState = gameplayState;
            this.background = background;
            panel = new UIPanel(game)
            {
                Position = new Vector2(100, 100),
                Size = Game.WindowSize - new Vector2(200, 200),
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                Game.SetCurrentScreen(new MainMenuScreen(Game));
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);

            var winner = gameplayState.IsPlayerOneTurn ? "Player 2" : "Player 1";
            var winnerText = $"{winner} Won!";
            spriteBatch.DrawString(Resources.PieceFont, winnerText, Game.WindowSize / 2 - Resources.PieceFont.MeasureString(winnerText) / 2, Color.White);
        }
    }
}