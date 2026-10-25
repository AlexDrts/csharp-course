using System.Text;
using Newtonsoft.Json; // dotnet add package Newtonsoft.Json

class Program
{
    private const string AddCitiesUrl = "http://lak.onlinewebshop.net/upload.php";
    private const string GetCitiesUrl = "http://lak.onlinewebshop.net/get_cities.php";
    private static readonly HttpClient client = new();

    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var cities = new List<string>();
        Console.Write("Введіть назву вашого міста: ");
        var myCity = Console.ReadLine();
        cities.Add(myCity!);

        await AddCitiesAsync(cities);

        var allCities = await GetCitiesAsync();

        Console.WriteLine("Список усіх міст:");
        foreach (var city in allCities)
        {
            Console.WriteLine(city);
        }
    }

    static async Task AddCitiesAsync(List<string> cities)
    {
        var json = JsonConvert.SerializeObject(cities);
        Console.WriteLine(json);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync(AddCitiesUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Міста успішно додано.");
            }
            else
            {
                Console.WriteLine("Помилка при додаванні міст: " + response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Сталася помилка: " + ex.Message);
        }
    }

    static async Task<List<string>> GetCitiesAsync()
    {
        // GET запит на сервер для отримання всіх міст
        var response = await client.GetStringAsync(GetCitiesUrl);

        if (response != null)
        {
            // перетворення відповіді в список рядків (міста)
            return JsonConvert.DeserializeObject<List<string>>(response);
        }

        return new List<string>(); // повертаємо порожній список, якщо нічого не надійшло
    }
}