using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;

namespace Varga_Gergely_Pacman
{
    public class Brief
    {
        public readonly static int[,] Ways = new int[5, 2] { { 0, 0 }, { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        public readonly static int X = 0;
        public readonly static int Y = 1;
        public readonly static int Stay = 0;
        public readonly static int North = 1;
        public readonly static int South = 2;
        public readonly static int East = 3;
        public readonly static int West = 4;
        private const string EndingScreenFileName = "\\GameOver.txt";
        public static bool Alive = true;
        public static Pixel[,] Map = new Pixel[Settings.xMax, Settings.yMax];

        public static void Draw(Player PlayerOne, Player PlayerTwo)
        {
            Console.Clear();
            //azért fordítva, hogy könnyebb legyen kezelni a föl-le-t, hiszen a kijelzőn az először kiírt sor van felül.
            for (int j = Settings.yMax - 1; j >= 0; j--)
            {
                Console.Write("\n");
                for (int i = 0; i < Settings.xMax; ++i)
                {
                    switch (Map[i, j])
                    {
                        case Pixel.Route:
                            Console.Write("  ");
                            break;
                        case Pixel.Wall:
                            Console.Write("||");
                            break;
                        case Pixel.Token:
                            Console.Write("$$");
                            break;
                        case Pixel.PlayerOne:
                            Console.Write(PlayerOne.Sign);
                            break;
                        case Pixel.PlayerTwo:
                            Console.Write(PlayerTwo.Sign);
                            break;
                        default:
                            Console.Write("██");
                            break;
                    }
                }
            }
            string PlayerOneStat = PlayerOne.Name + " [Hp: " + PlayerOne.HP + "]";
            string PlayerTwoStat = PlayerTwo.Name + " [Hp: " + PlayerTwo.HP + "]";
            int wallCount = Settings.xMax * 2 - (PlayerOneStat.Length + PlayerTwoStat.Length);
            string wall = "||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||";
            string fill = wall.Substring(0, wallCount);
            Console.Write("\n" + PlayerOneStat + fill + PlayerTwoStat);
        }

        public static string PathSetByUser(string FileName)
        {
            string Path;
            Console.WriteLine("Ha meg akarod adni a fajl helyet, írd/masold be ide egy sorban!");
            Console.WriteLine("Ha az a .exe file mellett van, csak uss ENTER-t!");
            List<char> InputList = new List<char>();
            string income = Console.ReadLine();
            for (int i = 0; i < income.Length; ++i)
            {
                InputList.Add(income[i]);
                if (income[i] == '\\')
                    InputList.Add('\\');
            }
            string input = new string(InputList.ToArray());
            Console.WriteLine(input);
            if (string.IsNullOrWhiteSpace(input))
                Path = PathSetAuto(Settings.DefaultMapName);
            else if (File.Exists(input + FileName))
                Path = input + FileName;
            else
            {
                Console.WriteLine("A fajl helye nem volt megfelelo formatumu. A program melletti fajl lett betoltve.");
                Path = PathSetAuto(Settings.DefaultMapName);
            }
            return Path;
        }

        public static string PathSetAuto(string FileName)
        {
            string Path = Directory.GetCurrentDirectory() + FileName;
            return Path;
        }

        public static void EndingScreen(Player Winner)
        {
            Console.Clear();
            string pathOfEndingScreen = PathSetAuto(EndingScreenFileName);
            string endingScreen = File.ReadAllText(pathOfEndingScreen);
            Console.Write(endingScreen + "\n");
            Console.Write("                     " + Winner.Name + " nyerte a jatekot!");
        }
    }
}
