using System;
using System.Collections.Generic;
using FourInLine.Game;

namespace FourInLine.AI
{
    public class SimpleNegamax : IAI
    {
        private int depth = 3;
        int nodeNums = 0;
        public int MakeDecision(Board board)
        {
            int bestColumn = -1;
            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                // Crear un nuevo tablero con la columna actual
                Board newBoard = new Board(board, col);

                // Llamar al método interno SimpleNegamax
                int recursedScore = SimpleNegamaxInternal(newBoard, depth);
                int currentScore = -recursedScore;

                nodeNums++;

                // Actualizar la mejor puntuación y la mejor columna
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColumn = col;
                }
            }

            return bestColumn;
        }

        public int SimpleNegamaxInternal(Board board, int maxDepth, int currentDepth = 2)
        {
            // Comprobar si ha terminado la función recursiva
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                return board.Evaluate();
            }

            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);

                int recursedScore = SimpleNegamaxInternal(newBoard, maxDepth, currentDepth + 1);
                int currentScore = -recursedScore;

                nodeNums++ ;

                // Actualizar el mejor puntaje.
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                }
            }

            return bestScore;
        }
    }
}

