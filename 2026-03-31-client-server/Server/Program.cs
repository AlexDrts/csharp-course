using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int PORT = 27015;

    static async Task Main()
    {
        Console.Title = "SERVER (Курс валют)";
        Console.OutputEncoding = Encoding.UTF8;

        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, PORT));
        listener.Listen(10);

        Console.WriteLine("Сервер запущено...\n");

        while (true)
        {
            var client = await listener.AcceptAsync();
            _ = HandleClientAsync(client);
        }
    }

    static async Task HandleClientAsync(Socket client)
    {
        string clientInfo = client.RemoteEndPoint?.ToString() ?? "unknown";
        Console.WriteLine($"[+] Підключення: {clientInfo} | {DateTime.Now}");

        var buffer = new byte[512];

        try
        {
            while (true)
            {
                int bytes = await client.ReceiveAsync(buffer);

                if (bytes == 0)
                    break;

                string request = Encoding.UTF8.GetString(buffer, 0, bytes).Trim().ToUpper();
                Console.WriteLine($"[{DateTime.Now}] Запит від {clientInfo}: {request}");

                string response = await GetRate(request);

                byte[] data = Encoding.UTF8.GetBytes(response);
                await client.SendAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine($"[-] Відключення: {clientInfo} | {DateTime.Now}\n");
        client.Close();
    }

    static async Task<string> GetRate(string request)
    {
        await Task.Delay(200);

        var parts = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            return "Формат: USD EUR";

        string from = parts[0];
        string to = parts[1];

        var rates = new Dictionary<string, double>
        {
            { "USD", 1.0 },
            { "EUR", 0.93 },
            { "UAH", 41.0 },
            { "GBP", 0.80 }
        };

        if (!rates.ContainsKey(from) || !rates.ContainsKey(to))
            return "Невідома валюта";

        double result = rates[to] / rates[from];

        return $"1 {from} = {result:F4} {to}";
    }
}
