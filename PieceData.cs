namespace ShogiClient
{
    public class PieceData
    {
        public PieceType Type { get; init; }
        public bool Promoted { get; init; } = false;
        public bool IsPlayerOne { get; init; }
    }
}