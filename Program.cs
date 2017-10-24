using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace GuessTheNumber
{
    class Program
    {

        public static int repetitionNumber = 0;
        public static int iterate;
        
        static void Main(string[] args)
        {
            int keepPlaying = 1;
            while (keepPlaying == 1)
            {
                Console.Title = string.Format("Number Guesser | Ready");
                Console.Clear();
                actualCode();
                Console.Write("Wanna keep playing? (1 = Yes, 0 = No) ");
                int.TryParse(Console.ReadLine(), out keepPlaying);
            }
            
        }

        public static void actualCode()
        {
            long rangeMinimum, rangeMaximum, chosenNumber = 0;
            Console.WriteLine("Please pick the minimum range.");
            long.TryParse(Console.ReadLine(), out rangeMinimum);
            Console.WriteLine("Please pick the maximum range.");
            long.TryParse(Console.ReadLine(), out rangeMaximum);
            Console.WriteLine("Now please pick the number inbetween the range.");
            long.TryParse(Console.ReadLine(), out chosenNumber);
            //Console.WriteLine("One of this value should not return true : {0}, {1}", chosenNumber < rangeMinimum, chosenNumber > rangeMaximum);
            while (chosenNumber < rangeMinimum || chosenNumber > rangeMaximum)
            {
                Console.WriteLine("Your selection is invalid. Please try insert a new one.");
                long.TryParse(Console.ReadLine(), out chosenNumber);
            }
            Console.WriteLine("Insert how many times it will guess. (For development use. Insert 1 if you want to just play)");
            int.TryParse(Console.ReadLine(), out iterate);
            while (iterate < 1)
            {
                Console.WriteLine("Your number you've input is invalid. Please reenter your input.");
                int.TryParse(Console.ReadLine(), out iterate);
            }
            iterate++;
            
            
            List<int> list = new List<int>();

            var stopWatch = System.Diagnostics.Stopwatch.StartNew(); //Ngeinitialize sekaligus mulai ngitung waktu sampai fungsi Stopwatch dihentikan.\

                //for (int i = 0; i < 100; i++)
            Parallel.For(1, iterate, i =>
            {
                int tries = guesserModified(rangeMinimum, rangeMaximum, chosenNumber);
                list.Add(tries);
            });

            stopWatch.Stop();
            /*
            while (true)
            {
                string[] currentElement;
                bool success = //concurrentqueue.TryDequeue(out currentElement);
                Debug.WriteLine("Succeed : {0}, currentElement : {1}", success, currentElement[0]);
                if (success)
                {
                    using (var sw = new StreamWriter("file3.txt", true))
                    {
                        sw.WriteLine(currentElement[0]);
                    }
                }
            }
            */
            int averageTries = 0;
            averageTries = list.Sum() / list.Count;
            int minTries = list.Min(), maxTries = list.Max();
            Console.WriteLine("This program predicts your number {0} times in an average of {1} tries to guess your number.", list.Count, averageTries);
            Console.WriteLine("The most lowest tries was {0} and the most highest tries was {1}. And also the test was running in {2} miliseconds.", minTries, maxTries, stopWatch.ElapsedMilliseconds);
            Console.Title = string.Format("Number Guesser | Finished");
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

        public static int guesserConventional(long rangeMin, long rangeMax, long actualNumber)
        {
            Console.WriteLine("Starting by guessing the half of the max...");
            long rangeMinAltered = rangeMin, rangeMaxAltered = rangeMax;
            //int guessedNumber = rand.Next(rangeMin, rangeMax);
            long guessedNumber = rangeMax / 2;
            Console.WriteLine(rangeMin);
            Console.WriteLine(rangeMax);
            Console.WriteLine("Initial guess : {0}", guessedNumber);
            bool numberGuessed = false;
            int counter = 0;
            while (numberGuessed != true)
            {
                if (guessedNumber > actualNumber)
                {
                    Console.WriteLine("{0} seems a bit too high. Choosing a lower number", guessedNumber);
                    rangeMaxAltered = guessedNumber;
                    //guessedNumber = rand1.Next(rangeMinAltered, rangeMaxAltered);
                    guessedNumber = rangeMaxAltered / 2;
                    Thread.Sleep(0);
                }
                else
                {
                    if (guessedNumber < actualNumber)
                    {
                        Console.WriteLine("{0} seems a bit too low. Choosing a higher number", guessedNumber);
                        rangeMinAltered = guessedNumber;
                        guessedNumber = rangeMinAltered / 2;
                        Thread.Sleep(0);
                    }
                    else
                    {
                        if (guessedNumber == actualNumber)
                        {
                            numberGuessed = true;
                        }//
                    }
                }
                counter = counter + 1;
            }
            Console.WriteLine("Found the number you've chosen which is {0}. Took me {1} tries to guessed it.", guessedNumber, counter);
            return counter;
        }

        
        //public static //concurrentQueue<string[]> //concurrentqueue = new //concurrentQueue<string[]>();
        public static int guesserModified(long rangeMin, long rangeMax, long actualNumber)
        {
            repetitionNumber++;
            Console.Title = string.Format("Number Guesser | Current Number of Repetition : {0} out of {1}", repetitionNumber, iterate-1);

            int threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Thread[{0}] : Start picking random number", threadId);
            //fileQueue(string.Format("Thread[{0}] : Start picking random number", threadId));

            //concurrentqueue.Enqueue(new string[]{string.Format("Thread[{0}] : Start picking random number", threadId)});
            Random rand = new Random();
            long rangeMinAltered = rangeMin, rangeMaxAltered = rangeMax;
            //int guessedNumber = rand.Next(rangeMin, rangeMax);
            long guessedNumber = RandomNumber(rangeMin, rangeMax);
            //Console.WriteLine(rangeMin);
            //Console.WriteLine(rangeMax);
            Console.WriteLine("Thread[{1}] : Initial guess : {0}", guessedNumber, threadId);
            fileQueue(string.Format("Thread[{1}] : Initial guess : {0}", guessedNumber, threadId));
            //concurrentqueue.Enqueue(new string[] { string.Format("Thread[{1}] : Initial guess : {0}", guessedNumber, threadId) });
            bool numberGuessed = false;
            int counter = 0;
            while (numberGuessed != true)
            {
                if (guessedNumber > actualNumber)
                {
                    Random rand1 = new Random();
                    //Thread.Sleep(2000);
                    Console.WriteLine("Thread[{1}] : In a range of {2} and {3}, {0} seems a bit too high. Choosing a lower number", guessedNumber, threadId, rangeMinAltered, rangeMaxAltered);
                    //concurrentqueue.Enqueue(new string[] { string.Format("Thread[{1}] : In a range of {2} and {3}, {0} seems a bit too high. Choosing a lower number", guessedNumber, threadId, rangeMinAltered, rangeMaxAltered)});
                    rangeMaxAltered = guessedNumber;
                    //guessedNumber = rand1.Next(rangeMinAltered, rangeMaxAltered);
                    guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                    Thread.Sleep(0);
                }
                else
                {
                    if (guessedNumber < actualNumber)
                    {
                        Console.WriteLine("Thread[{1}] : In a range of {2} and {3}, {0} seems a bit too low. Choosing a higher number", guessedNumber, threadId, rangeMinAltered, rangeMaxAltered);
                        //concurrentqueue.Enqueue(new string[] { string.Format("Thread[{1}] : In a range of {2} and {3}, {0} seems a bit too low. Choosing a higher number", guessedNumber, threadId, rangeMinAltered, rangeMaxAltered)});
                        rangeMinAltered = guessedNumber;
                        guessedNumber = RandomNumber(rangeMinAltered, rangeMaxAltered);
                        Thread.Sleep(0);
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

            //concurrentqueue.Enqueue(new string[] { string.Format("Thread[{2}] : Found the number you've chosen which is {0}. Took me {1} tries to guessed it.", guessedNumber, counter, threadId)});
            return counter;
        }

        public static void fileQueue(string input)
        {
            //using (var writer = new StreamWriter("file1.txt"))
            //{
            //    writer.WriteLine(input);
            //}
            
        }
    }
}
