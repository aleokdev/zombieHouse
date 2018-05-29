using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public class Room
    {
        Point t_roomPos;
        public Point roomPos;
        Point t_roomSize;
        public Point roomSize;
        public Tile[] containedFloor = new Tile[256];
        public Tile[] containedWalls = new Tile[256];

        public Room(Point pos, Point size)
        {
            t_roomPos = pos;
            t_roomSize = size;
        }
    }
}
