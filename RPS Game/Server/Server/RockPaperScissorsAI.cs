using System;

namespace Server
{
    //class for the Rock paper scissors AI and comparison between the client input and the server.
    internal static class RockPaperScissorsAI
    {
        private static string RPSAI()
        {
            //Random number generator to choose a selection for the AI in the server
            Random rand = new Random();
            int randNum = rand.Next(0, 2);

            //Switches between the different options based on the random number
            switch (randNum)
            {
                case 0:
                    return "Rock";
                case 1:
                    return "Paper";
                case 2:
                    return "Scissors";
                default:
                    return "Rock";
            }
        }

        public static string CliWinRPS(string ClientMsg)
        {
            string ServerMSG = RPSAI();

            //Checks if the client won or the server won
            if (ClientMsg.Contains("Scissors") && ServerMSG == "Rock")
            {
                return "You Lose!";
            }
            else if (ClientMsg.Contains("Rock") && ServerMSG == "Paper")
            {
                return "You Lose!";
            }
            else if (ClientMsg.Contains("Paper") && ServerMSG == "Scissors")
            {
                return "You Lose!";
            }
            else if (ClientMsg.Contains("Scissors") && ServerMSG == "Paper")
            {
                return "You Win!";
            }
            else if (ClientMsg.Contains("Paper") && ServerMSG == "Rock")
            {
                return "You Win!";
            }
            else if (ClientMsg.Contains("Rock") && ServerMSG == "Scissors")
            {
                return "You Win!";
            }
            else
            {
                return "It's a Tie!";
            }
        }
    }
}
