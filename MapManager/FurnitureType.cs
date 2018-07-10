﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZombieProyect_Desktop.Classes
{
    public class FurnitureType
    {
        public string textureRef;
        public FurnitureType(string textureRef)
        {
            this.textureRef = textureRef;
        }

        public static FurnitureType ParseFromXML(string node)
        {
            var x = Main.furnitureDocument.SelectSingleNode("/furniture/decoration[@name='" + node + "']")?.Name ?? Main.furnitureDocument.SelectSingleNode("/furniture/storage[@name='" + node + "']").Name;
            if (x=="storage")
                return StorageType.ParseFromXML(node);
            else if (x == "decoration")
                return DecorationType.ParseFromXML(node);
            return null;
        }

        public class DecorationType : FurnitureType
        {
            public DecorationType(string textureRef) : base(textureRef)
            {
            }

            public static DecorationType ParseFromXML(XmlNode node)
            {
                string _textureRef = node.Attributes["texture"].Value;
                return new DecorationType(_textureRef);
            }

            public new static DecorationType ParseFromXML(string node)
            {
                return ParseFromXML(Main.furnitureDocument.SelectSingleNode("/furniture/decoration[@name='" + node + "']"));
            }

            public static List<DecorationType> GetAllDecorationTypes()
            {
                List<DecorationType> types = new List<DecorationType>();
                foreach (XmlNode n in Main.furnitureDocument.SelectNodes("/furniture/decoration"))
                {
                    types.Add(ParseFromXML(n));
                }
                return types;
            }
        }

        public class StorageType : FurnitureType
        {
            string openedTextureRef;
            public StorageType(string textureRef, string openedTextureRef) : base(textureRef)
            {
                this.openedTextureRef = openedTextureRef;
            }

            public static StorageType ParseFromXML(XmlNode node)
            {
                string _textureRef = node.Attributes["texture"].Value;
                string _oTextureRef = node.Attributes["texture_opened"].Value;
                return new StorageType(_textureRef, _oTextureRef);
            }

            public new static StorageType ParseFromXML(string node)
            {
                return ParseFromXML(Main.furnitureDocument.SelectSingleNode("/furniture/storage[@name='" + node + "']"));
            }

            public static List<StorageType> GetAllStorageTypes()
            {
                List<StorageType> types = new List<StorageType>();
                foreach (XmlNode n in Main.furnitureDocument.SelectNodes("/furniture/storage"))
                {
                    types.Add(ParseFromXML(n));
                }
                return types;
            }
        }
    }
}
