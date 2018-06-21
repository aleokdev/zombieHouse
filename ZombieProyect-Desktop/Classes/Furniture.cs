using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public class Furniture : MapObject
    {
        public FurnitureType type;
        public Room room;

        public Furniture(Vector2 pos, FurnitureType type, Room r) : base(pos)
        {
            this.type = type;
            room = r;
        }

        public override void Draw(SpriteBatch draw)
        {
            draw.Draw(Main.wallpapers[0, 0], new Rectangle((int)pos.X * Main.tileSize, (int)pos.Y * Main.tileSize, 16, 16), Color.White);
        }
    }
}
