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

        public static List<(int X, int Y)> OpponentControl(
            Grid<PieceData> board,
            bool isPlayerOne)
        {
            var opponentControl = new List<(int X, int Y)>();
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var pieceOnBoard = board.GetAt(x, y);
                    if (pieceOnBoard != null && pieceOnBoard.IsPlayerOne != isPlayerOne)
                    {
                        opponentControl.AddRange(ValidMovesForPiece(pieceOnBoard, board, x, y));
                    }
                }
            }

            return opponentControl;
        }

        // TODO Have this function check if mvoing this piece would cause a check/checkmate
        public static List<(int X, int Y)> ValidMovesForPiece(
            PieceData piece,
            Grid<PieceData> board,
            int currentX,
            int currentY)
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
                                    validMoves.Add(positionOnBoard);
                            }
                            else if (c == 'M')
                            {
                                validMoves.AddRange(RayCastPathForMovement(
                                    currentX, currentY,
                                    localPosition.X, localPosition.Y,
                                    piece.IsPlayerOne,
                                    board
                                ));
                            }
                        }
                    }
                }
            }

            if (piece.Type == PieceType.King)
            {
                validMoves = validMoves.Except(OpponentControl(board, piece.IsPlayerOne)).ToList();
            }

            return validMoves;
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