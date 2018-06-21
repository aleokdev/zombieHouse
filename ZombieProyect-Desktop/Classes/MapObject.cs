using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public abstract class MapObject
    {
        public Point pos;

        public virtual void Draw() { }
    }
}
