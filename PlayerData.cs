using System.Collections.Generic;

namespace ShogiClient
{
    /// <summary>
    ///   Data related to an individual player.
    /// </summary>
    public class PlayerData
    {
        public List<PieceType> Hand = new List<PieceType>();
        public float TimeLeft = 65f;
        public int CheckCount = 0;
    }
}