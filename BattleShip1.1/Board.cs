using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip1._1
{
    public class Board
    {
        private char[,] grid;

        // Constructor to create a 10x10 board and initializes all cells to water (~)

        public Board()
        {
            grid = new char[10, 10];
            InitializeBoard();
        }

        // Sets all board cells to '~' for water at the start of the game
        private void InitializeBoard()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    grid[i, j] = '~';
        }
        // Displays the board in the console as a grid from A-J and 0-9
        // If hideShips is true, it hides ship locations ('S')
        public void Display(bool hideShips = false)
        {
            Console.Write("   ");
            for (char c = 'A'; c <= 'J'; c++)
                Console.Write($"{c} ");
            Console.WriteLine();

            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i}  ");
                for (int j = 0; j < 10; j++)
                {
                    char cell = grid[i, j];

                    // Hide ships from view
                    if (hideShips && (cell == 'S'|| cell == 'C'||cell == 'D'))
                        Console.Write("~ ");
                    else
                        Console.Write($"{cell} ");
                }
                Console.WriteLine();
            }
        }
        // Checks to ensure that the ship will be on board
        public bool IsWithinBounds(List<(int, int)> coords)
        {
            foreach (var (row, col) in coords)
            {
                if (row < 0 || row >= 10 || col < 0 || col >= 10)
                    return false;
            }
            return true;
        }
        // Checks to ensure that the ship will not overlap with another ship
        public bool IsOverlapping(List<(int, int)> coords)
        {
            foreach (var (row, col) in coords)
            {
                if (grid[row, col] != '~')  // Not water = occupied
                    return true;
            }
            return false;
        }
        // Adds the ship to the board by marking its coordinates with its symbol ('S', 'C', 'D')
        public void AddShip(Ship ship)
        {
            foreach (var (row, col) in ship.Coordinates)
            {
                grid[row, col] = ship.Symbol;
            }
        }
        // Marks a shot on the board, 'X' for hit and 'O' for miss
        public void MarkShot(int row, int col)
        {
            if (grid[row, col] == 'S' || grid[row, col] == 'C' || grid[row, col] == 'D')
                grid[row, col] = 'X'; // Hit
            else if (grid[row, col] == '~')
                grid[row, col] = 'O'; // Miss
        }

        // Check if coordinates have already been shot at
        public bool HasBeenShot(int row, int col)
        {
            return grid[row, col] == 'X' || grid[row, col] == 'O';
        }

        // Determine if a shot is a hit, and mark it
        public bool ReceiveShot(int row, int col)
        {
            if (grid[row, col] == 'S' || grid[row, col] == 'C' || grid[row, col] == 'D')
            {
                grid[row, col] = 'X'; // Mark hit
                return true;
            }
            else if (grid[row, col] == '~')
            {
                grid[row, col] = 'O'; // Mark miss
                return false;
            }

            return false; // Already shot
        }

        // Check if all ships have been sunk
        public bool AllShipsSunk()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (grid[i, j] == 'S' || grid[i, j] == 'C' || grid[i, j] == 'D')
                        return false; // At least one part of a ship is still unhit
                }
            }
            return true;
        }

        // Check if cell is on the board (safety utility)
        public bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < 10 && col >= 0 && col < 10;
        }
    }
}
