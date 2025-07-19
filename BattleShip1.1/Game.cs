using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip1._1
{
    public class Game
    {
        private Board playerBoard;
        private Board enemyBoard;
        private ShipPlacer playerPlacer;
        private ShipPlacer enemyPlacer;
        private HashSet<(int, int)> botShots;

        public Game()
        {
            playerBoard = new Board();
            enemyBoard = new Board();
            playerPlacer = new ShipPlacer(playerBoard);
            enemyPlacer = new ShipPlacer(enemyBoard);
            botShots = new HashSet<(int, int)>();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Battleship!\n");

            // Place ships
            playerPlacer.PlaceShip("Destroyer");
            playerPlacer.PlaceShip("Submarine");
            playerPlacer.PlaceShip("Cruiser");

            enemyPlacer.PlaceShipRandomly("Destroyer");
            enemyPlacer.PlaceShipRandomly("Submarine");
            enemyPlacer.PlaceShipRandomly("Cruiser");

            Console.Clear();
            Console.WriteLine("Game starts!\n");

            bool playerTurn = true;

            while (true)
            {
                if (enemyBoard.AllShipsSunk())
                {
                    Console.WriteLine("You won! All enemy ships have been destroyed.");
                    break;
                }

                if (playerBoard.AllShipsSunk())
                {
                    Console.WriteLine(" Game Over! The enemy has destroyed all your ships.");
                    break;
                }

                if (playerTurn)
                {
                    enemyBoard.Display(hideShips: true);
                    Console.WriteLine("\nYour turn to shoot!");

                    bool hit = PlayerTurn();

                    if (!hit)
                        playerTurn = false;
                }
                else
                {
                    Console.WriteLine("\nEnemy's turn...");
                    System.Threading.Thread.Sleep(1000);

                    bool hit = BotTurn();

                    if (!hit)
                        playerTurn = true;
                }
            }
        }

        private bool PlayerTurn()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter target row (0-9): ");
                    int row = int.Parse(Console.ReadLine());

                    Console.Write("Enter target column (0-9): ");
                    int col = int.Parse(Console.ReadLine());

                    if (!enemyBoard.IsInBounds(row, col))
                    {
                        Console.WriteLine("Target out of bounds!");
                        continue;
                    }

                    if (enemyBoard.HasBeenShot(row, col))
                    {
                        Console.WriteLine("You already shot there!");
                        continue;
                    }

                    bool hit = enemyBoard.ReceiveShot(row, col);
                    Console.WriteLine(hit ? "Hit!" : "Miss!");
                    return hit;
                }
                catch
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }

        private bool BotTurn()
        {
            Random rand = new Random();

            while (true)
            {
                int row = rand.Next(0, 10);
                int col = rand.Next(0, 10);

                if (botShots.Contains((row, col)))
                    continue;

                botShots.Add((row, col));
                bool hit = playerBoard.ReceiveShot(row, col);

                Console.WriteLine($"Enemy fires at ({row}, {col}) - {(hit ? "Hit!" : "Miss.")}");
                return hit;
            }
        }
    }
}

