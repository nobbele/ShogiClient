namespace ShogiClient
{
    public class PieceData
    {
        public PieceType Type { get; init; }
        public bool Promoted { get; set; }
        public bool IsPlayerOne { get; init; }
    }
}