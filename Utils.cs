using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ShogiClient
{
    public static class Utils
    {
        /// <summary>
        ///   Converts <paramref name="c"/> to the corresponding <see cref="PieceType"/> enumeration and promotion status
        /// </summary>
        /// <param name="c">The character to be used in the convertion.</param>
        /// <returns>
        ///   The <see cref="PieceType"/> and promotion status corresponding to <paramref name="c"/> if it's found.
        ///   If an empty character is passed, it's assumed to be a non-piece and so returns null.
        /// </returns>
        /// <remarks>
        ///   Note, an empty and an invalid piece type are very different, passing an invalid piece type which will throw an exception while an empty one will return null.
        /// </remarks>
        public static (PieceType type, bool promoted)? PieceNotationToPieceType(char c)
        {
            PieceType? type = char.ToUpper(c) switch
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
                _ => throw new ArgumentException("Unknown Piece Type"),
            };

            if (type == null)
            {
                return null;
            }

            return ((PieceType, bool)?)(type, char.IsUpper(c));
        }

        // TODO Documentation
        public static char PieceTypeToNotationChar(PieceType type, bool promoted)
        {
            var character = type switch
            {
                PieceType.Pawn => 'P',
                PieceType.Bishop => 'B',
                PieceType.Rook => 'R',
                PieceType.Lance => 'L',
                PieceType.Knight => 'N',
                PieceType.Silver => 'S',
                PieceType.Gold => 'G',
                PieceType.King => 'K',
                _ => throw new ArgumentException(),
            };

            if (promoted)
                return char.ToUpper(character);
            else
                return char.ToLower(character);
        }

        /// <summary>
        ///   Converts <paramref name="type"/> to the corresponding Kanji used to identify the piece.
        /// </summary>
        /// <param name="type">The type to get the Kanji for</param>
        /// <param name="isPlayerOne">Used to decide whether or not the king should be jeweled or not. (true is jeweled, false is not)</param>
        /// <param name="isPromoted">Used to decide whether or not to get the Kanji for the promoted form of the piece.</param>
        /// <returns>
        ///   The Kanji corresponding to specified type, player side and promotion options.
        /// </returns>
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

        /// <summary>
        ///   Returns whether the passed <paramref name="type"/> piece can be promoted.
        /// </summary>
        /// <param name="type">The type used to decided if it can be promoted.</param>
        /// <returns>True if it can be promoted, false if it can not.</returns>
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

        /// <summary>
        ///   Returns the move set defined for specified <paramref name="type"/> piece.
        /// </summary>
        /// <param name="type">The type used to decided if it can be promoted.</param>
        /// <returns>
        ///   A string array where each element is a row, each char in the string is the column.
        ///   '.' denotes where in the move set the piece to move is located, usually the center.
        ///   'S' represents a move where the piece can only walk 1 step in that direction.
        ///   'J' denotes a move similar to S but the piece can jump over any pieces in the way.
        ///   'M' represents a move in which the piece can move arbitrarily far in the direction the char is located, it may not pass through any pieces on the way
        /// </returns>
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

        /// <summary>
        ///   Returns the tiles that the opponent player has control over.
        /// </summary>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="isPlayerOne">Whether or not the player checking for the opponent is player 1 or 2.</param>
        /// <returns>
        ///   A dictionary where the key is a specific location of the opponent piece
        ///   and the value is a list of positions that the piece in the key can move to, the tiles it has control over.
        /// </returns>
        public static Dictionary<Point, List<Point>> OpponentControl(Grid<PieceData> board, bool isPlayerOne, PlayerData currentPlayer, PlayerData opponentPlayer)
        {
            var opponentControl = new Dictionary<Point, List<Point>>();
            foreach (var piece in board)
            {
                if (piece.Content != null && piece.Content.IsPlayerOne != isPlayerOne)
                {
                    opponentControl.Add(piece.Position, ValidMovesForPiece(piece.Content, board, piece.Position, currentPlayer, opponentPlayer, false));
                }
            }
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var pieceOnBoard = board.GetAt(x, y);

                }
            }

            return opponentControl;
        }

        /// <summary>
        ///   Returns legal moves for specified piece.
        /// </summary>
        /// <param name="piece">The piece to check legal moves on.</param>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="currentX">X Position of the specified piece.</param>
        /// <param name="currentY">Y Position of the specified piece.</param>
        /// <param name="checkForCheck">Whether or not to check if a move will cause a check for the player moving.</param>
        /// <returns>
        ///   A list of positions that the piece in the piece specified can move to.
        /// </returns>
        public static List<Point> ValidMovesForPiece(
            PieceData piece,
            Grid<PieceData> board,
            Point currentPosition,
            PlayerData currentPlayer,
            PlayerData opponentPlayer,
            bool checkForCheck = true)
        {
            var moveSet = Utils.PieceTypeMoveSet(piece.Type, piece.Promoted);
            // Center of the move set, aka the position where the piece is located.
            var center = new Point(-1, -1);
            for (int y = 0; y < moveSet.Length; y++)
            {
                int x = moveSet[y].IndexOf('.');
                if (x != -1)
                {
                    center = new Point(x, y);
                    break;
                }
            }

            if (center.X == -1 || center.Y == -1)
            {
                throw new System.Exception("Invalid center");
            }

            var validMoves = new List<Point>();
            for (int y = 0; y < moveSet.Length; y++)
            {
                for (int x = 0; x < moveSet[y].Length; x++)
                {
                    char c = moveSet[y][x];
                    if (c != ' ')
                    {
                        // Offset from the center.
                        var moveSetPositionOffset = SidedOffsetToGlobalOffset(center, new Point(x, y), piece.IsPlayerOne);
                        var positionOnBoard = currentPosition + moveSetPositionOffset;

                        if (board.AreIndicesWithinBounds(positionOnBoard.X, positionOnBoard.Y))
                        {
                            var pieceAtPosition = board.GetAt(positionOnBoard.X, positionOnBoard.Y);
                            var occupiedTile = pieceAtPosition != null && pieceAtPosition.IsPlayerOne == piece.IsPlayerOne;

                            // Since there's no distant non-jumping S, we can treat them as the same.
                            if (c == 'J' || c == 'S')
                            {
                                if (!occupiedTile)
                                {
                                    // If checkForCheck is true, it will check so the move doesn't cause a check for the player.
                                    bool isValid = true;
                                    if (checkForCheck)
                                    {
                                        var willMoveCauseCheck = WillMoveCauseCheck(piece, board, currentPosition, positionOnBoard, currentPlayer, opponentPlayer);
                                        var willMoveCauseOpponentCheck = WillMoveCauseCheckFor(piece, board, currentPosition, positionOnBoard, opponentPlayer, currentPlayer, !piece.IsPlayerOne);
                                        if ((currentPlayer.CheckCount > 3 && willMoveCauseOpponentCheck) || willMoveCauseCheck)
                                        {
                                            isValid = false;
                                        }
                                    }

                                    if (isValid)
                                    {
                                        validMoves.Add(positionOnBoard);
                                    }
                                }
                            }
                            else if (c == 'M')
                            {
                                var moves = RayCastPathForMovement(
                                    currentPosition,
                                    moveSetPositionOffset,
                                    piece.IsPlayerOne,
                                    board
                                );
                                if (checkForCheck)
                                {
                                    moves = moves.Where(move =>
                                    {
                                        var willMoveCauseCheck = WillMoveCauseCheck(piece, board, currentPosition, move, currentPlayer, opponentPlayer);
                                        if (currentPlayer.CheckCount > 3 && willMoveCauseCheck)
                                        {
                                            return false;
                                        }
                                        return !willMoveCauseCheck;
                                    }).ToList();
                                }
                                validMoves.AddRange(moves);
                            }
                        }
                    }
                }
            }

            return validMoves;
        }

        /// <summary>
        ///   Returns legal drop positions for specified piece.
        /// </summary>
        /// <param name="piece">The piece to check legal positions on.</param>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <returns>
        ///   A list of positions that the piece in the piece specified can be dropped to.
        /// </returns>
        public static List<Point> ValidPositionsForPieceDrop(
            PieceData piece,
            Grid<PieceData> board,
            PlayerData currentPlayer,
            PlayerData opponentPlayer)
        {
            var validMoves = new List<Point>();

            // To check two pawn drop, no need to run it on non-pawn pieces but this is easier.
            var pawnColumns = new bool[board.Width];
            // The two foreach's cannot be collapsed into ones, we need to check the whole board for columns first.
            foreach (var tile in board)
            {
                if (tile.Content != null && tile.Content.IsPlayerOne == piece.IsPlayerOne && tile.Content.Type == PieceType.Pawn && !tile.Content.Promoted)
                {
                    pawnColumns[tile.Position.X] = true;
                }
            }

            foreach (var tile in board)
            {
                bool legalPosition = true;

                // Can't drop on an existing piece.
                if (tile.Content != null)
                {
                    legalPosition = false;
                }

                if (piece.Type == PieceType.Pawn)
                {
                    // Two pawn drop: You can't drop a pawn in a column where you already have a pawn.
                    // Mate pawn drop: You can't drop a pawn on a tile that will instantly cause a check for the opponent king.
                    if (pawnColumns[tile.Position.X] || Utils.WillMoveCauseCheckFor(piece, board, tile.Position, tile.Position, opponentPlayer, currentPlayer, !piece.IsPlayerOne))
                    {
                        legalPosition = false;
                    }
                }

                if (Utils.WillMoveCauseCheck(piece, board, tile.Position, tile.Position, currentPlayer, opponentPlayer))
                {
                    legalPosition = false;
                }

                if (legalPosition)
                {
                    validMoves.Add(tile.Position);
                }
            }

            return validMoves;
        }

        /// <summary>
        ///   Checks if the specified player's king is in check mate.
        /// </summary>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="isPlayerOne">For which player to check for checkmate.</param>
        /// <returns>
        ///   true if checkmate, false if not.
        /// </returns>
        public static bool IsKingCheckMated(Grid<PieceData> board, bool isPlayerOne, PlayerData currentPlayer, PlayerData opponentPlayer)
        {
            foreach (var tile in board)
            {
                if (tile.Content != null && tile.Content.IsPlayerOne == isPlayerOne)
                {
                    if (ValidMovesForPiece(tile.Content, board, tile.Position, currentPlayer, opponentPlayer).Count > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///   Checks if the specified player's king is in check.
        /// </summary>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="isPlayerOne">For which player's king to check for check.</param>
        /// <returns>
        ///   true if in check, false if not.
        /// </returns>
        public static bool IsKingChecked(Grid<PieceData> board, bool isPlayerOne, PlayerData currentPlayer, PlayerData opponentPlayer)
        {
            var possibleKingPiece = GetPlayerKing(board, isPlayerOne);
            if (possibleKingPiece is (PieceData piece, Point position) kingPiece)
            {
                var opponentControl = OpponentControl(board, isPlayerOne, currentPlayer, opponentPlayer);
                var pieceThatCanTakeKing = opponentControl
                    .Where((pair) => pair.Value.Contains(kingPiece.Position))
                    .ToList();
                return pieceThatCanTakeKing.Count > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///   Checks if the specified move will cause a check to the player's king who's piece is being moved.
        /// </summary>
        /// <param name="piece">The piece to check who's move will check the king or not.</param>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="currentX">X Position of the specified piece.</param>
        /// <param name="currentY">Y Position of the specified piece.</param>
        /// <param name="targetX">X Position of where the specified piece will move to.</param>
        /// <param name="targetY">Y Position of where the specified piece will move to.</param>
        /// <returns>
        ///   true if will check, false if not.
        /// </returns>
        public static bool WillMoveCauseCheck(
            PieceData piece,
            Grid<PieceData> board,
            Point current,
            Point target,
            PlayerData currentPlayer,
            PlayerData opponentPlayer)
            => WillMoveCauseCheckFor(piece, board, current, target, currentPlayer, opponentPlayer, piece.IsPlayerOne);

        /// <summary>
        ///   Checks if the specified move will cause a check to the player's king who's piece is being moved.
        /// </summary>
        /// <param name="piece">The piece to check who's move will check the king or not.</param>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="currentX">X Position of the specified piece.</param>
        /// <param name="currentY">Y Position of the specified piece.</param>
        /// <param name="targetX">X Position of where the specified piece will move to.</param>
        /// <param name="targetY">Y Position of where the specified piece will move to.</param>
        /// <param name="isPlayerOne">For which player's king to check for check.</param>
        /// <returns>
        ///   true if it will check, false if not.
        /// </returns>
        public static bool WillMoveCauseCheckFor(
            PieceData piece,
            Grid<PieceData> board,
            Point current,
            Point target,
            PlayerData currentPlayer,
            PlayerData opponentPlayer,
            bool isPlayerOne)
        {
            var tempBoard = board.Clone();

            tempBoard.SetAt(current.X, current.Y, null);
            tempBoard.SetAt(target.X, target.Y, piece);

            return IsKingChecked(tempBoard, isPlayerOne, currentPlayer, opponentPlayer);
        }

        /// <summary>
        ///   Gets the specified player's king.
        /// </summary>
        /// <param name="board">The grid that houses the pieces to check over.</param>
        /// <param name="isPlayerOne">For which player's king to return.</param>
        /// <returns>
        ///   A tuple containing the <see name="PieceData" /> and the X and Y positions of the king.
        /// </returns>
        public static (PieceData Piece, Point Position)? GetPlayerKing(Grid<PieceData> board, bool isPlayerOne)
        {
            for (int y = 0; y < board.Height; y++)
            {
                for (int x = 0; x < board.Width; x++)
                {
                    var pieceOnBoard = board.GetAt(x, y);
                    // If there's a piece at x,y on the board, and if that piece is the players, and if that piece is a king.
                    if (pieceOnBoard != null && pieceOnBoard.IsPlayerOne == isPlayerOne && pieceOnBoard.Type == PieceType.King)
                    {
                        return (pieceOnBoard, new Point(x, y));
                    }
                }
            }

            return null;
        }

        /// <summary>
        ///   Used to calculate what offset from the center a specific target and center coordinate is for the specified player.
        ///   Move sets are flipped for the player 2.
        /// </summary>
        private static Point SidedOffsetToGlobalOffset(Point center, Point local, bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                return Point.Zero - (center - local);
            }
            else
            {
                return (center - local);
            }
        }

        /// <summary>
        ///   Performs a raycast from <paramref name="currentX"/> and <paramref name="currentY"/> to <paramref name="targetX"/> and <paramref name="targetY"/>.
        ///   It then checks if it hits something on the way to the target.
        ///   If it hit one of the player specified in <paramref name="isPlayerOne"/>'s pieces, it stops before that.
        ///   If it hits one of the opponent pieces, it will be allowed to hit the first opponent piece but no more.
        /// </summary>
        private static List<Point> RayCastPathForMovement(
            Point current,
            Point direction,
            bool isPlayerOne,
            Grid<PieceData> board)
        {
            var validMoves = new List<Point>();
            // The ratio for which the y values with be incremented in relation to x, aka the slope
            // Can't divide by 0 so we must handle special case directionX,
            // but it won't be used anyway in the case directionX is zero so we can set whatever value.
            var yRatio = Math.Abs(direction.X == 0 ? 0 : direction.Y / direction.X);
            var xSign = Math.Sign(direction.X);
            var ySign = Math.Sign(direction.Y);

            PieceData prevRayPoint = null;
            // Step by the x direction
            for (int step = Math.Abs(direction.X); true; step++)
            {
                // If there is no x direction, there will be no ratio and so just set the y direction equal to the step.
                var y = direction.X == 0 ? step + 1 : step * yRatio;

                var target = current + new Point(step * xSign, y * ySign);

                // If the ray stepped outside the bounds of the board, we stop the stepping.
                if (!board.AreIndicesWithinBounds(target.X, target.Y))
                {
                    break;
                }

                var pieceAtRayPoint = board.GetAt(target.X, target.Y);

                // If there is a piece at the raypoint and it's the current player's piece, we stop stepping
                // If there was a piece at the previous step of the ray, we stop stepping.
                // This, in effect, makes it so that we can take exactly 1 of the enemy's piece in the direction but no more.
                if (pieceAtRayPoint != null && pieceAtRayPoint.IsPlayerOne == isPlayerOne
                        || prevRayPoint != null)
                {
                    break;
                }

                validMoves.Add(new Point(target.X, target.Y));
                prevRayPoint = pieceAtRayPoint;
            }

            return validMoves;
        }
    }
}