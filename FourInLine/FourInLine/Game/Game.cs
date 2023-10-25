using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.Game
{
    internal class Game
    {
        Board board;
        Token turn;
        bool  end;

        public Game()
        {
            board = new Board();
            turn  = (Token)1;
            end   = false;

            GameLoop();
        }

        public void StartNewGame()
        {
            board = new Board();
            turn = (Token)1;
            end = false;

            GameLoop();
        }

        /// <summary>
        /// Base game loop for Four in line game
        /// </summary>
        public void GameLoop()
        {
            do
            {
                Console.Clear();
                board.PrintBoard();

                switch (turn)
                {
                    case Token.x:
                        end = PlayerTurn();
                        break;

                    case Token.o:
                        end = PlayerTurn();
                        break;
                }

                Console.Clear();

                // CHANGE PLAYER
                if (!end)
                {
                    NextTokenTurn();

                }
                else
                {
                    Console.WriteLine($"Player {turn} has won.");
                    board.PrintBoard();
                }
            } while (!end);
        }

        /// <summary>
        /// Call when you want a player take a decision.
        /// </summary>
        /// <returns>True if player wins</returns>
        bool PlayerTurn()
        {
            Console.Write($"Player {turn} turn. Insert token in a column: ");
            int playerCol;

            while (!int.TryParse(Console.ReadLine(), out playerCol))
            {
                Console.Write("ERROR - Insert token in a column: ");
            }

            var pos = board.InsertToken(turn, playerCol);
            return board.AnalyzeVictory(pos.row, pos.col);
        }

        /// <summary>
        /// Called when you want an AI take a decision.
        /// </summary>
        /// <returns>True if AI wins</returns>
        bool AiTurn()
        {
            //IA CLASS
            return false;
        }

        /// <summary>
        /// Change the turn to the next Token.
        /// </summary>
        void NextTokenTurn()
        {
            int valuesCount = Enum.GetNames(typeof(Token)).Length;
            int nextToken = (int)turn + 1;

            if(nextToken >= valuesCount) nextToken = 1;

            turn = (Token)nextToken;
        }
    }
}
