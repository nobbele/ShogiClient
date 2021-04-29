namespace ShogiClient
{
    public abstract class StatefulScreen<T> : Screen where T : ScreenState, new()
    {
        protected T state { get; } = new T();

        public StatefulScreen(Game1 game) : base(game)
        {
        }
    }
}