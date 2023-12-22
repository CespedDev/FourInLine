﻿using System;
using System.Collections.Generic;
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

                // Llamar al método interno NegamaxAB
                int recursedScore = -NegamaxABInternal(newBoard, depth, -beta, -alpha);
                int currentScore = -recursedScore;

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
                    break;
            }

            return bestColumn;
        }

        public int NegamaxABInternal(Board board, int maxDepth, int alpha, int beta, int currentDepth = 0)
        {
            // Comprobar si ha terminado la funcion recursiva
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

                // Actualizar el valor de alfa y beta.
                alpha = Math.Max(alpha, bestScore);
                beta = Math.Min(beta, bestScore);

                // Comprobar si se debe podar.
                if (alpha >= beta)
                {
                    break;
                }
            }

            return bestScore;
        }
    }
}

