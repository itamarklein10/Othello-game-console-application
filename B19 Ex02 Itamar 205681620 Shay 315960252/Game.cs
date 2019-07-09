using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Ex02_Othelo
{
    public class Game
    {
        private int r_GameMode;
        private int r_SizeBoard;
        public Board m_Board;
        public Player m_User1;
        public Player m_User2;
        public List<CellMatrix> m_User1PossibleMove = new List<CellMatrix>();
        public List<CellMatrix> m_User2PossibleMove = new List<CellMatrix>();
        public const int k_GameAgainstComputer = 1;
        public const int k_GameAgainstFriend = 2;
        public const int k_NoPossibleMoves = 0;
        public const int k_QuitProgram = -1;
        public const char k_BlackSymbol = 'X';
        public const char k_WhiteSymbol = 'O';
        public const char k_EmptyCell = ' ';
        public const char k_AnotherGame = 'Y';
        public const char k_StopPlaying = 'N';

        public Game()
        {
            UI.SetUser(ref m_User1);
            UI.SetGameMode(ref r_GameMode);
            UI.SetBoardSize(ref r_SizeBoard);
            SetBoard(r_SizeBoard, ref m_Board);
            SetGame(r_GameMode, ref m_User2);
        }

        private static void ProceedAccordingToDirection(ref int io_Row, ref int io_Col, eDirection i_Direction)
        {
            switch (i_Direction)
            {
                case eDirection.Down:
                    io_Row++;
                    break;
                case eDirection.Up:
                    io_Row--;
                    break;
                case eDirection.Left:
                    io_Col--;
                    break;
                case eDirection.Right:
                    io_Col++;
                    break;
                case eDirection.UpRight:
                    io_Col++;
                    io_Row--;
                    break;
                case eDirection.UpLeft:
                    io_Col--;
                    io_Row--;
                    break;
                case eDirection.DownRight:
                    io_Col++;
                    io_Row++;
                    break;
                case eDirection.DownLeft:
                    io_Col--;
                    io_Row++;
                    break;
                default:
                    break;
            }
        }

        private void SetBoard(int i_sizeBoard, ref Board io_Board)
        {
            io_Board = new Board(i_sizeBoard);
        }

        private void SetGame(int i_GameMode, ref Player io_User2)
        {
            m_User1.Symbol = k_BlackSymbol;

            if (i_GameMode == k_GameAgainstFriend)
            {
                UI.SetUser(ref io_User2);
                io_User2.Symbol = k_WhiteSymbol;
            }
            else if (i_GameMode == k_GameAgainstComputer)
            {
                io_User2 = new Player("computer");
                io_User2.Symbol = k_WhiteSymbol;
            }
        }

        public void StartGame()
        {
            char decision;
            do
            {
                GameRunning();
                PrintCurrentScreen();
                FindWinnerOfTheGame();
                decision = UI.AnotherGame();

                if (decision == k_AnotherGame)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    Game newGame = new Game();
                    newGame.StartGame();
                }
            }
            while (decision != k_StopPlaying);
        }

        private void GameRunning()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            ScanAndUpdatePossiblMovesList(m_User1.Symbol, ref m_User1PossibleMove);
            ScanAndUpdatePossiblMovesList(m_User2.Symbol, ref m_User2PossibleMove);

            while (m_User1PossibleMove.Count != k_NoPossibleMoves || m_User2PossibleMove.Count != k_NoPossibleMoves)
            {
                if (m_User1PossibleMove.Count != k_NoPossibleMoves)
                {
                    UserTurn(ref m_User1, ref m_User1PossibleMove);
                }

                m_User2PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User2.Symbol, ref m_User2PossibleMove);

                if (m_User2PossibleMove.Count != k_NoPossibleMoves)
                {
                    if (r_GameMode == k_GameAgainstComputer)
                    {
                        int randomCell = RandomMove(m_User2PossibleMove);
                        m_Board.SetCellValue(m_User2.Symbol, m_User2PossibleMove[randomCell].Row, m_User2PossibleMove[randomCell].Col);
                        CheckAndUpdateBoard(m_User2PossibleMove[randomCell].Row, m_User2PossibleMove[randomCell].Col, m_User2.Symbol, true);
                    }
                    else
                    {
                        UserTurn(ref m_User2, ref m_User2PossibleMove);
                    }
                }
                else
                {
                    UI.TurnPassed();
                }

                m_User1PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User1.Symbol, ref m_User1PossibleMove);
                m_User2PossibleMove.Clear();
                ScanAndUpdatePossiblMovesList(m_User2.Symbol, ref m_User2PossibleMove);
            }
        }

        private void PrintCurrentScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            m_User1.Points = CountPoints(m_Board, m_User1.Symbol);
            m_User2.Points = CountPoints(m_Board, m_User2.Symbol);
            UI.PrintResult(m_User1.Points, m_User2.Points, m_User1, m_User2);
            UI.PrintBoard(m_Board);
        }

        private void FindWinnerOfTheGame()
        {
            if (m_User1.Points > m_User2.Points)
            {
                UI.PrintWinner(m_User1);
            }
            else if (m_User2.Points > m_User1.Points)
            {
                UI.PrintWinner(m_User2);
            }
            else
            {
                ///Draw
                UI.PrintWinner(null);
            }
        }

        private void UserTurn(ref Player io_User, ref List<CellMatrix> io_UserPossibleMove)
        {
            int row = 0, col = 0;
            bool isInputOK = false;
            PrintCurrentScreen();
            if (r_GameMode == k_GameAgainstFriend)
            {
                UI.ShowWhoIsTurn(io_User);
            }

            UI.GetMoveFromUser(ref row, ref col, m_Board);

            if (row == k_QuitProgram && col == k_QuitProgram)
            {
                UI.QuitGame();
                Environment.Exit(0);
            }

            if (io_UserPossibleMove.Count != k_NoPossibleMoves)
            {
                do
                {
                    if (CheckIfMoveIsInList(io_UserPossibleMove, row, col) == true)
                    {
                        m_Board.SetCellValue(io_User.Symbol, row, col);
                        CheckAndUpdateBoard(row, col, io_User.Symbol, true);
                        isInputOK = true;
                    }
                    else
                    {
                        UI.SelectSquareAgain();
                        UI.GetMoveFromUser(ref row, ref col, m_Board);
                    }
                }
                while (!isInputOK);
            }
            else
            {
                UI.TurnPassed();
            }
        }

        private bool CheckIfMoveIsInList(List<CellMatrix> i_ListOfOptionalCell, int i_Row, int i_Col)
        {
            bool result = false;
            for (int k = 0; k < i_ListOfOptionalCell.Count; k++)
            {
                if ((i_ListOfOptionalCell[k].Row == i_Row) && i_ListOfOptionalCell[k].Col == i_Col)
                {
                    result = true;
                }
            }

            return result;
        }

        private int RandomMove(List<CellMatrix> I_ListOfMoves)
        {
            int size = I_ListOfMoves.Count;
            Random random = new Random();
            int randomNumber = random.Next(0, size);
            return randomNumber;
        }

        private void ScanAndUpdatePossiblMovesList(char i_Symbol, ref List<CellMatrix> i_ListOfOptionalCell)
        {
            bool isPossible = false;
            for (int i = 0; i < m_Board.Size; i++)
            {
                for (int j = 0; j < m_Board.Size; j++)
                {
                    if (m_Board.GetCellValue(i, j) == k_EmptyCell)
                    {
                        isPossible = CheckAndUpdateBoard(i, j, i_Symbol, false);
                        if (isPossible == true)
                        {
                            i_ListOfOptionalCell.Add(new CellMatrix(i, j));
                        }
                    }

                    isPossible = false;
                }
            }
        }

        private enum eDirection
        {
            Down, Up, Left, Right, UpRight, UpLeft, DownRight, DownLeft
        }

        private void UpdateTableByMove(char i_Symbol, char i_RivalSymbol, int i_Row, int i_Col, eDirection i_Direction)
        {
            bool stopUpdate = false;
            ProceedAccordingToDirection(ref i_Row, ref i_Col, i_Direction);

            while (!stopUpdate)
            {
                if (m_Board.GetCellValue(i_Row, i_Col) == i_Symbol)
                {
                    stopUpdate = true;
                }

                if (m_Board.GetCellValue(i_Row, i_Col) == i_RivalSymbol)
                {
                    m_Board.SetCellValue(i_Symbol, i_Row, i_Col);
                }

                if (m_Board.GetCellValue(i_Row, i_Col) == k_EmptyCell)
                {
                    break;
                }

                ProceedAccordingToDirection(ref i_Row, ref i_Col, i_Direction);
            }
        }

        private bool CheckAndUpdateBoard(int i_Row, int i_Col, char i_Symbol, bool i_UpdateBoard)
        {
            bool check = false;
            char rivalSymbol;
            bool wasRivalSymbol = false;

            if (i_Symbol == k_BlackSymbol)
            {
                rivalSymbol = k_WhiteSymbol;
            }
            else
            {
                rivalSymbol = k_BlackSymbol;
            }

            int i = i_Row, j = i_Col;

            int countDirectionEnum = Enum.GetNames(typeof(eDirection)).Length;

            for (int k = 0; k < countDirectionEnum; k++)
            {
                for (int q = 0; q < m_Board.Size; q++)
                {
                    ProceedAccordingToDirection(ref i, ref j, (eDirection)k);

                    if (CheckIfExceedingTheBoundariesOfTheMatrix(i, j) == true)
                    {
                        goto NextDirecrtion;
                    }

                    if (m_Board.GetCellValue(i, j) == rivalSymbol)
                    {
                        wasRivalSymbol = true;
                    }

                    if (m_Board.GetCellValue(i, j) == i_Symbol)
                    {
                        if (wasRivalSymbol == true)
                        {
                            check = true;
                            if (i_UpdateBoard == true)
                            {
                                UpdateTableByMove(i_Symbol, rivalSymbol, i_Row, i_Col, (eDirection)k);
                                goto NextDirecrtion;
                            }
                            else
                            {
                                goto Finish;
                            }
                        }
                        else
                        {
                            goto NextDirecrtion;
                        }
                    }

                    if (m_Board.GetCellValue(i, j) == k_EmptyCell)
                    {
                        goto NextDirecrtion;
                    }
                }

            NextDirecrtion:
                i = i_Row;
                j = i_Col;
                wasRivalSymbol = false;
            }

        Finish:
            return check;
        }

        private bool CheckIfExceedingTheBoundariesOfTheMatrix(int i_Row, int i_Col)
        {
            return i_Row >= m_Board.Size || i_Row < 0 || i_Col >= m_Board.Size || i_Col < 0;
        }

        private int CountPoints(Board i_Board, char i_Symbol)
        {
            int points = 0;
            for (int i = 0; i < i_Board.Size; i++)
            {
                for (int j = 0; j < i_Board.Size; j++)
                {
                    if (i_Board.GetCellValue(i, j) == i_Symbol)
                    {
                        points++;
                    }
                }
            }

            return points;
        }
    }
}