using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public static class Map
    {
        public static Tile[,] tileMap;

        public static void InitializeMap(Point size)
        {
            tileMap = new Tile[size.X, size.Y];
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    tileMap[ix, iy] = new Tile(ix, iy, TileType.none);
                }
            }
        }

        public static void GenerateRoom(Point pos, Point size)
        {
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    if (iy == 0 || iy == size.Y || ix == 0 || ix == size.X) // Tile is wall
                    {
                        tileMap[ix, iy] = new Tile(ix, iy, TileType.wall);
                    }
                    else
                    {
                        tileMap[ix, iy] = new Tile(ix, iy, TileType.floor);
                    }
                }
            }
        }
    }
}
