using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShogiClient
{
    /// <summary>
    ///   The screen used for actual gameplay.
    /// </summary>
    public class GameplayScreen : StatefulScreen<GameplayScreenState>
    {
        private DrawableBoard board;
        private DrawableHand playerOneHand;
        private DrawableHand playerTwoHand;
        private DrawableHand currentPlayerHand => State.IsPlayerOneTurn ? playerOneHand : playerTwoHand;
        private UIPanel CheckPanel;
        private UIButton pauseButton;
        private UIButton optionsButton;
        private UIButton helpButton;
        private UITable turnTable;
        private UIButton takeBackButton;

        public GameplayScreen(Game1 game) : base(game)
        {

        }

        public override void Initialize(GameResources resources)
        {
            base.Initialize(resources);

            State.BoardState.GameplayState = State;

            board = new DrawableBoard(resources, State.BoardState)
            {
                Position = Game.WindowSize / 2,
                Scale = new Vector2(2f, 2f),
            };

            playerOneHand = new DrawableHand(resources)
            {
                PlayerData = State.PlayerOne,
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y - 50),
                Scale = new Vector2(1.5f)
            };

            playerTwoHand = new DrawableHand(resources)
            {
                PlayerData = State.PlayerTwo,
                Position = new Vector2(Game.WindowSize.X / 2, 50),
                Scale = new Vector2(1.5f)
            };
            CheckPanel = new UIPanel(Game)
            {
                Position = Vector2.Zero,
                Size = Game.WindowSize,
                Color = new Color(Color.DarkRed, 0.0001f),
            };
            pauseButton = new UIButton(Resources)
            {
                Position = new Vector2(100, 50),
                Size = new Vector2(100, 50),
                Text = "Pause"
            };
            pauseButton.OnClick += () =>
            {
                var currentGraphic = Game.Screenshot();
                Game.SetCurrentScreen(new GameplayPauseScreen(Game, Resources, State, currentGraphic), false);
            };
            optionsButton = new UIButton(Resources)
            {
                Position = new Vector2(100, 110),
                Size = new Vector2(100, 50),
                Text = "Options"
            };
            optionsButton.OnClick += () =>
            {
                var currentGraphic = Game.Screenshot();
                Game.SetCurrentScreen(new OptionsScreen<GameplayScreenState>(Game, Resources, State, currentGraphic), false);
            };
            helpButton = new UIButton(Resources)
            {
                Position = new Vector2(Game.WindowSize.X - 100, 50),
                Size = new Vector2(100, 50),
                Text = "Help"
            };
            helpButton.OnClick += () =>
            {
                var currentGraphic = Game.Screenshot();
                Game.SetCurrentScreen(new GameplayHelpScreen(Game, Resources, State, currentGraphic), false);
            };

            turnTable = new UITable(Game, Resources)
            {
                Position = new Vector2(Game.WindowSize.X * 4 / 5, Game.WindowSize.Y / 2),
                Size = new Vector2(Game.WindowSize.X / 5 - 100, Game.WindowSize.Y * 2 / 3),
                TableWidth = 2,
                EntryHeight = 15,
            };
            turnTable.Data.AddRange(State.TurnList.Select(turn => turn.ToNotation()));

            takeBackButton = new UIButton(Resources)
            {
                Position = new Vector2(Game.WindowSize.X - 100, Game.WindowSize.Y / 2),
                Size = new Vector2(100, 50),
                Text = "Take back",
            };
            takeBackButton.OnClick += () =>
            {
                if (State.TurnList.Count > 0)
                {
                    var lastTurn = State.TurnList[State.TurnList.Count - 1];
                    turnTable.Data.RemoveAt(turnTable.Data.Count - 1);
                    State.TurnList.RemoveAt(State.TurnList.Count - 1);

                    var lastTurnPlayer = State.IsPlayerOneTurn ? State.PlayerTwo : State.PlayerOne;

                    if (lastTurn is MoveTurn moveTurn)
                    {
                        if (moveTurn.DidPromote)
                        {
                            moveTurn.Piece.Promoted = false;
                        }
                        State.BoardState.Data.SetAt(moveTurn.XFrom, moveTurn.YFrom, moveTurn.Piece);
                        State.BoardState.Data.SetAt(moveTurn.XTarget, moveTurn.YTarget, moveTurn.Captured);
                        if (moveTurn.Captured != null)
                        {
                            lastTurnPlayer.Hand.Remove(moveTurn.Captured.Type);
                        }
                    }
                    else if (lastTurn is DropTurn dropTurn)
                    {
                        State.BoardState.Data.SetAt(dropTurn.X, dropTurn.Y, null);
                        lastTurnPlayer.Hand.Add(dropTurn.Type);
                    }

                    EndOfTurn();
                }
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                var currentGraphic = Game.Screenshot();
                Game.SetCurrentScreen(new GameplayPauseScreen(Game, Resources, State, currentGraphic), false);
                return;
            }

            var mousePosition = mouseState.Position.ToVector2();

            var boardIndex = board.GetTileForCoordinate(mousePosition);
            var currentHandIndex = currentPlayerHand.GetIndexForCoordinate(mousePosition);
            // If the player just clicked the left button and if he is not holding anything.
            if ((mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed) && board.State.HeldPiece == null)
            {
                bool failedToPick = true;

                // Checks if the piece is within the bounds of the board and is allowed to be picked up by them.
                if (
                    boardIndex.X >= 0 && boardIndex.X < board.State.Data.Width
                    && boardIndex.Y >= 0 && boardIndex.Y < board.State.Data.Height)
                {
                    if (board.State.PickUpPiece(boardIndex, State.IsPlayerOneTurn))
                    {
                        board.State.HeldPiecePickUpPosition = boardIndex;
                        failedToPick = false;
                    }
                }
                else // If the piece is not on the board it might be on the hand.
                {
                    // Checks if the piece is within the bounds of their hand, if so it will pick it up from there.
                    if (currentHandIndex.X >= 0 && currentHandIndex.X < State.CurrentPlayer.Hand.Count
                    && currentHandIndex.Y == 0)
                    {
                        var idx = currentHandIndex.X;
                        var pieceType = State.CurrentPlayer.Hand[idx];
                        State.CurrentPlayer.Hand.RemoveAt(idx);
                        board.State.HeldPiece = new PieceData()
                        {
                            Type = pieceType,
                            Promoted = false,
                            IsPlayerOne = State.IsPlayerOneTurn,
                        };
                        board.State.HeldPiecePickUpPosition = null;
                        failedToPick = false;
                    }
                }

                // Play a sound effect if the piece was picked up.
                if (!failedToPick)
                {
                    Resources.RandomPiecePickSFX.Play(0.5f, 0, 0);
                }
            }

            var leftMouseButtonReleased = (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed);
            var rightMouseButtonReleased = mouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed;

            // If the player just released the left (normal drop) or the right (promotion drop) mouse button and that they are holding a piece.
            if ((leftMouseButtonReleased || rightMouseButtonReleased) && board.State.HeldPiece != null)
            {
                bool failedToPlace = false;
                var tryPromote = false;
                ITurn turnData = null;

                // Checks if the dropped piece is within the bounds of the board.
                if (board.State.Data.AreIndicesWithinBounds(boardIndex.X, boardIndex.Y))
                {
                    // Checks if the piece was picked up from the board
                    if (board.State.HeldPiecePickUpPosition is Point pickUpPosition)
                    {
                        // Checks if the piece is allowed to be placed and that positions.
                        if (!board.State.PlacePiece(pickUpPosition, boardIndex, out PieceData captured))
                        {
                            failedToPlace = true;
                        }
                        else
                        {
                            // If a piece was captured, it will be added to the hand.
                            if (captured != null)
                            {
                                State.CurrentPlayer.Hand.Add(captured.Type);
                            }

                            // If the player used right mouse button release, the piece will be attempted to be promoted.
                            if (rightMouseButtonReleased)
                            {
                                tryPromote = true;
                            }

                            // Specifies the data used for this move turn.
                            var piece = State.BoardState.Data.GetAt(boardIndex.X, boardIndex.Y);
                            turnData = new MoveTurn(piece.Data, false, false, pickUpPosition.X, pickUpPosition.Y, boardIndex.X, boardIndex.Y, captured);
                        }
                    }
                    else // Or if it was picked up from the hand
                    {
                        if (!board.State.PlacePieceFromHand(boardIndex))
                        {
                            failedToPlace = true;
                        }
                        else
                        {
                            // Specifies the data used for this drop turn.
                            turnData = new DropTurn(State.BoardState.Data.GetAt(boardIndex.X, boardIndex.Y).Data.Type, boardIndex.X, boardIndex.Y, false);
                        }
                    }
                }
                else
                {
                    failedToPlace = true;
                }

                // If the piece was successfully placed.
                if (!failedToPlace)
                {
                    Resources.RandomPiecePlaceSFX.Play(0.5f, 0, 0);

                    var placedPiece = board.State.Data.GetAt(boardIndex.X, boardIndex.Y).Data;

                    bool didPromote = false;

                    // We will calculate the rows in which the player is allowed to promote on. The promotion zone.

                    // Third last and third row respectively
                    var firstPromotionRow = State.IsPlayerOneTurn ? 2 : board.State.Data.Height - 3;
                    var indexToPromotionRow = boardIndex.Y - firstPromotionRow;
                    if (State.IsPlayerOneTurn)
                    {
                        indexToPromotionRow = -indexToPromotionRow;
                    }

                    // The pawn, lance and knight will be forcefully promoted as they can't move otherwise.
                    if (placedPiece.Type == PieceType.Pawn || placedPiece.Type == PieceType.Lance)
                    {
                        if (boardIndex.Y == (State.IsPlayerOneTurn ? 0 : board.State.Data.Height - 1))
                        {
                            tryPromote = true;
                        }
                    }
                    if (placedPiece.Type == PieceType.Knight)
                    {
                        if (indexToPromotionRow >= 1)
                        {
                            tryPromote = true;
                        }
                    }

                    if (tryPromote)
                    {
                        // Checks if the piece is within range of the promotion zone.
                        if (indexToPromotionRow >= 0)
                        {
                            var promotePiece = placedPiece;
                            if (Utils.CanPromotePieceType(promotePiece.Type))
                            {
                                placedPiece.Promoted = true;
                                didPromote = true;
                            }
                        }
                    }

                    EndOfTurn();

                    // If there was a successful turn, add that data to the turn and turn table lists.
                    if (turnData != null)
                    {
                        turnData.DidCheck = State.IsCheck;
                        if (turnData is MoveTurn moveData)
                        {
                            moveData.DidPromote = didPromote;
                        }
                        System.Console.WriteLine(turnData.ToNotation());
                        turnTable.Data.Add(turnData.ToNotation());
                        State.TurnList.Add(turnData);
                    }

                    // If the previous move caused a check mate, go to the result screen.
                    if (State.IsCheckMate)
                    {
                        var currentGraphic = Game.Screenshot();
                        Game.SetCurrentScreen(new ResultScreen(Game, State, currentGraphic), false);
                        return;
                    }
                }
                else // If the piece was not successfully placed, it will go back to it's origin.
                {
                    if (board.State.HeldPiecePickUpPosition is Point pickUpPosition)
                    {
                        board.State.PlacePiece(pickUpPosition);
                    }
                    else
                    {
                        State.CurrentPlayer.Hand.Add(board.State.HeldPiece.Type);
                        board.State.HeldPiece = null;
                    }
                }
            }

            if (State.CurrentPlayer.TimeLeft <= 0)
            {
                // TODO Lose by running out of time
            }
            else if (State.clockRunning)
            {
                State.CurrentPlayer.TimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (board.State.HeldPiece != null)
            {
                board.State.HeldPiecePosition = mousePosition;
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Resources.RandomGameplaySong);
            }

            pauseButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            optionsButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            helpButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            takeBackButton.Update(gameTime, keyboardState, mouseState, prevMouseState);
            turnTable.Update(gameTime, keyboardState, mouseState, prevMouseState);
        }

        // Sets various data at the end of the turn.
        public void EndOfTurn()
        {
            State.IsPlayerOneTurn = !State.IsPlayerOneTurn;

            State.IsCheck = false;
            State.IsCheckMate = false;
            if (Utils.IsKingChecked(board.State.Data, State.IsPlayerOneTurn, State.CurrentPlayer, State.OpponentPlayer))
            {
                State.IsCheck = true;
                if (Utils.IsKingCheckMated(board.State.Data, State.IsPlayerOneTurn, State.CurrentPlayer, State.OpponentPlayer))
                {
                    State.IsCheckMate = true;
                }

                State.OpponentPlayer.CheckCount += 1;
            }
            else
            {
                State.OpponentPlayer.CheckCount = 0;
            }

            State.clockRunning = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Resources.GameplayBackground, new Rectangle(0, 0, (int)Game.WindowSize.X, (int)Game.WindowSize.Y), null, Color.White);
            board.Draw(spriteBatch);
            playerOneHand.Draw(spriteBatch);
            playerTwoHand.Draw(spriteBatch);
            if (State.IsCheck)
            {
                CheckPanel.Draw(spriteBatch);
            }

            pauseButton.Draw(spriteBatch);
            optionsButton.Draw(spriteBatch);
            helpButton.Draw(spriteBatch);
            takeBackButton.Draw(spriteBatch);

            turnTable.Draw(spriteBatch);

            var tableText = $@"{Utils.PieceToKanji(new PieceData { Type = PieceType.Pawn, Promoted = false })} - Pawn
{Utils.PieceToKanji(new PieceData { Type = PieceType.Bishop, Promoted = false })} - Bishop
{Utils.PieceToKanji(new PieceData { Type = PieceType.Rook, Promoted = false })} - Rook
{Utils.PieceToKanji(new PieceData { Type = PieceType.Lance, Promoted = false })} - Lance
{Utils.PieceToKanji(new PieceData { Type = PieceType.Knight, Promoted = false })} - Knight
{Utils.PieceToKanji(new PieceData { Type = PieceType.Silver, Promoted = false })} - Silver
{Utils.PieceToKanji(new PieceData { Type = PieceType.Gold, Promoted = false })} - Gold
{Utils.PieceToKanji(new PieceData { Type = PieceType.King, Promoted = false })}/{Utils.PieceToKanji(new PieceData { Type = PieceType.King, Promoted = false, IsPlayerOne = true })} - King";
            spriteBatch.DrawString(Resources.PieceFont, tableText, new Vector2(Game.WindowSize.X / 5, Game.WindowSize.Y * 2 / 6) - Resources.PieceFont.MeasureString(tableText) / 2, Color.White);
            var promotedTableText = $@"{Utils.PieceToKanji(new PieceData { Type = PieceType.Pawn, Promoted = true })} - tokin (Promoted Pawn)
{Utils.PieceToKanji(new PieceData { Type = PieceType.Bishop, Promoted = true })} - Horse (Promoted Bishop)
{Utils.PieceToKanji(new PieceData { Type = PieceType.Rook, Promoted = true })} - Dragon (Promoted Rook)
{Utils.PieceToKanji(new PieceData { Type = PieceType.Lance, Promoted = true })} - Promoted Lance
{Utils.PieceToKanji(new PieceData { Type = PieceType.Knight, Promoted = true })} - Promoted Knight
{Utils.PieceToKanji(new PieceData { Type = PieceType.Silver, Promoted = true })} - Promoted Silver";
            spriteBatch.DrawString(Resources.PieceFont, promotedTableText, new Vector2(Game.WindowSize.X / 5, Game.WindowSize.Y * 4 / 6) - Resources.PieceFont.MeasureString(promotedTableText) / 2, Color.Red);
            if (State.TurnList.Count > 0)
            {
                var lastMoveText = State.TurnList[State.TurnList.Count - 1].ToNotation();
                spriteBatch.DrawString(Resources.PieceFont, lastMoveText, new Vector2(Game.WindowSize.X / 12, Game.WindowSize.Y / 2) - Resources.PieceFont.MeasureString(lastMoveText) / 2, Color.Black);
            }
            var playerOneTimerText = $"{(int)(State.PlayerOne.TimeLeft / 60):00}:{(int)(State.PlayerOne.TimeLeft % 60):00}";
            spriteBatch.DrawString(Resources.PieceFont, playerOneTimerText, new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y - 95) - Resources.PieceFont.MeasureString(playerOneTimerText) / 2, Color.Black);
            var playerTwoTimerText = $"{(int)(State.PlayerTwo.TimeLeft / 60):00}:{(int)(State.PlayerTwo.TimeLeft % 60):00}";
            spriteBatch.DrawString(Resources.PieceFont, playerTwoTimerText, new Vector2(Game.WindowSize.X / 2, 95) - Resources.PieceFont.MeasureString(playerTwoTimerText) / 2, Color.Black);
        }
    }
}