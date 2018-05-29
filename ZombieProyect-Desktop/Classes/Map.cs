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
        public static Point tileMapSize;
        static Random r = new Random();

        public static void InitializeMap(Point size)
        {
            tileMapSize = size;
            tileMap = new Tile[size.X, size.Y];
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    tileMap[ix, iy] = new Tile(ix, iy, TileType.none);
                }
            }
        }

        public static void MakeStartingRoom()
        {
            Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
            GenerateRoom(new Point(tileMapSize.X / 2 - roomSize.X / 2, tileMapSize.Y / 2 - roomSize.Y / 2), roomSize);
        }

        public static void GenerateRoom(Point pos, Point size)
        {
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    if (iy == 0 || iy == size.Y-1 || ix == 0 || ix == size.X-1) // Tile is border
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Tile(ix + pos.X, iy + pos.Y, TileType.wall);
                    }
                    else
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Tile(ix + pos.X, iy + pos.Y, TileType.floor);
                    }
                }
            }
        }
    }
}
