using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("Пошук слова у файлах");
        Console.WriteLine(new string('─', 25));

        Console.Write("Введіть шлях до директорії: ");
        string directory = Console.ReadLine();

        Console.Write("Введіть слово для пошуку: ");
        string keyword = Console.ReadLine();

        if (!Directory.Exists(directory)) return;

        var cts = new CancellationTokenSource();
        var pauseEvent = new ManualResetEventSlim(true);
        var files = new ConcurrentQueue<string>(Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories));
        int filesProcessed = 0;
        int occurrences = 0;

        Task searchTask = Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested && files.TryDequeue(out string file))
            {
                pauseEvent.Wait();

                try
                {
                    string text = File.ReadAllText(file);
                    int count = CountOccurrences(text, keyword);
                    Interlocked.Add(ref occurrences, count);
                }
                catch { }

                Interlocked.Increment(ref filesProcessed);
            }
        }, cts.Token);

        Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.P:
                            pauseEvent.Reset();
                            Console.WriteLine("[Пауза]");
                            break;
                        case ConsoleKey.R:
                            pauseEvent.Set();
                            Console.WriteLine("[Продовжено]");
                            break;
                        case ConsoleKey.S:
                            cts.Cancel();
                            pauseEvent.Set();
                            Console.WriteLine("[Зупинено]");
                            break;
                        case ConsoleKey.Escape:
                            cts.Cancel();
                            pauseEvent.Set();
                            Environment.Exit(0);
                            break;
                    }
                }
                Thread.Sleep(100);
            }
        });

        while (!cts.Token.IsCancellationRequested && (!searchTask.IsCompleted || files.Count > 0))
        {
            Console.Clear();
            Console.WriteLine("Пошук слова у файлах");
            Console.WriteLine(new string('─', 25));
            Console.WriteLine($"Оброблено файлів: {filesProcessed} / {filesProcessed + files.Count}");
            Console.WriteLine($"Знайдено входжень: {occurrences}");
            Console.WriteLine("\nКоманди: P – пауза, R – продовжити, S – зупинити, Esc – вихід");
            Thread.Sleep(500);
        }

        searchTask.Wait();

        Console.WriteLine("\n=== Пошук завершено ===");
        Console.WriteLine($"Оброблено файлів: {filesProcessed}");
        Console.WriteLine($"Знайдено входжень: {occurrences}");
        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ReadKey();
    }

    static int CountOccurrences(string text, string keyword)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return 0;
        int count = 0, index = 0;
        while ((index = text.IndexOf(keyword, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            count++;
            index += keyword.Length;
        }
        return count;
    }
}
