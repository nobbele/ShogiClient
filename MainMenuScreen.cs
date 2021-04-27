using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class MainMenuScreen : Screen
    {
        private UIButton startGameButton;

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
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            startGameButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Resources.MainMenuBackground, new Rectangle(Point.Zero, Game.WindowSize.ToPoint()), null, Color.White);

            var logoPosition = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y / 5);
            spriteBatch.Draw(Resources.Logo, logoPosition - Resources.Logo.Bounds.Size.ToVector2(), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

            startGameButton.Draw(spriteBatch);
        }
    }
}