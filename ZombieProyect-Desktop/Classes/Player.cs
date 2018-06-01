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
        public static Point pos;
        public static void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W))
                pos.Y-=100;
            if (state.IsKeyDown(Keys.A))
                pos.X -= 100;
            if (state.IsKeyDown(Keys.S))
                pos.Y += 100;
            if (state.IsKeyDown(Keys.D))
                pos.X += 100;
        }
    }
}
