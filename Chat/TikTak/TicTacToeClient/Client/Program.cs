using System;
using System.Net.Sockets;
using System.Text;

namespace TicTacToeClient
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Starting Tic-Tac-Toe Client...");
                Console.Write("Enter server IP (default: 127.0.0.1): ");
                var ip = Console.ReadLine();
                if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";

                var client = new GameClient(ip);
                client.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
        }
    }

    public class GameClient
    {
        private const int Port = 8888;
        private readonly string _serverIp;
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly char[,] _board = new char[3, 3];
        private char _currentPlayer = 'X';

        public GameClient(string serverIp)
        {
            _serverIp = serverIp;
        }

        public void Start()
        {
            InitializeBoard();
            _client = new TcpClient(_serverIp, Port);
            _stream = _client.GetStream();
            Console.WriteLine("Connected to server!");

            while (true)
            {
                ReceiveBoard();

                if (CheckGameOver())
                {
                    break;
                }

                if (_currentPlayer == 'O')
                {
                    ProcessLocalMove();
                }
                else
                {
                    Console.WriteLine("Waiting for server move...");
                }
            }

            _stream.Close();
            _client.Close();
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

        private void ReceiveBoard()
        {
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
                    case ConsoleKey.D2: 
                        if (_board[y, x] == ' ')
                        {
                            _board[y, x] = 'O';
                            _currentPlayer = 'X'; 
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
            Console.WriteLine("Your turn (O)");
            Console.WriteLine("Use arrow keys to move, 2 for O, Enter to confirm"); 
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

        private void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine(_currentPlayer == 'O' ? "Your turn (O)" : "Opponent's turn (X)");
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