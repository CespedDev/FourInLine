using FourInLine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.AI
{
    public class NegamaxAB : IAI
    {
        private int depth = 3;

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

                // Llamar al método interno NegamaxAB para la recursión
                int recursedScore = -NegamaxABInternal(newBoard, depth, -beta, -alpha);
                int currentScore = -recursedScore;

                // Actualizar el mejor puntaje y la mejor columna
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestColumn = col;
                }

                // Actualizar el valor de alfa
                alpha = Math.Max(alpha, bestScore);

                // Comprobar si se debe podar
                if (alpha >= beta)
                    break;
            }

            return bestColumn;
        }

        private int NegamaxABInternal(Board board, int maxDepth, int alpha, int beta, int currentDepth = 0)
        {
            // Comprobar si hemos terminado la recursión.
            if (board.IsGameOver() || currentDepth == maxDepth)
            {
                return board.Evaluate(); // Suponiendo que Evaluate devuelve un entero.
            }

            int bestScore = int.MinValue; // Inicializar con un valor muy bajo

            foreach (int col in board.PosiblesInserts())
            {
                // Crear un nuevo tablero con la columna actual
                Board newBoard = new Board(board, col);

                // Recursión
                int recursedScore = -NegamaxABInternal(newBoard, maxDepth, -beta, -alpha, currentDepth + 1);
                int currentScore = -recursedScore;

                // Actualizar el mejor puntaje.
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                }

                // Actualizar el valor de alfa.
                alpha = Math.Max(alpha, bestScore);

                // Comprobar si se debe podar.
                if (alpha >= beta)
                    break;
            }

            return bestScore;
        }
    }

}
