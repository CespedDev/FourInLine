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
    public class AspirationSearch : IAI
    {
        private int depth = 3;
        private int aspirationWindow = 50;
        private int aspirationMin = int.MinValue;
        private int aspirationMax = int.MaxValue;

        public int MakeDecision(Board board)
        {
            int alpha = aspirationMin;
            int beta = aspirationMax;
            int bestColumn = -1;
            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);
                int recursedScore = -NegamaxABInternal(newBoard, depth, -beta, -alpha);
                int currentScore = -recursedScore;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColumn = col;
                }

                alpha = Math.Max(alpha, bestScore);

                if (alpha >= beta)
                {
                    AdjustAspirationWindow(bestScore);
                    break;
                }
            }

            return bestColumn;
        }

        private void AdjustAspirationWindow(int bestScore)
        {
            aspirationMin = Math.Max(bestScore - aspirationWindow, int.MinValue);
            aspirationMax = Math.Min(bestScore + aspirationWindow, int.MaxValue);
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
















