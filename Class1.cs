using System;
using System.Security.Cryptography.X509Certificates;

namespace Minesweeper.Core
{

    public class Board
    {
        // 2D array representing the grid of tiles
        private Tile[,] grid;
        private int size;
        private int mines;
        private Random random = new Random();

        public int Size => size;

        // constructor that initializes the board with the specified size and number of mines
        public Board(int size, int mines)
        {
            // validate size and mine count
            if (size != 8 && size != 12 && size != 16)
            {
                throw new ArgumentException("Invalid board size. Allowed sizes are 8, 12, or 16.");
            }
            this.size = size;
            this.mines = mines;

            grid = new Tile[size, size];

            // initialize the board and place mines
            InitializeBoard();
            PlaceMines();
            CalculateAdjacentMines();
        }

        //creates board and initializes each tile
        private void InitializeBoard()
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = new Tile();
                }
            }
        }

        private void PlaceMines()
        {
            int placedMines = 0;

            //loop for mine placement

            while (placedMines < mines)
            {
                int x = random.Next(size);
                int y = random.Next(size);

                if (!grid[x, y].IsMine)
                {
                    grid[x, y].IsMine = true;
                    placedMines++;
                }
            }
        }

        // calculates the number of adjacent mines for each tiles on board
        private void CalculateAdjacentMines()
        {
            // loop through each tile and count adjacent mines
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (grid[x, y].IsMine)
                    {
                        grid[x, y].AdjacentMines = CountAdjacentMines(x, y);
                    }
                }
            }


        }

        // counts the number of mines adjacent to a given tile
        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;

            // loop through the 3x3 area around the tile
            for (int x = row - 1; x <= row + 1; x++)
            {
                for (int y = col - 1; y <= col + 1; y++)
                {
                    if (x == row && y == col)
                        continue;
                    if (IsInBounds(x, y) && grid[x, y].IsMine)
                        count++;
                }
            }
            return count;
        }

       
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < size && y >= 0 && y < size;
        }

        // method to get the tile at a specific position
        public Tile GetTile(int x, int y)
        {
            return grid[x, y];
        }

        // class for the tiles on reach board
        public class Tile
        {
            public bool IsMine { get; set; }
            public int AdjacentMines { get; set; }

            public bool IsRevealed { get; set; }
            public bool IsFlagged { get; set; }
        }

        // method to print the board to the console
        public void PrintBoard()
        {
            Console.Write(" ");
            for (int y = 0; y < size; y++)
                Console.WriteLine(y + " ");
            Console.WriteLine();

            for (int x = 0; x < size; x++)
            {
                Console.Write(x + " ");
                for (int y = 0; y < size; y++)
                {
                    if (grid[x, y].IsMine)
                        Console.Write("# ");
                }
                Console.WriteLine();
            }
        }

        public bool Reveal(int r, int c)
        {
            // stops if out of bounds
            if (!IsInBounds(r, c))
                return false;

            Tile tile = grid[r, c];

            // already revealed or flagged
            if (tile.IsFlagged || tile.IsRevealed)
                return true;

            tile.IsRevealed = true;

            // game ends after hitting a mine
            if (tile.IsMine)
                return false;

            
            if (tile.AdjacentMines == 0)
            {
                for (int x = r - 1; x <= r + 1; x++)
                {
                    for (int y = c - 1; y <= c + 1; y++)
                    {
                        if (IsInBounds(x, y))
                            Reveal(x, y);
                    }
                }
            }

            return true;
        }


    }
}



