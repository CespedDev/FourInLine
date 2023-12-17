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

    public class Board
    {
        private Token[,] board;
        public Token turn { get; private set; }

        public int rows { get; private set; }
        public int cols { get; private set; }

        public Board(int rows = 6, int cols = 7)
        {
            this.rows = rows;
            this.cols = cols;

            board = new Token[rows,cols];
            CleanBoard();

            turn = (Token)1;
        }

        public Board(Board board) 
        {
            this.board = (Token[,]) board.board.Clone();
            this.rows  = board.rows;
            this.cols  = board.cols;
            this.turn  = board.turn;
        }

        public Board(Board board, int colInsert)
        {
            this.board = (Token[,])board.board.Clone();
            this.rows  = board.rows;
            this.cols  = board.cols;
            this.turn  = board.turn;

            InsertToken(turn, colInsert);
            NextTokenTurn();
        }

        /*
        /// <summary>
        /// Get board position value by ref.
        /// </summary>
        public ref Token GetPos(int row, int col)
        {
            return ref board[row, col];
        }
        */

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

        public void NextTokenTurn()
        {
            int valuesCount = Enum.GetNames(typeof(Token)).Length;
            int nextToken = (int)turn + 1;

            if (nextToken >= valuesCount) nextToken = 1;

            turn = (Token)nextToken;
        }

        #region Evaluate functions
        public int Evaluate()
        {
            int score = 0;

            //Evaluate each direction (horizontal, vertical, diagonal)
            score += EvaluateDirection(1, 0);   // Horizontal
            score += EvaluateDirection(0, 1);   // Vertical
            score += EvaluateDirection(1, 1);   // Diagonal  \
            score += EvaluateDirection(1, -1);  // Diagonal  /
            return score;
        }

        private int EvaluateDirection(int rowIncrement, int colIncrement) 
        {
            int score = 0;
            for(int row = 0; row < rows; row ++)
            {
                for(int col = 0; col < cols; col++)
                {
                    Token token = GetPosValue(row, col);

                    if(token != Token.Empty)
                    {
                        //Check for a sequence of four tokens in the current direction
                        int count = 0;
                        int r = row;
                        int c = col;

                        for(int i= 0; i < 4; i++)
                        {
                            if(r>=0 && r<rows && c>=0&& c < cols && GetPosValue(r,c) == token)
                            {
                                count++;
                                r += rowIncrement;
                                c += colIncrement;

                            }
                        }

                        //Assign scores based on the count of tokens in a row
                        if (count == 4)
                            score += 10000; //Winning move
                        else if (count == 3)
                            score += 100;   //Potencial winnig move
                        else if (count == 2)
                            score += 10;    //Open row with two tokens
                        else if (count == 1)
                            score += 1;     //Open row with one token
                    }
                }
            }

            return score;
        }

        public bool IsGameOver()
        {
            for (int row = rows - 1; row >= 0; --row)
            {
                for(int col = cols - 1; col >= 0; --col)
                {
                    if (board[row, col] == Token.Empty) return false;
                }

            }
            return true;
        }

        public Queue<int> PosiblesInsert()
        {
            Queue<int> colInserts = new Queue<int> ();
            for(int col = 0; col < cols; ++col)
            {
                if(GetPosValue(rows-1,col) == Token.Empty)
                    colInserts.Enqueue(col);
            }
            return colInserts;

        }

        #endregion
    }
}
