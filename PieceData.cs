namespace ShogiClient
{
    /// <summary>
    ///   Data related to a specific piece.
    /// </summary>
    public class PieceData
    {
        public PieceType Type { get; init; }
        public bool Promoted { get; set; }
        public bool IsPlayerOne { get; init; }
    }
}