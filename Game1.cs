using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShogiClient
{
    public class Game1 : Game
    {
        public Vector2 WindowSize => Window.ClientBounds.Size.ToVector2();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameResources resources = new GameResources();

        private DrawableBoard board;

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

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (board.HeldPiece == null)
                {
                    var positionOnBoard = mouseState.Position.ToVector2() - board.Position + board.Size / 2 + board.TileSize / 2;
                    int tileX = (int)Math.Floor(positionOnBoard.X / board.TileSize.X);
                    int tileY = (int)Math.Floor(positionOnBoard.Y / board.TileSize.Y);

                    if (tileX >= 0 && tileX < board.Data.Width && tileY >= 0 && tileY < board.Data.Height)
                    {
                        if (board.PickUpPiece(tileX, tileY, true))
                        {
                            Console.WriteLine("Picked up piece");
                            board.HeldPiecePickUpPosition = (tileX, tileY);
                        }
                    }
                }

                if (board.HeldPiece != null)
                {
                    board.HeldPiecePosition = mouseState.Position.ToVector2();
                }

            }

            if (mouseState.LeftButton == ButtonState.Released && board.HeldPiece != null)
            {
                var positionOnBoard = board.HeldPiecePosition - board.Position + board.Size / 2 + board.TileSize / 2;
                int tileX = (int)Math.Floor(positionOnBoard.X / board.TileSize.X);
                int tileY = (int)Math.Floor(positionOnBoard.Y / board.TileSize.Y);

                if (tileX >= 0 && tileX < board.Data.Width
                    && tileY >= 0 && tileY < board.Data.Height
                    && board.PlacePiece(tileX, tileY, true, out PieceType? captured))
                {
                    Console.WriteLine($"Piece was placed, captured: {captured != null}");

                }
                else
                {
                    board.PlacePiece(board.HeldPiecePickUpPosition.X, board.HeldPiecePickUpPosition.Y);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            board.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
