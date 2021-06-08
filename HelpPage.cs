using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    /// <summary>
    ///   Help Page base class.
    /// </summary>
    public abstract class HelpPage
    {
        public GameResources Resources { get; set; }
        public Game1 Game { get; set; }

        public HelpPage(GameResources resources, Game1 game)
        {
            this.Resources = resources;
            this.Game = game;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}