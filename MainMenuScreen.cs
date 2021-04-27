using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class MainMenuScreen : Screen
    {
        private UIButton testButton;

        public MainMenuScreen(Game1 game) : base(game)
        {
        }

        public override void Initialize(GameResources resources)
        {
            base.Initialize(resources);

            testButton = new UIButton(resources)
            {
                Position = new Vector2(100, 100),
                Size = new Vector2(200, 100),
                Text = "Hello World",
            };
            testButton.OnClick += () =>
            {
                System.Console.WriteLine("Clicked");
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            base.Update(gameTime, keyboardState, mouseState, prevMouseState);

            testButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            testButton.Draw(spriteBatch);
        }
    }
}