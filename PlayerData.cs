using System.Collections.Generic;

namespace ShogiClient
{
    public class PlayerData
    {
        public List<PieceType> Hand = new List<PieceType>() {
            PieceType.Pawn,
            PieceType.Pawn,
            PieceType.Gold,
        };
    }
}