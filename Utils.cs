using System;
using System.Collections.Generic;
using System.Linq;

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
            PieceType.Pawn => !isPromoted ? "歩" : "と",
            PieceType.Bishop => !isPromoted ? "角" : "馬",
            PieceType.Rook => !isPromoted ? "飛" : "龍",
            PieceType.Lance => !isPromoted ? "香" : "香",
            PieceType.Knight => !isPromoted ? "桂" : "圭",
            PieceType.Silver => !isPromoted ? "銀" : "全",
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
            _ => throw new System.Exception("Unknown Piece Type"),
        };

        public static string[] PieceTypeMoveSet(PieceType type, bool isPromoted) => type switch
        {
            PieceType.Pawn => !isPromoted ? new string[] {
                " S ",
                " . ",
                "   ",
            } : new string[] {
                "SSS",
                "S.S",
                " S ",
            },
            PieceType.Bishop => !isPromoted ? new string[] {
                "M M",
                " . ",
                "M M",
            } : new string[] {
                "MSM",
                "S.S",
                "MSM",
            },
            PieceType.Rook => !isPromoted ? new string[] {
                " M ",
                "M.M",
                " M ",
            } : new string[] {
                "SMS",
                "M.M",
                "SMS",
            },
            PieceType.Lance => !isPromoted ? new string[] {
                " M ",
                " . ",
                "   ",
            } : new string[] {
                "SSS",
                "S.S",
                " S ",
            },
            PieceType.Knight => !isPromoted ? new string[] {
                "J J",
                "   ",
                " . ",
                "   ",
                "   "
            } : new string[] {
                "SSS",
                "S.S",
                " S ",
            },
            PieceType.Silver => !isPromoted ? new string[] {
                "SSS",
                " . ",
                "S S",
            } : new string[] {
                "SSS",
                "S.S",
                " S ",
            },
            PieceType.Gold => new string[] {
                "SSS",
                "S.S",
                " S ",
            },
            PieceType.King => new string[] {
                "SSS",
                "S.S",
                "SSS"
            },
            _ => throw new System.Exception("Unknown Piece Type"),
        };

        public static Dictionary<(int X, int Y), List<(int X, int Y)>> OpponentControl(
            Grid<PieceData> board,
            bool isPlayerOne)
        {
            var opponentControl = new Dictionary<(int X, int Y), List<(int X, int Y)>>();
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var pieceOnBoard = board.GetAt(x, y);
                    if (pieceOnBoard != null && pieceOnBoard.IsPlayerOne != isPlayerOne)
                    {
                        opponentControl.Add((x, y), ValidMovesForPiece(pieceOnBoard, board, x, y, false));
                    }
                }
            }

            return opponentControl;
        }

        public static List<(int X, int Y)> ValidMovesForPiece(
            PieceData piece,
            Grid<PieceData> board,
            int currentX,
            int currentY,
            bool checkForCheck = true)
        {
            var moveSet = Utils.PieceTypeMoveSet(piece.Type, piece.Promoted);
            (int X, int Y) center = (-1, -1);
            for (int y = 0; y < moveSet.Length; y++)
            {
                int x = moveSet[y].IndexOf('.');
                if (x != -1)
                {
                    center = (x, y);
                    break;
                }
            }

            if (center.X == -1 || center.Y == -1)
            {
                throw new System.Exception("Invalid center");
            }

            var validMoves = new List<(int X, int Y)>();
            for (int y = 0; y < moveSet.Length; y++)
            {
                for (int x = 0; x < moveSet[y].Length; x++)
                {
                    char c = moveSet[y][x];
                    if (c != ' ')
                    {
                        var localPosition = SidedOffsetToGlobalOffset(center.X, center.Y, x, y, piece.IsPlayerOne);
                        (int X, int Y) positionOnBoard = (
                            currentX + localPosition.X,
                            currentY + localPosition.Y
                        );
                        if (board.AreIndicesWithinBounds(positionOnBoard.X, positionOnBoard.Y))
                        {
                            var pieceAtPosition = board.GetAt(positionOnBoard.X, positionOnBoard.Y);
                            var occupiedTile = pieceAtPosition != null && pieceAtPosition.IsPlayerOne == piece.IsPlayerOne;

                            // Since there's no distant non-jumping S, we can treat them as the same
                            if (c == 'J' || c == 'S')
                            {
                                if (!occupiedTile)
                                {
                                    if (!checkForCheck || !WillMoveCauseCheck(piece, board, currentX, currentY, positionOnBoard.X, positionOnBoard.Y))
                                    {
                                        validMoves.Add(positionOnBoard);
                                    }
                                }
                            }
                            else if (c == 'M')
                            {
                                var moves = RayCastPathForMovement(
                                    currentX, currentY,
                                    localPosition.X, localPosition.Y,
                                    piece.IsPlayerOne,
                                    board
                                );
                                if (checkForCheck)
                                {
                                    moves = moves.Where(move => !WillMoveCauseCheck(piece, board, currentX, currentY, move.X, move.Y)).ToList();
                                }
                                validMoves.AddRange(moves);
                            }
                        }
                    }
                }
            }

            return validMoves;
        }

        public static List<(int X, int Y)> ValidPositionsForPieceDrop(
            PieceData piece,
            Grid<PieceData> board)
        {
            var validMoves = new List<(int X, int Y)>();

            // To check two pawn drop, no need to run it on non-pawn pieces but this is easier
            var pawnColumns = new bool[board.Width];
            // The two foreach's cannot be collapsed into ones, we need to check the whole board for columns first
            foreach (var tile in board)
            {
                if (tile.Content != null && tile.Content.IsPlayerOne == piece.IsPlayerOne && tile.Content.Type == PieceType.Pawn)
                {
                    pawnColumns[tile.X] = true;
                }
            }

            foreach (var tile in board)
            {
                bool legalPosition = true;

                if (tile.Content != null)
                {
                    legalPosition = false;
                }

                if (piece.Type == PieceType.Pawn)
                {
                    if (pawnColumns[tile.X] || Utils.WillMoveCauseCheckFor(piece, board, tile.X, tile.Y, tile.X, tile.Y, !piece.IsPlayerOne))
                    {
                        legalPosition = false;
                    }
                }

                if (legalPosition)
                {
                    validMoves.Add((tile.X, tile.Y));
                }
            }

            return validMoves;
        }

        public static bool IsKingCheckMated(Grid<PieceData> board, bool isPlayerOne)
        {
            foreach (var tile in board)
            {
                if (tile.Content != null && tile.Content.IsPlayerOne == isPlayerOne)
                {
                    if (ValidMovesForPiece(tile.Content, board, tile.X, tile.Y).Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsKingChecked(Grid<PieceData> board, bool isPlayerOne)
        {
            var kingPiece = GetPlayerKing(board, isPlayerOne);

            var opponentControl = OpponentControl(board, isPlayerOne);
            var pieceThatCanTakeKing = opponentControl
                .Where((pair) => pair.Value.Contains((kingPiece.X, kingPiece.Y)))
                .ToList();
            return pieceThatCanTakeKing.Count > 0;
        }

        public static bool WillMoveCauseCheck(PieceData piece, Grid<PieceData> board, int currentX, int currentY, int targetX, int targetY)
            => WillMoveCauseCheckFor(piece, board, currentX, currentY, targetX, targetY, piece.IsPlayerOne);

        public static bool WillMoveCauseCheckFor(PieceData piece, Grid<PieceData> board, int currentX, int currentY, int targetX, int targetY, bool isPlayerOne)
        {
            var tempBoard = board.Clone();

            tempBoard.SetAt(currentX, currentY, null);
            tempBoard.SetAt(targetX, targetY, piece);

            return IsKingChecked(tempBoard, isPlayerOne);
        }

        public static (PieceData piece, int X, int Y) GetPlayerKing(Grid<PieceData> board, bool isPlayerOne)
        {
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var pieceOnBoard = board.GetAt(x, y);
                    if (pieceOnBoard != null && pieceOnBoard.IsPlayerOne == isPlayerOne && pieceOnBoard.Type == PieceType.King)
                    {
                        return (pieceOnBoard, x, y);
                    }
                }
            }

            return (null, -1, -1);
        }

        private static (int X, int Y) SidedOffsetToGlobalOffset(int centerX, int centerY, int x, int y, bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                return (-(centerX - x), -(centerY - y));
            }
            else
            {
                return (centerX - x, centerY - y);
            }
        }

        private static List<(int X, int Y)> RayCastPathForMovement(
            int currentX, int currentY,
            int directionX, int directionY,
            bool isPlayerOne,
            Grid<PieceData> board)
        {
            var validMoves = new List<(int X, int Y)>();
            var yRatio = Math.Abs(directionX == 0 ? 0 : directionY / directionX);
            var xSign = Math.Sign(directionX);
            var ySign = Math.Sign(directionY);

            PieceData prevRayPoint = null;
            for (int x = Math.Abs(directionX); true; x++)
            {
                var y = directionX == 0 ? x + 1 : x * yRatio;

                var targetX = currentX + x * xSign;
                var targetY = currentY + y * ySign;

                if (!board.AreIndicesWithinBounds(targetX, targetY))
                {
                    break;
                }

                var pieceAtRayPoint = board.GetAt(targetX, targetY);

                bool canReach = true;
                if (pieceAtRayPoint != null && pieceAtRayPoint.IsPlayerOne == isPlayerOne
                        || prevRayPoint != null)
                {
                    canReach = false;
                }

                if (!canReach)
                {
                    break;

                }

                validMoves.Add((targetX, targetY));
                prevRayPoint = pieceAtRayPoint;
            }

            return validMoves;
        }
    }
}