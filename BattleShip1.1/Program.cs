namespace BattleShip1._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleship!");

            // Step 1: Initialize the board and ship placer
            Board playerBoard = new Board();
            ShipPlacer shipPlacer = new ShipPlacer(playerBoard);

            // Step 2: Show empty board
            Console.WriteLine("\nYour board:");
            playerBoard.Display();

            // Step 3: Place all 3 ships
            shipPlacer.PlaceShip("Destroyer");
            playerBoard.Display();

            shipPlacer.PlaceShip("Submarine");
            playerBoard.Display();

            shipPlacer.PlaceShip("Cruiser");
            playerBoard.Display();

            // Step 4: Show final board
            Console.WriteLine("\nAll ships placed! Final board:");
            playerBoard.Display(hideShips: false);

            // Computer places ships
            Board enemyBoard = new Board();
            ShipPlacer enemyPlacer = new ShipPlacer(enemyBoard);

            // Enemy randomly places all 3 ships
            enemyPlacer.PlaceShipRandomly("Destroyer");
            enemyPlacer.PlaceShipRandomly("Submarine");
            enemyPlacer.PlaceShipRandomly("Cruiser");

            Console.WriteLine("\nEnemy board (hidden):");
            enemyBoard.Display(hideShips: true);

            // === Game Loop ===
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Your Board:");
                playerBoard.Display();

                Console.WriteLine("\nEnemy Board:");
                enemyBoard.Display(hideShips: true);

                // --- Player Turn ---
                bool playerGetsAnotherTurn = true;
                while (playerGetsAnotherTurn)
                {
                    Console.WriteLine("\nYour turn to fire!");
                    int targetRow, targetCol;

                    while (true)
                    {
                        Console.Write("Enter row (0-9): ");
                        if (!int.TryParse(Console.ReadLine(), out targetRow) || targetRow < 0 || targetRow > 9)
                            continue;

                        Console.Write("Enter column (0-9): ");
                        if (!int.TryParse(Console.ReadLine(), out targetCol) || targetCol < 0 || targetCol > 9)
                            continue;

                        if (enemyBoard.HasBeenShot(targetRow, targetCol))
                        {
                            Console.WriteLine("You've already fired at that location. Try again.");
                            continue;
                        }

                        break;
                    }

                    bool playerHit = enemyBoard.ReceiveShot(targetRow, targetCol);
                    Console.WriteLine(playerHit ? "Hit! You get another shot!" : "Miss!");
                    Console.ReadKey();

                    if (enemyBoard.AllShipsSunk())
                    {
                        Console.WriteLine("You sunk all enemy ships! You win!");
                        return; // Exit the game
                    }

                    // If miss, enemy's turn
                    playerGetsAnotherTurn = playerHit;
                }

                // --- Enemy Turn(s) ---
                bool enemyGetsAnotherTurn = true;
                while (enemyGetsAnotherTurn)
                {
                    Random rand = new Random();
                    int enemyRow, enemyCol;

                    do
                    {
                        enemyRow = rand.Next(0, 10);
                        enemyCol = rand.Next(0, 10);
                    } while (playerBoard.HasBeenShot(enemyRow, enemyCol));

                    bool enemyHit = playerBoard.ReceiveShot(enemyRow, enemyCol);
                    Console.WriteLine($"\nEnemy fires at ({enemyRow}, {enemyCol}) and it's a {(enemyHit ? "hit! Enemy gets another shot." : "miss.")}");
                    Console.ReadKey();

                    if (playerBoard.AllShipsSunk())
                    {
                        Console.WriteLine("The enemy sunk all your ships. You lose.");
                        return; // Exit the game
                    }

                    // If miss, return to player's turn
                    enemyGetsAnotherTurn = enemyHit;
                }
            }
        }
    }
}
