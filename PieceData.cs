namespace ShogiClient
{
    public class PieceData
    {
        public PieceType Type { get; init; }
        private bool promoted = false;
        public bool Promoted
        {
            get => promoted;
            set
            {
                promoted = value;
                if (!Utils.CanPromotePieceType(Type))
                {
                    promoted = false;
                }
            }
        }
        public bool IsPlayerOne { get; init; }
    }
}