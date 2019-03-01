using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class Cell
    {
        public Coord Position { get; }
        public CellType Type { get; }
        public bool IsLightbulb { get; set; }
        
        public Cell(Coord coord, CellType type)
        {
            Position = coord;
            Type = type;
            IsLightbulb = false;
        }

        public void ToggleLight()
        {
            if (Type == CellType.WHITE)
            {
                if (IsLightbulb == false)
                {
                    IsLightbulb = true;
                }
                else
                {
                    IsLightbulb = false;
                }
            }
            else
            {
                Console.WriteLine("This is not a white cell.");

                Utilities.WaitForSeconds(1.5f);
            }

        }
    }
}

