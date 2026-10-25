using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private const int MESSAGE_LENGTH = 512;
    private const int SERVER_PORT = 27015;

    static void Main()
    {
        Console.Title = "CLIENT";

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        try
        {
            var ipAddress = IPAddress.Parse("127.0.0.1"); // IP-адреса локального хоста (127.0.0.1), яка використовується для підключення до сервера на поточному пристрої
            var remoteEndPoint = new IPEndPoint(ipAddress, SERVER_PORT);

            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(remoteEndPoint); // ініціює підключення клієнта до сервера за вказаною кінцевою точкою (IP-адреса та порт)

            Console.WriteLine("Підключено до сервера!");
            Console.WriteLine("\nДоступні команди:");
            Console.WriteLine(" - привіт");
            Console.WriteLine(" - як справи");
            Console.WriteLine(" - котра година");
            Console.WriteLine(" - день тижня");
            Console.WriteLine(" - дата\n");


            while (true)
            {
                Console.Write("Введіть повідомлення: ");
                string? message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    continue; 

                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                clientSocket.Send(messageBytes);

                var buffer = new byte[MESSAGE_LENGTH];
                int bytesReceived = clientSocket.Receive(buffer);

                string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                Console.WriteLine($"Сервер: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}