using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class GameplayPauseScreen : Screen
    {
        // Have yet to implement a screen stack so this will have to do for now
        private GameplayScreenState gameplayState;
        private Texture2D background;

        private UIPanel panel;

        public GameplayPauseScreen(Game1 game, GameplayScreenState gameplayState, Texture2D background) : base(game)
        {
            this.gameplayState = gameplayState;
            this.background = background;
            panel = new UIPanel(game) {
                Position = new Vector2(100, 100),
                Size = new Vector2(700, 500),
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) 
            {
                var gameplayScreen = new GameplayScreen(Game);
                gameplayScreen.State = gameplayState;
                Game.SetCurrentScreen(gameplayScreen);
                return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);
            spriteBatch.DrawString(Resources.PieceFont, "Paused", Game.WindowSize / 2 - Resources.PieceFont.MeasureString("Paused") / 2, Color.White);
        }
    }
}