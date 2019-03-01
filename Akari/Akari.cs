using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public abstract class Akari
    {
        static void Main(string[] args)
        {

            /// TODO first level is hardcoded and must implement remaining levels
            CellType[,] levelOneLayout = new CellType[,]
                {
                {CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_1, CellType.BLACK, CellType.WHITE, CellType.WHITE, CellType.WHITE},
                {CellType.WHITE, CellType.BLACK, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_1, CellType.WHITE},
                {CellType.WHITE, CellType.WHITE, CellType.BLACK_1, CellType.WHITE, CellType.WHITE, CellType.BLACK, CellType.WHITE, CellType.WHITE},
                {CellType.BLACK, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_0},
                {CellType.BLACK, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_1},
                {CellType.WHITE, CellType.WHITE, CellType.BLACK_2, CellType.WHITE, CellType.WHITE, CellType.BLACK, CellType.WHITE, CellType.WHITE},
                {CellType.WHITE, CellType.BLACK_1, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_2, CellType.WHITE},
                {CellType.WHITE, CellType.WHITE, CellType.WHITE, CellType.BLACK_1, CellType.BLACK, CellType.WHITE, CellType.WHITE, CellType.WHITE}
                };

            Level levelOne = new Level(8, 8, levelOneLayout);
            GameManager.instance.LevelList[0] = levelOne;
            /////////////////////////////////////////////////////
            

            bool programIsOpened = true; /// controls the game loop
            bool gameOver = true;

            GameManager.DrawIntro();

            while (programIsOpened)
            {
                if (gameOver)
                {
                    PlayMainMenuScene(ref gameOver, ref programIsOpened);
                } else
                {
                    PlayGameScene(ref gameOver);
                }
            }
        }

        public static void PlayGameScene(ref bool gameOver)
        {
            Console.Clear();
            Console.WriteLine("Level: " + GameManager.instance.CurrentLevelIndex+1+"\n");
            GameManager.DrawGrid();
            /// Developer tool: Lightmap
            //Console.Write("\n\n");
            //GameManager.DrawLightmap();
            Console.Write("\n\n");
            Console.WriteLine("1- Place/remove light bulb.\n2- Try solution.\n3- Return to main menu.");
            string input = Console.ReadLine();

            try
            {
                 switch (Convert.ToInt32(input))
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Level: " + GameManager.instance.CurrentLevelIndex + 1 + "\n");
                        GameManager.DrawGrid();
                        Console.WriteLine("\nPlease indicate row.");
                        input = Console.ReadLine();
                        int row = Convert.ToInt32(input) - 1;
                        Console.WriteLine("\nPlease indicate column.");
                        input = Console.ReadLine();
                        int column = Convert.ToInt32(input) - 1;

                        GameManager.ToggleLight(row, column);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Level: " + GameManager.instance.CurrentLevelIndex + 1 + "\n");
                        GameManager.DrawGrid();
                        /*
                        Console.Write("\n\n");
                        GameManager.DrawLightmap();
                        */
                        Console.Write("\n\n");
                        Utilities.TypeWrite("Analizing your solution.", 0.030f);
                        Utilities.TypeWrite(". . .", 0.3f);
                        if (GameManager.CheckSolution())
                        {
                            Console.WriteLine("\nCongratulations!");
                            Utilities.WaitForSeconds(1.5f);
                            Console.WriteLine("Level completed.");
                            GameManager.instance.CurrentLevel.Completed = true;
                            Utilities.WaitForSeconds(1.5f);
                            Console.WriteLine("Press any key to return to main menu.");
                            Console.ReadKey();
                            gameOver = true;
                        }
                        else
                        {
                            Console.WriteLine("\nWrong answer. Try again.");
                            Utilities.WaitForSeconds(2);
                        }
                        break;
                    case 3:
                        Console.WriteLine("\nYour progress will not be saved. Press Y to proceed or any other key to cancel.");
                        var key = Console.ReadKey();
                        if (key.Key == System.ConsoleKey.Y)
                        {
                            gameOver = true;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid command. Try again.");
                        Utilities.WaitForSeconds(1.5f);
                        break;
                }
            }
            catch (FormatException ex) /// will catch any invalid input when requires to enter a number
            {
                Console.WriteLine("Invalid input. Try again.");
                Utilities.WaitForSeconds(1.5f);
            }
            catch (IndexOutOfRangeException ex) /// will catch any out-of-boundary coordinate
            {
                Console.WriteLine("Invalid coordinates. Try again.");
                Utilities.WaitForSeconds(1.5f);
            }
        }

        public static void PlayMainMenuScene(ref bool gameOver, ref bool programIsOpened)
        {
            Console.Clear();
            Utilities.VerticalWrite(GameManager.title1, 0.030f);
            Utilities.WaitForSeconds(1);
            Console.WriteLine("1- New game.\n2- How to play.\n3- Quit.");
            string input = Console.ReadLine();
            try
            {
                switch (Convert.ToInt32(input))
                {
                    case 1:
                        try
                        {
                            Console.Clear();
                            Console.WriteLine("Select level (1-500)");
                            GameManager.instance.CurrentLevelIndex = Convert.ToInt32(Console.ReadLine()) - 1;
                            GameManager.LoadLevel(GameManager.instance.LevelList[GameManager.instance.CurrentLevelIndex]);
                            gameOver = false;
                        } catch (NullReferenceException ex)
                        {
                            Console.WriteLine("Further levels have not been implemented yet. Thank you for playing!");
                            Utilities.WaitForSeconds(2.5f);
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("RULE 1: Place light bulbs in some of the white cells so that all white cells are lit.");
                        Console.WriteLine("RULE 2: A light bulb shines horizontally and vertically up to the next black cell or the edge of the grid.");
                        Console.WriteLine("RULE 3: No light bulb must be lit by any other light bulb.");            
                        Console.WriteLine("RULE 4: A number in a black cell indicates how many light bulbs must be placed in orthogonally adjacent cells.");

                        Utilities.TypeWrite("Press any key to see an example.", 0.030f);
                        Console.ReadKey();
                        Console.WriteLine("\nExample:");
                        Console.WriteLine(@"| | | |1|n| | | |
| |n| | | | |1| |
| | |1| | |n| | |
|n| | | | | | |0|
|n| | | | | | |1|
| | |2| | |n| | |
| |1| | | | |2| |
| | | |1|n| | | |");
                        Utilities.TypeWrite("Press any key to read more.", 0.030f);
                        Console.ReadKey();
                        Console.WriteLine("\nReferences: | | = White cell; |+| White cell with light bulb; |1| = Black cell with 1 adjacent light bulb");
                        Console.WriteLine("|2| = Black cell with 2 adjacents light bulbs; |3| = Black cell with 3 adjacents light bulbs");
                        Console.WriteLine("|4| = Black cell with 4 adjacents light bulbs; |n| = Black cell with n number (any number) of adjacent light bulbs.");
                        Utilities.TypeWrite("Press any key to see the solution.", 0.030f);
                        Console.ReadKey();
                        Console.WriteLine("\nSolution:");
                        
                        Console.WriteLine(@"| | |+|1|n| | |+|
|+|n| | |+| |1| |
| |+|1| | |n|+| |
|n| | | | |+| |0|
|n| |+| | | | |1|
| | |2|+| |n| |+|
|+|1| | | |+|2| |
| | |+|1|n| |+| |");
                        Utilities.TypeWrite("Press any key to read more.", 0.030f);
                        Console.ReadKey();
                        /*
                        Console.WriteLine("PS: Optionally, you may activate the lightmap from the options menu.\nThe lightmap shows you which cells are lit and which are not.\nIt refreshes after trying out a solution.\nIt's a beginner-friendly feature.");
                        Utilities.TypeWrite("\n\nPress any key to read more.", 0.030f);
                        Console.ReadKey();
                        */
                        Console.WriteLine("You can also find the rules at https://www.janko.at/Raetsel/Akari/index.htm");
                        Utilities.TypeWrite("\n\nPress any key to return.", 0.030f);
                        Console.ReadKey();
                        break;
                    case 3:
                        programIsOpened = false;
                        break;
                    default:

                        break;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid input. Try again.");
                Utilities.WaitForSeconds(1.5f);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Invalid input. Try again.");
                Utilities.WaitForSeconds(1.5f);
            }
        }
    }
}
