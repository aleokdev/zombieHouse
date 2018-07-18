using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    public static class Player
    {
        /// <summary>
        /// The position of the player, measured in tiles.
        /// </summary>
        public static Vector2 pos;
        public static void Update()
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 befPos = pos;
            if (state.IsKeyDown(Keys.W))
                pos.Y -= 1;
            if (state.IsKeyDown(Keys.A))
                pos.X -= 1;
            if (state.IsKeyDown(Keys.S))
                pos.Y += 1;
            if (state.IsKeyDown(Keys.D))
                pos.X += 1;

            if (Main.mainMap.tileMap.Cast<Tile>().Where(x => (x.GetType() == typeof(Tiles.Wall) || x.GetType() == typeof(Tiles.Door))
                                                           && CollidesWith(x)).ToArray().Length != 0)
                pos = befPos;
        }

        static bool CollidesWith(Tile t)
        {
            return new Rectangle(t.Pos, new Point(1)).Intersects(new Rectangle(pos.ToPoint(), new Point(1)));
        }
    }
}
