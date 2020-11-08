using System;

namespace Varga_Gergely_Pacman
{
    class Moves
    {
        public static void TheGame(Player PlayerOne, Player PlayerTwo, Player Winner)
        {
            while (Brief.Alive)
            {
                Brief.Draw(PlayerOne, PlayerTwo);
                Move(PlayerOne, PlayerTwo);
                Accessories.TokenSpawn();
                if (PlayerOne.HP < 1)
                {
                    Brief.Alive = false;
                    Winner = PlayerTwo;
                }
                if (PlayerTwo.HP < 1)
                {
                    Brief.Alive = false;
                    Winner = PlayerOne;
                }
            }
            Brief.EndingScreen(Winner);
        }

        public static void Move(Player PlayerOne, Player PlayerTwo)
        {
            if (!PlayerOne.TurnEnded)
                SinglePlayerKeyHandler(PlayerOne, PlayerTwo);
            else if (Settings.GameMode == Settings.SinglePlayer)
                Step(Bot.NextStep(PlayerOne, PlayerTwo), PlayerTwo, PlayerOne);
            else
                TwoPlayerKeyHandler(PlayerOne, PlayerTwo);
        }

        static void Step(int Way, Player Who, Player OtherPlayer)
        {
            int[] step = new int[2] { Brief.Ways[Way, 0], Brief.Ways[Way, 1] };
            switch (Brief.Map[Who.X + step[0], Who.Y + step[1]])
            {
                case Pixel.Route:
                    Brief.Map[Who.X, Who.Y] = Pixel.Route;
                    Who.X += step[0];
                    Who.Y += step[1];
                    if (Who.Identifier == 1)
                        Brief.Map[Who.X, Who.Y] = Pixel.PlayerTwo;
                    else
                        Brief.Map[Who.X, Who.Y] = Pixel.PlayerOne;
                    break;
                case Pixel.Token:
                    Brief.Map[Who.X, Who.Y] = Pixel.Route;
                    Who.X += step[0];
                    Who.Y += step[1];
                    Who.HP += 1;
                    if (Who.Identifier == 1)
                        Brief.Map[Who.X, Who.Y] = Pixel.PlayerTwo;
                    else
                        Brief.Map[Who.X, Who.Y] = Pixel.PlayerOne;
                    break;
                case Pixel.PlayerOne:
                    PlayersCollide(Who, OtherPlayer);
                    break;
                case Pixel.PlayerTwo:
                    PlayersCollide(Who, OtherPlayer);
                    break;
                case Pixel.Wall:
                default:
                    break;
            }
            OthersTurn(Who, OtherPlayer);
        }

        private static void PlayersCollide(Player Stepper, Player Punched)
        {
            if (Stepper.HP > Punched.HP)
            {
                Brief.Map[Stepper.X, Stepper.Y] = Pixel.Route;
                Stepper.X = Punched.X;
                Stepper.Y = Punched.Y;
                Punched.HP -= 1;
                if (Settings.InstantDeath)
                    Punched.HP = 0;
                if (Stepper.Identifier == 0)
                    Brief.Map[Stepper.X, Stepper.Y] = Pixel.PlayerOne;
                else
                    Brief.Map[Stepper.X, Stepper.Y] = Pixel.PlayerTwo;
                Accessories.Respawn(Punched);
            }
            else if (Stepper.HP < Punched.HP)
            {
                Brief.Map[Stepper.X, Stepper.Y] = Pixel.Route;
                if (Settings.InstantDeath)
                    Stepper.HP = 0;
                else
                    Stepper.HP -= 1;
                Accessories.Respawn(Stepper);
            }
            //Ki kéne valamit okoskodni, hogy mi legyen ha egyforma a HP-juk :D
        }

        static void SinglePlayerKeyHandler(Player PlayerOne, Player PlayerTwo)
        {
            bool Understood = false;
            while (!Understood)
            {
                var input = Console.ReadKey();
                char pressedButton = input.KeyChar;
                pressedButton = char.ToUpper(pressedButton);
                switch (pressedButton)
                {
                    case 'W':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.North, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'A':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.West, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'S':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.South, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'D':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.East, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'Q':
                        Brief.Alive = false;
                        Understood = true;
                        break;
                    default:
                        break;
                }
            }
        }

        static void TwoPlayerKeyHandler(Player PlayerOne, Player PlayerTwo)
        {
            bool Understood = false;
            while (!Understood)
            {
                var input = Console.ReadKey();
                char pressedButton = input.KeyChar;
                pressedButton = char.ToUpper(pressedButton);
                switch (pressedButton)
                {
                    case 'W':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.North, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'A':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.West, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'S':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.South, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'D':
                        if (!PlayerOne.TurnEnded)
                        {
                            Step(Brief.East, PlayerOne, PlayerTwo);
                            Understood = true;
                        }
                        break;
                    case 'I':
                        if (!PlayerTwo.TurnEnded)
                        {
                            Step(Brief.North, PlayerTwo, PlayerOne);
                            Understood = true;
                        }
                        break;
                    case 'J':
                        if (!PlayerTwo.TurnEnded)
                        {
                            Step(Brief.West, PlayerTwo, PlayerOne);
                            Understood = true;
                        }
                        break;
                    case 'K':
                        if (!PlayerTwo.TurnEnded)
                        {
                            Step(Brief.South, PlayerTwo, PlayerOne);
                            Understood = true;
                        }
                        break;
                    case 'L':
                        if (!PlayerTwo.TurnEnded)
                        {
                            Step(Brief.East, PlayerTwo, PlayerOne);
                            Understood = true;
                        }
                        break;
                    case 'Q':
                        Brief.Alive = false;
                        Understood = true;
                        break;
                    default:
                        break;
                }
            }
        }

        static void OthersTurn(Player Stepped, Player Next)
        {
            Stepped.TurnEnded = true;
            Next.TurnEnded = false;
        }
    }
}
