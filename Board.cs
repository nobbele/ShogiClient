namespace ShogiClient
{
    public class Board
    {
        public Grid<PieceData> Data { get; private set; }
        public PieceData HeldPiece { get; private set; }

        /// <summary>
        ///   Removes a piece from the board and puts it in the HeldPiece property.
        /// </summary>
        public bool PickUpPiece(int x, int y, bool isPlayerOne)
        {
            var piece = Data.GetAt(x, y);
            if (piece == null || piece.IsPlayerOne != isPlayerOne)
                return false;
            Data.SetAt(x, y, null);
            HeldPiece = piece;
            return true;
        }

        /// <summary>
        ///   Removes a piece from the board and puts it in the HeldPiece property.
        /// </summary>
        public bool PlacePiece(int x, int y, bool isPlayerOne, out PieceType? captured)
        {
            captured = null;

            var piece = Data.GetAt(x, y);
            if (piece != null)
            {
                if (piece.IsPlayerOne == isPlayerOne)
                {
                    return false;
                }
                else
                {
                    captured = piece.Type;
                }
            }
            PlacePiece(x, y);
            return true;
        }

        public void PlacePiece(int x, int y)
        {
            Data.SetAt(x, y, HeldPiece);
            HeldPiece = null;
        }

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