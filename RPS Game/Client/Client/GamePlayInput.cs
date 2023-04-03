using System;


namespace Client
{
    internal class GamePlayInput
    {
        public static string Choice()
        {
            int choice;
            string input = "";
            bool isValid=false;

            do
            {
                Console.WriteLine("Enter the number of your choice:\n1. Rock\n2. Paper\n3. Scissors\n");
                input = Console.ReadLine();

                choice = Convert.ToInt32(input);

                if (choice == 1)
                {
                    return "Rock";
                }
                else if (choice == 2)
                {
                    return "Paper";
                }
                else if (choice == 3)
                {
                    return "Scissors";
                }
                else
                {
                    Console.WriteLine("Invalid input please try again!\n");
                    isValid = false;
                }
                
            } while (isValid == false);

            return "<EOF>";
        }
    }
}
