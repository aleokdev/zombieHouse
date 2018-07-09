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

        public virtual void Draw(SpriteBatch draw)
        {
            draw.Draw(Main.blankTexture, new Rectangle(new Point((int)(pos.X * Main.tileSize), (int)(pos.Y * Main.tileSize)) - (Player.pos * Main.tileSize).ToPoint(), new Point(Main.tileSize)), Color.White);
        }

        public virtual void LoadContent() { }
    }
}
