using System;
using System.Collections.Generic;
using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.AI
{
    public class MTDf : IAI
    {
        private int depth = 3;
        private NegamaxAB negamaxAB;

        public MTDf(int depth)
        {
            this.depth = depth;
            this.negamaxAB = new NegamaxAB();
        }

        public int MakeDecision(Board board)
        {
            int firstGuess = 0;
            int upperBound = int.MaxValue;
            int lowerBound = int.MinValue;
            int bestMove = -1;

            while (lowerBound < upperBound)
            {
                int beta;
                if (firstGuess == lowerBound)
                    beta = firstGuess + 1;
                else
                    beta = firstGuess;

                Board newBoard = new Board(board);
                firstGuess = negamaxAB.NegamaxABInternal(newBoard, depth, beta - 1, beta);

                if (firstGuess < beta)
                    upperBound = firstGuess;
                else
                    lowerBound = firstGuess;

                bestMove = negamaxAB.MakeDecision(board);
            }

            return bestMove;
        }
    }
}












