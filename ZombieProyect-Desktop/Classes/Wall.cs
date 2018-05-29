using Microsoft.Xna.Framework;

namespace ZombieProyect_Desktop.Classes
{
    public enum WallType
    {
        none,
        wall,
        door,
        blockeddoor
    }
    public enum WallDirection
    {
        none,
        right,
        down,
        left,
        up
    }
    public class Wall
    {
        public WallType type;
        public Point pos;
        public WallDirection outerEdge;
    }
}