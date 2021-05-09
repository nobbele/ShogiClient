using System.Collections.Generic;

namespace ShogiClient
{
    public class GameplayScreenState : ScreenState
    {
        public PlayerData CurrentPlayer => IsPlayerOneTurn ? PlayerOne : PlayerTwo;

        private bool isPlayerOneTurn = true;
        public bool IsPlayerOneTurn
        {
            get => Game1.DEBUG_PLAYERONE ? true : isPlayerOneTurn;
            set => isPlayerOneTurn = value;
        }

        public bool IsCheck = false;
        public bool IsCheckMate = false;

        public PlayerData PlayerOne = new PlayerData();
        public PlayerData PlayerTwo = new PlayerData();

        public Board BoardState = new Board(9, 9);

        public List<ITurn> TurnList = new List<ITurn>();
    }
}