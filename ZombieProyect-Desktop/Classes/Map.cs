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
        public static Room[] rooms;
        static int lastRoom;
        public static Point tileMapSize;
        static Random r = new Random();

        public static void InitializeMap(Point size)
        {
            // Initialize tiles
            tileMapSize = size;
            tileMap = new Tile[size.X, size.Y];
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    tileMap[ix, iy] = new Tile(ix, iy, TileType.none);
                }
            }

            // Initialize rooms
            rooms = new Room[256];
        }

        public static void MakeStartingRoom()
        {
            Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
            GenerateRoom(new Point(tileMapSize.X / 2 - roomSize.X / 2, tileMapSize.Y / 2 - roomSize.Y / 2), roomSize);
        }

        public static void MakeAdjacentRoomFromWall(Tile wall)
        {
            if (wall.CheckOuterEdgeOfWall() != null)
            {
                GridAxis edgeOut = (GridAxis)wall.CheckOuterEdgeOfWall();

                Point startingPos = wall.pos; // Set the starting pos for the room.

                //for(int try_ = 0;try_<50;try_++)
                //{
                    Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
                    switch (edgeOut)
                    {
                        case GridAxis.positiveX: // Build room expanding on the X axis and randomly setting the Y axis
                            GenerateRoom(new Point(startingPos.X,startingPos.Y - r.Next(0, roomSize.Y - 1)), roomSize);
                            break;
                        case GridAxis.negativeX: // Build room expanding on the -X axis and randomly setting the Y axis
                            GenerateRoom(new Point(startingPos.X-roomSize.X+1, startingPos.Y - r.Next(0, roomSize.Y - 1)), roomSize);
                            break;
                        case GridAxis.positiveY: // Build room randomly setting the X axis and expanding on the Y axis
                            GenerateRoom(new Point(startingPos.X - r.Next(0, roomSize.X - 1), startingPos.Y), roomSize);
                            break;
                        case GridAxis.negativeY: // Build room randomly setting the X axis and expanding on the -Y axis
                            GenerateRoom(new Point(startingPos.X - r.Next(0, roomSize.X - 1), startingPos.Y-roomSize.Y+1), roomSize);
                            break;
                        default:
                            break;
                    }
                //}
            }
        }

        public static void GenerateRoom(Point pos, Point size)
        {
            Room ro = new Room(pos, size);
            rooms[lastRoom] = ro;
            lastRoom++;
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    if (iy == 0 || iy == size.Y-1 || ix == 0 || ix == size.X-1) // Tile is border
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Tile(ix + pos.X, iy + pos.Y, TileType.wall);
                        ro.containedWalls[ro.containedWalls.Count(s => s != null)] = tileMap[ix + pos.X, iy + pos.Y];
                    }
                    else
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Tile(ix + pos.X, iy + pos.Y, TileType.floor, ro);
                        ro.containedFloor[ro.containedFloor.Count(s => s != null)] = tileMap[ix + pos.X, iy + pos.Y];
                    }
                }
            }
        }
    }
}
