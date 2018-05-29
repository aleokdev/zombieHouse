using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieProyect_Desktop.Classes
{
    abstract public class House
    {
        /// <summary>
        /// The rooms that form the house.
        /// </summary>
        Room[] rooms = new Room[256];
    }
}
