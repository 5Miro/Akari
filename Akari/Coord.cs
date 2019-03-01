using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akari
{
    public struct Coord
    {
        public int row, column;

        public Coord(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
}
