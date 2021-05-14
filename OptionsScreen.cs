using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShogiClient
{
    public class OptionsScreen<TState> : Screen
        where TState : ScreenState
    {
        // Have yet to implement a screen stack so this will have to do for now
        private TState prevState;
        private Texture2D background;

        private UIPanel panel;
        private UIText titleLabel;
        private UIText volumeLabel;
        private UISlider volumeSlider;

        public OptionsScreen(Game1 game, GameResources resources, TState prevState, Texture2D background) : base(game)
        {
            this.prevState = prevState;
            this.background = background;

            panel = new UIPanel(game)
            {
                Position = new Vector2(100, 100),
                Size = Game.WindowSize - new Vector2(200, 200),
            };

            titleLabel = new UIText(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y / 5),
                Text = "Options"
            };

            volumeLabel = new UIText(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y * 2 / 5 - 30),
                Text = "Volume"
            };
            volumeSlider = new UISlider(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y * 2 / 5),
                Size = new Vector2(100, 20),
                Min = 0,
                Max = 0.10f,
            };
            volumeSlider.SetCurrentValue(MediaPlayer.Volume);
            volumeSlider.OnChange += p =>
            {
                System.Console.WriteLine($"Setting volume to {p}");
                MediaPlayer.Volume = p;
            };
            volumeSlider.OnRelease += p =>
            {
                // TODO Save audio volume
                System.Console.WriteLine($"Saving volume as {p}");
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                Screen screen = null;
                if (prevState is GameplayScreenState gameplayState)
                {
                    var gameplayScreen = new GameplayScreen(Game);
                    gameplayScreen.State = gameplayState;
                    screen = gameplayScreen;
                }
                else if (prevState is MainMenuScreenState mainMenuState)
                {
                    var mainMenuScreen = new MainMenuScreen(Game);
                    mainMenuScreen.State = mainMenuState;
                    screen = mainMenuScreen;
                }
                Game.SetCurrentScreen(screen, false);
                return;
            }

            volumeSlider.Update(gameTime, keyboardState, mouseState, prevMouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);
            titleLabel.Draw(spriteBatch);
            volumeLabel.Draw(spriteBatch);
            volumeSlider.Draw(spriteBatch);
        }
    }
}