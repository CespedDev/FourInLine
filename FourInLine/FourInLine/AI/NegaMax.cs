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
        
            int bestColum = -1; //Initialize with an invalid colum

            //Initialize bestScore to a very low value
            int bestScore = int.MinValue;

            foreach(int col in board.PosiblesInsert())
            {
                Board newBoard = new Board(board, col);

                //Recurse.
                int recursedScore = NegaMax_(new Board(newBoard), depth);
                int currentScore = -recursedScore;

                //Update the best score and colum
                if(currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColum = col;
                }

            }
            return bestColum;
        }

        private int NegaMax_(Board board, int maxDepht, int currentDepth = 0)
        {
            //Check if we're done recursing
            if (board.IsGameOver() || currentDepth == maxDepht)
                return board.Evaluate(); //Assuming Evaluate return an interger.

            //Otherwise bubble up values from below
            int bestScore = int.MinValue;

            //Go through each move
            foreach(int col in board.PosiblesInsert())
            {
                Board newBoard = new Board(board, col);
                //Recurse
                int recursedScore = NegaMax_(new Board(newBoard), maxDepht, currentDepth + 1);
                int currentScore = -recursedScore;
            
                //Update the best score
                if(currentScore>bestScore)
                {   
                        bestScore = currentScore;                           
                }
            }
            return bestScore;
        }
    }
}
