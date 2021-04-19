using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class Game1 : Game
    {
        public Vector2 WindowSize => Window.ClientBounds.Size.ToVector2();

        public PlayerData CurrentPlayer => isPlayerOneTurn ? playerOne : playerTwo;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameResources resources = new GameResources();

        private DrawableBoard board;
        private DrawableHand playerOneHand;
        private DrawableHand playerTwoHand;
        private DrawableHand currentPlayerHand => isPlayerOneTurn ? playerOneHand : playerTwoHand;

        private bool isPlayerOneTurn = true;
        private PlayerData playerOne = new PlayerData();
        private PlayerData playerTwo = new PlayerData();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            board = new DrawableBoard(resources, 9, 9)
            {
                Position = WindowSize / 2,
                Scale = new Vector2(2f, 2f),
            };

            playerOneHand = new DrawableHand(resources)
            {
                PlayerData = playerOne,
                Position = new Vector2(WindowSize.X / 2, WindowSize.Y - 50),
                Scale = new Vector2(1.5f)
            };

            playerTwoHand = new DrawableHand(resources)
            {
                PlayerData = playerTwo,
                Position = new Vector2(WindowSize.X / 2, 50),
                Scale = new Vector2(1.5f)
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resources.LoadContent(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var mousePosition = mouseState.Position.ToVector2();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            var boardIndex = board.GetTileForCoordinate(mousePosition);
            var currentHandIndex = currentPlayerHand.GetIndexForCoordinate(mousePosition);
            if (mouseState.LeftButton == ButtonState.Pressed && board.HeldPiece == null)
            {
                if (
                    boardIndex.X >= 0 && boardIndex.X < board.Data.Width
                    && boardIndex.Y >= 0 && boardIndex.Y < board.Data.Height)
                {

                    if (board.PickUpPiece(boardIndex.X, boardIndex.Y, isPlayerOneTurn))
                    {
                        Console.WriteLine("Picked up piece");
                        board.HeldPiecePickUpPosition = (boardIndex.X, boardIndex.Y);
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
                            IsPlayerOne = isPlayerOneTurn,
                        };
                        board.HeldPiecePickUpPosition = null;
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Released && board.HeldPiece != null)
            {
                bool failedToPlace = false;
                if (boardIndex.X >= 0 && boardIndex.X < board.Data.Width
                    && boardIndex.Y >= 0 && boardIndex.Y < board.Data.Height)
                {
                    if (board.HeldPiecePickUpPosition is (int, int) pickUpPosition)
                    {
                        if (board.PlacePiece(pickUpPosition.X, pickUpPosition.Y, boardIndex.X, boardIndex.Y, isPlayerOneTurn, out PieceType? captured))
                        {
                            Console.WriteLine($"Piece was placed, captured: {captured != null}");
                            // If there was a captured piece, not null
                            if (captured is PieceType type)
                            {
                                CurrentPlayer.Hand.Add(type);
                            }
                            isPlayerOneTurn = !isPlayerOneTurn;

                        }
                        else
                        {
                            failedToPlace = true;

                        }
                    }
                    else
                    {
                        if (board.PlacePieceFromHand(boardIndex.X, boardIndex.Y))
                        {
                            Console.WriteLine("Piece placed from hand");
                            isPlayerOneTurn = !isPlayerOneTurn;
                        }
                        else
                        {
                            failedToPlace = true;

                        }
                    }
                }
                else
                {
                    failedToPlace = true;
                }

                if (failedToPlace)
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            board.Draw(spriteBatch);
            playerOneHand.Draw(spriteBatch);
            playerTwoHand.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
