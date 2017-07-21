using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    /// <summary>
    /// a class, defining a Ship object
    /// </summary>
    public class Ship
    {
        public string Direction { get; set; }

        public Point StartingPoint { get; set; }

        public string Type { get; set; }      // type Destroyer or Battleship

        public bool Sunk { get; set; }

        public int Size
        {
            get
            {
                if (Type == "Destroyer") { return 4; }

                else if (Type == "Battleship") { return 5; }

                else { throw new Exception("Invalid Ship Type"); }
            }
        }

        /// <summary>
        /// Generates the direction of the ship at a random principle
        /// </summary>
        /// <returns>
        /// a string with one of the two values - "Horizontal" or "Vertical"
        /// </returns>
        public static string GenerateRandomDirection()
        {
            string[] directions = new string[] { "Vertical", "Horizontal" };

            Random random = new Random();
            double index = random.NextDouble();
            var direction = random.Next(2) < 0.5f ? directions[0] : directions[1];
            return direction;
        }

        /// <summary>
        /// Generates a Point object with randomly generated coordinates
        /// Checks if there's any overlap with any other ship
        /// </summary>
        /// <param name="grid">an object of the type Grid</param>
        /// <param name="ship">an object of the type Ship</param>
        /// <returns>a Point object with a row and column values</returns>
        public static Point GenerateRandomPoint(Grid grid, Ship ship)
        {
            Random random = new Random();

            Point randomPoint = new Point();
            int randomColumn;
            int randomRow;

            if (ship.Direction == "Horizontal")
            {
                randomColumn = random.Next(0, 9 - ship.Size);
                randomRow = random.Next(0, 9);
            }

            else
            {
                randomColumn = random.Next(0, 9);
                randomRow = random.Next(0, 9 - ship.Size);
            }

            bool noOverlap = true;

            switch (ship.Direction)
            {
                case "Horizontal":

                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (grid.Board[randomRow, randomColumn + i] != 0)
                        {
                            noOverlap = false;
                        }
                    }

                    break;

                case "Vertical":

                    for (int i = 0; i < ship.Size; i++)
                    {
                        if (grid.Board[randomRow + i, randomColumn] != 0)
                        {
                            noOverlap = false;
                        }
                    }
                    break;

                default:
                    break;
            }

            if (noOverlap)
            {
                randomPoint.Column = randomColumn;
                randomPoint.Row = randomRow;
                return randomPoint;
            }
            else
            {
                return GenerateRandomPoint(grid, ship);         // recursion, until the ship finds a "free spot" on the board
            }
        }

        /// <summary>
        /// This method checks if the hit is critical, meaning did this hit sunk the ship?
        /// </summary>
        /// <param name="ship">a Ship object of any type</param>
        /// <param name="grid">a Grid Object</param>
        /// <returns>a boolean statement, whether this hit caused a sink or not</returns>
        public static bool CriticalHit(Ship ship, Grid grid)
        {
            bool criticalHit = true;

            // If any of the ship's positions on the grid are not marked with an 'H' IsSunk will become false  
            switch (ship.Direction)
            {
                case "Horizontal":

                    for (int i = 0; i <= ship.Size - 1; i++)
                    {
                        if (grid.Board[ship.StartingPoint.Row, ship.StartingPoint.Column + i] != 'H')
                        {
                            criticalHit = false;
                            return criticalHit;
                        }
                    }
                    break;

                case "Vertical":

                    for (int i = 0; i <= ship.Size - 1; i++)
                    {
                        if (grid.Board[ship.StartingPoint.Row + i, ship.StartingPoint.Column] != 'H')
                        {
                            criticalHit = false;
                            return criticalHit;
                        }
                    }
                    break;

                default:
                    break;
            }

            return criticalHit;
        }
    }
}
