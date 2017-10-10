using System;
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
            int rangeMinimum, rangeMaximum, chosenNumber = 0;
            Console.WriteLine("Please pick the minimum range.");
            int.TryParse(Console.ReadLine(), out rangeMinimum);
            Console.WriteLine("Please pick the maximum range.");
            int.TryParse(Console.ReadLine(), out rangeMaximum);
            Console.WriteLine("Now please pick the number inbetween the range.");
            int.TryParse(Console.ReadLine(), out chosenNumber);
            while (chosenNumber < rangeMinimum || chosenNumber > rangeMaximum)
            {
                Console.WriteLine("Your selection is invalid. Please try insert a new one.");
                int.TryParse(Console.ReadLine(), out chosenNumber);
            }

            guesser(rangeMinimum, rangeMaximum, chosenNumber);
        }

        //Function to get a random number 
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
        public static void guesser(int rangeMin, int rangeMax, int actualNumber)
        {
            Console.WriteLine("Start picking random number");

            Random rand = new Random();
            int rangeMinAltered = rangeMin, rangeMaxAltered = rangeMax;
            //int guessedNumber = rand.Next(rangeMin, rangeMax);
            int guessedNumber = RandomNumber(rangeMin, rangeMax);
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
    }
}
