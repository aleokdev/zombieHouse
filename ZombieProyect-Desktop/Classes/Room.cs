using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    abstract public class Room
    {
        /// <summary>
        /// The furniture that is inside the room.
        /// </summary>
        Furniture[] furniture = new Furniture[256];
    }
}
