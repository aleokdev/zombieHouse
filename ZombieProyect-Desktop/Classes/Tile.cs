using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public enum TileType
    {
        none,
        floor,
        wall,
        door,
        blockeddoor
    }
    public class Tile
    {
        public TileType type;
        private Point t_pos;
        public Point pos
        {
            get
            {
                return t_pos;
            }
        }

        public Tile(int x, int y, TileType tileType)
        {
            t_pos = new Point(x, y);
            type = tileType;
        }
    }
}
