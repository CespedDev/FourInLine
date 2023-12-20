using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using FourInLine.AI;

namespace FourInLine.Game
{
    internal class Game
    {
        Board board;
        bool  end;

        IAI? ai1;
        IAI? ai2;

        public Game()
        {
            board = new Board();
            end   = false;

            GameLoop();
        }

        public Game(IAI ai)
        {
            board=new Board();
            end = false;
            this.ai1 = ai;

            GameLoop();
        }

        public Game(IAI ai1, IAI ai2)
        {
            board = new Board();
            end = false;
            this.ai1 = ai1;
            this.ai2= ai2;
            GameLoop();
        }

        public void StartNewGame()
        {
            board = new Board();
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

                switch (board.turn)
                {
                    case Token.o:
                        if (ai2 == null)
                            end = PlayerTurn();
                        else
                            end = AiTurn(ai2);

                        break;

                    case Token.x:
                        if (ai1 == null)
                            end = PlayerTurn();
                        else
                            end = AiTurn(ai1);
                        
                        break;
                }

                Console.Clear();

                // CHANGE PLAYER
                if (board.IsGameOver())
                {
                    Console.WriteLine($"There is no more space.");
                    board.PrintBoard();
                    end = true;
                }
                else if (!end)
                {
                    board.NextTokenTurn();
                }

                else 
                {
                    Console.WriteLine($"Player {board.turn} has won.");
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
            Console.Write($"Player {board.turn} turn. Insert token in a column: ");
            int playerCol;

            while (!int.TryParse(Console.ReadLine(), out playerCol))
            {
                Console.Write("ERROR - Insert token in a column: ");
            }

            var pos = board.InsertToken(board.turn, playerCol);
            return board.AnalyzeVictory(pos.row, pos.col);
        }

        /// <summary>
        /// Called when you want an AI take a decision.
        /// </summary>
        /// <returns>True if AI wins</returns>
        bool AiTurn(IAI ai)
        {
            //IA CLASS

            //CHANGE VALUE 0
            var pos = board.InsertToken(board.turn, ai.MakeDecision(board));
            return board.AnalyzeVictory(pos.row, pos.col);
        }
        /*
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
        */
    }
}
