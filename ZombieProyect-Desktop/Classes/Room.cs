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
        public Point roomPos { get { return t_roomPos; } }
        Point t_roomSize;
        public Point roomSize { get { return t_roomSize; } }
        public Tile[] containedFloor = new Tile[256];
        public Tile[] containedWalls = new Tile[256];
        public int wallpaper { get; private set; }

        public Room(Point pos, Point size)
        {
            t_roomPos = pos;
            t_roomSize = size;
            SetRandomWallpaper();
        }

        private void SetRandomWallpaper()
        {
            wallpaper = Map.r.Next(1, Main.wallpapers.Length);
        }
        /// <summary>
        /// Returns the position of the walls that should be present around the floor of the room.
        /// </summary>
        /// <returns>The position of the walls around the floor.</returns>
        public Point[] GetWallsAroundFloor()
        {
            Point[] walls = new Point[2*roomSize.X+2*(roomSize.Y-2)];
            int walln = 0;
            for (int iy = 0; iy < roomSize.Y; iy++)
            {
                for (int ix = 0; ix < roomSize.X; ix++)
                {
                    if (iy == 0 || iy == roomSize.Y - 1 || ix == 0 || ix == roomSize.X - 1) // Tile is border
                    {
                        walls[walln] = new Point(ix+t_roomPos.X, iy+t_roomPos.Y);
                        walln++;
                    }
                }
            }
            return walls;
        }

        public bool RoomIntersects(Rectangle r)
        {
            return r.Intersects(new Rectangle(roomPos, roomSize));
        }
        public bool FloorIntersects(Rectangle r)
        {
            return r.Intersects(new Rectangle(roomPos+new Point(1), roomSize-new Point(2))); // Add a point to the XY pos and remove 2 to the size to get the floors
        }
    }
}
