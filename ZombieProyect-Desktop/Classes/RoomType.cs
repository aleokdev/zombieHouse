﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZombieProyect_Desktop.Classes
{
    public class RoomType
    {
        public int floorType;
        public int wallpaperType;
        public Dictionary<string, float> relations = new Dictionary<string, float>();

        public RoomType(int wallpaper, int floor, Dictionary<string, float> relations)
        {
            wallpaperType = wallpaper;
            floorType = floor;
            this.relations = relations;
        }

        public static RoomType ParseFromXML(XmlNode node)
        {
            int wallpaperType = Int32.Parse(node.SelectSingleNode("wallpaper").InnerText);
            int floorType = Int32.Parse(node.SelectSingleNode("floor").InnerText);
            Dictionary<string, float> relations = new Dictionary<string, float>();
            foreach (XmlNode r in node.SelectSingleNode("relations"))
                relations.Add(r.Attributes["ref"].Value, float.Parse(r.Attributes["chance"].Value));
            return new RoomType(wallpaperType,floorType,relations);
        }

        public static RoomType ParseFromXML(string node)
        {
            return ParseFromXML(Main.roomsDocument.SelectSingleNode("/rooms/room[@name='"+node+"']"));
        }

        public static List<RoomType> GetAllRoomTypes()
        {
            XmlNodeList xmlnodes=Main.roomsDocument.ChildNodes[1].ChildNodes;
            List<RoomType> types = new List<RoomType>();
            foreach(XmlNode n in xmlnodes)
            {
                types.Add(ParseFromXML(n));
            }
            return types;
        }

        public static RoomType GetRandomRoomType()
        {
            List<RoomType> rooms = GetAllRoomTypes();
            return rooms[Map.r.Next(0, rooms.Count)];
        }
    }
}
