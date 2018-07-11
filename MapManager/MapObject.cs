using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public abstract class MapObject
    {
        public Vector2 pos;

        public MapObject(Vector2 pos) { this.pos = pos; }
    }
}
