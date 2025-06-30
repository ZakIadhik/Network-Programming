using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static TcpListener listener;
    static TcpClient client;
    static NetworkStream stream;
    static List<string> messageHistory = new List<string>();
    static bool isRunning = true;

    static void Main(string[] args)
    {
        Console.Write("Введите порт для входящих соединений: ");
        int port = int.Parse(Console.ReadLine());

        Console.Write("Введите IP удаленного собеседника (или пусто для пропуска): ");
        string remoteIp = Console.ReadLine();

        Console.Write("Введите порт удаленного собеседника (или пусто для пропуска): ");
        string remotePortStr = Console.ReadLine();

        Thread serverThread = new Thread(() => StartServer(port));
        serverThread.Start();

        if (!string.IsNullOrWhiteSpace(remoteIp) && !string.IsNullOrWhiteSpace(remotePortStr))
        {
            int remotePort = int.Parse(remotePortStr);
            ConnectToPeer(remoteIp, remotePort);
        }

        while (isRunning)
        {
            string input = Console.ReadLine();

            if (input == "/exit")
            {
                isRunning = false;
                break;
            }

            SendMessage(input);
            messageHistory.Add($"[Я]: {input}");
        }

        Console.WriteLine("\nИстория сообщений:");
        foreach (var msg in messageHistory)
            Console.WriteLine(msg);

        listener.Stop();
        stream?.Close();
        client?.Close();
    }

    static void StartServer(int port)
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();
            stream = client.GetStream();

            while (isRunning)
            {
                byte[] buffer = new byte[1024];
                int byteCount = stream.Read(buffer, 0, buffer.Length);
                if (byteCount == 0) continue;

                string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                Console.WriteLine($"\n[Собеседник]: {message}");
                messageHistory.Add($"[Собеседник]: {message}");
            }
        }
        catch { }
    }

    static void ConnectToPeer(string ip, int port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ip), port);
            stream = client.GetStream();

            Task.Run(() =>
            {
                while (isRunning)
                {
                    byte[] buffer = new byte[1024];
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) continue;

                    string message = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"\n[Собеседник]: {message}");
                    messageHistory.Add($"[Собеседник]: {message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
        }
    }

    static void SendMessage(string message)
    {
        if (stream == null) return;

        byte[] data = Encoding.UTF8.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }
}
