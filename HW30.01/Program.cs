namespace BusStopSimulation
{
    internal class Program
    {
        static readonly object syncLock = new object();
        static int waitingPassengers = 0;
        static readonly Random rng = new Random();
        static readonly int busMaxCapacity = 30;
        static readonly int totalBusesCount = 10;

        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Імітація роботи автобусної кінцевої зупинки розпочата!");
                Console.ResetColor();

                Thread passengerGeneratorThread = new Thread(GeneratePassengerFlow);
                passengerGeneratorThread.Start();

                for (int busIndex = 0; busIndex < totalBusesCount; busIndex++)
                {
                    Thread busArrivalThread = new Thread(BusArrivalHandler);
                    busArrivalThread.Start(busIndex + 1);
                    Thread.Sleep(rng.Next(1000, 5000)); 
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nІмітація завершена. Кінцева зупинка закривається");
                Console.ResetColor();
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка під час виконання програми: {error.Message}");
                Console.ResetColor();
            }
        }

        static void GeneratePassengerFlow()
        {
            try
            {
                for (int iteration = 0; iteration < 20; iteration++)
                {
                    int incomingPassengers = rng.Next(5, 15);
                    lock (syncLock)
                    {
                        waitingPassengers += incomingPassengers;
                        PrintPassengerInfo(incomingPassengers);
                    }
                    Thread.Sleep(rng.Next(2000, 4000)); 
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка під час генерації пасажирів: {error.Message}");
                Console.ResetColor();
            }
        }

        static void PrintPassengerInfo(int incomingPassengers)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Прибуло {incomingPassengers} пасажирів");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Загалом пасажирів на зупинці: {waitingPassengers}");
            Console.ResetColor();

            Console.WriteLine(new string('-', 50)); 
        }

        static void BusArrivalHandler(object busIdentifier)
        {
            try
            {
                lock (syncLock)
                {
                    int busNumber = (int)busIdentifier;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\nАвтобус №{busNumber} прибув");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Пасажири, що чекають: {waitingPassengers}");
                    int boardingPassengers = Math.Min(waitingPassengers, busMaxCapacity);
                    waitingPassengers -= boardingPassengers;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Автобус №{busNumber} забрав {boardingPassengers} пасажирів");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Залишилось на зупинці: {waitingPassengers}");
                    Console.ResetColor();

                    Console.WriteLine(new string('-', 50)); 
                }
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Помилка під час обробки автобуса №{busIdentifier}: {error.Message}");
                Console.ResetColor();
            }
        }
    }
}
