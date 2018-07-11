using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes.Tiles
{
    public class Wall : Tile
    {
        public Wall(int x, int y, Map parent) : base(x, y, parent) { }
        public Wall(int x, int y, Room parentR, Map parent) : base(x, y, parentR, parent) { }

        public GridAxis? CheckOuterEdgeOfWall()
        {
            if ((parentMap.tileMapSize.X<=Pos.X+1)||(parentMap.tileMap[Pos.X + 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                return GridAxis.positiveX;

            if ((0>Pos.X - 1)||(parentMap.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                return GridAxis.negativeX;

            if ((parentMap.tileMapSize.Y <= Pos.Y + 1) || (parentMap.tileMap[Pos.X, Pos.Y + 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                return GridAxis.positiveY;

            if ((0 > Pos.Y - 1) || (parentMap.tileMap[Pos.X, Pos.Y - 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                return GridAxis.negativeY;

            return null; // Tile is floor and there are no outer edges, or wall is sorrounded by other tiles.
        }

        public bool IsCornerWall()
        {
            int a = 0;
            if ((parentMap.tileMap[Pos.X + 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((parentMap.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((parentMap.tileMap[Pos.X, Pos.Y + 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;
            if ((parentMap.tileMap[Pos.X, Pos.Y - 1]?.GetType() ?? typeof(OutsideFloor)) == typeof(OutsideFloor))
                a++;

            if (a > 1) return true;
            else return false;
        }
    }
}
