using System.Collections.Generic;

namespace Varga_Gergely_Pacman
{
    class Bot
    {
        const int WasThere = 0;
        const int On = 1;
        const int Free = 2;
        const int Target = 3;
        static int TheWay;
        static List<int[]> OnIt = new List<int[]>();
        static List<int[]> Neighbour = new List<int[]>();
        static int[,] BotMap = new int[Settings.xMax, Settings.yMax];

        static void BotMapSet(Player Player, Player Bot)
        {
            for (int j = 0; j < Settings.yMax; ++j)
            {
                for (int i = 0; i < Settings.xMax; ++i)
                {
                    switch (Brief.Map[i, j])
                    {
                        case Pixel.Route:
                            BotMap[i, j] = Free;
                            break;
                        case Pixel.Wall:
                            BotMap[i, j] = WasThere;
                            break;
                        case Pixel.Token:
                            if (Bot.HP > Player.HP)
                                BotMap[i, j] = Free;
                            else
                                BotMap[i, j] = Target;
                            break;
                        case Pixel.PlayerOne:
                            if (Bot.HP > Player.HP)
                                BotMap[i, j] = Target;
                            else
                                BotMap[i, j] = WasThere;
                            break;
                        case Pixel.PlayerTwo:
                            BotMap[i, j] = Free;
                            break;
                        default:
                            BotMap[i, j] = WasThere;
                            break;
                    }
                }
            }
        }

        public static int NextStep(Player Player, Player Bot)
        {
            bool Gotcha;
            bool NoMore = false;
            OnIt.Clear();
            Neighbour.Clear();
            BotMapSet(Player, Bot);
            Gotcha = FirstLookAround(Bot);
            while (!NoMore && !Gotcha)
            {
                NoMore = ListReset();
                Gotcha = NeighbourGetter();
                if (Gotcha)
                    NoMore = false;
            }
            if (NoMore)
            {
                TheWay = Flee(Player, Bot);
            }
            return TheWay;
        }

        

        static bool NeighbourGetter()
        {
            bool Gotcha = false;
            for (int i = 0; i < OnIt.Count; ++i)
            {
                Gotcha = LookAround(OnIt[i]);
                if (Gotcha)
                    break;
            }
            return Gotcha;
        }

        static bool ListReset()
        {
            bool NoMore = false;
            OnIt.Clear();
            if (Neighbour.Count == 0)
                NoMore = true;
            else
            {
                for (int i = 0; i < Neighbour.Count; ++i)
                {
                    OnIt.Add(Neighbour[i]);
                    var current = OnIt[i];
                    BotMap[current[0], current[1]] = On;
                }
                Neighbour.Clear();
            }
            return NoMore;
        }

        static bool FirstLookAround(Player Bot)
        {
            bool FoundIt = false;
            for (int Way = 1; Way <= 4; ++Way)
            {
                switch (BotMap[Bot.X + Brief.Ways[Way, 0], Bot.Y + Brief.Ways[Way, 1]])
                {
                    case Free:
                        int[] Adder = new int[3] { Bot.X + Brief.Ways[Way, 0], Bot.Y + Brief.Ways[Way, 1], Way };
                        Neighbour.Add(Adder);
                        break;
                    case Target:
                        FoundIt = true;
                        TheWay = Way;
                        break;
                    default:
                        break;
                }
            }
            BotMap[Bot.X, Bot.Y] = WasThere;
            return FoundIt;
        }

        static bool LookAround(int[] Input)
        {
            bool FoundIt = false;
            int X = Input[0];
            int Y = Input[1];
            int Origin = Input[2];
            for (int Way = 1; Way <= 4; ++Way)
            {
                switch (BotMap[X + Brief.Ways[Way, 0], Y + Brief.Ways[Way, 1]])
                {
                    case Free:
                        int[] Adder = new int[] { X + Brief.Ways[Way, 0], Y + Brief.Ways[Way, 1], Origin };
                        Neighbour.Add(Adder);
                        break;
                    case Target:
                        FoundIt = true;
                        TheWay = Origin;
                        break;
                    default:
                        break;
                }
                if (FoundIt)
                    break;
            }
            BotMap[X, Y] = WasThere;
            return FoundIt;
        }

        static int Flee(Player Player, Player Bot)
        {
            int Furthest = 0;
            int BiggestDistance = 0;
            for (int Way = 0; Way < 5; ++Way)
            {
                int CurrentDistance = 0;
                bool Gotcha;
                bool NoMoreRunning = false;
                OnIt.Clear();
                Neighbour.Clear();
                Player BotCopy = new Player
                {
                    X = Bot.X + Brief.Ways[Way, 0],
                    Y = Bot.Y + Brief.Ways[Way, 1],
                    HP = Player.HP + 1,
                };
                if (Brief.Map[BotCopy.X, BotCopy.Y] == Pixel.Wall)
                    continue;
                BotMapSet(Player, BotCopy);
                Gotcha = FirstLookAround(BotCopy);
                if (Gotcha)
                    continue;
                while (!Gotcha && !NoMoreRunning)
                {
                    ++CurrentDistance;
                    NoMoreRunning = ListReset();
                    Gotcha = NeighbourGetter();
                }
                if (BiggestDistance < CurrentDistance)
                {
                    BiggestDistance = CurrentDistance;
                    Furthest = Way;
                }
            }
            return Furthest;
        }
    }
}