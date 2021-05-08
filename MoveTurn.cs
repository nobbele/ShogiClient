using System;

namespace ShogiClient
{
    public struct MoveTurn : ITurn
    {
        public PieceType Type { get; set; }
        public bool Promoted { get; set; }
        public bool DidPromote { get; set; }
        public bool DidCheck { get; set; }
        public int XFrom { get; set; }
        public int YFrom { get; set; }
        public int XTarget { get; set; }
        public int YTarget { get; set; }
        public PieceType? Captured { get; set; }

        public MoveTurn(PieceType type, bool promoted, bool didCheck, bool didPromote, int xFrom, int yFrom, int xTarget, int yTarget, PieceType? captured)
        {
            Type = type;
            Promoted = promoted;
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
                {Utils.PieceTypeToNotationChar(Type, Promoted)}
                {(char)('A' + XFrom)}
                {YFrom}
                x
                {(char)('A' + XTarget)}
                {YTarget}
                {(Captured != null
                    ? $"*{Utils.PieceTypeToNotationChar(Type, false)}"
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