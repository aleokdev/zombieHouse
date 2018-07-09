using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes.Tiles
{
    public sealed class OutsideFloor : Tile
    {
        public OutsideFloor(int x, int y) : base(x, y) { }
        public OutsideFloor(int x, int y, Room parent) : base(x, y, parent) { }

    }
}
