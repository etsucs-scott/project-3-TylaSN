using System;
using Minesweeper.Core;


    class Program
{
    // Main method to run the game
    static void Main(string[] args)
    {
        while (true)
        {
            // Menu for selecting board size
            Console.WriteLine("Menu:\n1) 8x8\n2) 12x12\n3) 16x16\nChoose a board size: ");
            string choice = Console.ReadLine();

            int size = 0;
            int mines = 0;

            // choices for board size and mines based on user input
            if (choice == "1")
            {
                size = 8;
                mines = 10;
            }
            else if (choice == "2")
            {
                size = 12;
                mines = 25;
            }
            else if (choice == "3")
            {
                size = 16;
                mines = 40;
            }
            else
            {
                //Statement for invalid input
                Console.WriteLine("Invalid choice. Please select a valid board size.");
                continue;
            }

            // creates the board

            Board board = new Board(size, mines);
            
            // Main game loop
            while (true)
            {
                //prints board
                board.PrintBoard();

                // User input for contunueing the game/ quiting the game
                Console.Write("Type r to reveal (example: r 2 3), or q ro quit: ");
                string input = Console.ReadLine() ?? "";
                if (input == "q")
                    break;

                string[] parts = input.Split(' ');

                if (parts.Length != 3 || parts[0] != "r")
                {
                    Console.WriteLine("Invalid input. ");
                    continue;
                }

                int row = int.Parse(parts[1]);
                int col = int.Parse(parts[2]);

                bool safe = board.Reveal(row, col);

                if (!safe)
                {
                    Console.WriteLine("You hit a mine! Game over.");
                    board.PrintBoard();
                    break;
                }

            }

            
            break;

        }
    }
}