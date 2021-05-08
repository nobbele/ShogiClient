namespace ShogiClient
{
    public interface ITurn
    {
        bool DidCheck { get; set; }
        string ToNotation();
    }
}