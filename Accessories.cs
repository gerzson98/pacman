using System;

namespace Varga_Gergely_Pacman
{
    class Accessories
    {
        public static void Respawn(Player Who)
        {
            int xTo = 0;
            int yTo = 0;
            bool thatsCool = false;
            while (!thatsCool)
            {
                Random x = new Random();
                Random y = new Random();
                xTo = x.Next(1, 28);
                yTo = y.Next(1, 23);
                if (Brief.Map[xTo, yTo] == Pixel.Route)
                    thatsCool = true;
            }
            if (Who.Identifier == 1)
                Brief.Map[xTo, yTo] = Pixel.PlayerTwo;
            else
                Brief.Map[xTo, yTo] = Pixel.PlayerOne;
            Who.X = xTo;
            Who.Y = yTo;
        }

        public static void TokenSpawn()
        {
            int roll;
            Random random = new Random();
            roll = random.Next(100);
            if (roll < Settings.TokenSpawnChance)
            {
                bool thatsCool = false;
                int xTo = -1;
                int yTo = -1;
                while (!thatsCool)
                {
                    xTo = random.Next(1, Settings.xMax - 2);
                    yTo = random.Next(1, Settings.yMax - 2);
                    if (Brief.Map[xTo, yTo] == Pixel.Route)
                        thatsCool = true;
                }
                Brief.Map[xTo, yTo] = Pixel.Token;
            }
        }

        public static bool SignFormatCheck(Player Setter)
        {
            bool Okay = true;
            if (Setter.Sign.Length == 2)
            {
                for (int i = 0; i < 2; ++i)
                {
                    switch (Setter.Sign[i])
                    {
                        case ' ':
                            Okay = false;
                            break;
                        case '|':
                            Okay = false;
                            break;
                        case '$':
                            Okay = false;
                            break;
                        case '.':
                            Okay = false;
                            break;
                        case ',':
                            Okay = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
                Okay = false;
            return Okay;
        }
    }
}
