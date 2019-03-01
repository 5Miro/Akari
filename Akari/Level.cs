using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public class Level
    {
        public int Rows { get; }
        public int Columns { get; }
        public CellType[,] CellLayout { get; }
        public bool Completed { get; set; }

        public Level(int rows, int columns, CellType[,] cellLayout)
        {
            Rows = rows;
            Columns = columns;
            CellLayout = cellLayout;
            Completed = false;
        }

    }
}
