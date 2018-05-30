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
    public enum GridAxis
    {
        positiveX,
        negativeX,
        positiveY,
        negativeY
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
        public Room parentRoom;

        public Tile(int x, int y, TileType tileType)
        {
            t_pos = new Point(x, y);
            type = tileType;
        }
        public Tile(int x, int y, TileType tileType, Room parent)
        {
            t_pos = new Point(x, y);
            type = tileType;
            parentRoom = parent;
        }

        public GridAxis? CheckOuterEdgeOfWall()
        {
            if (Map.tileMap[pos.X + 1, pos.Y].type == TileType.none)
                return GridAxis.positiveX;
            if (Map.tileMap[pos.X - 1, pos.Y].type == TileType.none)
                return GridAxis.negativeX;
            if (Map.tileMap[pos.X, pos.Y +1].type == TileType.none)
                return GridAxis.positiveY;
            if (Map.tileMap[pos.X, pos.Y -1].type == TileType.none)
                return GridAxis.negativeY;
            return null; // Tile is floor and there are no outer edges, or wall is sorrounded by other tiles.
        }

        public bool IsCornerWall()
        {
            int a = 0;
            if (Map.tileMap[pos.X + 1, pos.Y].type == TileType.none)
                a++;
            if (Map.tileMap[pos.X - 1, pos.Y].type == TileType.none)
                a++;
            if (Map.tileMap[pos.X, pos.Y + 1].type == TileType.none)
                a++;
            if (Map.tileMap[pos.X, pos.Y - 1].type == TileType.none)
                a++;

            if (a > 1) return true;
            else return false;
        }

        public bool IsConnectedToTwoFloors()
        {
            int a = 0;
            if (Map.tileMap[pos.X + 1, pos.Y].type == TileType.floor)
                a++;
            if (Map.tileMap[pos.X - 1, pos.Y].type == TileType.floor)
                a++;
            if (Map.tileMap[pos.X, pos.Y + 1].type == TileType.floor)
                a++;
            if (Map.tileMap[pos.X, pos.Y - 1].type == TileType.floor)
                a++;

            if (a == 2) return true;
            else return false;
        }
    }
}
