namespace ShogiClient
{
    public class DropTurn : ITurn
    {
        public PieceType Type { get; set; }
        public bool DidCheck { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public DropTurn(PieceType type, int x, int y, bool didCheck)
        {
            Type = type;
            X = x;
            Y = y;
            DidCheck = didCheck;
        }

        public string ToNotation()
            => $"V{Utils.PieceTypeToNotationChar(Type, false)}{(char)('A' + X)}{Y}{(DidCheck ? "#" : string.Empty)}";
    }
}