using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

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
        private UIButton saveButton;
        private UIButton loadButton;
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
                ReturnToGame();
            };

            saveButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 4, 250),
                Size = new Vector2(200, 100),
                Text = "Save",
            };
            saveButton.OnClick += () =>
            {
                string json = JsonConvert.SerializeObject(this.gameplayState, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                });
                File.WriteAllText("save.json", json);
            };

            loadButton = new UIButton(resources)
            {
                Position = new Vector2(Game.WindowSize.X / 4, 360),
                Size = new Vector2(200, 100),
                Text = "Load",
            };
            loadButton.OnClick += () =>
            {
                string json = File.ReadAllText("save.json");
                this.gameplayState = JsonConvert.DeserializeObject<GameplayScreenState>(json, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                ReturnToGame();
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

            MediaPlayer.Pause();
        }

        public void ReturnToGame()
        {
            var gameplayScreen = new GameplayScreen(Game);
            gameplayScreen.State = gameplayState;
            Game.SetCurrentScreen(gameplayScreen, false);
            MediaPlayer.Resume();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                ReturnToGame();
                return;
            }

            continueGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            restartGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            goBackButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            saveButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            if (File.Exists("save.json"))
            {
                loadButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);
            panel.Draw(spriteBatch);
            pausedText.Draw(spriteBatch);
            continueGameButton.Draw(spriteBatch);
            restartGameButton.Draw(spriteBatch);
            goBackButton.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            if (File.Exists("save.json"))
            {
                loadButton.Draw(spriteBatch);
            }
        }
    }
}