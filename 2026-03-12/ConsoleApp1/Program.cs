using System;
using System.Threading;

class PassengerStop
{
    private int waitingPassengers = 0;
    private readonly object locker = new object();

    public void AddPassengers(int count)
    {
        lock (locker)
        {
            waitingPassengers += count;
            Console.WriteLine($"На зупинку прибуло {count} пасажирів. Тепер їх {waitingPassengers}");
        }
    }

    public int GetPassengersForBus(int busCapacity)
    {
        lock (locker)
        {
            int boarding = Math.Min(waitingPassengers, busCapacity);
            waitingPassengers -= boarding;
            return boarding;
        }
    }

    public int WaitingPassengers
    {
        get
        {
            lock (locker)
            {
                return waitingPassengers;
            }
        }
    }
}

class Bus
{
    public int Number { get; }
    public int Capacity { get; }
    private readonly PassengerStop stop;
    private readonly SemaphoreSlim boardingSemaphore;
    private readonly Barrier boardingBarrier;

    public Bus(int number, int capacity, PassengerStop stop)
    {
        Number = number;
        Capacity = capacity;
        this.stop = stop;
        boardingSemaphore = new SemaphoreSlim(capacity, capacity);
        boardingBarrier = new Barrier(2);
    }

    public void Arrive()
    {
        Console.WriteLine($"\nАвтобус №{Number} під’їхав");

        int passengersToBoard = stop.GetPassengersForBus(Capacity);

        int boarded = 0;

        Thread boardingThread = new Thread(() =>
        {
            for (int i = 0; i < passengersToBoard; i++)
            {
                boardingSemaphore.Wait();
                boarded++;
                Thread.Sleep(50);
                boardingSemaphore.Release();
            }
            boardingBarrier.SignalAndWait();
        });

        boardingThread.Start();

        boardingBarrier.SignalAndWait();

        Console.WriteLine($"Автобус №{Number} забрав {boarded} пасажирів. Залишилося {stop.WaitingPassengers} пасажирів");
    }
}

class Dispatcher
{
    private readonly PassengerStop stop;
    private readonly Random rnd = new Random();
    public event Action PassengersArrived;

    public Dispatcher(PassengerStop stop)
    {
        this.stop = stop;
    }

    public void Run()
    {
        while (true)
        {
            Thread.Sleep(rnd.Next(500, 1500));
            int newPassengers = rnd.Next(1, 6);
            stop.AddPassengers(newPassengers);
            PassengersArrived?.Invoke();
        }
    }
}

class Program
{
    static void Main()
    {
        PassengerStop stop = new PassengerStop();
        Bus bus150 = new Bus(150, 10, stop);
        Dispatcher dispatcher = new Dispatcher(stop);

        dispatcher.PassengersArrived += () =>
        {
            if (stop.WaitingPassengers > 0)
            {
                bus150.Arrive();
            }
        };

        Thread dispatcherThread = new Thread(dispatcher.Run);
        dispatcherThread.IsBackground = true;
        dispatcherThread.Start();

        Console.WriteLine("Натисніть Esc для виходу...");
        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                break;
            Thread.Sleep(100);
        }
    }
}
