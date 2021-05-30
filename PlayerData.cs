using System.Collections.Generic;

namespace ShogiClient
{
    public class PlayerData
    {
        public List<PieceType> Hand = new List<PieceType>();
        public float TimeLeft = 65f;
        public int CheckCount = 0;
    }
}