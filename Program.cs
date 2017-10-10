using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GuessTheNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            int keepPlaying = 1;
            while (keepPlaying == 1)
            {
                Console.Clear();
                actualCode();
                Console.Write("Wanna keep playing? (1 = Yes, 0 = No) ");
                int.TryParse(Console.ReadLine(), out keepPlaying);
            }
            
        }

        public static void actualCode()
        {
            long rangeMinimum, rangeMaximum, chosenNumber = 0;
            int iterate;
            Console.WriteLine("Please pick the minimum range.");
            long.TryParse(Console.ReadLine(), out rangeMinimum);
            Console.WriteLine("Please pick the maximum range.");
            long.TryParse(Console.ReadLine(), out rangeMaximum);
            Console.WriteLine("Now please pick the number inbetween the range.");
            long.TryParse(Console.ReadLine(), out chosenNumber);
            Console.WriteLine("Insert how many times it will guess. (For development use. Insert 1 if you want to just play.");
            int.TryParse(Console.ReadLine(), out iterate);
            while (chosenNumber < rangeMinimum || chosenNumber > rangeMaximum)
            {
                Console.WriteLine("Your selection is invalid. Please try insert a new one.");
                long.TryParse(Console.ReadLine(), out chosenNumber);
            }
            
            List<int> list = new List<int>();

            using (var writer = new StreamWriter("file.txt"))
            {
                //for (int i = 0; i < 100; i++)
                Parallel.For(1, iterate, i =>
                {
                    int tries = guesserModified(rangeMinimum, rangeMaximum, chosenNumber);
                    list.Add(tries);
                    writer.WriteLine(tries);
                });
            }
            int averageTries = 0;
            averageTries = list.Sum() / list.Count;
            Console.WriteLine("This program predicts your number {0} times in an average of {1} tries to guess your number.", 100, averageTries);
        }

        //Function to get a random number 
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static long RandomNumber(long min, long max)
        {
            lock (syncLock)
            { // synchronize
                //return random.Nextlong() * (max - min) + min;
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                long longRand = BitConverter.ToInt64(buf, 0);

                return (Math.Abs(longRand % (max - min)) + min);
            }
        }
        public static void guesser(long rangeMin, long rangeMax, long actualNumber)
        {
            Console.WriteLine("Start picking random number");

            Random rand = new Random();
            long rangeMinAltered = rangeMin, rangeMaxAltered = rangeMax;
            //int guessedNumber = rand.Next(rangeMin, rangeMax);
            long guessedNumber = RandomNumber(rangeMin, rangeMax);
            Console.WriteLine(rangeMin);
            Console.WriteLine(rangeMax);
            Console.WriteLine("Initial guess : {0}", guessedNumber);
            bool numberGuessed = false;
            int counter = 0;
            while (numberGuessed != true)
            {
                if (guessedNumber > actualNumber)
                {
                    Random rand1 = new Random();
                    //Thread.Sleep(2000);
                    Console.WriteLine("{0} seems a bit too high. Choosing a lower number", guessedNumber);
                    rangeMaxAltered = guessedNumber;
                    //guessedNumber = rand1.Next(rangeMinAltered, rangeMaxAltered);
                    guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                }
                else
                {
                    if (guessedNumber < actualNumber)
                    {
                        Console.WriteLine("{0} seems a bit too low. Choosing a higher number", guessedNumber);
                        rangeMinAltered = guessedNumber;
                        guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                    }
                    else
                    {
                        if (guessedNumber == actualNumber)
                        {
                            numberGuessed = true;
                        }
                    }
                }
                counter = counter + 1;
            }
            Console.WriteLine("Found the number you've chosen which is {0}. Took me {1} tries to guessed it.", guessedNumber, counter);
        }

        public static int guesserModified(long rangeMin, long rangeMax, long actualNumber)
        {
            
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Thread[{0}] : Start picking random number", threadId);
            Random rand = new Random();
            long rangeMinAltered = rangeMin, rangeMaxAltered = rangeMax;
            //int guessedNumber = rand.Next(rangeMin, rangeMax);
            long guessedNumber = RandomNumber(rangeMin, rangeMax);
            //Console.WriteLine(rangeMin);
            //Console.WriteLine(rangeMax);
            Console.WriteLine("Thread[{1}] : Initial guess : {0}", guessedNumber, threadId);
            bool numberGuessed = false;
            int counter = 0;
            while (numberGuessed != true)
            {
                if (guessedNumber > actualNumber)
                {
                    Random rand1 = new Random();
                    //Thread.Sleep(2000);
                    Console.WriteLine("Thread[{1}] : {0} seems a bit too high. Choosing a lower number", guessedNumber, threadId);
                    rangeMaxAltered = guessedNumber;
                    //guessedNumber = rand1.Next(rangeMinAltered, rangeMaxAltered);
                    guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                }
                else
                {
                    if (guessedNumber < actualNumber)
                    {
                        Console.WriteLine("Thread[{1}] : {0} seems a bit too low. Choosing a higher number", guessedNumber, threadId);
                        rangeMinAltered = guessedNumber;
                        guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                    }
                    else
                    {
                        if (guessedNumber == actualNumber)
                        {
                            numberGuessed = true;
                        }
                    }
                }
                counter = counter + 1;
            }
            Console.WriteLine("Thread[{2}] : Found the number you've chosen which is {0}. Took me {1} tries to guessed it.", guessedNumber, counter, threadId);
            return counter;
        }
    }
}
