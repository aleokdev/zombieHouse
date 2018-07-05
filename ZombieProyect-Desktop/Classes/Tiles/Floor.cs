using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes.Tiles
{
    public sealed class Floor : Tile
    {
        public Floor(int x, int y) : base(x, y) { }
        public Floor(int x, int y, Room parent) : base(x, y, parent) { }
        
    }
}
