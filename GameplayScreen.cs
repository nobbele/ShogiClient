using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class GameplayScreen : Screen
    {
        public PlayerData CurrentPlayer => isPlayerOneTurn ? playerOne : playerTwo;
        public bool IsPlayerOneTurn
        {
            get => Game1.DEBUG_PLAYERONE ? true : isPlayerOneTurn;
            set => isPlayerOneTurn = value;
        }

        private DrawableBoard board;
        private DrawableHand playerOneHand;
        private DrawableHand playerTwoHand;
        private DrawableHand currentPlayerHand => IsPlayerOneTurn ? playerOneHand : playerTwoHand;

        private bool isPlayerOneTurn = true;
        private PlayerData playerOne = new PlayerData();
        private PlayerData playerTwo = new PlayerData();

        private MouseState previousMouseState = new MouseState();

        public GameplayScreen(Game1 game) : base(game)
        {

        }

        public override void Initialize(GameResources resources)
        {
            base.Initialize(resources);

            board = new DrawableBoard(resources, 9, 9)
            {
                Position = Game.WindowSize / 2,
                Scale = new Vector2(2f, 2f),
            };

            playerOneHand = new DrawableHand(resources)
            {
                PlayerData = playerOne,
                Position = new Vector2(Game.WindowSize.X / 2, Game.WindowSize.Y - 50),
                Scale = new Vector2(1.5f)
            };

            playerTwoHand = new DrawableHand(resources)
            {
                PlayerData = playerTwo,
                Position = new Vector2(Game.WindowSize.X / 2, 50),
                Scale = new Vector2(1.5f)
            };
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            base.Update(gameTime, keyboardState, mouseState);

            var mousePosition = mouseState.Position.ToVector2();

            var boardIndex = board.GetTileForCoordinate(mousePosition);
            var currentHandIndex = currentPlayerHand.GetIndexForCoordinate(mousePosition);
            if ((mouseState.LeftButton == ButtonState.Pressed || mouseState.RightButton == ButtonState.Pressed) && board.HeldPiece == null)
            {
                bool failedToPick = true;

                if (
                    boardIndex.X >= 0 && boardIndex.X < board.Data.Width
                    && boardIndex.Y >= 0 && boardIndex.Y < board.Data.Height)
                {
                    if (board.PickUpPiece(boardIndex.X, boardIndex.Y, IsPlayerOneTurn))
                    {
                        board.HeldPiecePickUpPosition = (boardIndex.X, boardIndex.Y);
                        failedToPick = false;
                    }
                }
                else
                {
                    if (currentHandIndex.X >= 0 && currentHandIndex.X < CurrentPlayer.Hand.Count
                    && currentHandIndex.Y == 0)
                    {
                        var idx = currentHandIndex.X;
                        var pieceType = CurrentPlayer.Hand[idx];
                        CurrentPlayer.Hand.RemoveAt(idx);
                        board.HeldPiece = new PieceData()
                        {
                            Type = pieceType,
                            Promoted = false,
                            IsPlayerOne = IsPlayerOneTurn,
                        };
                        board.HeldPiecePickUpPosition = null;
                        failedToPick = false;
                    }
                }

                if (!failedToPick)
                {
                    Resources.RandomPiecePickSFX.Play(0.5f, 0, 0);
                }
            }

            var tryPromote = false;

            var leftMouseButtonReleased = (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed);
            var rightMouseButtonReleased = mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;

            if ((leftMouseButtonReleased || rightMouseButtonReleased) && board.HeldPiece != null)
            {
                bool failedToPlace = false;

                if (rightMouseButtonReleased)
                {
                    tryPromote = true;
                }

                if (board.Data.AreIndicesWithinBounds(boardIndex.X, boardIndex.Y))
                {
                    if (board.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                    {
                        if (!board.PlacePiece(pickUpPosition.X, pickUpPosition.Y, boardIndex.X, boardIndex.Y, out PieceType? captured, tryPromote))
                        {
                            failedToPlace = true;
                        }
                        else
                        {
                            // If there was a captured piece, not null
                            if (captured is PieceType type)
                            {
                                CurrentPlayer.Hand.Add(type);
                            }
                        }
                    }
                    else
                    {
                        if (!board.PlacePieceFromHand(boardIndex.X, boardIndex.Y))
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
                        var firstPromotionRow = IsPlayerOneTurn ? 2 : board.Data.Height - 3;
                        var indexToPromotionRow = boardIndex.Y - firstPromotionRow;
                        if (IsPlayerOneTurn)
                        {
                            indexToPromotionRow = -indexToPromotionRow;
                        }
                        if (indexToPromotionRow >= 0)
                        {
                            var promotePiece = board.Data.GetAt(boardIndex.X, boardIndex.Y);
                            if (Utils.CanPromotePieceType(promotePiece.Type))
                                board.Data.GetAt(boardIndex.X, boardIndex.Y).Promoted = true;
                        }
                    }

                    IsPlayerOneTurn = !IsPlayerOneTurn;

                    if (Utils.IsKingChecked(board.Data, IsPlayerOneTurn))
                    {
                        System.Console.WriteLine("Check");
                        if (Utils.IsKingCheckMated(board.Data, IsPlayerOneTurn))
                        {
                            System.Console.WriteLine("Checkmate");
                        }
                    }
                }
                else
                {
                    if (board.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                    {
                        board.PlacePiece(pickUpPosition.X, pickUpPosition.Y);
                    }
                    else
                    {
                        CurrentPlayer.Hand.Add(board.HeldPiece.Type);
                        board.HeldPiece = null;
                    }
                }
            }

            if (board.HeldPiece != null)
            {
                board.HeldPiecePosition = mousePosition;
            }

            previousMouseState = mouseState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(Resources.Background, new Rectangle(0, 0, (int)Game.WindowSize.X, (int)Game.WindowSize.Y), null, Color.White);
            board.Draw(spriteBatch);
            playerOneHand.Draw(spriteBatch);
            playerTwoHand.Draw(spriteBatch);
        }
    }
}