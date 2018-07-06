using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using ZombieProyect_Desktop.Classes;
using ZombieProyect_Desktop.Classes.Tiles;

namespace ZombieProyect_Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D blankTexture;
        static Texture2D[,] wallTextures;
        public static Texture2D[,] wallpapers;
        public static Texture2D[,] floors;
        public static Texture2D[,] doorTextures;
        public static XmlDocument roomsDocument = new XmlDocument();
        public static XmlDocument furnitureDocument = new XmlDocument();
        public static Dictionary<string, Texture2D> furnitureTextures = new Dictionary<string, Texture2D>();
        public static SpriteFont font;
        public static FrameCounter fpsC = new FrameCounter();
        public static int tileSize = 16;
        public static readonly string texturePackage = "sekritDLC_CrappyTextures";
        public static ContentManager contentManager;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Assign contentManager
            contentManager = Content;

            // TODO: use this.Content to load your game content here
            blankTexture = Content.Load<Texture2D>("textures/blank");
            wallTextures = Content.Load<Texture2D>("textures/" + texturePackage + "/walls-common").SplitTileset(new Point(16,16));
            wallpapers = Content.Load<Texture2D>("textures/" + texturePackage + "/wallpapers").SplitTileset(new Point(16, 16));
            doorTextures = Content.Load<Texture2D>("textures/" + texturePackage + "/doors").SplitTileset(new Point(16, 16));
            floors = Content.Load<Texture2D>("textures/" + texturePackage + "/floors").SplitTileset(new Point(16, 16));
            string docPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Content\rooms.xml");
            roomsDocument.Load(docPath);
            docPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Content\furniture.xml");
            furnitureDocument.Load(docPath);
            font = Content.Load<SpriteFont>("font");
            
            RoomType.GetAllRoomTypes();

            Tile[,] bestTileMap=new Tile[1,1];
            Room[] bestRooms=new Room[1];
            int bestLastRoom=0;
            int bestScore=0;
            Console.WriteLine("Generating houses.");
            for (int h = 0; h < 25; h++)
            {
                Console.WriteLine(h / 25f * 100 + "%; h = " + h);
                Map.GenerateHouse(15, out int currentBranches, out int currentRooms);
                if (currentBranches + currentRooms > bestScore)
                {
                    bestTileMap = Map.tileMap;
                    bestRooms = Map.rooms;
                    bestLastRoom = Map.lastRoom;
                    bestScore = currentRooms * currentBranches;
                }
            }
            Map.tileMap = bestTileMap;
            Map.rooms = bestRooms;
            Map.lastRoom = bestLastRoom;

            Console.WriteLine("Placing doors.");
            Map.PlaceDoorsBetweenAllRooms();

            Console.WriteLine("Placing furniture.");
            Map.GenerateFurniture();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        int lastScrollValue;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Mouse.GetState().ScrollWheelValue != lastScrollValue)
            {
                tileSize += (Mouse.GetState().ScrollWheelValue - lastScrollValue)/32;
                lastScrollValue = Mouse.GetState().ScrollWheelValue;
            }
            // TODO: Add your update logic here
            Player.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp, blendState:BlendState.AlphaBlend);

            bool roomView = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) roomView = true;
            
            foreach (Tile t in Map.tileMap)
            {
                Color c = Color.White;
                if (t != null)
                {
                    if (roomView)
                    {
                        if (t?.parentRoom == null)
                            c = Color.Gray;
                        else
                            c = new Color((t.parentRoom.roomPos.X+1) / ((t.parentRoom.roomPos.Y+1) * 1f), (t.parentRoom.roomPos.Y+1) / ((t.parentRoom.roomPos.X+1) * 1f), 0);
                    }

                    switch (t)
                    {
                        case Wall _w:
                            if (_w.GetType() == typeof(Door))
                            {
                                if (t.GetAccordingTexture() == WallTextureType.horizontal) // Door is horizontal
                                    spriteBatch.Draw(doorTextures[0, 0], new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                                if (t.GetAccordingTexture() == WallTextureType.vertical) // Door is vertical
                                    spriteBatch.Draw(doorTextures[1, 0], new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                            }
                            else
                            {
                                Texture2D tex = blankTexture;
                                switch (t.GetAccordingTexture())
                                {
                                    case WallTextureType.horizontal:
                                        tex = wallTextures[1, 0];
                                        // Set wallpaper
                                        Texture2D wallpaper = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1].parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                        spriteBatch.Draw(wallpaper, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                                        break;
                                    case WallTextureType.vertical:
                                        tex = wallTextures[0, 1];
                                        break;
                                    case WallTextureType.rightbottomcorner:
                                        tex = wallTextures[0, 0];
                                        break;
                                    case WallTextureType.leftbottomcorner:
                                        tex = wallTextures[1, 1];
                                        break;
                                    case WallTextureType.righttopcorner:
                                        tex = wallTextures[0, 2];
                                        // Set wallpaper
                                        Texture2D wallp = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1]?.parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                        spriteBatch.Draw(wallp, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                                        break;
                                    case WallTextureType.lefttopcorner:
                                        tex = wallTextures[1, 2];
                                        // Set wallpaper
                                        Texture2D wallpa = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1]?.parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                        spriteBatch.Draw(wallpa, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                                        break;
                                    case WallTextureType.allbutupjoint:
                                        tex = wallTextures[3, 0];
                                        break;
                                    case WallTextureType.allbutrightjoint:
                                        tex = wallTextures[2, 1];
                                        break;
                                    case WallTextureType.allbutbottomjoint:
                                        tex = wallTextures[2, 0];
                                        // Set wallpaper
                                        Texture2D wallpap = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1]?.parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                        spriteBatch.Draw(wallpap, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                                        break;
                                    case WallTextureType.allbutleftjoint:
                                        tex = wallTextures[2, 2];
                                        break;
                                    case WallTextureType.all:
                                        tex = wallTextures[3, 1];
                                        break;
                                    case WallTextureType.unknown:
                                        tex = blankTexture;
                                        break;
                                    default:
                                        break;
                                }
                                spriteBatch.Draw(tex, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), Color.White);
                            } break;
                            
                        case Floor _w:
                            spriteBatch.Draw(floors[t.parentRoom.type.floorType, 0], new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), c);
                            break;

                        case OutsideFloor _w:
                            spriteBatch.Draw(floors[3, 0], new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), c);
                            break;

                        default:
                            spriteBatch.Draw(blankTexture, new Rectangle(new Point(t.Pos.X * tileSize, t.Pos.Y * tileSize) - (Player.pos * tileSize).ToPoint(), new Point(tileSize)), c);
                            break;
                    }
                }

                if (roomView)
                {
                    for (int r = 0; r < Map.rooms.Length; r++)
                    {
                        Room room = Map.rooms[r];
                        if (room == null) continue;
                        spriteBatch.DrawString(font, "Room " + r, room.roomPos.ToVector2()* tileSize - Player.pos*tileSize, Color.White);
                        spriteBatch.DrawString(font, Map.rooms[r].type.name ?? "", room.roomPos.ToVector2() * tileSize - Player.pos * tileSize+ new Vector2(20), Color.White);
                    }
                }
            }

            foreach(MapObject o in Map.objectMap)
            {
                o.Draw(spriteBatch);
            }

            #region Frame counter
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            fpsC.Update(deltaTime);

            spriteBatch.DrawString(font, fpsC.AverageFramesPerSecond + " FPS", new Vector2(1, 1), Color.Magenta);
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
