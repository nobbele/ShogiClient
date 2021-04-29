using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public abstract class Screen
    {
        public Game1 Game { get; private set; }
        public GameResources Resources { get; private set; }

        public Screen(Game1 game)
        {
            Game = game;
        }

        public virtual void Initialize(GameResources resources)
        {
            Resources = resources;
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}