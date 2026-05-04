using System;

namespace Minesweeper.Core
{
    public class Board
    {
        private Tile[,] grid;
        private int size;
        private int mines;
        private Random random;

        public int Size => size;

        // sets up the board
        public Board(int size, int mines, int seed)
        {
            this.size = size;
            this.mines = mines;
            random = new Random(seed);

            grid = new Tile[size, size];

            InitializeBoard();
            PlaceMines();
            CalculateAdjacentMines();
        }

        // empty board
        private void InitializeBoard()
        {
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    grid[r, c] = new Tile();
        }

        // place mines
        private void PlaceMines()
        {
            int placed = 0;

            while (placed < mines)
            {
                int r = random.Next(size);
                int c = random.Next(size);

                if (!grid[r, c].IsMine)
                {
                    grid[r, c].IsMine = true;
                    placed++;
                }
            }
        }

        // numbers
        private void CalculateAdjacentMines()
        {
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    if (!grid[r, c].IsMine)
                        grid[r, c].AdjacentMines = CountAdjacentMines(r, c);
        }

        // counts mines around tile
        private int CountAdjacentMines(int r, int c)
        {
            int count = 0;

            for (int i = r - 1; i <= r + 1; i++)
            {
                for (int j = c - 1; j <= c + 1; j++)
                {
                    if (i == r && j == c) continue;

                    if (IsInBounds(i, j) && grid[i, j].IsMine)
                        count++;
                }
            }

            return count;
        }

        // bounds check
        private bool IsInBounds(int r, int c)
        {
            return r >= 0 && r < size && c >= 0 && c < size;
        }

        // reveals tile
        public bool Reveal(int r, int c)
        {
            if (!IsInBounds(r, c)) return true;

            Tile tile = grid[r, c];

            if (tile.IsFlagged || tile.IsRevealed)
                return true;

            tile.IsRevealed = true;

            if (tile.IsMine)
                return false;

            // cascade
            if (tile.AdjacentMines == 0)
            {
                for (int i = r - 1; i <= r + 1; i++)
                    for (int j = c - 1; j <= c + 1; j++)
                        if (IsInBounds(i, j))
                            Reveal(i, j);
            }

            return true;
        }

        // flag
        public void ToggleFlag(int r, int c)
        {
            if (!IsInBounds(r, c)) return;

            Tile tile = grid[r, c];

            if (!tile.IsRevealed)
                tile.IsFlagged = !tile.IsFlagged;
        }

        // checks if user wins
        public bool CheckWin()
        {
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    if (!grid[r, c].IsMine && !grid[r, c].IsRevealed)
                        return false;

            return true;
        }

        // prints board
        public void PrintBoard()
        {
            Console.Write("  ");

            for (int c = 0; c < size; c++)
                Console.Write(c + " ");

            Console.WriteLine();

            for (int r = 0; r < size; r++)
            {
                Console.Write(r + " ");

                for (int c = 0; c < size; c++)
                {
                    Tile t = grid[r, c];

                    if (!t.IsRevealed)
                        Console.Write(t.IsFlagged ? "f " : "# ");
                    else if (t.IsMine)
                        Console.Write("b ");
                    else if (t.AdjacentMines > 0)
                        Console.Write(t.AdjacentMines + " ");
                    else
                        Console.Write(". ");
                }

                Console.WriteLine();
            }
        }
    }

    public class Tile
    {
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
    }
}