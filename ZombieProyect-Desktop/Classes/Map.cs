﻿using Microsoft.Xna.Framework;
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
        public static Random r = new Random();

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
            lastRoom = 0;
        }

        public static Room MakeStartingRoom(RoomType type)
        {
            Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
            return GenerateRoom(new Point(tileMapSize.X / 2 - roomSize.X / 2, tileMapSize.Y / 2 - roomSize.Y / 2), roomSize, type);
        }

        /// <summary>
        /// Make a room adjacent to the wall of another one. If the room cannot be made, null will be returned instead.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="maxTriesForRoomCreation"></param>
        /// <returns>The room placed or null.</returns>
        public static Room MakeAdjacentRoomFromWall(Tile wall, int maxTriesForRoomCreation=50)
        {
            if (wall.CheckOuterEdgeOfWall() != null)
            {
                GridAxis edgeOut = (GridAxis)wall.CheckOuterEdgeOfWall();

                for (int try_ = 0; try_ < maxTriesForRoomCreation; try_++)
                {
                    Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
                    Point startingPos = new Point(-256, -256); // Set the starting pos for the room.
                    switch (edgeOut)
                    {
                        case GridAxis.positiveX: // Build room expanding on the X axis and randomly setting the Y axis
                            startingPos = new Point(wall.pos.X, wall.pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.negativeX: // Build room expanding on the -X axis and randomly setting the Y axis
                            startingPos = new Point(wall.pos.X - roomSize.X + 1, wall.pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.positiveY: // Build room randomly setting the X axis and expanding on the Y axis
                            startingPos = new Point(wall.pos.X - r.Next(1, roomSize.X - 1), wall.pos.Y);
                            break;
                        case GridAxis.negativeY: // Build room randomly setting the X axis and expanding on the -Y axis
                            startingPos = new Point(wall.pos.X - r.Next(1, roomSize.X - 1), wall.pos.Y - roomSize.Y + 1);
                            break;
                        default:
                            break;
                    }
                    bool isCollidingWithOtherRooms = false;
                    foreach(Room r in rooms)
                    {
                        if (r?.FloorIntersects(new Rectangle(startingPos, roomSize))??false) isCollidingWithOtherRooms = true; // Floor intersects with any room's walls or floor? nope, repeat
                    }
                    if (!isCollidingWithOtherRooms)
                    {
                        Room room = GenerateRoom(startingPos, roomSize);
                        return room;
                    }
                }
                return null;
            }
            else return null;
        }

        /// <summary>
        /// Make a room adjacent to the wall of another one. If the room cannot be made, null will be returned instead.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="maxTriesForRoomCreation"></param>
        /// <returns>The room placed or null.</returns>
        public static Room MakeAdjacentRoomFromWall(Tile wall, Point minSize, Point maxSize, int maxTriesForRoomCreation = 50)
        {
            if (wall.CheckOuterEdgeOfWall() != null)
            {
                GridAxis edgeOut = (GridAxis)wall.CheckOuterEdgeOfWall();

                for (int try_ = 0; try_ < maxTriesForRoomCreation; try_++)
                {
                    Point roomSize = new Point(r.Next(minSize.X, maxSize.X+1), r.Next(minSize.Y, maxSize.Y + 1)); // Add 1 to maxSize because r.Next excludes the max number when randomizing
                    Point startingPos = new Point(-256, -256); // Set the starting pos for the room.
                    switch (edgeOut)
                    {
                        case GridAxis.positiveX: // Build room expanding on the X axis and randomly setting the Y axis
                            startingPos = new Point(wall.pos.X, wall.pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.negativeX: // Build room expanding on the -X axis and randomly setting the Y axis
                            startingPos = new Point(wall.pos.X - roomSize.X + 1, wall.pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.positiveY: // Build room randomly setting the X axis and expanding on the Y axis
                            startingPos = new Point(wall.pos.X - r.Next(1, roomSize.X - 1), wall.pos.Y);
                            break;
                        case GridAxis.negativeY: // Build room randomly setting the X axis and expanding on the -Y axis
                            startingPos = new Point(wall.pos.X - r.Next(1, roomSize.X - 1), wall.pos.Y - roomSize.Y + 1);
                            break;
                        default:
                            break;
                    }
                    bool isCollidingWithOtherRooms = false;
                    foreach (Room r in rooms)
                    {
                        if (r?.FloorIntersects(new Rectangle(startingPos, roomSize)) ?? false) isCollidingWithOtherRooms = true; // Floor intersects with any room's walls or floor? nope, repeat
                    }
                    if (!isCollidingWithOtherRooms)
                    {
                        Room room = GenerateRoom(startingPos, roomSize);
                        return room;
                    }
                }
                return null;
            }
            else return null;
        }

        public static void GenerateHouse(int complexity, int maxTriesForRoomCreation = 50)
        {
            InitializeMap(new Point(200, 200));
            
            List<Room> roomTree = new List<Room>();
            roomTree[0] = MakeStartingRoom();
            for (int c = 1; c < complexity; c++)
            {
                Room lastRoomInTree = roomTree.Last();
                Tile randomWallInLastRoom = lastRoomInTree.containedWalls[r.Next(0, lastRoomInTree.containedWalls.Length)];
                MakeAdjacentRoomFromWall(randomWallInLastRoom);
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
        public static Tile PlaceDoorBetweenRooms(Room r1, Room r2)
        {
            Tile[] walls = GetCommonWalls(r1, r2); // Get common walls.
            walls = walls.Where(x => x != null).ToArray(); // Remove null elements.
            walls = walls.Where(x => x.IsConnectedToTwoFloors()).ToArray(); // Remove corner walls
            if (walls.Length == 0)
                return null;
            else
            {
                tileMap[walls[r.Next(0, walls.Length - 1)].pos.X, walls[r.Next(0, walls.Length - 1)].pos.Y].type = TileType.door; // Transform any of the walls remaining into a door.
                return tileMap[walls[r.Next(0, walls.Length - 1)].pos.X, walls[r.Next(0, walls.Length - 1)].pos.Y];
            }
        }

        /// <summary>
        /// Places a room on the map and then returns it.
        /// </summary>
        /// <param name="pos">The position of the room.</param>
        /// <param name="size">The size of the room.</param>
        /// <returns>The room made.</returns>
        public static Room GenerateRoom(Point pos, Point size, RoomType type)
        {
            Room ro = new Room(pos, size, type);
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
            return ro;
        }
    }
}
