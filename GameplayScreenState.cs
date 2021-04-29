namespace ShogiClient
{
    public class GameplayScreenState : ScreenState
    {
        public PlayerData CurrentPlayer => isPlayerOneTurn ? playerOne : playerTwo;
        public bool IsPlayerOneTurn
        {
            get => Game1.DEBUG_PLAYERONE ? true : isPlayerOneTurn;
            set => isPlayerOneTurn = value;
        }

        public bool isPlayerOneTurn = true;

        public PlayerData playerOne = new PlayerData();
        public PlayerData playerTwo = new PlayerData();
    }
}