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
        private UIButton continueGameButton;
        private UIButton restartGameButton;
        private UIButton goBackButton;
        private UIText pausedText;

        public GameplayPauseScreen(Game1 game, GameResources resources, GameplayScreenState gameplayState, Texture2D background) : base(game)
        {
            this.gameplayState = gameplayState;
            this.background = background;

            panel = new UIPanel(game)
            {
                Position = new Vector2(100, 100),
                Size = Game.WindowSize - new Vector2(200, 200),
            };

            continueGameButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, 250),
                Size = new Vector2(200, 100),
                Text = "Continue",
            };
            continueGameButton.OnClick += () =>
            {
                var gameplayScreen = new GameplayScreen(Game);
                gameplayScreen.State = gameplayState;
                Game.SetCurrentScreen(gameplayScreen, false);
            };
            restartGameButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, 360),
                Size = new Vector2(200, 100),
                Text = "Restart",
            };
            restartGameButton.OnClick += () =>
            {
                Game.SetCurrentScreen(new GameplayScreen(game));
            };

            goBackButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, 470),
                Size = new Vector2(200, 100),
                Text = "Go back",
            };
            goBackButton.OnClick += () =>
            {
                Game.SetCurrentScreen(new MainMenuScreen(game));
            };

            pausedText = new UIText(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, 150),
                Text = "Paused",
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                var gameplayScreen = new GameplayScreen(Game);
                gameplayScreen.State = gameplayState;
                Game.SetCurrentScreen(gameplayScreen, false);
                return;
            }

            continueGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            restartGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            goBackButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);
            pausedText.Draw(spriteBatch);
            continueGameButton.Draw(spriteBatch);
            restartGameButton.Draw(spriteBatch);
            goBackButton.Draw(spriteBatch);
        }
    }
}