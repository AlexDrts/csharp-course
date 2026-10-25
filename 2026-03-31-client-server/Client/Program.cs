using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private const int PORT = 27015;

    static async Task Main()
    {
        Console.Title = "CLIENT (Курс валют)";
        Console.OutputEncoding = Encoding.UTF8;

        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        await socket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), PORT));

        Console.WriteLine("Підключено до сервера!\n");
        Console.WriteLine("Введіть валюту (наприклад: USD EUR)");
        Console.WriteLine("exit - вихід\n");

        _ = ReceiveLoop(socket);

        while (true)
        {
            Console.Write(">> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.ToLower() == "exit")
                break;

            byte[] data = Encoding.UTF8.GetBytes(input);
            await socket.SendAsync(data);
        }

        socket.Close();
    }

    static async Task ReceiveLoop(Socket socket)
    {
        var buffer = new byte[512];

        while (true)
        {
            int bytes = await socket.ReceiveAsync(buffer);

            if (bytes == 0)
                break;

            string response = Encoding.UTF8.GetString(buffer, 0, bytes);
            Console.WriteLine($"Сервер: {response}");
        }
    }
}
