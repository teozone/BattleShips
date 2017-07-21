using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    /// <summary>
    /// A class for the grid object and 
    /// </summary>
    public class Grid
    {
        public char[,] Board = new char[10, 10];         // jagged array, representing the 10x10 field

        public bool AllShipsDestroyed { get; set; }

        /// <summary>
        /// prints the column numbers of the grid
        /// </summary>
        public static void PrintColumns()
        {
            var columnNumber = new int[10];

            for (int i = 0; i < 10; i++)
            {
                columnNumber[i] = i + 1;
            }

            Console.Write("  ");
            Console.WriteLine(string.Join(" ", columnNumber));
        }

        /// <summary>
        /// Prints the entire Grid at the start of the game with all the column numbers and row letters
        /// </summary>
        public static void PrintStartingGrid(Grid grid)
        {
            Console.WriteLine();
            Console.WriteLine();

            Grid.PrintGrid(grid);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Enter coordinates (row, col), e.g. A5");
        }

        /// <summary>
        /// This Method prints the current grid
        /// </summary>
        /// <param name="grid">a Grid object</param>
        public static void PrintGrid(Grid grid)
        {
            Grid.PrintColumns();

            for (int row = 0; row < 10; row++)
            {
                Console.Write((char)(65 + row) + " ");
                for (int col = 0; col < 10; col++)
                {
                    if (grid.Board[row, col] == 'X' || grid.Board[row, col] == '\0')
                    {
                        Console.Write('.');
                    }
                    else if (grid.Board[row, col] == 'H')
                    {
                        Console.Write('X');
                    }
                    else
                    {
                        Console.Write(grid.Board[row, col]);
                    }
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// This method places a ship on the grid matrix
        /// The logic behind this method is that unoccupied positions on the board are marked with a char \0 (empty array position) and ship positions are marked with a char 'X'
        /// </summary>
        /// <param name="ship">a Ship object of any kind</param>
        /// <param name="grid">a Grid object</param>
        public static Grid PlaceShipOnGrid(Ship ship, Grid grid)
        {
            switch (ship.Direction)
            {
                case "Horizontal":
                    switch (ship.Type)
                    {
                        case "Destroyer":
                            for (int i = 0; i < 4; i++)
                            {
                                grid.Board[ship.StartingPoint.Row, ship.StartingPoint.Column + i] = 'X';
                            }
                            break;
                        case "Battleship":
                            for (int i = 0; i < 5; i++)
                            {
                                grid.Board[ship.StartingPoint.Row, ship.StartingPoint.Column + i] = 'X';
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                case "Vertical":
                    switch (ship.Type)
                    {
                        case "Destroyer":
                            for (int i = 0; i < 4; i++)
                            {
                                grid.Board[ship.StartingPoint.Row + i, ship.StartingPoint.Column] = 'X';
                            }
                            break;
                        case "Battleship":
                            for (int i = 0; i < 5; i++)
                            {
                                grid.Board[ship.StartingPoint.Row + i, ship.StartingPoint.Column] = 'X';
                            }
                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            return grid;
        }
    }
}
