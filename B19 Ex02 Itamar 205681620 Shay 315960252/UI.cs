using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class UI
    {
        public const int k_BigBoard = 8;
        public const int k_SmallBoard = 6;
        public const int k_BigBoardSelection = 2;
        public const int k_SmallBoardSelection = 1;

        public static void SetUser(ref Player io_User)
        {
            Console.WriteLine("please enter your name");
            string playerName = Console.ReadLine();
            io_User = new Player(playerName);
        }

        public static int SetGameMode(ref int io_GameMode)
        {
            bool isInputLegal = false;
            string msg = string.Format(
@"Choose game mode:
1-against computer      2-two players");
            Console.WriteLine(msg);

            while (!isInputLegal)
            {
                isInputLegal = int.TryParse(Console.ReadLine(), out io_GameMode);
                if (!isInputLegal || (io_GameMode != 1 && io_GameMode != 2))
                {
                    Console.WriteLine("That was invalid. Enter 1 or 2");
                    isInputLegal = false;
                }
            }

            return io_GameMode;
        }

        public static void SetBoardSize(ref int io_BoardSize)
        {
            int input = 0;
            bool isInputLegal = false;
            string msg = string.Format(
@"Choose game size:
1-6x6      2-8x8");
            Console.WriteLine(msg);

            while (!isInputLegal)
            {
                isInputLegal = int.TryParse(Console.ReadLine(), out input);
                if (!isInputLegal || (input != 1 && input != 2))
                {
                    Console.WriteLine("That was invalid. Enter 1 or 2");
                    isInputLegal = false;
                }
            }

            if (input == k_SmallBoardSelection)
            {
                io_BoardSize = k_SmallBoard;
            }
            else
            {
                io_BoardSize = k_BigBoard;
            }
        }

        public static void GetMoveFromUser(ref int io_Row, ref int io_Col, Board i_Board)
        {
            Console.WriteLine("please enter your next move");
            bool valid = false;
            while (!valid)
            {
                string input = Console.ReadLine();
                if (input[0] == 'Q')
                {
                    io_Row = -1;
                    io_Col = -1;
                    return;
                }

                if (input.Length == 2)
                {
                    io_Col = input[0] - 'A';
                    io_Row = input[1] - '0' - 1;
                    if (io_Row >= 0 && io_Row <= i_Board.Size && io_Col >= 0 && io_Col <= i_Board.Size)
                    {
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("invalid move,please try again");
                    }
                }
                else
                {
                    Console.WriteLine("invalid move,please try again");
                }
            }
        }

        public static void SelectSquareAgain()
        {
            Console.WriteLine("Can not be placed in the square you selected");
        }

        public static void TurnPassed()
        {
            Console.WriteLine("There are no legal moves possible, the turn has moved to the second player");
        }

        public static void PrintBoard(Board i_BoardGame)
        {
            if (i_BoardGame.Size == k_BigBoard)
            {
                string msg = string.Format(

   @"      A   B   C   D   E   F   G   H
    =================================");
                Console.WriteLine(msg);
            }
            else
            {
                string msg = string.Format(

 @"      A   B   C   D   E   F     
    =========================");
                Console.WriteLine(msg);
            }

            for (int i = 0; i < i_BoardGame.Size; i++)
            {
                Console.Write("{0}   |", i + 1);
                for (int j = 0; j < i_BoardGame.Size; j++)
                {
                    Console.Write(" {0} |", i_BoardGame.GetCellValue(i, j));
                }

                if (i_BoardGame.Size == k_BigBoard)
                {
                    Console.WriteLine("\n    =================================");
                }
                else
                {
                    Console.WriteLine("\n    =========================");
                }
            }
        }

        public static void PrintWinner(Player i_Winner)
        {
            if (i_Winner == null)
            {
                Console.WriteLine("Draw");
            }
            else
            {
                Console.WriteLine("the winner is {0}", i_Winner.Name);
            }
        }

        public static void PrintResult(int i_PointsOfPlayerOne, int i_PointsOfPlayerTwo, Player i_User1, Player i_User2)
        {
            Console.WriteLine("{0}:{1}  ('{2}')", i_User1.Name, i_PointsOfPlayerOne, i_User1.Symbol);
            Console.WriteLine("{0}:{1}  ('{2}')", i_User2.Name, i_PointsOfPlayerTwo, i_User2.Symbol);
        }

        public static char AnotherGame()
        {
            Console.WriteLine("replay? press Y for yes or N for no");
            char.TryParse(Console.ReadLine(), out char decision);
            return decision;
        }

        public static void ShowWhoIsTurn(Player i_User)
        {
            Console.WriteLine("{0}'s turn", i_User.Name);
        }

        public static void QuitGame()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}