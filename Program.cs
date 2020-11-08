namespace Varga_Gergely_Pacman
{
    public enum Pixel : byte
    {
        Route,
        Wall,
        Token,
        PlayerOne,
        PlayerTwo,
    }
    
    public class Player
    {
        public int X;
        public int Y;
        public int HP;
        public string SettingName;
        public string Name;
        public string Sign;
        public bool TurnEnded;
        public int Identifier;
    }

    class Program
    {
        static void Main()
        {
            Player PlayerOne = new Player
            {
                SettingName = "Egyes Jatekos",
                Identifier = 0,
                TurnEnded = false
            };
            Player PlayerTwo = new Player
            {
                SettingName = "Kettes Jatekos",
                Identifier = 1,
                TurnEnded = true
            };
            Player Winner = new Player
            {
                Name = "Quit gomb"
            };
            Settings.Setting(PlayerOne, PlayerTwo);
            Moves.TheGame(PlayerOne, PlayerTwo, Winner);
        }
    }
}
