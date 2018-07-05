using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes.Tiles
{
    public class Wall : Tile
    {
        public Wall(int x, int y) : base(x, y) { }
        public Wall(int x, int y, Room parent) : base(x, y, parent) { }

        public GridAxis? CheckOuterEdgeOfWall()
        {
            try
            {
                if ((Map.tileMap[Pos.X + 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                    return GridAxis.positiveX;
            }
            catch
            {
                return GridAxis.positiveX;
            }
            try
            {
                if ((Map.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                    return GridAxis.negativeX;
            }
            catch
            {
                return GridAxis.negativeX;
            }
            try
            {
                if ((Map.tileMap[Pos.X, Pos.Y + 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                    return GridAxis.positiveY;
            }
            catch
            {
                return GridAxis.positiveY;
            }
            try
            {
                if ((Map.tileMap[Pos.X, Pos.Y - 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                    return GridAxis.negativeY;
            }
            catch
            {
                return GridAxis.negativeY;
            }
            return null; // Tile is floor and there are no outer edges, or wall is sorrounded by other tiles.
        }

        public bool IsCornerWall()
        {
            int a = 0;
            if ((Map.tileMap[Pos.X + 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((Map.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((Map.tileMap[Pos.X, Pos.Y + 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((Map.tileMap[Pos.X, Pos.Y - 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;

            if (a > 1) return true;
            else return false;
        }
    }
}
