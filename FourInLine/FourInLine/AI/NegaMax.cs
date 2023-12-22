using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.AI
{
    public class NegaMax : IAI
    {
        int depth = 3;

        public int MakeDecision(Board board)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int bestColum = -1; //Initialize with an invalid colum

            //Initialize bestScore to a very low value
            int bestScore = int.MinValue;

            foreach(int col in board.PosiblesInsert())
            {
                Board newBoard = new Board(board, col);

                //Recurse.
                int recursedScore = -NegaMax_(new Board(newBoard), depth, -beta, -alpha);
                int currentScore = -recursedScore;

                //Update the best score and colum
                if(currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColum = col;
                }

                //Update the alpha value
                alpha = Math.Max(alpha, bestScore);

                //Check for pruning
                if (alpha >= beta)
                    break;

            }
            return bestColum;
        }

        private int NegaMax_(Board board, int maxDepht, int alpha, int beta, int currentDepth = 0)
        {
            //Check if we're done recursing
            if (board.IsGameOver() || currentDepth == maxDepht) {
                if(currentDepth%2==0)
                    return board.Evaluate(); 
                else
                    return -board.Evaluate();
            }
            else 
            { 
                //Otherwise bubble up values from below
                int bestScore = int.MinValue;

                //Keep track of the Test window value
                int adaptiveBeta = beta;

                //Go through each move
                foreach(int col in board.PosiblesInsert())
                {
                    Board newBoard = new Board(board, col);
                    //Recurse
                    int recursedScore = -NegaMax_(new Board(newBoard), maxDepht, -adaptiveBeta, -Math.Max(alpha, bestScore), currentDepth + 1);
                    int currentScore = -recursedScore;
            
                    //Update the best score
                    if(currentScore>bestScore)
                    {
                        //if we are in 'narrow-mode' then widen and do a regular
                        //AB negamax search
                        if(adaptiveBeta==beta||currentDepth>=maxDepht-2)
                        {
                            bestScore = currentScore;
                        }
                        //Otherwise, we can do a Test
                        else
                        {
                            int negativeBestScore = -NegaMax_(new Board(newBoard), maxDepht, -beta, -currentScore, currentDepth);
                            bestScore = -negativeBestScore;
                        }

                        //if we're outside the bound, prune by exiting
                        //if(bestScore >=beta) {return bestScore;}

                        //Otherwise update the window location
                        //adaptiveBeta = Math.Max(alpha, bestScore) + 1;
                    }
                }
                return bestScore;
            }
            
        }


    }
}
