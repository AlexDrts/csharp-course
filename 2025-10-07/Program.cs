namespace DZ_1__07._10._2025
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1
            Console.WriteLine("1)Arithmetic mean:");
            int a = 10;
            int b = 7;
            int c = 12;
            Console.WriteLine((a + b + c) / 3);

            // 2
            int kil = 10;
            Console.WriteLine("In sea miles:");
            Console.WriteLine(kil / 1.852);
            Console.WriteLine("In earth miles:");
            Console.WriteLine(kil / 1.609);

            // 3
            double P = 64.9;
            int N = 987;
            Console.WriteLine("Procent:");
            Console.WriteLine(P * 100 / N);

            // 4
            int C = 29;
            int F = 77;
            Console.WriteLine("Fahrenheit:");
            Console.WriteLine((C * 1.8) + 32);
            Console.WriteLine("Celcium:");
            Console.WriteLine((F - 32) / 1.8);

        }
    }
}
