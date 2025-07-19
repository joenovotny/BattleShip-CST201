using System;

public class ShipPlacer
{
    private Board board;

    public ShipPlacer(Board board)
    {
        this.board = board;
        placedShips = new HashSet<string>();
    }
    /* 
     the below method handles the placement of the ships
     it locks in the pattern of how the user can place the ship
     all the user needs to do is determin the oriantation and
     the grid where the top left most part of the ship should go.
    */
    public void PlaceShip(string name)
    {

        // Prevent placing the same ship twice
        if (placedShips.Contains(name.ToLower()))
        {
            Console.WriteLine($"The {name} has already been placed!");
            return;
        }

        while (true)
        {
            Console.WriteLine($"\nPlacing {name}...");

            try
            {
                Console.Write("Enter starting row (0-9): ");
                int row = int.Parse(Console.ReadLine());

                Console.Write("Enter starting column (0-9): ");
                int col = int.Parse(Console.ReadLine());

                List<(int, int)> coords = new List<(int, int)>();

                switch (name.ToLower())
                {
                    case "destroyer":
                        coords.Add((row, col));
                        coords.Add((row + 1, col));
                        coords.Add((row, col + 1));
                        coords.Add((row + 1, col + 1));
                        break;

                    case "submarine":
                        Console.Write("Diagonal direction (\\ or /): ");
                        string diagDir = Console.ReadLine();
                        if (diagDir == "\\")
                        {
                            coords.Add((row, col));
                            coords.Add((row + 1, col + 1));
                            coords.Add((row + 2, col + 2));
                        }
                        else if (diagDir == "/")
                        {
                            coords.Add((row, col));
                            coords.Add((row + 1, col - 1));
                            coords.Add((row + 2, col - 2));
                        }
                        else
                        {
                            Console.WriteLine("Invalid diagonal direction.");
                            continue;
                        }
                        break;

                    case "cruiser":
                        Console.Write("Orientation (H for horizontal, V for vertical): ");
                        string orientation = Console.ReadLine().ToUpper();
                        if (orientation == "H")
                        {
                            coords.Add((row, col));
                            coords.Add((row, col + 1));
                            coords.Add((row, col + 2));
                        }
                        else if (orientation == "V")
                        {
                            coords.Add((row, col));
                            coords.Add((row + 1, col));
                            coords.Add((row + 2, col));
                        }
                        else
                        {
                            Console.WriteLine("Invalid orientation.");
                            continue;
                        }
                        break;

                        //this is where the error catches start, the console message should tell you what error it is for.
                    default:
                        Console.WriteLine("Unknown ship type.");
                        return;
                }

                if (!board.IsWithinBounds(coords))
                {
                    Console.WriteLine("Error: Ship out of bounds. Try again.");
                    continue;
                }

                if (board.IsOverlapping(coords))
                {
                    Console.WriteLine("Error: Ship overlaps with another ship. Try again.");
                    continue;
                }

                char symbol = name[0]; // we could do something like this for the ship charactors D, S, or C
                Ship newShip = new Ship(name, symbol, coords);
                board.AddShip(newShip);

                Console.WriteLine($"{name} placed at: ");
                foreach (var coord in coords)
                    Console.WriteLine($"({coord.Item1}, {coord.Item2})");

                break; // ship successfully placed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid input: {ex.Message}");
            }
        }
    }
}
