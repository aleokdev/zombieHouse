using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using ZombieProyect_Desktop.Classes;

namespace ZombieProyect_Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Texture2D blankTexture;
        static Texture2D wallTexture;
        static Texture2D[,] wallTextures;
        public static Texture2D[,] wallpapers;
        public static Texture2D[,] floors;
        public static Texture2D[,] doorTextures;
        public static XmlDocument roomsDocument = new XmlDocument();

        public Main()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
            };
            Content.RootDirectory = "Content";
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

            // TODO: use this.Content to load your game content here
            blankTexture = Content.Load<Texture2D>("blank");
            wallTextures = Content.Load<Texture2D>("walls-common").SplitTileset(new Point(16,16));
            wallpapers = Content.Load<Texture2D>("wallpapers").SplitTileset(new Point(16, 16));
            doorTextures = Content.Load<Texture2D>("doors").SplitTileset(new Point(16, 16));
            floors = Content.Load<Texture2D>("floors").SplitTileset(new Point(16, 16));
            string docPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Content\rooms.xml");
            roomsDocument.Load(docPath);
            
            RoomType.GetAllRoomTypes();

            Tile[,] bestTileMap=new Tile[1,1];
            Room[] bestRooms=new Room[1];
            int bestLastRoom=0;
            int bestScore=0;
            for(int h = 0; h < 25; h++)
            {
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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
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
            foreach (Tile t in Map.tileMap)
            {
                Color c = Color.Magenta;
                if (t != null)
                    switch (t?.type)
                    {
                        case TileType.none:
                            c = Color.LightGreen;
                            break;
                        case TileType.floor:
                            if (Keyboard.GetState().IsKeyDown(Keys.Space) && t.parentRoom != null)
                            {
                                c = new Color(t.parentRoom.roomPos.X/(t.parentRoom.roomPos.Y*1f), t.parentRoom.roomPos.Y /(t.parentRoom.roomPos.X * 1f), 0);
                            }
                            else c = Color.LightGray;
                            break;
                        case TileType.wall:
                            c = Color.Gray;
                            break;
                        case TileType.door:
                            c = Color.MonoGameOrange;
                            break;
                        case TileType.blockeddoor:
                            c = Color.Red;
                            break;
                        default:
                            break;
                    }

                if(t!=null)
                    switch (t.type)
                    {
                        case TileType.wall:
                            Texture2D tex = wallTexture;
                            switch (t.GetAccordingTexture())
                            {
                                case WallTextureType.horizontal:
                                    tex = wallTextures[1, 0];
                                    // Set wallpaper
                                    Texture2D wallpaper = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1].parentRoom?.type.wallpaperType ?? 0,0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                    spriteBatch.Draw(wallpaper, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
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
                                    Texture2D wallp = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1].parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                    spriteBatch.Draw(wallp, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
                                    break;
                                case WallTextureType.lefttopcorner:
                                    tex = wallTextures[1, 2];
                                    // Set wallpaper
                                    Texture2D wallpa = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1].parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                    spriteBatch.Draw(wallpa, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
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
                                    Texture2D wallpap = wallpapers[Map.tileMap[t.Pos.X, t.Pos.Y + 1].parentRoom?.type.wallpaperType ?? 0, 0]; // This line here gets the floor below the wall (Since the wall itself isn't on any rooms) and checks its room to get its wallpaper number.
                                    spriteBatch.Draw(wallpap, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
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
                            spriteBatch.Draw(tex, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
                        
                            break;
                        case TileType.door:
                            if(t.GetAccordingTexture()== WallTextureType.horizontal) // Door is horizontal
                                spriteBatch.Draw(doorTextures[0,0], new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
                            if (t.GetAccordingTexture() == WallTextureType.vertical) // Door is vertical
                                spriteBatch.Draw(doorTextures[1, 0], new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
                            break;

                        case TileType.floor:
                            spriteBatch.Draw(floors[t.parentRoom.type.floorType, 0], new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), Color.White);
                            break;

                        default:
                            spriteBatch.Draw(blankTexture, new Rectangle(new Point(t.Pos.X * 32, t.Pos.Y * 32) - Player.pos, new Point(32)), c);
                            break;
                    }
                
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
