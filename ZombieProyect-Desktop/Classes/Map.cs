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

        /// <summary>
        /// Gets the walls that are common between 2 rooms. Rooms have to be adjacent for this algorithm to work.
        /// </summary>
        /// <param name="r1">A room.</param>
        /// <param name="r2">Another adjacent room.</param>
        /// <returns>The common walls.</returns>
        public static Tile[] GetCommonWalls(Room r1, Room r2)
        {
            /*Because of the adjacentRoom algorithm, old walls from one room are removed when the new adjacent second room is placed.      *
             * So we can detect walls in common by comparing the walls one room SHOULD have which the others that the other room HAVE.     *
             * Or, if one room lacks walls that the other room has, those walls are common between the two rooms.                          */

            Point[] commonPos = r1.GetWallsAroundFloor().Where(x => r2.GetWallsAroundFloor().Contains(x)).ToArray(); // Get the positions of the walls in common
            Tile[] commonWalls = r1.containedWalls.Concat(r2.containedWalls).Where(x => commonPos.Contains(x?.pos ?? new Point(-256,-256))).ToArray(); // Search for those positions on both of the rooms
            return commonWalls;
        }

        /// <summary>
        /// Places a door between 2 rooms. Rooms have to be adjacent for this function to work.
        /// </summary>
        /// <param name="r1">A room.</param>
        /// <param name="r2">Another adjacent room.</param>
        public static void PlaceDoorBetweenRooms(Room r1, Room r2)
        {
            Tile[] walls = GetCommonWalls(r1, r2); // Get common walls.
            walls = walls.Where(x => x != null).ToArray(); // Remove null elements.
            walls = walls.Where(x => x != walls.First()&&x!=walls.Last()).ToArray(); // Remove first and last element, so doors aren't created on corners
            tileMap[walls[r.Next(0, walls.Length - 1)].pos.X, walls[r.Next(0, walls.Length - 1)].pos.Y].type = TileType.door; // Transform any of the walls remaining into a door.
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
