using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ShogiClient
{
    /// <summary>
    ///   Resources used in the game
    /// </summary>
    public class GameResources
    {
        public Texture2D Logo;
        public Texture2D MainMenuBackground;
        public Texture2D GameplayBackground;
        public Texture2D UIButton;
        public Texture2D Tile;
        public Texture2D Piece;
        public SpriteFont PieceFont;
        public Texture2D MoveIndicator;
        public SoundEffect RandomPiecePlaceSFX => PiecePlaceSFXs[random.Next(PiecePlaceSFXs.Length)];
        public SoundEffect[] PiecePlaceSFXs;
        public SoundEffect RandomPiecePickSFX => PiecePickSFXs[random.Next(PiecePickSFXs.Length)];
        public SoundEffect[] PiecePickSFXs;
        public Song RandomMainMenuSong => MainMenuSongs[random.Next(MainMenuSongs.Length)];
        public Song[] MainMenuSongs;
        public Song RandomGameplaySong => GameplaySongs[random.Next(GameplaySongs.Length)];
        public Song[] GameplaySongs;

        private Random random = new Random();

        public void LoadContent(ContentManager content)
        {
            Logo = content.Load<Texture2D>("Shogi Logo");
            GameplayBackground = content.Load<Texture2D>("Tatami");
            MainMenuBackground = content.Load<Texture2D>("Stone Background");
            UIButton = content.Load<Texture2D>("UI Button");
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

            MainMenuSongs = new Song[1];
            MainMenuSongs[0] = content.Load<Song>("Music/To the Limitt");

            GameplaySongs = new Song[2];
            GameplaySongs[0] = content.Load<Song>("Music/Silver Forest - Cherry Phantasm - 11 - ");
            GameplaySongs[1] = content.Load<Song>("Music/Haru - Taketori (Instrumental)");
        }
    }
}