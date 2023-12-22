using FourInLine.AI;

namespace FourInLine.Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game;
            bool fullAI = false;

            Console.WriteLine("1) AI vs Player  2) AI vs AI");
            switch (Console.ReadLine())
            {
                case "1":
                    fullAI = false;
                    break;

                case "2":
                    fullAI = true;
                    break;
            }

            Console.WriteLine("1) Negamax  2) NegamaxAB  3) NegaScout  4) AspirationSearch");
            switch (Console.ReadLine())
            {
                case "1":
                    if (fullAI)
                    {
                        game = new Game(new SimpleNegamax(), new SimpleNegamax());
                    }
                    else
                    {
                        game = new Game(new SimpleNegamax());
                    }
                    break;

                case "2":
                    if (fullAI)
                    {
                        game = new Game(new NegamaxAB(), new NegamaxAB());
                    }
                    else
                    {
                        game = new Game(new NegamaxAB());
                    }
                    break;

                case "3":
                    if (fullAI)
                    {
                        game = new Game(new NegaScout(), new NegaScout());
                    }
                    else
                    {
                        game = new Game(new NegaScout());
                    }
                    break;

                case "4":
                    if (fullAI)
                    {
                        game = new Game(new AspirationSearch(), new AspirationSearch());
                    }
                    else
                    {
                        game = new Game(new AspirationSearch());
                    }
                    break;
            }
        }
    }
}