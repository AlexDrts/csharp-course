using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private const int PORT = 27015;

    static async Task Main()
    {
        Console.Title = "CLIENT";
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        await socket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.2"), PORT));

        Console.WriteLine("Підключено до сервера!\n");

        Console.WriteLine("Команди:");
        Console.WriteLine("час");
        Console.WriteLine("дата");
        Console.WriteLine("погода <місто>");
        Console.WriteLine("євро");
        Console.WriteLine("біткоїн\n");

        _ = ReceiveLoop(socket);

        while (true)
        {
            Console.Write(">> ");
            string? message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            byte[] data = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(data);
        }
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
            Console.WriteLine($"\nСервер: {response}");
        }
    }
}
