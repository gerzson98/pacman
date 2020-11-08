using System;
using System.IO;

namespace Varga_Gergely_Pacman
{
    static class Settings
    {
        public static int GameMode;
        public const int SinglePlayer = 1;
        public const int RegularGame = 2;
        public const int QuickMatch = 3;
        public static int Hardness;
        public static bool InstantDeath = false;
        public readonly static int yMax = 25;
        public readonly static int xMax = 30;
        public readonly static int TokenSpawnChance = 2;
        public const string DefaultMapName = "\\map_0.txt";
        private const string UsersMapName = "\\map.txt";

        public static void Setting(Player PlayerOne, Player PlayerTwo)
        {
            string Path = GetPathOfMap();
            MapReading(Path, PlayerOne, PlayerTwo);
            GameModeSet(PlayerOne, PlayerTwo);
        }

        public static string GetPathOfMap()
        {
            Console.WriteLine("Akarsz megadni sajat terkepet? [Y/N]");
            var input = Console.ReadKey();
            char pressedButton = input.KeyChar;
            pressedButton = char.ToUpper(pressedButton);
            switch (pressedButton)
            {
                case 'Y':
                    Console.Clear();
                    Console.WriteLine("A terkeped neve legyen: \n map.txt");
                    return Brief.PathSetByUser(UsersMapName);
                default:
                    return Brief.PathSetAuto(DefaultMapName);
            }
            
        }

        public static void MapReading(string pathOfMap, Player PlayerOne, Player PlayerTwo)
        {
            var input = File.ReadAllLines(pathOfMap);
            for (int j = 0; j < yMax; ++j)
            {
                var lineConverter = input[j].Split(' ');
                for (int i = 0; i < xMax; ++i)
                {
                    switch (Convert.ToInt32(lineConverter[i]))
                    {
                        case 0:
                            Brief.Map[i, j] = Pixel.Route;
                            break;
                        case 1:
                            Brief.Map[i, j] = Pixel.Wall;
                            break;
                        case 2:
                            Brief.Map[i, j] = Pixel.Token;
                            break;
                        case 3:
                            Brief.Map[i, j] = Pixel.PlayerOne;
                            PlayerOne.X = i;
                            PlayerOne.Y = j;
                            break;
                        case 4:
                            Brief.Map[i, j] = Pixel.PlayerTwo;
                            PlayerTwo.X = i;
                            PlayerTwo.Y = j;
                            break;
                        default:
                            Brief.Map[i, j] = Pixel.Wall;
                            break;
                    }
                }
            }
        }

        static void SinglePlayerSet(Player PlayerOne, Player PlayerTwo)
        {
            InstantDeath = true;
            PlayerTwo.Name = "roBOB";
            PlayerTwo.Sign = "><";
            bool nemjo = true;
            PlayerSet(PlayerOne);
            while (nemjo)
            {
                Console.Clear();
                Console.WriteLine("Valaszd ki a nehezseget![1-3]");
                var income = Console.ReadLine();
                try
                {
                    var check = Convert.ToInt32(income);
                    if (check == 1 || check == 2 || check == 3)
                    {
                        Hardness = check;
                        nemjo = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Nem volt jo a valasz formatuma. Probald pontosabban");
                }
            }
            PlayerTwo.HP = Hardness * 2;
        }

        public static void GameModeSet(Player PlayerOne, Player PlayerTwo)
        {
            Brief.Alive = true;
            Console.WriteLine("Valassz jatekmodot!");
            Console.WriteLine("Player vs BOT | Player vs Player | Player vs Player (quickmatch)");
            Console.WriteLine("      [1]             [2]                     [3]");
            int setting = 0;
            bool triedAlready = false;
            while (setting != 1 && setting != 2 && setting != 3)
            {
                if (triedAlready)
                {
                    Console.Clear();
                    Console.WriteLine("Valassz jatekmodot!");
                    Console.WriteLine("Player vs BOT | Player vs Player | Player vs Player (quickmatch)");
                    Console.WriteLine("      [1]             [2]                     [3]");
                    Console.WriteLine("\n Az elozo valaszod nem volt megfelelo! \n Kerlek nyomd meg az altalad kivalasztott jatekmodhoz tartozo szamot a billentyuzeten!");
                }
                triedAlready = true;
                var input = Console.ReadLine();
                setting = Convert.ToInt32(input);
            }
            GameMode = setting;
            switch (GameMode)
            {
                case SinglePlayer:
                    SinglePlayerSet(PlayerOne, PlayerTwo);
                    break;
                case RegularGame:
                    PlayerSet(PlayerOne);
                    PlayerSet(PlayerTwo);
                    InstantDeath = false;
                    break;
                case QuickMatch:
                    PlayerSet(PlayerOne);
                    PlayerSet(PlayerTwo);
                    InstantDeath = true;
                    break;
                default:
                    Console.WriteLine("Valami felrecsuszott. Allitsuk be ujra a dolgokat!");
                    GameModeSet(PlayerOne, PlayerTwo);
                    break;
            }
            Console.Clear();
        }

        public static void PlayerSet(Player Player)
        {
            Player.HP = 1;
            bool siker = false;
            bool triedAlready = false;
            while (!siker)
            {
                Console.Clear();
                if(triedAlready)
                    Console.WriteLine("A nevednek legalabb ket karakter hosszunak kell lennie! ");
                Console.WriteLine("Ki jatszik " + Player.SettingName + "kent ?");
                Player.Name = Console.ReadLine();
                if (Player.Name.Length > 1)
                    siker = true;
                else
                    triedAlready = true;
            }
            siker = false;
            triedAlready = false;
            while (!siker)
            {
                Console.Clear();
                if (triedAlready)
                    Console.WriteLine("A jelednek legalabb ket karakter hosszunak kell lennie! \n Nem tartalazhat terkepelemet (|, ' ', $) illetve nehezen lathato karaktert!");
                Console.WriteLine("Ird be a Jatek kozben megjelenitendo Jeled!");
                var input = Console.ReadLine();
                if (input.Length > 1)
                {
                    Player.Sign = input.Substring(0, 2);
                    siker = Accessories.SignFormatCheck(Player);
                }
                else
                    triedAlready = true;
            }
        }
    }
}