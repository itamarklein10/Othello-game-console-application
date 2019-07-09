using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class Board
    {
        private char[,] r_BoardGame;
        private int r_Size;
        public const char k_BlackSymbol = 'X';
        public const char k_WhiteSymbol = 'O';
        public const char k_EmptyCell = ' ';

        public Board(int size)
        {
            r_BoardGame = new char[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    r_BoardGame[i, j] = k_EmptyCell;
                }
            }

            r_Size = size;
            r_BoardGame[(size / 2) - 1, (size / 2) - 1] = r_BoardGame[size / 2, size / 2] = k_WhiteSymbol;
            r_BoardGame[size / 2, (size / 2) - 1] = r_BoardGame[(size / 2) - 1, size / 2] = k_BlackSymbol;
        }

        public int Size
        {
            get { return r_Size; }
        }

        public char GetCellValue(int i_Row, int i_Col)
        {
            return r_BoardGame[i_Row, i_Col];
        }

        public void SetCellValue(char i_Symbol, int i_Row, int i_Col)
        {
            r_BoardGame[i_Row, i_Col] = i_Symbol;
        }
    }
}
