using System;
using System.Collections.Generic;
using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FourInLine.AI
{
    public class NegamaxAB : IAI
    {
        private int depth = 3;
        int nodeNums = 0;

        public int MakeDecision(Board board)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int bestColumn = -1; // Inicializar con una columna no válida

            int bestScore = int.MinValue; // Inicializar con un valor muy bajo

            foreach (int col in board.PosiblesInserts())
            {
                // Crear un nuevo tablero con la columna actual
                Board newBoard = new Board(board, col);

                // Llamar al método interno NegamaxAB
                int recursedScore = NegamaxABInternal(newBoard, depth, -beta, -alpha);
                int currentScore = -recursedScore;

                Debug.WriteLine($"NODO profundidad: {1} score: {currentScore}");
                nodeNums++;

                // Actualizar la mejor puntuacion y la mejor columna
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColumn = col;
                }

                // Actualizar el valor de alfa
                alpha = Math.Max(alpha, bestScore);

                // Comprobar si se debe podar
                if (alpha >= beta)
                {
                    break;
                }
                    
            }

            Debug.WriteLine($"NODO profundidad: {0} score: {-bestScore}");
            return bestColumn;
        }

        public int NegamaxABInternal(Board board, int maxDepth, int alpha, int beta, int currentDepth = 2)
        {
            // Comprobar si ha terminado la funcion recursiva
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                int score = -board.Evaluate();
                Debug.WriteLine($"NODO profundidad: {currentDepth} score: {score}");
                return score;
            }

            int bestScore = int.MinValue;

            foreach (int col in board.PosiblesInserts())
            {
                Board newBoard = new Board(board, col);

                int recursedScore = NegamaxABInternal(newBoard, maxDepth, -beta, -alpha, currentDepth + 1);
                int currentScore = -recursedScore;
                nodeNums++;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                }

                // Actualizar el valor de alfa y beta.
                alpha = Math.Max(alpha, bestScore);
                beta = Math.Min(beta, bestScore);

                // Comprobar si se debe podar.
                if (beta >= alpha)
                {
                    break;
                }
            }

            Debug.WriteLine($"NODO profundidad: {currentDepth} score: {bestScore}");
            return bestScore;
        }
    }
}

