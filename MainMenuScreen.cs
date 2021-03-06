using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace ShogiClient
{
    /// <summary>
    ///   The screen displayed when the user boots up the game
    /// </summary>
    public class MainMenuScreen : StatefulScreen<MainMenuScreenState>
    {
        private UIButton startGameButton;
        private UIButton optionsButton;
        private UIButton exitGameButton;
        private UIButton playPauseButton;
        private UIButton startFromSaveButton;

        public MainMenuScreen(Game1 game) : base(game)
        {

        }

        public override void Initialize(GameResources resources)
        {
            base.Initialize(resources);

            startGameButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y * 2 / 5),
                Size = new Vector2(200, 100),
                Text = "Start Game",
            };
            startGameButton.OnClick += () =>
            {
                Game.SetCurrentScreen(new GameplayScreen(Game));
            };

            startFromSaveButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 4, Game.WindowSize.Y * 2 / 5),
                Size = new Vector2(200, 100),
                Text = "Start Game From Last Save",
            };
            startFromSaveButton.OnClick += () =>
            {
                string json = File.ReadAllText("save.json");
                var gameplayState = JsonConvert.DeserializeObject<GameplayScreenState>(json, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                Game.SetCurrentScreen(new GameplayScreen(Game)
                {
                    State = gameplayState
                });
            };

            optionsButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y * 3 / 5),
                Size = new Vector2(200, 100),
                Text = "Options",
            };
            optionsButton.OnClick += () =>
            {
                Game.SetCurrentScreen(new OptionsScreen<MainMenuScreenState>(Game, Resources, State, Game.Screenshot()), false);
            };

            exitGameButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y * 4 / 5),
                Size = new Vector2(200, 100),
                Text = "Quit",
            };
            exitGameButton.OnClick += () =>
            {
                Game.Exit();
            };

            playPauseButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X * 4 / 5, 25),
                Size = new Vector2(100, 50),
                Text = "Play/Pause",
            };
            playPauseButton.OnClick += () =>
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
                else if (MediaPlayer.State == MediaState.Paused)
                    MediaPlayer.Resume();
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Resources.RandomMainMenuSong);
            }

            startGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            optionsButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            exitGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            playPauseButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            if (File.Exists("save.json"))
            {
                startFromSaveButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Resources.MainMenuBackground, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);

            var logoPosition = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y / 5);
            spriteBatch.Draw(Resources.Logo, logoPosition - Resources.Logo.Bounds.Size.ToVector2(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            startGameButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            exitGameButton.Draw(spriteBatch);
            playPauseButton.Draw(spriteBatch);
            if (File.Exists("save.json"))
            {
                startFromSaveButton.Draw(spriteBatch);
            }
        }
    }
}