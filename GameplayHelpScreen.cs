using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class GameplayHelpScreen : StatefulScreen<GameplayHelpScreenState>
    {
        // Have yet to implement a screen stack so this will have to do for now
        private GameplayScreenState gameplayState;
        private Texture2D background;

        private UIPanel panel;
        private HelpPage[] helpPages;
        private UIButton prevButton;
        private UIButton nextButton;

        public GameplayHelpScreen(Game1 game, GameResources resources, GameplayScreenState gameplayState, Texture2D background) : base(game)
        {
            this.gameplayState = gameplayState;
            this.background = background;

            panel = new UIPanel(game)
            {
                Position = new Vector2(100, 100),
                Size = Game.WindowSize - new Vector2(200, 200),
            };

            helpPages = new HelpPage[] {
                new HelpPage1(resources, game),
                new HelpPage2(resources, game),
                new HelpPage3(resources, game),
            };

            prevButton = new UIButton(resources)
            {
                Position = new Vector2(180, Game.WindowSize.Y - 150),
                Size = new Vector2(100, 50),
                Text = "Prev",
            };
            prevButton.OnClick += () =>
            {
                State.CurrentPage--;
            };

            nextButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X - 180, Game.WindowSize.Y - 150),
                Size = new Vector2(100, 50),
                Text = "Next",
            };
            nextButton.OnClick += () =>
            {
                State.CurrentPage++;
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

            if (State.CurrentPage > 0)
            {
                prevButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            }
            if (State.CurrentPage + 1 < helpPages.Length)
            {
                nextButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);
            helpPages[State.CurrentPage].Draw(spriteBatch);
            if (State.CurrentPage > 0)
            {
                prevButton.Draw(spriteBatch);
            }
            if (State.CurrentPage + 1 < helpPages.Length)
            {
                nextButton.Draw(spriteBatch);
            }
        }
    }
}