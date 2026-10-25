using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("Enter city name: ");
        var city = Console.ReadLine();

        string srcUrl = "https://wttr.in/" + city + "?format=j1";

        string folderPath = @"C:\Users\arg50\Рабочий стол\09.04.2026\09.04.2026";
        string destFile = Path.Combine(folderPath, "weather.json");

        try
        {
            Directory.CreateDirectory(folderPath);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                using (var response = await client.GetAsync(srcUrl))
                {
                    Console.WriteLine($"Status code: {(int)response.StatusCode} ({response.ReasonPhrase})");

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();

                    File.WriteAllText(destFile, content, Encoding.UTF8);

                    Console.WriteLine($"Saved to: {destFile}");

                    Match temp = Regex.Match(
                        content,
                        @"""temp_C"":\s*""([^""]+)"""
                    );

                    Match wind = Regex.Match(
                        content,
                        @"""windspeedKmph"":\s*""([^""]+)"""
                    );

                    Match humidity = Regex.Match(
                        content,
                        @"""humidity"":\s*""([^""]+)"""
                    );

                    Console.WriteLine("\n===== WEATHER =====");

                    Console.WriteLine(
                        "Temperature: " +
                        (temp.Success ? temp.Groups[1].Value + " °C" : "No data")
                    );

                    Console.WriteLine(
                        "Wind speed: " +
                        (wind.Success ? wind.Groups[1].Value + " km/h" : "No data")
                    );

                    Console.WriteLine(
                        "Humidity: " +
                        (humidity.Success ? humidity.Groups[1].Value + " %" : "No data")
                    );
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}