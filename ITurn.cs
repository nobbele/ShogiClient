namespace ShogiClient
{
    /// <summary>
    ///   An interface for a turn.
    /// </summary>
    public interface ITurn
    {
        bool DidCheck { get; set; }
        string ToNotation();
    }
}