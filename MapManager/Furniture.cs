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
            LoadContent();
        }

        public override void Draw(SpriteBatch draw)
        {
            draw.Draw(type.Texture ?? Main.blankTexture, new Rectangle(new Point((int)(pos.X * Main.tileSize), (int)(pos.Y * Main.tileSize)) - (Player.pos * Main.tileSize).ToPoint(), new Point(Main.tileSize)), Color.White);
        }

        public override void LoadContent()
        {
            if(!Main.furnitureTextures.ContainsKey(type.textureRef))
                Main.furnitureTextures.Add(type.textureRef, Main.contentManager.Load<Texture2D>("textures/" + Main.texturePackage + "/furniture/" + type.textureRef));
        }
    }
}
