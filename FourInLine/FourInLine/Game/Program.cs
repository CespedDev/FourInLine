using FourInLine.AI;

namespace FourInLine.Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Game game = new Game(new NegaScout());

            Game game = new Game(new NegamaxAB());
        }
    }
}