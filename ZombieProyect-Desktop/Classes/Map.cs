using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieProyect_Desktop.Classes.Tiles;

namespace ZombieProyect_Desktop.Classes
{
    public static class Map
    {
        /// <summary>
        /// Contains all tiles of the map.
        /// </summary>
        public static Tile[,] tileMap;

        /// <summary>
        /// Contains all objects of the map.
        /// </summary>
        public static List<MapObject> objectMap = new List<MapObject>();

        /// <summary>
        /// Contains all rooms of the map.
        /// </summary>
        public static Room[] rooms;

        /// <summary>
        /// The last room index of the rooms array.
        /// </summary>
        public static int lastRoom;

        /// <summary>
        /// The size of the tileMap.
        /// </summary>
        public static Point tileMapSize;

        /// <summary>
        /// The random class of the Map.
        /// </summary>
        public static Random r = new Random();

        /// <summary>
        /// Initialize the map with a certain size.
        /// </summary>
        /// <param name="size">The size of the tileMap.</param>
        public static void InitializeMap(Point size)
        {
            // Initialize tiles
            tileMapSize = size;
            tileMap = new Tile[size.X, size.Y];
            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    tileMap[ix, iy] = new OutsideFloor(ix, iy);
                }
            }

            // Initialize rooms
            rooms = new Room[256];
            lastRoom = 0;
        }

        /// <summary>
        /// Make a room approximately at the center of the map.
        /// </summary>
        /// <param name="type">The RoomType of the room.</param>
        /// <returns></returns>
        public static Room MakeStartingRoom(RoomType type)
        {
            Point roomSize = new Point(r.Next(6, 10), r.Next(6, 10));
            return GenerateRoom(new Point((tileMapSize.X / 2) - (roomSize.X / 2), (tileMapSize.Y / 2) - (roomSize.Y / 2)), roomSize, type);
        }

        /// <summary>
        /// Make a room adjacent to the wall of another one. If the room cannot be made, null will be returned instead.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="maxTriesForRoomCreation"></param>
        /// <returns>The room placed or null.</returns>
        public static Room MakeAdjacentRoomFromWall(Wall wall, RoomType type, int maxTriesForRoomCreation=50)
        {
            return MakeAdjacentRoomFromWall(wall, type, new Point(6, 6), new Point(10, 10));
        }

        /// <summary>
        /// Make a room adjacent to the wall of another one. If the room cannot be made, null will be returned instead.
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="maxTriesForRoomCreation"></param>
        /// <returns>The room placed or null.</returns>
        public static Room MakeAdjacentRoomFromWall(Wall wall, RoomType type, Point minSize, Point maxSize, int maxTriesForRoomCreation = 50)
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
                            startingPos = new Point(wall.Pos.X, wall.Pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.negativeX: // Build room expanding on the -X axis and randomly setting the Y axis
                            startingPos = new Point(wall.Pos.X - roomSize.X + 1, wall.Pos.Y - r.Next(1, roomSize.Y - 1));
                            break;
                        case GridAxis.positiveY: // Build room randomly setting the X axis and expanding on the Y axis
                            startingPos = new Point(wall.Pos.X - r.Next(1, roomSize.X - 1), wall.Pos.Y);
                            break;
                        case GridAxis.negativeY: // Build room randomly setting the X axis and expanding on the -Y axis
                            startingPos = new Point(wall.Pos.X - r.Next(1, roomSize.X - 1), wall.Pos.Y - roomSize.Y + 1);
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
                        Room room = GenerateRoom(startingPos, roomSize, type);
                        return room;
                    }
                }
                return null;
            }
            else return null;
        }

        /// <summary>
        /// Generate a house with the current house generation algorithm.
        /// </summary>
        /// <param name="complexity">How many rooms can be in one branch.</param>
        /// <param name="numberOfBranches">The number of times the rooms have branched out.</param>
        /// <param name="numberOfRooms">The number of rooms the algorithm has generated.</param>
        public static void GenerateHouse(int complexity, out int numberOfBranches, out int numberOfRooms)
        {
            InitializeMap(new Point(complexity*4, complexity * 4));
            Console.WriteLine("Generating " + (complexity * 4) + "x" + (complexity * 4) + "t " + complexity + "c house.");

            numberOfBranches = 0;
            numberOfRooms = 0;

            List<Room> roomTree = new List<Room>
            {
                MakeStartingRoom(RoomType.GetRandomRoomType())
            };

            for (int c = 1; c < complexity; c++)
            {
                Room lastRoomInTree = roomTree.Last();
                //Console.WriteLine("Looped. c == " + c);
                if (lastRoomInTree == null) //If the room was null, that means it wasn't able to create it without colliding with something or without creating it out of the map.
                {
                    //Console.WriteLine("Last room was null; Stepping out.");
                    numberOfBranches++;
                    roomTree.Remove(lastRoomInTree);
                    c--; //Go one step out of the branch.
                    complexity--;

                    if (roomTree.Count() == 0)
                        break;
                    lastRoomInTree = roomTree.Last();
                }
                if (c+1 == complexity) //If c has almost reached complexity, step out to create more branches.
                {
                    //Console.WriteLine("c reached max complexity; Stepping out.");
                    numberOfBranches++;
                    roomTree.Remove(lastRoomInTree);
                    c--; //Go one step out of the branch.

                    if (roomTree.Count() == 0)
                        break;
                    lastRoomInTree = roomTree.Last();
                }
                if (lastRoomInTree.type.relations.Count() == 0) //If the last room has no relations, step out.
                {
                    //Console.WriteLine("No more relations found with current room. Stepping out.");
                    numberOfBranches++;
                    roomTree.Remove(lastRoomInTree);
                    c--; //Go one step out of the branch.

                    if (roomTree.Count() == 0)
                        break;
                    lastRoomInTree = roomTree.Last();
                }
                if (r.Next(0,complexity) == 0 && c>0)
                {
                    //Console.WriteLine("Random check; Stepping out.");
                    numberOfBranches++;
                    roomTree.Remove(lastRoomInTree);
                    c--; //Go one step out of the branch.

                    if (roomTree.Count() == 0)
                        break;
                    lastRoomInTree = roomTree.Last();
                }

                //Console.WriteLine("Trying to place new room...");
                for (int try_ = 0; try_ < lastRoomInTree.containedWalls.Where(x => x != null).ToArray().Length; try_++)
                {
                    //Console.WriteLine("try_ = " + try_);
                    Wall wallInLastRoom = (Wall)lastRoomInTree.containedWalls.Where(x => x != null).ToArray()[try_];
                    Room ro = MakeAdjacentRoomFromWall(wallInLastRoom, RoomType.ParseFromXML(lastRoomInTree.type.relations.Keys.ElementAt(r.Next(0, lastRoomInTree.type.relations.Count()-1))));
                    if (ro != null)
                    {
                        numberOfRooms++;
                        roomTree.Add(ro);
                        break;
                    }      
                }
            }
        }
        
        public static void GenerateFurniture()
        {
            foreach(Room ro in rooms)
            {
                if(ro!=null)
                    foreach(string s in ro.type.furniture.Keys)
                    {
                        if((float)r.NextDouble()<ro.type.furniture[s].chance) //If random chance...
                        {
                            switch (ro.type.furniture[s].anchor)
                            {
                                case FurnitureAnchor.top:
                                    while (true) {
                                        int posX = r.Next(1, ro.roomSize.X - 1); // Random position for the furniture
                                        if (tileMap[ro.roomPos.X + posX, ro.roomPos.Y]?.GetType()== typeof(Door)|| tileMap[ro.roomPos.X + posX + 1, ro.roomPos.Y+1]?.GetType() == typeof(Door) || tileMap[ro.roomPos.X + posX-1, ro.roomPos.Y+1]?.GetType() == typeof(Door))
                                        {
                                            continue; // Furniture is next to door, repeat random process
                                        }
                                        else
                                        {
                                            objectMap.Add(new Furniture(new Vector2(ro.roomPos.X + posX, ro.roomPos.Y + 1), FurnitureType.ParseFromXML(s), ro));
                                            break;
                                        }
                                    }
                                    break;
                                case FurnitureAnchor.sides:
                                    while (true)
                                    {
                                        int posY = r.Next(2, ro.roomSize.Y - 1); // Random position for the furniture
                                        if (r.Next(0, 2) == 0) // Spawn on the right or left side
                                        {
                                            // Left side
                                            if (tileMap[ro.roomPos.X, ro.roomPos.Y + posY]?.GetType() == typeof(Door))
                                            {
                                                continue; // Furniture is next to door, repeat random process
                                            }
                                            else
                                            {
                                                objectMap.Add(new Furniture(new Vector2(ro.roomPos.X + 1, ro.roomPos.Y + posY), FurnitureType.ParseFromXML(s), ro));
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            // Right side
                                            if (tileMap[ro.roomPos.X+ro.roomSize.X-1, ro.roomPos.Y + posY]?.GetType() == typeof(Door))
                                            {
                                                continue; // Furniture is next to door, repeat random process
                                            }
                                            else
                                            {
                                                objectMap.Add(new Furniture(new Vector2(ro.roomPos.X + ro.roomSize.X - 2, ro.roomPos.Y + posY), FurnitureType.ParseFromXML(s), ro));
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case FurnitureAnchor.center:
                                    break;
                                case FurnitureAnchor.corners:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
            }
        }

        public static void PlaceDoorsBetweenAllRooms()
        {
            foreach(Room r in rooms)
            {
                foreach (Room r2 in rooms)
                {
                    if (r2 != null&&r!=null&& !r.connections.Contains(r2) && r != r2&&(r?.RoomIntersects(new Rectangle(r2.roomPos,r2.roomSize)) ?? false))
                        PlaceDoorBetweenRooms(r, r2);
                }
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
            Tile[] commonWalls = r1.containedWalls.Concat(r2.containedWalls).Where(x => commonPos.Contains(x?.Pos ?? new Point(-256,-256))).ToArray(); // Search for those positions on both of the rooms
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
                tileMap[walls[r.Next(0, walls.Length - 1)].Pos.X, walls[r.Next(0, walls.Length - 1)].Pos.Y] = tileMap[walls[r.Next(0, walls.Length - 1)].Pos.X, walls[r.Next(0, walls.Length - 1)].Pos.Y] as Door; // Transform any of the walls remaining into a door.
                r1.connections.Add(r2); // Add connections
                r2.connections.Add(r1);
                return tileMap[walls[r.Next(0, walls.Length - 1)].Pos.X, walls[r.Next(0, walls.Length - 1)].Pos.Y];
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

            if (pos.X < 0 || pos.Y < 0 || (pos + size).X >= tileMapSize.X || (pos + size).Y >= tileMapSize.Y) return null; // Return null if room will be out of map if made

            for (int iy = 0; iy < size.Y; iy++)
            {
                for (int ix = 0; ix < size.X; ix++)
                {
                    if (iy == 0 || iy == size.Y - 1 || ix == 0 || ix == size.X - 1) // Tile is border
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Wall(ix + pos.X, iy + pos.Y);
                        ro.containedWalls[ro.containedWalls.Count(s => s != null)] = tileMap[ix + pos.X, iy + pos.Y];
                    }
                    else
                    {
                        tileMap[ix + pos.X, iy + pos.Y] = new Floor(ix + pos.X, iy + pos.Y, ro);
                        ro.containedFloor[ro.containedFloor.Count(s => s != null)] = tileMap[ix + pos.X, iy + pos.Y];
                    }
                }
            }
            rooms[lastRoom] = ro;
            lastRoom++;
            return ro;
        }
    }
}
