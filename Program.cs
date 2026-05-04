using System;
using Minesweeper.Core;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            // menu
            Console.WriteLine("Menu:");
            Console.WriteLine("1) 8x8");
            Console.WriteLine("2) 12x12");
            Console.WriteLine("3) 16x16");
            Console.Write("Choose a board size: ");

            string choice = Console.ReadLine();

            int size;
            int mines;

            // board settings
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
                Console.WriteLine("Invalid choice.\n");
                continue;
            }

            // seed (level number)
            Console.Write("Enter level number (or Enter for random): ");
            string input = Console.ReadLine();

            int seed;

            if (string.IsNullOrWhiteSpace(input))
                seed = Environment.TickCount;
            else if (!int.TryParse(input, out seed))
            {
                Console.WriteLine("Invalid level number.");
                seed = Environment.TickCount;
            }

            Console.WriteLine($"Level: {seed}");

            Board board = new Board(size, mines, seed);

            int moves = 0;
            DateTime startTime = DateTime.Now;

            // game loop
            while (true)
            {
                board.PrintBoard();

                Console.Write("Choice (r # #/ f # #):");
                string command = Console.ReadLine() ?? "";

                if (command == "q")
                    break;

                string[] parts = command.Split(' ');

                if (parts.Length != 3)
                {
                    Console.WriteLine("invalid input: (Format: r # #/ f # #) ");
                    continue;
                }

                if (!int.TryParse(parts[1], out int row) ||
                    !int.TryParse(parts[2], out int col))
                {
                    Console.WriteLine("Bad coords");
                    continue;
                }

                // reveal
                if (parts[0] == "r")
                {
                    moves++;

                    if (!board.Reveal(row, col))
                    {
                        Console.WriteLine("Boom! GAME OVER!!");
                        board.PrintBoard();
                        break;
                    }

                    if (board.CheckWin())
                    {
                        Console.WriteLine("You win");
                        break;
                    }
                }
                // flag
                else if (parts[0] == "f")
                {
                    moves++;
                    board.ToggleFlag(row, col);
                }
            }

            Console.WriteLine("Back to menu\n");
        }
    }
}