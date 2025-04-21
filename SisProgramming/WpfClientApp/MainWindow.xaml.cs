using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace WpfClientApp
{
    public partial class MainWindow : Window
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;

        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageBoxInput.Text;

            if (!string.IsNullOrEmpty(message))
            {
                string response = SendMessage(message);
                ResponseBox.Text = response; 
                MessageBoxInput.Clear();
            }
        }

       
        private string SendMessage(string message)
        {
            try
            {
            
                _tcpClient = new TcpClient("127.0.0.1", 8080);
                _stream = _tcpClient.GetStream();

            
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                _stream.Write(messageBytes, 0, messageBytes.Length);

            
                byte[] buffer = new byte[1024];
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                _stream.Close();
                _tcpClient.Close();

                return response;
            }
            catch (Exception ex)
            {
                return $"Ошибка при отправке сообщения: {ex.Message}";
            }
        }
    }
}