namespace ShogiClient
{
    public class Board
    {
        public Grid<PieceData> Data { get; private set; }

        public Board(int width, int height)
        {
            Data = new Grid<PieceData>(width, height);
            // The variable is ordered from bottom-up but arrays are accessed from top-to-bottom so it needs to be read in reverse
            // it makes more sense to this for readability purposes
            string[] setup = new[] {
                "PPPPPPPPP",
                " B     R ",
                "LNSGKGSNL"
            };

            for (int y = 0; y < setup.Length; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    // Read in reverse, bottom up
                    char c = setup[setup.Length - y - 1][x];
                    SetPieceAtWithNotation(x, y, c);
                    SetPieceAtWithNotation(x, Data.Height - y - 1, c);
                }
            }
        }

        private void SetPieceAtWithNotation(int x, int y, char c)
        {
            PieceType? maybeType = c switch
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
                _ => throw new System.Exception("Unknown Piece Type in Setup"),
            };
            if (maybeType is PieceType type)
            {
                Data.SetAt(x, y, new PieceData()
                {
                    Type = type,
                    Promoted = false,
                    IsPlayerOne = false,
                });
            }
        }
    }
}