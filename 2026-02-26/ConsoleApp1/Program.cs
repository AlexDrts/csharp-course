using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Список процесів ===");
            Console.WriteLine("ID\tНазва\t\tПріоритет");

            Process[] processes = Process.GetProcesses().OrderBy(p => p.Id).ToArray();

            foreach (var proc in processes)
            {
                string priority;
                try
                {
                    priority = proc.PriorityClass.ToString();
                }
                catch
                {
                    priority = "Недоступно";
                }
                Console.WriteLine($"{proc.Id}\t{proc.ProcessName,-20}\t{priority}");
            }

            Console.WriteLine("\nКоманди:");
            Console.WriteLine("1 - Завершити процес за ID");
            Console.WriteLine("2 - Змінити пріоритет процесу за ID");
            Console.WriteLine("3 - Запустити новий процес");
            Console.WriteLine("0 - Оновити список");

            Console.Write("\nВиберіть команду: ");
            string cmd = Console.ReadLine();

            switch (cmd)
            {
                case "1":
                    Console.Write("Введіть ID процесу для завершення: ");
                    if (int.TryParse(Console.ReadLine(), out int killId))
                    {
                        try
                        {
                            var p = Process.GetProcessById(killId);
                            p.Kill();
                            Console.WriteLine($"Процес {p.ProcessName} (PID={p.Id}) завершено!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невірний ID!");
                    }
                    break;

                case "2":
                    Console.Write("Введіть ID процесу для зміни пріоритету: ");
                    if (int.TryParse(Console.ReadLine(), out int prioId))
                    {
                        try
                        {
                            var p = Process.GetProcessById(prioId);
                            Console.WriteLine($"Поточний пріоритет: {p.PriorityClass}");
                            Console.WriteLine("Доступні пріоритети:");
                            Console.WriteLine("1 - Idle");
                            Console.WriteLine("2 - BelowNormal");
                            Console.WriteLine("3 - Normal");
                            Console.WriteLine("4 - AboveNormal");
                            Console.WriteLine("5 - High");
                            Console.WriteLine("6 - RealTime");

                            Console.Write("Виберіть новий пріоритет: ");
                            string prioCmd = Console.ReadLine();
                            switch (prioCmd)
                            {
                                case "1": p.PriorityClass = ProcessPriorityClass.Idle; break;
                                case "2": p.PriorityClass = ProcessPriorityClass.BelowNormal; break;
                                case "3": p.PriorityClass = ProcessPriorityClass.Normal; break;
                                case "4": p.PriorityClass = ProcessPriorityClass.AboveNormal; break;
                                case "5": p.PriorityClass = ProcessPriorityClass.High; break;
                                case "6": p.PriorityClass = ProcessPriorityClass.RealTime; break;
                                default: Console.WriteLine("Невірний вибір!"); continue;
                            }
                            Console.WriteLine($"Пріоритет процесу {p.ProcessName} змінено на {p.PriorityClass}!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невірний ID!");
                    }
                    break;

                case "3":
                    Console.Write("Введіть назву або шлях до exe для запуску: ");
                    string exePath = Console.ReadLine();
                    try
                    {
                        Process.Start(exePath);
                        Console.WriteLine("Процес запущено!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка запуску: {ex.Message}");
                    }
                    break;

                case "0":
                    break;

                default:
                    Console.WriteLine("Невірна команда!");
                    break;
            }

            Console.WriteLine("\nОчікування 10 секунд перед оновленням списку...");
            Thread.Sleep(10000); 
        }
    }
}
