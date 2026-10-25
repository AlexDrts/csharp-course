using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        string filePath = "numbers.txt";

//        if (!File.Exists(filePath))
//        {
//            Console.WriteLine($"Файл {filePath} не знайдено!");
//            return;
//        }

//        List<int> numbers = File.ReadAllLines(filePath)
//                                .Select(line => int.Parse(line))
//                                .ToList();

//        var parallelNumbers = numbers.AsParallel();

//        int sum = parallelNumbers.Sum();
//        long product = parallelNumbers.Aggregate(1L, (acc, val) => acc * val);
//        double average = parallelNumbers.Average();
//        int max = parallelNumbers.Max();
//        int min = parallelNumbers.Min();
//        int uniqueCount = parallelNumbers.Distinct().Count();

//        Console.WriteLine($"Сума: {sum}");
//        Console.WriteLine($"Добуток: {product}");
//        Console.WriteLine($"Середнє арифметичне: {average:F2}");
//        Console.WriteLine($"Максимум: {max}");
//        Console.WriteLine($"Мінімум: {min}");
//        Console.WriteLine($"Кількість унікальних значень: {uniqueCount}");
//    }
//}


//class Program
//{
//    static void Main()
//    {
//        Console.Write("Введіть початок діапазону: ");
//        int start = int.Parse(Console.ReadLine());

//        Console.Write("Введіть кінець діапазону: ");
//        int end = int.Parse(Console.ReadLine());

//        string filePath = "multiplication_table.txt";

//        using StreamWriter writer = new StreamWriter(filePath);

//        Parallel.For(start, end + 1, i =>
//        {
//            string line = "";
//            for (int j = start; j <= end; j++)
//            {
//                line += $"{i}*{j}={i * j}\t";
//            }

//            lock (writer)
//            {
//                writer.WriteLine(line);
//            }
//        });

//        Console.WriteLine($"Таблиця множення від {start} до {end} збережена у файлі {filePath}");
//    }
//}