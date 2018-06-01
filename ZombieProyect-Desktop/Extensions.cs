using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ZombieProyect_Desktop
{
    public static class Extensions
    {
        public static Texture2D[,] SplitTileset(this Texture2D tileset, Point tileSize)
        {
            Color[] slt = new Color[tileset.Width * tileset.Height];
            tileset.GetData(slt);
            Texture2D[,] tiles = new Texture2D[tileset.Width / tileSize.X, tileset.Height / tileSize.Y];
            //split tileset to tiles
            for (byte iy = 0; iy < tileset.Height / tileSize.Y; iy++) //raw tileset is 16tilesx16tiles.
            {
                for (byte ix = 0; ix < tileset.Width / tileSize.X; ix++)
                {
                    tiles[ix, iy] = new Texture2D(tileset.GraphicsDevice, tileSize.X, tileSize.Y);
                    List<Color> tile = new List<Color>();
                    for (byte iiy = 0; iiy < tileSize.Y; iiy++)
                    {
                        for (byte iix = 0; iix < tileSize.X; iix++)
                        {
                            tile.Add(slt[(ix * tileSize.X) + (iy * tileSize.Y * tileset.Width) + iix + (iiy * tileset.Width)]);
                        }
                    }
                    tiles[ix, iy].SetData(tile.ToArray());
                }
            }
            return tiles;
        }
    }
}
