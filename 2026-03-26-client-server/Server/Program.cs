using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private const int PORT = 27015;

    static async Task Main()
    {
        Console.Title = "SERVER";
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, PORT));
        listener.Listen(10);

        Console.WriteLine("Сервер запущено...");

        while (true)
        {
            var client = await listener.AcceptAsync();
            _ = HandleClientAsync(client);
        }
    }

    static async Task HandleClientAsync(Socket client)
    {
        Console.WriteLine("Клієнт підключився");

        var buffer = new byte[512];

        try
        {
            while (true)
            {
                int bytes = await client.ReceiveAsync(buffer);

                if (bytes == 0)
                    break;

                string request = Encoding.UTF8.GetString(buffer, 0, bytes).ToLower().Trim();
                Console.WriteLine($"Клієнт: {request}");

                string response = await GetResponse(request);

                byte[] data = Encoding.UTF8.GetBytes(response);
                await client.SendAsync(data);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        Console.WriteLine("Клієнт відключився");
        client.Close();
    }

    static async Task<string> GetResponse(string request)
    {
        if (request == "час")
            return DateTime.Now.ToString("HH:mm:ss");

        if (request == "дата")
            return DateTime.Now.ToString("dd.MM.yyyy");

        if (request.StartsWith("погода"))
        {
            string city = request.Replace("погода", "").Trim();

            if (string.IsNullOrEmpty(city))
                return "Вкажіть місто: погода Київ";

            await Task.Delay(500); 
            return $"Погода у {city}: +20°C";
        }

        if (request == "євро")
        {
            await Task.Delay(300);
            return "Курс євро: 42.50 грн";
        }

        if (request == "біткоїн")
        {
            await Task.Delay(300);
            return "Bitcoin: 65000$";
        }

        return "Невідома команда";
    }
}
