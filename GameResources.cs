using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ShogiClient
{
    public class GameResources
    {
        public Texture2D Tile;
        public Texture2D Piece;
        public SpriteFont PieceFont;
        public Texture2D MoveIndicator;
        public SoundEffect RandomPiecePlaceSFX => PiecePlaceSFXs[random.Next(PiecePlaceSFXs.Length)];
        public SoundEffect[] PiecePlaceSFXs;
        public SoundEffect RandomPiecePickSFX => PiecePickSFXs[random.Next(PiecePickSFXs.Length)];
        public SoundEffect[] PiecePickSFXs;

        private Random random = new Random();

        public void LoadContent(ContentManager content)
        {
            Tile = content.Load<Texture2D>("Shogi Tile");
            Piece = content.Load<Texture2D>("Shogi Piece");
            PieceFont = content.Load<SpriteFont>("Piece Font");
            MoveIndicator = content.Load<Texture2D>("Shogi Move Indicator");

            PiecePlaceSFXs = new SoundEffect[5];
            PiecePlaceSFXs[0] = content.Load<SoundEffect>("Shogi Piece Place1");
            PiecePlaceSFXs[1] = content.Load<SoundEffect>("Shogi Piece Place2");
            PiecePlaceSFXs[2] = content.Load<SoundEffect>("Shogi Piece Place3");
            PiecePlaceSFXs[3] = content.Load<SoundEffect>("Shogi Piece Place4");
            PiecePlaceSFXs[4] = content.Load<SoundEffect>("Shogi Piece Place5");

            PiecePickSFXs = new SoundEffect[2];
            PiecePickSFXs[0] = content.Load<SoundEffect>("Shogi Piece Pick1");
            PiecePickSFXs[1] = content.Load<SoundEffect>("Shogi Piece Pick2");
        }
    }
}