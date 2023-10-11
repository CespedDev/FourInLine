using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInLine.Game
{
    public enum Token
    {
        Empty   = 0,
        o       = 1,
        x       = 2,
    }

    internal class Board
    {
        private Token[,] board;

        public int rows { get; private set; }
        public int cols { get; private set; }

        public Board(int rows = 6, int cols = 7)
        {
            this.rows = rows;
            this.cols = cols;

            board = new Token[rows,cols];
            CleanBoard();
        }

        /// <summary>
        /// Get board position value by ref.
        /// </summary>
        public ref Token GetPos(int row, int col)
        {
            return ref board[row, col];
        }

        /// <summary>
        /// Get board position value.
        /// </summary>
        public Token GetPosValue(int row, int col)
        {
            return board[row, col];
        }

        /// <summary>
        /// Simulate the action of insert one token.
        /// </summary>
        public (int row, int col) InsertToken(Token token, int col)
        {
            // If attempted to insert an Empty token throw exception
            if (token == Token.Empty)
                throw new Exception("Empty token is not valid");

            // If column is full throw exception 
            if (board[rows - 1, col] != Token.Empty)
                throw new Exception("Column is complete");

            // Insert token on the first free place
            for (int _row = 0; _row < rows; ++_row)
            {
                if (board[_row, col] == Token.Empty)
                {
                    board[_row, col] = token;
                    return (_row, col); 
                }
            }

            throw new Exception("Token not inserted");
        }

        // VICTORY CONDITION


        /// <summary>
        /// Set al positions on empty.
        /// </summary>
        public void CleanBoard()
        {
            for(int _row = 0; _row < rows; ++_row)
            {
                for(int _col = 0; _col < cols; ++_col)
                {
                    board[_row, _col] = Token.Empty;
                }
            }
        }

        /// <summary>
        /// Print board state.
        /// </summary>
        public void PrintBoard()
        {
            for (int _row = rows - 1; _row >= 0; --_row)
            {
                Console.Write("|");

                for (int _col = 0; _col < cols; ++_col)
                {
                    if(board[_row, _col] == Token.Empty)
                        Console.Write(" |");
                    else
                        Console.Write(board[_row, _col].ToString() + "|");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
