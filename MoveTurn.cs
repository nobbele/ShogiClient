using System;

namespace ShogiClient
{
    public class MoveTurn : ITurn
    {
        public PieceData Piece { get; set; }
        public bool DidPromote { get; set; }
        public bool DidCheck { get; set; }
        public int XFrom { get; set; }
        public int YFrom { get; set; }
        public int XTarget { get; set; }
        public int YTarget { get; set; }
        public PieceData Captured { get; set; }

        public MoveTurn(PieceData piece, bool didCheck, bool didPromote, int xFrom, int yFrom, int xTarget, int yTarget, PieceData captured)
        {
            Piece = piece;
            DidCheck = didCheck;
            DidPromote = didPromote;
            XFrom = xFrom;
            YFrom = yFrom;
            XTarget = xTarget;
            YTarget = yTarget;
            Captured = captured;
        }

        public string ToNotation()
            => $@"
                {Utils.PieceToNotationChar(Piece)}
                {(char)('A' + XFrom)}
                {YFrom}
                x
                {(char)('A' + XTarget)}
                {YTarget}
                {(Captured != null
                    ? $"*{Utils.PieceToNotationChar(Captured)}"
                    : string.Empty
                )}
                {(DidPromote
                    ? "+"
                    : string.Empty
                )}
                {(DidCheck
                    ? "#"
                    : string.Empty
                )}
            ".Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(" ", string.Empty).Replace("\t", string.Empty);
    }
}