namespace ShogiClient
{
    public class PieceData
    {
        public PieceType Type { get; init; }
        private bool promoted = false;
        public bool Promoted { get; set; }
        public bool IsPlayerOne { get; init; }
    }
}