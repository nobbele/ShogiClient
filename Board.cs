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
            var setup = new string[] {
                "PPPPPPPPP",
                " B     R ",
                "LNSGKGSNL"
            };

            for (int y = 0; y < setup.Length; y++)
            {
                for (int x = 0; x < Data.Width; x++)
                {
                    // Read in reverse, bottom up
                    var c = setup[setup.Length - y - 1][x];

                    if (Utils.PieceNotationToPieceType(c) is PieceType type)
                    {
                        Data.SetAt(x, y, new PieceData()
                        {
                            Type = type,
                            Promoted = true,
                            IsPlayerOne = false,
                        });
                        Data.SetAt(Data.Width - x - 1, Data.Height - y - 1, new PieceData()
                        {
                            Type = type,
                            Promoted = false,
                            IsPlayerOne = true,
                        });
                    }
                }
            }
        }
    }
}