using Microsoft.Xna.Framework;

namespace ShogiClient
{
    public class Board
    {
        public Grid<PieceData> Data { get; private set; }
        public PieceData HeldPiece { get; set; }

        public Vector2 HeldPiecePosition { get; set; } = new Vector2(0, 0);
        // If null, that means it's taken from the hand
        public Point? HeldPiecePickUpPosition { get; set; } = null;

        /// <summary>
        ///   Removes a piece from the board and puts it in the HeldPiece property.
        /// </summary>
        public bool PickUpPiece(Point point, bool isPlayerOne)
        {
            var piece = Data.GetAt(point);
            if (piece == null || piece.IsPlayerOne != isPlayerOne)
                return false;
            Data.SetAt(point, null);
            HeldPiece = piece;
            return true;
        }

        /// <summary>
        ///   Removes a piece from the HeldPiece property and puts it on the board.
        /// </summary>
        public bool PlacePiece(Point from, Point target, out PieceData captured)
        {
            captured = null;

            if (!Utils.ValidMovesForPiece(HeldPiece, Data, from).Contains(target))
            {
                return false;
            }

            var piece = Data.GetAt(target);
            if (piece != null)
            {
                captured = piece;
            }
            PlacePiece(target);
            return true;
        }

        /// <summary>
        ///   Removes a piece from the hand and puts it on the board.
        /// </summary>
        public bool PlacePieceFromHand(Point target)
        {
            if (!Utils.ValidPositionsForPieceDrop(HeldPiece, Data).Contains(target))
            {
                return false;
            }

            var piece = Data.GetAt(target);
            PlacePiece(target);
            return true;
        }


        public void PlacePiece(Point point)
        {
            Data.SetAt(point, HeldPiece);
            HeldPiece = null;
        }

        public Board(int width, int height)
        {
            Data = new Grid<PieceData>(width, height);
            // The variable is ordered from bottom-up but arrays are accessed from top-to-bottom so it needs to be read in reverse
            // it makes more sense to this for readability purposes

            var setup = new string[] {
                "ppppppppp",
                " b     r ",
                "lnsgkgsnl"
            };

            for (int y = 0; y < setup.Length; y++)
            {
                for (int x = 0; x < setup[0].Length; x++)
                {
                    // Read in reverse, bottom up
                    var c = setup[setup.Length - y - 1][x];

                    if (Utils.PieceNotationToPieceType(c) is (PieceType, bool) type)
                    {
                        Data.SetAt(Data.Width - x - 1, y, new PieceData()
                        {
                            Type = type.type,
                            Promoted = type.promoted && Utils.CanPromotePieceType(type.type),
                            IsPlayerOne = false,
                        });
                        Data.SetAt(x, Data.Height - y - 1, new PieceData()
                        {
                            Type = type.type,
                            Promoted = type.promoted && Utils.CanPromotePieceType(type.type),
                            IsPlayerOne = true,
                        });
                    }
                }
            }
        }
    }
}