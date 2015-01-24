using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using SpriteManager;
using SpriteClasses;

namespace Bob_Lob_the_Blob
{
    public class JelloWorld : Level
    {
        //protected MultiBackground scrollBackground;

        public JelloWorld(ContentManager Content, GraphicsDevice Device, Stream FileStream)
            : base(Content, Device, FileStream)
        {   
           // backgroundImage = content.Load<Texture2D>("Images/JelloWorld/JelloBackground");   //just loads background, will eventually be a layer
            DeclareTileArray(FileStream);

            scrollBackground = new MultiBackground(Device);
            scrollBackground.RemoveLayers();

            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/BlobWorld/Background/jello_background"), 1f, -40f);
            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/BlobWorld/Background/jello_midground"), 0.5f, -70f);
            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/BlobWorld/Background/jello_foreground"), 0.1f, -100f);
            scrollBackground.SetMoveLeftRight();
            scrollBackground.StartMoving();
        }

        //Initializing the ARRAYS
        #region LOADING TILES

        protected override Tile LoadTile(char tileChar, int x, int y)
        {
            //checks what the character is in the specified line
            switch (tileChar) 
            {
                case '#':
                    return LoadTile("tile_4", TileType.Impassable, x, y); // tile_5
                case 'P':
                    return LoadPlayerTile(x, y);
                case '1':
                    return LoadGameObject("Checkpoint1", x, y);
                case '2':
                    return LoadGameObject("Checkpoint2", x, y);
                case '3':
                    return LoadGameObject("Checkpoint3", x, y);
                case '4':
                    return LoadGameObject("Gem1", x, y);
                case '5':
                    return LoadGameObject("Gem2", x, y);
                case '6':
                    return LoadGameObject("Gem3", x, y);

                case 'H':
                    return LoadGameObject("Hat", x, y);
                case 'E':
                    return LoadGameObject("Exit", x, y);
                case 'S':
                    return LoadGameObject("SawBladeLeft", x, y);
                case 'A':
                    return LoadGameObject("SawBladeUp", x, y);

                default:
                    return new Tile(null, TileType.Passable, x, y);
                    throw new NotSupportedException("Unsupported tile type character '{0}' at position {1}, {2}.");
            }
        }

        //Loads the Player tile
        protected override Tile LoadPlayerTile(int x, int y) 
        {
            Vector2 position = new Vector2(x, y) * Tile.Size;
            resetPos = position;
            player = new Player(content.Load<Texture2D>("Images/GameObjects/Blob/Blob"), 1, 1, 0.3f, 0f);
            player.AddAnimation("Idle", 1);
            player.Animation = "Idle";
            player.position = position;
            player.IsLooping = false;
            player.FramesPerSecond = 1;
            player.IsAlive = false;

            return new Tile(null, TileType.Passable, x, y);
        }
        
