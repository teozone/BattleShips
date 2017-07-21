/// <summary>
/// A classic game of Battleships, created according to the specifications in "Battleships Programming Test v4"
/// Created for TechHuddle by Teodor Ivanov
/// </summary>
namespace BattleShips
{
    using System;
    using System.Text.RegularExpressions;

    public class BattleShips
    {
        static void Main(string[] args)
        {
            // First we'll need a 10x10 board to place our ships
            Grid grid = new Grid();

            // Now we're going to create and place our ships on the board
            Ship destroyer1 = new Ship();
            destroyer1.Type = "Destroyer";
            destroyer1.Direction = Ship.GenerateRandomDirection();
            destroyer1.StartingPoint = Ship.GenerateRandomPoint(grid, destroyer1);
            grid = Grid.PlaceShipOnGrid(destroyer1, grid);

            Ship destroyer2 = new Ship();
            destroyer2.Type = "Destroyer";
            destroyer2.Direction = Ship.GenerateRandomDirection();
            destroyer2.StartingPoint = Ship.GenerateRandomPoint(grid, destroyer2);
            grid = Grid.PlaceShipOnGrid(destroyer2, grid);

            Ship battleship1 = new Ship();
            battleship1.Type = "Battleship";
            battleship1.Direction = Ship.GenerateRandomDirection();
            battleship1.StartingPoint = Ship.GenerateRandomPoint(grid, battleship1);
            grid = Grid.PlaceShipOnGrid(battleship1, grid);

            Grid.PrintStartingGrid(grid);

            var totalAttempts = 0;

            while (grid.AllShipsDestroyed == false)
            {
                CallUserInput(grid, destroyer1, destroyer2, battleship1, totalAttempts);
                totalAttempts++;
            }

            Console.ReadLine();
        }

        /// <summary>
        /// This method holds the whole game logic.
        /// It contains hit, miss, sunk and game won actions and feedbacks to the user.
        /// </summary>
        /// <param name="grid">a grid object</param>
        /// <param name="destroyer1">a ship of type destroyer</param>
        /// <param name="destroyer2">a ship of type destroyer</param>
        /// <param name="battleship1">a ship of type battleship</param>
        /// <param name="totalAttempts">the total valid inputs by the user</param>
        /// <returns>an updated board</returns>
        public static Grid CallUserInput(Grid grid, Ship destroyer1, Ship destroyer2, Ship battleship1, int totalAttempts)
        {
            string constantMessage = "Enter coordinates (row, col), e.g. A5";
            string feedbackMiss = "*** Miss ***";
            string feedbackHit = "*** Hit ***";
            string feedbackSunk = "*** Sunk ***";
            string error = "*** Error ***";

            string topMessage = string.Empty;

            bool validInput = false;
            string userInput = null;

            // This While Loop makes sure the user's input is in the correct format - [letter from A to J / number from 1 to 10]
            while (validInput == false)
            {
                userInput = Console.ReadLine();

                if (userInput == "show" || userInput == "hide" || userInput == "reset")
                {
                    EnableCheatMode(userInput, grid);
                    userInput = Console.ReadLine();
                }

                string patternRegex = @"^[A-J](10|[1-9])$";
                Regex inputRegex = new Regex(patternRegex);
                bool validPattern = inputRegex.IsMatch(userInput);

                if (validPattern && userInput.Length <= 3)
                {
                    validInput = true;
                }
                else
                {
                    topMessage = error;
                    Console.Clear();
                    Console.WriteLine(topMessage);
                    Console.WriteLine();
                    Grid.PrintGrid(grid);
                    Console.WriteLine();
                    Console.WriteLine(constantMessage);
                }
            }

            int rowInput = userInput[0] - 65;
            int colInput = userInput.Length == 2 ? int.Parse(userInput[1].ToString()) - 1 : 9;

            // Next conditional statement holds the logic behind a "hit or miss" action.
            // It also holds the condition when the game is won - when all ships are sunk
            if (grid.Board[rowInput, colInput] == '\0' || grid.Board[rowInput, colInput] == '-')        
            {
                grid.Board[rowInput, colInput] = '-';         // Miss logic - grid position is updated with a '-' char on the missed spot
                topMessage = feedbackMiss;                    // Miss logic - feedback               
            }
            else if (grid.Board[rowInput, colInput] == 'H')
            {
                topMessage = feedbackMiss;
            }
            else
            {
                grid.Board[rowInput, colInput] = 'H';         // Hit action - 'H' marks a hit position on the grid

                if (
                        (destroyer1.Sunk == false && Ship.CriticalHit(destroyer1, grid) == true) ||          // sunk ship from current hit condition 
                        (destroyer2.Sunk == false && Ship.CriticalHit(destroyer2, grid) == true) ||          // sunk ship from current hit condition
                        (battleship1.Sunk == false && Ship.CriticalHit(battleship1, grid) == true)           // sunk ship from current hit condition
                   )
                {
                    if (destroyer1.Sunk == false && Ship.CriticalHit(destroyer1, grid) == true)
                    {
                        destroyer1.Sunk = true;
                        topMessage = feedbackSunk;
                    }
                    else if (destroyer2.Sunk == false && Ship.CriticalHit(destroyer2, grid) == true)
                    {
                        destroyer2.Sunk = true;
                        topMessage = feedbackSunk;
                    }
                    else if (battleship1.Sunk == false && Ship.CriticalHit(battleship1, grid) == true)
                    {
                        battleship1.Sunk = true;
                        topMessage = feedbackSunk;
                    }
                }
                else
                {
                    topMessage = feedbackHit;           // Hit without Sink                                                                                  
                }
            }

            if (IsGameWon(destroyer1, destroyer2, battleship1))
            {
                constantMessage = "Well done! You completed the game in " + (totalAttempts + 1) + " shots";            // game won - final message
                topMessage = feedbackSunk;
                grid.AllShipsDestroyed = true;
            }

            Console.Clear();
            Console.WriteLine(topMessage);
            Console.WriteLine();
            Grid.PrintGrid(grid);
            Console.WriteLine();
            Console.WriteLine(constantMessage);

            return grid;
        }

        /// <summary>
        /// Checks if all ships are sunk
        /// </summary>
        /// <param name="destroyer1">a ship of type destroyer</param>
        /// <param name="destroyer2">a ship of type destroyer</param>
        /// <param name="battleship1">a ship of type battleship</param>
        /// <returns>a boolean value - true if game is won, false if not</returns>
        public static bool IsGameWon(Ship destroyer1, Ship destroyer2, Ship battleship1)
        {
            if (destroyer1.Sunk == true && destroyer2.Sunk == true && battleship1.Sunk == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method enables the cheat mode
        /// It shows the exact location of all ships on the grid
        /// </summary>
        /// <param name="inputCommand"> user's input command</param>
        /// <param name="grid">a grid object</param>
        public static void EnableCheatMode(string inputCommand, Grid grid)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();

            Grid.PrintColumns();
            for (int row = 0; row < 10; row++)
            {
                Console.Write((char)(65 + row) + " ");

                for (int col = 0; col < 10; col++)
                {
                    if (grid.Board[row, col] == 'X')
                    {
                        Console.Write('X');
                    }

                    else
                    {
                        Console.Write(' ');
                    }

                    Console.Write(' ');
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Enter coordinates (row, col), e.g. A5");
        }
    }   
}