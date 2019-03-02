using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class GameManager
    {
        /// Singleton
        public static GameManager instance = new GameManager();
        public Cell[,] Grid { get; set; }
        public int[,] LightMap { get; set; }
        public Level CurrentLevel { get; set; }
        public int CurrentLevelIndex { get; set; }
        public Level[] LevelList { get; set; }

        public static string[] title1 = new string[] {@" .----------------.  .----------------.  .----------------.  .----------------.  .----------------. ",
@"| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |",
@"| |      __      | || |  ___  ____   | || |      __      | || |  _______     | || |     _____    | |",
@"| |     /  \     | || | |_  ||_  _|  | || |     /  \     | || | |_   __ \    | || |    |_   _|   | |",
@"| |    / /\ \    | || |   | |_/ /    | || |    / /\ \    | || |   | |__) |   | || |      | |     | |",
@"| |   / ____ \   | || |   |  __'.    | || |   / ____ \   | || |   |  __ /    | || |      | |     | |",
@"| | _/ /    \ \_ | || |  _| |  \ \_  | || | _/ /    \ \_ | || |  _| |  \ \_  | || |     _| |_    | |",
@"| ||____|  |____|| || | |____||____| | || ||____|  |____|| || | |____| |___| | || |    |_____|   | |",
@"| |              | || |              | || |              | || |              | || |              | |",
@"| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |",
@" '----------------'  '----------------'  '----------------'  '----------------'  '----------------' ",
"v0.1.1-alpha\n\n"
        }; // ASCII Art font is "Blocks"
        public static string text1 = "Programmed by Miro";
        public static string text2 = "All rights reserved to Nikoli Co., Ltd..";

        private GameManager()
        {
            LevelList = new Level[500];
        }

        public static void LoadLevel (Level level)
        {
            instance.CurrentLevel = level;
            instance.CreateNewGrid();
        }

        public void CreateNewGrid ()
        {
            /// Creates a copy of the current level's grid
            Grid = new Cell[CurrentLevel.Rows, CurrentLevel.Columns];

            /// Fills grid from level's layout
            for (int row = 0; row < CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < CurrentLevel.Columns; column++)
                {
                    Grid[row, column] = new Cell(new Coord(row, column), CurrentLevel.CellLayout[row, column]);
                }
            }

            /// Initializes lightmap
            instance.LightMap = new int[instance.CurrentLevel.Rows, instance.CurrentLevel.Columns];
        }

        public static void ToggleLight(int row, int column)
        {
            instance.Grid[row, column].ToggleLight();
        }

        public static void DrawGrid()
        {
            /// Writes columns numbers
            for (int column = 0; column < instance.CurrentLevel.Columns; column++)
            {
                Console.Write(" " + (column + 1));
            }
            Console.WriteLine("\n");
            /// Runs through the grid array, drawing each cell
            for (int row = 0; row < instance.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < instance.CurrentLevel.Columns; column++)
                {
                    switch (instance.Grid[row, column].Type)
                    {
                        case CellType.WHITE:
                            if (instance.Grid[row, column].IsLightbulb)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("|+");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.Write("| ");
                            }
                            break;
                        case CellType.BLACK:
                            Console.Write("|n");
                            break;
                        case CellType.BLACK_0:
                            Console.Write("|0");
                            break;
                        case CellType.BLACK_1:
                            Console.Write("|1");
                            break;
                        case CellType.BLACK_2:
                            Console.Write("|2");
                            break;
                        case CellType.BLACK_3:
                            Console.Write("|3");
                            break;
                        case CellType.BLACK_4:
                            Console.Write("|4");
                            break;
                    }
                }
                /// Writes rows numbers
                Console.Write("|  " + (row + 1) + "\n");
            }   
        }

        public static void DrawIntro()
        {            
            /*
            Utilities.VerticalWrite(title1, 0.030f);
            Utilities.WaitForSeconds(1);
            */
            Utilities.TypeWrite(text1, 0.030f);
            Utilities.WaitForSeconds(0.5f);
            Utilities.TypeWrite(text2, 0.030f);
            Utilities.WaitForSeconds(1.5f);
        }
        
        public static bool CheckSolution()
        {
            /// This is sort of a floodfill algorithm, but not quite.
            
            /// Instanciates a queue that will store the nodes (light bulbs)            
            Queue<Cell> lightSources = new Queue<Cell>();

            ///Instanciates a lightmap, which is a copy of the current level's grid (in a 2d int array) that represents which white cells are lit (1) and which are not (0)
            instance.LightMap = new int[instance.CurrentLevel.Rows, instance.CurrentLevel.Columns];

            ///Runs through the current level grid and adds the nodes (lightbulbs) to the queue
            for (int row = 0; row < instance.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < instance.CurrentLevel.Columns; column++)
                {
                    if (instance.Grid[row, column].IsLightbulb)
                    {
                        lightSources.Enqueue(instance.Grid[row, column]);
                    }
                    
                }
            }
            
            ///Runs through each of the lightsources, lits the corresponding white cells and updates the lightmap. It also checks if any bulb lits another bulb
            foreach (Cell light in lightSources)
            {
                instance.LightMap[light.Position.row, light.Position.column]++;

                //RULE 2: A light bulb shines horizontally and vertically up to the next black cell or the edge of the diagram.
                // DOWN
                for (int drow = 1; drow < instance.CurrentLevel.Rows - light.Position.row; drow++)
                {
                    try
                    {
                        if (instance.Grid[light.Position.row + drow, light.Position.column].Type == CellType.WHITE)
                        {
                            if (!instance.Grid[light.Position.row + drow, light.Position.column].IsLightbulb)
                            {
                                instance.LightMap[light.Position.row + drow, light.Position.column] = 1;
                            } else
                            {
                                return false;
                            }            
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch { }
                }
                // UP
                for (int drow = 1; drow <= light.Position.row; drow++)
                {
                    try
                    {
                        if (instance.Grid[light.Position.row - drow, light.Position.column].Type == CellType.WHITE)
                        {
                            if (!instance.Grid[light.Position.row - drow, light.Position.column].IsLightbulb)
                            {
                                instance.LightMap[light.Position.row - drow, light.Position.column] = 1;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch { }
                }
                // RIGHT
                for (int dcolumn = 1; dcolumn < instance.CurrentLevel.Columns - light.Position.column; dcolumn++)
                {
                    try
                    {
                        if (instance.Grid[light.Position.row, light.Position.column + dcolumn].Type == CellType.WHITE)
                        {
                            if (!instance.Grid[light.Position.row, light.Position.column + dcolumn].IsLightbulb)
                            {
                                instance.LightMap[light.Position.row, light.Position.column + dcolumn] = 1;
                            } else
                            {
                                return false;
                            }       
                        }
                        else
                        {
                            break;
                        }
                    } catch { } 
                }
                // LEFT
                for (int dcolumn = 1; dcolumn <= light.Position.column; dcolumn++)
                {
                    try
                    {
                        if (instance.Grid[light.Position.row, light.Position.column - dcolumn].Type == CellType.WHITE)
                        {
                            // RULE 3: No light bulb must be lit by any other light bulb.
                            if (!instance.Grid[light.Position.row, light.Position.column - dcolumn].IsLightbulb)
                            {
                                instance.LightMap[light.Position.row, light.Position.column - dcolumn] = 1;
                            } else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch { }
                }
            }
            
            /// Loops through the lightmap to see if all white cells are lit. Also it checks black cells and their adjacent cells.
            for (int row = 0; row < instance.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < instance.CurrentLevel.Columns; column++)
                {
                    
                    //RULE 1: Place light bulbs in some of the white cells so that all white cells are lit.
                    if (instance.Grid[row,column].Type == CellType.WHITE)
                    { 
                        if (instance.LightMap[row,column] != 1)
                        {
                            return false;
                        }
                    }

                     

                    //RULE 4: A number in a black cell indicates how many light bulbs must be placed in orthogonally adjacent cells.
                    if (instance.Grid[row, column].Type != CellType.WHITE)
                    {
                        switch (instance.Grid[row,column].Type)
                        {
                            case CellType.BLACK:
                                break;
                            case CellType.BLACK_0:
                                if (instance.AmountOfAdjacentLightbulbs(row, column) != 0)
                                {
                                    return false;
                                }
                                break;
                            case CellType.BLACK_1:
                                if (instance.AmountOfAdjacentLightbulbs(row, column) != 1)
                                {
                                    return false;
                                }
                                break;
                            case CellType.BLACK_2:
                                if (instance.AmountOfAdjacentLightbulbs(row, column) != 2)
                                {
                                    return false;
                                }
                                break;
                            case CellType.BLACK_3:
                                if (instance.AmountOfAdjacentLightbulbs(row, column) != 3)
                                {
                                    return false;
                                }
                                break;
                            case CellType.BLACK_4:
                                if (instance.AmountOfAdjacentLightbulbs(row, column) != 4)
                                {
                                    return false;
                                }
                                break;
                        }
                    }
                }
            }

            return true;
        }

        /// Draws the lightmap for debugging purposes.
        public static void DrawLightmap()
        {
            for (int row = 0; row < instance.CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < instance.CurrentLevel.Columns; column++)
                {
                    if (instance.Grid[row,column].Type == CellType.WHITE)
                    {
                        Console.Write("|" + instance.LightMap[row, column]);
                    } else
                    {
                        Console.Write("|X");
                    }
                    
                }
                Console.Write("|\n");
            }
        }

        /// Small method that counts how many adjacent light bulbs a black cell has.
        public int AmountOfAdjacentLightbulbs(int row, int column)
        {
            int amount = 0;
            try
            {
                if (instance.Grid[row, column + 1].IsLightbulb)
                {
                    amount++;
                }
            }
            catch { }
            try
            {
                if (instance.Grid[row, column - 1].IsLightbulb)
                {
                    amount++;
                }
            }
            catch { }
            try
            {
                if (instance.Grid[row + 1, column].IsLightbulb)
                {
                    amount++;
                }
            }
            catch { }
            try
            {
                if (instance.Grid[row - 1, column].IsLightbulb)
                {
                    amount++;
                }
            }
            catch { }

            return amount;
        } 
    }
}
