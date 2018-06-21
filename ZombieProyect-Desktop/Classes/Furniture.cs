using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public class Furniture : MapObject
    {
        public FurnitureType type;
        public Room room;

        public Furniture(Point pos, FurnitureType type, Room r)
        {
            this.pos = pos;
            this.type = type;
            room = r;
        }
    }
}
