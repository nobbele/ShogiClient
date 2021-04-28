using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShogiClient
{
    public class Game1 : Game
    {
        public const bool DEBUG_PLAYERONE = false;
        public const bool DEBUG_DISPLAY = false;

        public Vector2 WindowSize => Window.ClientBounds.Size.ToVector2();
        public Screen CurrentScreen { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameResources resources = new GameResources();

        private MouseState prevMouseState = new MouseState();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
        }

        public void SetCurrentScreen(Screen screen)
        {
            CurrentScreen = screen;
            MediaPlayer.Stop();
            CurrentScreen.Initialize(resources);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            MediaPlayer.Volume = 0.05f;

            SetCurrentScreen(new MainMenuScreen(this));
            //SetCurrentScreen(new GameplayScreen(this));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resources.LoadContent(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            CurrentScreen.Update(gameTime, keyboardState, mouseState, prevMouseState);

            prevMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            CurrentScreen.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