        //Loads any Game Object
        protected Tile LoadGameObject(string name, int x, int y)
        {
            Vector2 position = new Vector2(x, y) * Tile.Size;;
            switch (name)
            {
                case "Hat":
                    hatIndex = Game1.random.Next(0, Game1.invManager.totalInventoryHats);
                    hatOnLevel = Game1.invManager.GetInventoryHat(hatIndex);
                    hatOnLevel.Position = position;
                    return new Tile(null, TileType.Passable, x, y);

                case "Checkpoint1":
                    listOfCheckpoints[0] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[0].AddAnimation("Rise", 1);
                    listOfCheckpoints[0].Animation = "Rise";
                    listOfCheckpoints[0].position = position;
                    listOfCheckpoints[0].IsLooping = false;
                    listOfCheckpoints[0].FramesPerSecond = 8;
                    listOfCheckpoints[0].IsActive = false;
                    return new Tile(null, TileType.Passable, x, y);
                
                case "Checkpoint2":
                    listOfCheckpoints[1] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[1].AddAnimation("Rise", 1);
                    listOfCheckpoints[1].Animation = "Rise";
                    listOfCheckpoints[1].position = position;
                    listOfCheckpoints[1].IsLooping = false;
                    listOfCheckpoints[1].FramesPerSecond = 8;
                    listOfCheckpoints[1].IsActive = false;
                    return new Tile(null, TileType.Passable, x, y);
                    
                case "Checkpoint3":
                    listOfCheckpoints[2] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[2].AddAnimation("Rise", 1);
                    listOfCheckpoints[2].Animation = "Rise";
                    listOfCheckpoints[2].position = position;
                    listOfCheckpoints[2].IsLooping = false;
                    listOfCheckpoints[2].FramesPerSecond = 8;
                    listOfCheckpoints[2].IsActive = false;
                    
                    return new Tile(null, TileType.Passable, x, y);

                case "Gem1":
                    listOfGems[0] = (new GameObject(name, content.Load<Texture2D>("Images/Collectibles/TimeCrystal"), 1, 1, 1.2f, 0f));
                    listOfGems[0].AddAnimation("Float", 1);
                    listOfGems[0].Animation = "Float";
                    listOfGems[0].position = position;
                    listOfGems[0].IsLooping = false;
                    listOfGems[0].FramesPerSecond = 3;
                    listOfGems[0].IsActive = true;
                    return new Tile(null, TileType.Passable, x, y);

                case "Gem2":
                    listOfGems[1] = (new GameObject(name, content.Load<Texture2D>("Images/Collectibles/TimeCrystal"), 1, 1, 1.2f, 0f));
                    listOfGems[1].AddAnimation("Float", 1);
                    listOfGems[1].Animation = "Float";
                    listOfGems[1].position = position;
                    listOfGems[1].IsLooping = false;
                    listOfGems[1].FramesPerSecond = 3;
                    listOfGems[1].IsActive = true;
                    return new Tile(null, TileType.Passable, x, y);

                case "Gem3":
                    listOfGems[2] = (new GameObject(name, content.Load<Texture2D>("Images/Collectibles/TimeCrystal"), 1, 1, 1.2f, 0f));
                    listOfGems[2].AddAnimation("Float", 1);
                    listOfGems[2].Animation = "Float";
                    listOfGems[2].position = position;
                    listOfGems[2].IsLooping = false;
                    listOfGems[2].FramesPerSecond = 3;
                    listOfGems[2].IsActive = true;

                    return new Tile(null, TileType.Passable, x, y);
                    
                case "Exit":
                    endPortal = (new GameObject(name, content.Load<Texture2D>("Images/end_portal"), 1, 1, 1.2f, 0f));
                    endPortal.AddAnimation("Float", 1);
                    endPortal.Animation = "Float";
                    endPortal.position = position;
                    endPortal.IsLooping = false;
                    endPortal.FramesPerSecond = 3;
                    endPortal.IsActive = false;

                    return new Tile(null, TileType.Passable, x, y);


                case "SawBladeLeft":
                    listOfGameObjects.Add(new GameObject(name, content.Load<Texture2D>("Images/GameObjects/SawBlades/SawBladeLeft"), 6, 1, 0.345f, 0f));
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Spin", 1);
                    listOfGameObjects[listOfGameObjects.Count - 1].Animation = "Spin";
                    listOfGameObjects[listOfGameObjects.Count - 1].position = position;
                    listOfGameObjects[listOfGameObjects.Count - 1].IsLooping = true;
                    listOfGameObjects[listOfGameObjects.Count - 1].FramesPerSecond = 20;
                    listOfGameObjects[listOfGameObjects.Count - 1].FrameIndex = Game1.random.Next(0, 6);
                    
                    return new Tile(null, TileType.Passable, x, y);
                case "SawBladeUp":
                    listOfGameObjects.Add(new GameObject(name, content.Load<Texture2D>("Images/GameObjects/SawBlades/SawBladeUp"), 6, 1, 0.2f, 0f));
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Spin", 1);
                    listOfGameObjects[listOfGameObjects.Count - 1].Animation = "Spin";
                    listOfGameObjects[listOfGameObjects.Count - 1].position = position;
                    listOfGameObjects[listOfGameObjects.Count - 1].IsLooping = true;
                    listOfGameObjects[listOfGameObjects.Count - 1].FramesPerSecond = 15;
                    listOfGameObjects[listOfGameObjects.Count - 1].FrameIndex = Game1.random.Next(0, 6);
                    return new Tile(null, TileType.Passable, x, y);
                  
                default:
                    break;
            }
            throw new Exception("Trying to add an object with an incorrect name @ " + x + "," + y);
        }
        
        //Loads Tiles
        protected override Tile LoadTile(string fileName, TileType tileType, int x, int y)
        {
            return new Tile(content.Load<Texture2D>("Images/Levels/BlobWorld/Tiles/" + fileName), tileType, x, y);
        }

        #endregion
    }
}
