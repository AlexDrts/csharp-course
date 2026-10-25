using System.Net; // основний простір імен для роботи з мережевими адресами та протоколами
using System.Net.Sockets; // простір імен для роботи з сокетами
using System.Text; // простір імен для роботи з кодуваннями

class Server
{
    private const int MESSAGE_LENGTH = 512; // задає розмір буфера для отримання даних
    private const int SERVER_PORT = 27015; // вказує порт, на якому сервер прослуховуватиме підключення
    private const int PAUSE = 0; // задає паузу в мілісекундах для краси та зручності виведення повідомлень (можна сміливо прибрати)

    static void Main()
    {
        Console.Title = "SERVER";

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        try
        {
            var ipAddress = IPAddress.Any; // отримує будь-яку доступну IP-адресу для прослуховування (означає, що сервер слухатиме на всіх інтерфейсах, наприклад, Wi-Fi, Ethernet
            var localEndPoint = new IPEndPoint(ipAddress, SERVER_PORT); // створює кінцеву точку (адресу та порт), до якої сервер буде прив’язаний

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // створює сокет для використання TCP-з’єднання (потоковий сокет)
            listener.Bind(localEndPoint); // прив’язує сокет до вказаної адреси та порту

            listener.Listen(10); // починає прослуховування вхідних з’єднань, встановлюючи максимальну кількість з’єднань, що очікують (10), тобто сервер може мати до 10 клієнтів (з’єднань) у черзі на підключення

            Console.WriteLine("Сервер запущено. Очікування підключення...");

            var clientSocket = listener.Accept(); // очікує підключення клієнта та приймає його, повертаючи сокет для спілкування з клієнтом. є AcceptAsync(), щоб не блокувати потік
            Console.WriteLine("Клієнт підключився!");

            while (true)
            {
                var buffer = new byte[MESSAGE_LENGTH]; // створює буфер для зберігання отриманих даних
                int bytesReceived = clientSocket.Receive(buffer); // отримує дані від клієнта та зберігає їх у буфер

                if (bytesReceived > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived).ToLower().Trim();
                    Console.WriteLine($"Клієнт: {message}"); // виводить отримане повідомлення

                    // формування відповіді на запит клієнта
                    string response = GetResponse(message);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response); // перетворює відповідь у масив байтів
                    clientSocket.Send(responseBytes);
                }
                else if (bytesReceived == 0) // якщо клієнт закрив з’єднання (отримано 0 байтів)
                {
                    Console.WriteLine("Клієнт відключився..."); // інформує про те, що з’єднання буде закрито
                    break;
                }
                else
                {
                    Console.WriteLine("Помилка при отриманні даних.");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static string GetResponse(string message)
    {
        return message switch
        {
            "привіт" => "привіт",
            "як справи" => "чудово",
            "котра година" => DateTime.Now.ToString("HH:mm"),
            "день тижня" => GetUkrainianDay(DateTime.Now.DayOfWeek),
            "дата" => DateTime.Now.ToString("dd.MM.yyyy"),
            _ => "не розумію запит"
        };
    }

    static string GetUkrainianDay(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday => "понеділок",
            DayOfWeek.Tuesday => "вівторок",
            DayOfWeek.Wednesday => "середа",
            DayOfWeek.Thursday => "четвер",
            DayOfWeek.Friday => "п’ятниця",
            DayOfWeek.Saturday => "субота",
            DayOfWeek.Sunday => "неділя",
            _ => ""
        };
    }
}