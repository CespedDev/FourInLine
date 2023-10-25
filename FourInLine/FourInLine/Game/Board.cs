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
        public bool AnalyzeVictory(int row, int col)
        {
            // Check if its empty
            Token token = GetPosValue(row, col);
            if (token == Token.Empty) return false;

            // Check horizontal
            int count = 0;
            for (int i = Math.Clamp(col - 3, 0, cols - 1); i < col + 4 && i < cols; ++i)
            {
                if (GetPosValue(row, i) == token)
                    ++count;
                else
                    count = 0;

                if (count >= 4) return true;
            }

            // Check vertical
            count = 0;
            for (int i = Math.Clamp(row - 3, 0, rows -1); i < row + 4 && i < rows; ++i)
            {
                if (GetPosValue(i, col) == token)
                    count++;
                else
                    count = 0;

                if (count >= 4) return true;
            }

            // Check diagonal left-down to right-up
            count = 0;
            for (int r = row - Math.Min(row, col), c = col - Math.Min(row, col); r < rows && c < cols; r++, c++)
            {
                if (GetPosValue(r, c) == token)
                    count++;
                else
                    count = 0;

                if (count >= 4) return true;
            }

            // Check diagonal left-up to right-down
            count = 0;
            for (int r = row + Math.Min(rows - row - 1, col), c = col - Math.Min(rows - row - 1, col); r >= 0 && c < cols; r--, c++)
            {
                if (GetPosValue(r, c) == token)
                    count++;
                else
                    count = 0;

                if (count >= 4) return true;
            }

            return false;
        }


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
                Console.Write($"{_row}|");

                for (int _col = 0; _col < cols; ++_col)
                {
                    if(board[_row, _col] == Token.Empty)
                        Console.Write(" |");
                    else
                        Console.Write(board[_row, _col].ToString() + "|");
                }

                Console.WriteLine();
            }

            Console.Write("  ");

            for (int _col = 0; _col < cols; ++_col)
            {
                Console.Write($"{_col} ");
            }

            Console.WriteLine();
        }
    }
}
