using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        List<int> numbers = new List<int> { 5, 10, 2, 8, 15, 3 };

        Task<int> minTask = Task.Run(() => numbers.Min());
        Task<int> maxTask = Task.Run(() => numbers.Max());
        Task<double> avgTask = Task.Run(() => numbers.Average());
        Task<int> sumTask = Task.Run(() => numbers.Sum());

        await Task.WhenAll(minTask, maxTask, avgTask, sumTask);

        Console.WriteLine($"Мінімальне значення: {minTask.Result}");
        Console.WriteLine($"Максимальне значення: {maxTask.Result}");
        Console.WriteLine($"Середнє значення: {avgTask.Result:F2}");
        Console.WriteLine($"Сума всіх елементів: {sumTask.Result}");
    }
}
