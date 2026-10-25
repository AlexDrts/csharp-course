using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static string logFile = "text_stats.txt";

    static void Main()
    {
        string text = "";
        Console.WriteLine("Введіть текст (після кожного рядка натисніть Enter, введіть 'exit' для виходу):");

        while (true)
        {
            string line = Console.ReadLine();
            if (line.ToLower() == "exit") break;

            text += line + " ";

            Task<int> wordsTask = Task.Run(() => CountWords(text));
            Task<int> numbersTask = Task.Run(() => CountNumbers(text));
            Task<int> punctuationTask = Task.Run(() => CountPunctuation(text));
            Task<string> mostCommonTask = Task.Run(() => MostCommonWord(text));
            Task<double> avgLengthTask = Task.Run(() => AverageWordLength(text));

            Task.WaitAll(wordsTask, numbersTask, punctuationTask, mostCommonTask, avgLengthTask);

            Console.WriteLine("\n=== Статистика ===");
            Console.WriteLine($"Слів: {wordsTask.Result}");
            Console.WriteLine($"Чисел: {numbersTask.Result}");
            Console.WriteLine($"Розділових знаків: {punctuationTask.Result}");
            Console.WriteLine($"Найпоширеніше слово: {mostCommonTask.Result}");
            Console.WriteLine($"Середня довжина слова: {avgLengthTask.Result:F2}");

            SaveStatisticsToFile(wordsTask.Result, numbersTask.Result, punctuationTask.Result, mostCommonTask.Result, avgLengthTask.Result);
        }
    }

    static int CountWords(string text)
    {
        return Regex.Matches(text, @"\b\w+\b").Count;
    }

    static int CountNumbers(string text)
    {
        return Regex.Matches(text, @"\b-?\d+(\.\d+)?\b").Count;
    }

    static int CountPunctuation(string text)
    {
        return text.Count(c => char.IsPunctuation(c));
    }

    static string MostCommonWord(string text)
    {
        var words = Regex.Matches(text.ToLower(), @"\b\w+\b")
                         .Cast<Match>()
                         .Select(m => m.Value);
        if (!words.Any()) return "Немає";
        return words.GroupBy(w => w)
                    .OrderByDescending(g => g.Count())
                    .First().Key;
    }

    static double AverageWordLength(string text)
    {
        var words = Regex.Matches(text, @"\b\w+\b")
                         .Cast<Match>()
                         .Select(m => m.Value);
        if (!words.Any()) return 0;
        return words.Average(w => w.Length);
    }

    static void SaveStatisticsToFile(int words, int numbers, int punctuation, string mostCommon, double avgLength)
    {
        try
        {
            File.WriteAllLines(logFile, new string[]
            {
                $"Слів: {words}",
                $"Чисел: {numbers}",
                $"Розділових знаків: {punctuation}",
                $"Найпоширеніше слово: {mostCommon}",
                $"Середня довжина слова: {avgLength:F2}"
            });
        }
        catch {}
    }
}
