using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes.Tiles
{
    public sealed class Floor : Tile
    {
        public Floor(int x, int y, Map parent) : base(x, y, parent) { }
        public Floor(int x, int y, Room parentR, Map parent) : base(x, y, parentR, parent) { }

    }
}
