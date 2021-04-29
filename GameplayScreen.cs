using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ShogiClient
{
    public class GameplayScreen : StatefulScreen<GameplayScreenState>
    {
        private DrawableBoard board;
        private DrawableHand playerOneHand;
        private DrawableHand playerTwoHand;
        private DrawableHand currentPlayerHand => State.IsPlayerOneTurn ? playerOneHand : playerTwoHand;

        public GameplayScreen(Game1 game) : base(game)
        {

        }

        public override void Initialize(GameResources resources)
        {
            base.Initialize(resources);

            board = new DrawableBoard(resources, State.boardState)
            {
                Position = Game.WindowSize / 2,
                Scale = new Vector2(2f, 2f),
            };

            playerOneHand = new DrawableHand(resources)
            {
                PlayerData = State.playerOne,
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y - 50),
                Scale = new Vector2(1.5f)
            };

            playerTwoHand = new DrawableHand(resources)
            {
                PlayerData = State.playerTwo,
                Position = new Vector2(Game.WindowSize.X / 2, 50),
                Scale = new Vector2(1.5f)
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState prevKeyboardState, MouseState mouseState, MouseState prevMouseState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape)) 
            {
                var currentGraphic = Game.Screenshot();
                Game.SetCurrentScreen(new GameplayPauseScreen(Game, State, currentGraphic));
                return;
            }

            var mousePosition = mouseState.Position.ToVector2();

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Resources.RandomGameplaySong);
            }

            var boardIndex = board.GetTileForCoordinate(mousePosition);
            var currentHandIndex = currentPlayerHand.GetIndexForCoordinate(mousePosition);
            if ((mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed) && board.State.HeldPiece == null)
            {
                bool failedToPick = true;

                if (
                    boardIndex.X >= 0 && boardIndex.X < board.State.Data.Width
                    && boardIndex.Y >= 0 && boardIndex.Y < board.State.Data.Height)
                {
                    if (board.State.PickUpPiece(boardIndex.X, boardIndex.Y, State.IsPlayerOneTurn))
                    {
                        board.State.HeldPiecePickUpPosition = (boardIndex.X, boardIndex.Y);
                        failedToPick = false;
                    }
                }
                else
                {
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

                if (!failedToPick)
                {
                    Resources.RandomPiecePickSFX.Play(0.5f, 0, 0);
                }
            }

            var leftMouseButtonReleased = (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed);
            var rightMouseButtonReleased = mouseState.RightButton == ButtonState.Released && prevMouseState.RightButton == ButtonState.Pressed;

            if ((leftMouseButtonReleased || rightMouseButtonReleased) && board.State.HeldPiece != null)
            {
                bool failedToPlace = false;
                var tryPromote = false;

                if (board.State.Data.AreIndicesWithinBounds(boardIndex.X, boardIndex.Y))
                {
                    if (board.State.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                    {
                        if (!board.State.PlacePiece(pickUpPosition.X, pickUpPosition.Y, boardIndex.X, boardIndex.Y, out PieceType? captured, tryPromote))
                        {
                            failedToPlace = true;
                        }
                        else
                        {
                            // If there was a captured piece, not null
                            if (captured is PieceType type)
                            {
                                State.CurrentPlayer.Hand.Add(type);
                            }

                            if (rightMouseButtonReleased) {
                                tryPromote = true;
                            }
                        }
                    }
                    else
                    {
                        if (!board.State.PlacePieceFromHand(boardIndex.X, boardIndex.Y))
                        {
                            failedToPlace = true;
                        }
                    }
                }
                else
                {
                    failedToPlace = true;
                }

                if (!failedToPlace)
                {
                    Resources.RandomPiecePlaceSFX.Play(0.5f, 0, 0);

                    if (tryPromote)
                    {
                        // Third last and third row respectively
                        var firstPromotionRow = State.IsPlayerOneTurn ? 2 : board.State.Data.Height - 3;
                        var indexToPromotionRow = boardIndex.Y - firstPromotionRow;
                        if (State.IsPlayerOneTurn)
                        {
                            indexToPromotionRow = -indexToPromotionRow;
                        }
                        if (indexToPromotionRow >= 0)
                        {
                            var promotePiece = board.State.Data.GetAt(boardIndex.X, boardIndex.Y);
                            if (Utils.CanPromotePieceType(promotePiece.Type))
                                board.State.Data.GetAt(boardIndex.X, boardIndex.Y).Promoted = true;
                        }
                    }

                    State.IsPlayerOneTurn = !State.IsPlayerOneTurn;

                    if (Utils.IsKingChecked(board.State.Data, State.IsPlayerOneTurn))
                    {
                        System.Console.WriteLine("Check");
                        if (Utils.IsKingCheckMated(board.State.Data, State.IsPlayerOneTurn))
                        {
                            System.Console.WriteLine("Checkmate");
                        }
                    }
                }
                else
                {
                    if (board.State.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                    {
                        board.State.PlacePiece(pickUpPosition.X, pickUpPosition.Y);
                    }
                    else
                    {
                        State.CurrentPlayer.Hand.Add(board.State.HeldPiece.Type);
                        board.State.HeldPiece = null;
                    }
                }
            }

            if (board.State.HeldPiece != null)
            {
                board.State.HeldPiecePosition = mousePosition;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Resources.GameplayBackground, new Rectangle(0, 0, (int)Game.WindowSize.X, (int)Game.WindowSize.Y), null, Color.White);
            board.Draw(spriteBatch);
            playerOneHand.Draw(spriteBatch);
            playerTwoHand.Draw(spriteBatch);
        }
    }
}