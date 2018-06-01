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
    public enum WallTextureType
    {
        horizontal,
        vertical,
        rightbottomcorner,
        leftbottomcorner,
        righttopcorner,
        lefttopcorner,
        allbutupjoint,
        allbutrightjoint,
        allbutbottomjoint,
        allbutleftjoint,
        unknown
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

        public WallTextureType GetAccordingTexture()
        {
            bool a = Map.tileMap[pos.X, pos.Y - 1].type == TileType.wall|| Map.tileMap[pos.X, pos.Y - 1].type == TileType.door; // Upper tile is wall
            bool b = Map.tileMap[pos.X + 1, pos.Y].type == TileType.wall|| Map.tileMap[pos.X + 1, pos.Y].type == TileType.door; // Next right tile is wall
            bool c = Map.tileMap[pos.X, pos.Y + 1].type == TileType.wall|| Map.tileMap[pos.X, pos.Y + 1].type == TileType.door; // Bottom tile is wall
            bool d = Map.tileMap[pos.X - 1, pos.Y].type == TileType.wall|| Map.tileMap[pos.X - 1, pos.Y].type == TileType.door; // Next left tile is wall

            if (!a && b && !c && d) // Right & Left
                return WallTextureType.horizontal;
            if (a && !b && c && !d) // Upper & Bottom
                return WallTextureType.vertical;
            if (!a && !b && c && d) // Left & Bottom
                return WallTextureType.leftbottomcorner;
            if (!a && b && c && !d) // Right & Bottom
                return WallTextureType.rightbottomcorner;
            if (a && b && !c && !d) // Right & Top
                return WallTextureType.righttopcorner;
            if (a && !b && !c && d) // Left & Top
                return WallTextureType.lefttopcorner;
            if(!a&&b&&c&&d) // All but up
                return WallTextureType.allbutupjoint;
            if (a && !b && c && d) // All but right
                return WallTextureType.allbutrightjoint;
            if (a && b && !c && d) // All but bottom
                return WallTextureType.allbutbottomjoint;
            if (a && b && c && !d) // All but left
                return WallTextureType.allbutleftjoint;

            return WallTextureType.unknown;
        }
    }
}
