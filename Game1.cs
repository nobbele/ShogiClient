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
        private KeyboardState prevKeyboardState = new KeyboardState();

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

            Console.WriteLine($"Loaded Screen {screen.GetType().Name}");
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

            CurrentScreen.Update(gameTime, keyboardState, prevKeyboardState, mouseState, prevMouseState);

            prevMouseState = mouseState;
            prevKeyboardState = keyboardState;

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

        public Texture2D Screenshot() {
            var renderTarget = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GraphicsDevice.SetRenderTarget(renderTarget);
            Draw(new GameTime());
            GraphicsDevice.SetRenderTarget(null);
            return renderTarget;
        }
    }
}
