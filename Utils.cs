namespace ShogiClient
{
    public static class Utils
    {
        public static PieceType? PieceNotationToPieceType(char c) => c switch
        {
            'P' => PieceType.Pawn,
            'B' => PieceType.Bishop,
            'R' => PieceType.Rook,
            'L' => PieceType.Lance,
            'N' => PieceType.Knight,
            'S' => PieceType.Silver,
            'G' => PieceType.Gold,
            'K' => PieceType.King,
            ' ' => null,
            _ => throw new System.Exception("Unknown Piece Type"),
        };

        public static string PieceTypeToKanji(PieceType type, bool isPlayerOne, bool isPromoted) => type switch
        {
            PieceType.Pawn => isPromoted ? "と" : "歩",
            PieceType.Bishop => isPromoted ? "馬" : "角",
            PieceType.Rook => isPromoted ? "龍" : "飛",
            PieceType.Lance => isPromoted ? "香" : "香",
            PieceType.Knight => isPromoted ? "圭" : "桂",
            PieceType.Silver => isPromoted ? "全" : "銀",
            PieceType.Gold => "金",
            PieceType.King => isPlayerOne ? "玉" : "王",
            _ => throw new System.Exception("Unknown Piece Type"),
        };

        public static bool CanPromotePieceType(PieceType type) => type switch
        {
            PieceType.Pawn => true,
            PieceType.Bishop => true,
            PieceType.Rook => true,
            PieceType.Lance => true,
            PieceType.Knight => true,
            PieceType.Silver => true,
            PieceType.Gold => false,
            PieceType.King => false,
            _ => throw new System.Exception("Unknown Piece Typep"),
        };
    }
}