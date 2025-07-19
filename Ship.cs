using System;

public class Ship
{
    public string Name { get; set; }
    public char Symbol { get; set; }
    public List<(int Row, int Col)> Coordinates { get; set; }

    public Ship(string name, char symbol, List<(int Row, int Col)> coordinates)
    {
        Name = name;
        Symbol = symbol;
        Coordinates = coordinates;
    }

    // this can be used to check game over condition if all ships return true for this method then all ships are sunk
    public bool IsSunk(HashSet<(int, int)> hits)
    {
        foreach (var coord in Coordinates)
        {
            if (!hits.Contains(coord))
                return false;
        }
        return true;
    }
}
