using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace FourInLine.AI
{
    public class NegaScout : IAI
    {
        int depth = 3;
        int nodeNums = 0;

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
                int recursedScore = NegaScoutAB(new Board(newBoard), depth, -beta, -alpha);
                int currentScore = -recursedScore;

                Debug.WriteLine($"NODO profundidad: {1} score: {currentScore}");
                nodeNums++;
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

        private int NegaScoutAB(Board board, int maxDepth, int alpha, int beta, int currentDepth = 2)
        {
            // Check if we're done recursing.
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                int score = board.Evaluate();
                Debug.WriteLine($"NODO profundidad: {currentDepth} score: {score}");
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
                int recursedScore = NegamaxABInternal(new Board(newBoard), maxDepth, -adaptiveBeta, -Math.Max(alpha, bestScore), currentDepth + 1);
                int currentScore = -recursedScore;

                Debug.WriteLine($"NODO profundidad: {currentDepth} score: {currentScore}");
                nodeNums++;

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
                        int negativeBestScore = NegaScoutAB(new Board(newBoard), maxDepth, -beta, -currentScore, currentDepth);
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

        private int NegamaxABInternal(Board board, int maxDepth, int alpha, int beta, int currentDepth = 0)
        {
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                return board.Evaluate();
            }

            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);
                int recursedScore = -NegamaxABInternal(newBoard, maxDepth, -beta, -alpha, currentDepth + 1);
                int currentScore = -recursedScore;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                }

                alpha = Math.Max(alpha, bestScore);
                beta = Math.Min(beta, bestScore);  // Añade esta línea

                if (alpha >= beta)
                {
                    break;
                }
            }

            return bestScore;
        }

    }

}
