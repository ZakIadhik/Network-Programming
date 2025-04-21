using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfServerApp
{
    public partial class MainWindow : Window
    {
        private TcpListener _tcpListener;
        private Thread _listenerThread;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
          
            _tcpListener = new TcpListener(IPAddress.Any, 8080);
            _tcpListener.Start();

       
            _listenerThread = new Thread(ListenForClients);
            _listenerThread.Start();

            MessageBox.Show("Сервер запущен и слушает на порту 8080.");
        }


        private void ListenForClients()
        {
            while (true)
            {
            
                var client = _tcpListener.AcceptTcpClient();
                var clientThread = new Thread(() => HandleClientComm(client));
                clientThread.Start();
            }
        }


        private void HandleClientComm(TcpClient tcpClient)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            
                string response = $"Сервер получил сообщение: {message}";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                
                Dispatcher.Invoke(() => MessagesBox.AppendText($"Получено сообщение: {message}\n"));
            }
        }
    }
}
