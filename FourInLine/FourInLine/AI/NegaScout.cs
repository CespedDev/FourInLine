using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.AI
{
    public class NegaScout : IAI
    {
        int depth = 3;

        public int MakeDecision(Board board)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int bestColumn = -1; // Initialize with an invalid column

            // Initialize bestScore to a very low value
            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);

                // Recurse.
                int recursedScore = -NegaScoutAB(new Board(newBoard), depth, -beta, -alpha);
                int currentScore = -recursedScore;

                // Update the best score and column.
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColumn = col;
                }

                // Update the alpha value.
                alpha = Math.Max(alpha, bestScore);

                // Check for pruning.
                if (alpha >= beta)
                    break;
            }

            return bestColumn;
        }

        private int NegaScoutAB(Board board, int maxDepth, int alpha, int beta, int currentDepth = 0)
        {
            // Check if we're done recursing.
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                return board.Evaluate(); // Assuming Evaluate returns an integer.
            }

            // Otherwise bubble up values from below.
            int bestScore = int.MinValue;

            // Keep track of the Test window value
            int adaptiveBeta = beta;

            // Go through each move.
            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);

                // Recurse.
                int recursedScore = -NegaScoutAB(new Board(newBoard), maxDepth, -adaptiveBeta, -Math.Max(alpha, bestScore), currentDepth + 1);
                int currentScore = -recursedScore;

                // Update the best score.
                if (currentScore > bestScore)
                {
                    // If we are in 'narrow-mode' then widen and do a regular
                    // AB negamax search.
                    if (adaptiveBeta == beta || currentDepth >= maxDepth - 2)
                    {
                        bestScore = currentScore;
                    }
                    // Otherwise, we can do a Test.
                    else
                    {
                        int negativeBestScore = -NegaScoutAB(new Board(newBoard), maxDepth, -beta, -currentScore, currentDepth);
                        bestScore = -negativeBestScore;
                    }

                    // If we're outside the bounds, prune by exiting.
                    if (bestScore >= beta)
                    {
                        return bestScore;
                    }

                    // Otherwise update the window location.
                    adaptiveBeta = Math.Max(alpha, bestScore) + 1;
                }
            }

            return bestScore;
        }
    }

}
