namespace ShogiClient
{
    /// <summary>
    ///   A screen that contains a state.
    /// </summary>
    public abstract class StatefulScreen<T> : Screen where T : ScreenState, new()
    {
        public T State { get; set; } = new T();

        public StatefulScreen(Game1 game) : base(game)
        {
        }
    }
}