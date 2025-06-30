using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TicTacToeServer
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Starting Tic-Tac-Toe Server...");
                var server = new GameServer();
                server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
        }
    }

    public class GameServer
    {
        private const int Port = 8888;
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly char[,] _board = new char[3, 3];
        private char _currentPlayer = 'X';

        public void Start()
        {
            InitializeBoard();
            _listener = new TcpListener(IPAddress.Any, Port);
            _listener.Start();
            
            Console.WriteLine($"Waiting for connection on port {Port}...");
            _client = _listener.AcceptTcpClient();
            _stream = _client.GetStream();
            Console.WriteLine("Client connected!");

            SendBoard();

            while (true)
            {
                if (_currentPlayer == 'O')
                {
                    ReceiveMove();
                }
                else
                {
                    Console.WriteLine("Waiting for your move...");
                    Console.WriteLine("Use arrow keys to move, 1 for X, 2 for O, Enter to confirm");
                    ProcessLocalMove();
                }

                if (CheckGameOver())
                {
                    break;
                }
            }

            _stream.Close();
            _client.Close();
            _listener.Stop();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j] = ' ';
                }
            }
        }

        private void SendBoard()
        {
            var boardString = BoardToString();
            var data = Encoding.ASCII.GetBytes(boardString);
            _stream.Write(data, 0, data.Length);
        }

        private string BoardToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sb.Append(_board[i, j]);
                }
            }
            sb.Append(_currentPlayer);
            return sb.ToString();
        }

        private void ProcessLocalMove()
        {
            int x = 1, y = 1;
            ConsoleKeyInfo key;
            bool moveConfirmed = false;

            while (!moveConfirmed)
            {
                DrawBoardWithCursor(x, y);
                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        y = Math.Max(0, y - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        y = Math.Min(2, y + 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        x = Math.Max(0, x - 1);
                        break;
                    case ConsoleKey.RightArrow:
                        x = Math.Min(2, x + 1);
                        break;
                    case ConsoleKey.D1:
                        if (_board[y, x] == ' ')
                        {
                            _board[y, x] = 'X';
                            _currentPlayer = 'O';
                            moveConfirmed = true;
                        }
                        break;
                    case ConsoleKey.D2:
                        if (_board[y, x] == ' ')
                        {
                            _board[y, x] = 'O';
                            _currentPlayer = 'O';
                            moveConfirmed = true;
                        }
                        break;
                    case ConsoleKey.Enter:
                        moveConfirmed = true;
                        break;
                }
            }

            SendBoard();
        }

        private void DrawBoardWithCursor(int cursorX, int cursorY)
        {
            Console.Clear();
            Console.WriteLine("Your turn (X)");
            Console.WriteLine("Use arrow keys to move, 1 for X, 2 for O, Enter to confirm");
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == cursorY && j == cursorX)
                    {
                        Console.Write($"[{_board[i, j]}]");
                    }
                    else
                    {
                        Console.Write($" {_board[i, j]} ");
                    }

                    if (j < 2) Console.Write("|");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine("-----------");
            }
        }

        private void ReceiveMove()
        {
            Console.WriteLine("Waiting for opponent's move...");
            var buffer = new byte[10];
            int bytesRead = _stream.Read(buffer, 0, buffer.Length);
            var receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j] = receivedData[i * 3 + j];
                }
            }

            _currentPlayer = receivedData[9];
            DrawBoard();
        }

        private void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine(_currentPlayer == 'X' ? "Your turn (X)" : "Opponent's turn (O)");
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write($" {_board[i, j]} ");
                    if (j < 2) Console.Write("|");
                }
                Console.WriteLine();
                if (i < 2) Console.WriteLine("-----------");
            }
        }

        private bool CheckGameOver()
        {
           
            for (int i = 0; i < 3; i++)
            {
                if (_board[i, 0] != ' ' && _board[i, 0] == _board[i, 1] && _board[i, 1] == _board[i, 2])
                {
                    Console.WriteLine($"Player {_board[i, 0]} wins!");
                    return true;
                }
            }

          
            for (int j = 0; j < 3; j++)
            {
                if (_board[0, j] != ' ' && _board[0, j] == _board[1, j] && _board[1, j] == _board[2, j])
                {
                    Console.WriteLine($"Player {_board[0, j]} wins!");
                    return true;
                }
            }

        
            if (_board[0, 0] != ' ' && _board[0, 0] == _board[1, 1] && _board[1, 1] == _board[2, 2])
            {
                Console.WriteLine($"Player {_board[0, 0]} wins!");
                return true;
            }

            if (_board[0, 2] != ' ' && _board[0, 2] == _board[1, 1] && _board[1, 1] == _board[2, 0])
            {
                Console.WriteLine($"Player {_board[0, 2]} wins!");
                return true;
            }

         
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_board[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }

            Console.WriteLine("It's a draw!");
            return true;
        }
    }
}