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
    class Egyptian : Level
    {
        //Constructor
        public Egyptian(ContentManager Content, GraphicsDevice Device, Stream FileStream)
            : base(Content, Device, FileStream)
        {       
            DeclareTileArray(FileStream);

            scrollBackground = new MultiBackground(Device);
            scrollBackground.RemoveLayers();

            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/EgyptianWorld/Background/egypt_background"), 1f, -40f);
            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/EgyptianWorld/Background/egypt_midground"), 0.5f, -70f);
            scrollBackground.AddLayer(Content.Load<Texture2D>("Images/Levels/EgyptianWorld/Background/egypt_foreground"), 0.1f, -100f);
            scrollBackground.SetMoveLeftRight();
            scrollBackground.StartMoving();            
        }

        #region LOADING TILES
        
        //Main Load Tile Method
        protected override Tile LoadTile(char tileChar, int x, int y)
        {
            switch (tileChar) //checks what the character is in the specified line
            {
                case '#':
                    switch (Game1.random.Next(10))
                    {
                        case 1:
                            return LoadTile("egypt tile cracked", TileType.Impassable, x, y);
                        default:
                            return LoadTile("egypt tile", TileType.Impassable, x, y);

                    }

                case '@':
                    return LoadTile("egypt_tile", TileType.Impassable, x, y);
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


                //Saws
                case 'h':
                    return LoadGameObject("SawBladeUp", x, y);




                //Elevators
                case 'Y':
                    return LoadGameObject("Elevator1", x, y);
                case 'y':
                    return LoadGameObject("ESwitch1", x, y);

                case 'J':
                    return LoadGameObject("Elevator2", x, y);
                case 'j':
                    return LoadGameObject("ESwitch2", x, y);

                case 'F':
                    return LoadGameObject("Elevator3", x, y);
                case 'f':
                    return LoadGameObject("ESwitch3", x, y);

                case 'N':
                    return LoadGameObject("Elevator4", x, y);
                case 'n':
                    return LoadGameObject("ESwitch4", x, y);

                case 'G':
                    return LoadGameObject("Elevator5", x, y);
                case 'g':
                    return LoadGameObject("ESwitch5", x, y);

                case 'B':
                    return LoadGameObject("Elevator10", x, y);
                case 'b':
                    return LoadGameObject("ESwitch10", x, y);






                //DOOR CHARACTERS
                //1
                case 'X':
                    return LoadGameObject("Door1", x, y);
                case 'x':
                    return LoadGameObject("Switch1", x, y);
                //2
                case 'S':
                    return LoadGameObject("Door2", x, y);
                case 's':
                    return LoadGameObject("Switch2", x, y);
                //3
                case 'T':
                    return LoadGameObject("Door3", x, y);
                case 't':
                    return LoadGameObject("Switch3", x, y);
                //4
                case 'Q':
                    return LoadGameObject("Door4", x, y);
                case 'q':
                    return LoadGameObject("Switch4", x, y);
                //5
                case 'A':
                    return LoadGameObject("Door5", x, y);
                case 'a':
                    return LoadGameObject("Switch5", x, y);
                //6
                case 'W':
                    return LoadGameObject("Door6", x, y);
                case 'w':
                    return LoadGameObject("Switch6", x, y);


                case 'U':
                    return LoadGameObject("ArrowUp", x, y);
                case 'D':
                    return LoadGameObject("ArrowDown", x, y);
                case 'R':
                    return LoadGameObject("ArrowRight", x, y);
                case 'L':
                    return LoadGameObject("ArrowLeft", x, y);

                default:
                    return new Tile(null, TileType.Passable, x, y);
                    throw new NotSupportedException("Unsupported tile type character '{0}' at position {1}, {2}.");
            }
        }

        //Load Tile Method
        protected override Tile LoadTile(string fileName, TileType tileType, int x, int y)
        {
            return new Tile(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Tiles/" + fileName), tileType, x, y);
        }

        //Load the Player Tile
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
            player.IsAlive = true;



            return new Tile(null, TileType.Passable, x, y);
        }

        //Load all Game Objects
        protected Tile LoadGameObject(string name, int x, int y)
        {
            Vector2 position = new Vector2(x, y) * Tile.Size; ;
            switch (name)
            {
                case "Hat":
                    hatIndex = Game1.random.Next(0, Game1.invManager.totalInventoryHats);
                    hatOnLevel = Game1.invManager.GetInventoryHat(hatIndex);
                    hatOnLevel.Position = position;
                    return new Tile(null, TileType.Passable, x, y);

                case "Checkpoint1":
                    listOfCheckpoints[0] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[0].AddAnimation("GLOW", 1);
                    listOfCheckpoints[0].Animation = "GLOW";
                    listOfCheckpoints[0].position = position;
                    listOfCheckpoints[0].IsLooping = false;
                    listOfCheckpoints[0].FramesPerSecond = 9;
                    listOfCheckpoints[0].IsActive = false;
                    return new Tile(null, TileType.Passable, x, y);

                case "Checkpoint2":
                    listOfCheckpoints[1] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[1].AddAnimation("GLOW", 1);
                    listOfCheckpoints[1].Animation = "GLOW";
                    listOfCheckpoints[1].position = position;
                    listOfCheckpoints[1].IsLooping = false;
                    listOfCheckpoints[1].FramesPerSecond = 9;
                    listOfCheckpoints[1].IsActive = false;
                    return new Tile(null, TileType.Passable, x, y);

                case "Checkpoint3":
                    listOfCheckpoints[2] = (new GameObject(name, content.Load<Texture2D>("Images/GameObjects/checkpoint_ss"), 10, 1, 1f, 0f));
                    listOfCheckpoints[2].AddAnimation("GLOW", 1);
                    listOfCheckpoints[2].Animation = "GLOW";
                    listOfCheckpoints[2].position = position;
                    listOfCheckpoints[2].IsLooping = false;
                    listOfCheckpoints[2].FramesPerSecond = 9;
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
                    listOfGameObjects.Add(new GameObject(name, content.Load<Texture2D>("Images/GameObjects/SawBlades/SawBladeUp"), 6, 1, 0.1f, 0f));
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Spin", 1);
                    listOfGameObjects[listOfGameObjects.Count - 1].Animation = "Spin";
                    listOfGameObjects[listOfGameObjects.Count - 1].position = position;
                    listOfGameObjects[listOfGameObjects.Count - 1].IsLooping = true;
                    listOfGameObjects[listOfGameObjects.Count - 1].FramesPerSecond = 15;
                    listOfGameObjects[listOfGameObjects.Count - 1].FrameIndex = Game1.random.Next(0, 6);
                    return new Tile(null, TileType.Passable, x, y);

                case "ArrowUp":
                    listOfEnemies.Add(new ArrowTrap(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Tiles/arrow trap"), 1, 1, 1f, 0f));
                    listOfEnemies[listOfEnemies.Count - 1].AddAnimation("Idle", 1);
                    listOfEnemies[listOfEnemies.Count - 1].Animation = "Idle";
                    listOfEnemies[listOfEnemies.Count - 1].position = position;
                    listOfEnemies[listOfEnemies.Count - 1].IsLooping = true;
                    listOfEnemies[listOfEnemies.Count - 1].FramesPerSecond = 15;
                    return LoadTile("egypt tile", TileType.Impassable, x, y);
                case "ArrowDown":
                    listOfEnemies.Add(new ArrowTrap(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Tiles/arrow trap"), 1, 1, 1f, 0f));
                    listOfEnemies[listOfEnemies.Count - 1].AddAnimation("Idle", 1);
                    listOfEnemies[listOfEnemies.Count - 1].Animation = "Idle";
                    listOfEnemies[listOfEnemies.Count - 1].position = position;
                    listOfEnemies[listOfEnemies.Count - 1].IsLooping = true;
                    listOfEnemies[listOfEnemies.Count - 1].FramesPerSecond = 15;
                    return LoadTile("egypt tile", TileType.Impassable, x, y);
                case "ArrowLeft":
                    listOfEnemies.Add(new ArrowTrap(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Tiles/arrow trap"), 1, 1, 1f, 0f));
                    listOfEnemies[listOfEnemies.Count - 1].AddAnimation("Idle", 1);
                    listOfEnemies[listOfEnemies.Count - 1].Animation = "Idle";
                    listOfEnemies[listOfEnemies.Count - 1].position = position;
                    listOfEnemies[listOfEnemies.Count - 1].IsLooping = true;
                    listOfEnemies[listOfEnemies.Count - 1].FramesPerSecond = 15;
                    return LoadTile("egypt tile", TileType.Impassable, x, y);
                case "ArrowRight":
                    listOfEnemies.Add(new ArrowTrap(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Tiles/arrow trap"), 1, 1, 1f, 0f));
                    listOfEnemies[listOfEnemies.Count - 1].AddAnimation("Idle", 1);
                    listOfEnemies[listOfEnemies.Count - 1].Animation = "Idle";
                    listOfEnemies[listOfEnemies.Count - 1].position = position;
                    listOfEnemies[listOfEnemies.Count - 1].IsLooping = true;
                    listOfEnemies[listOfEnemies.Count - 1].FramesPerSecond = 15;
                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Door1":
                case "Door2":
                case "Door3":
                case "Door4":
                case "Door5":
                case "Door6":
                case "Door7":
                case "Door8":
                case "Door9":
                case "Door10":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Door"), 1, 1, 1f, 0f, position, DoorType.Normal, 4));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;
                    listOfDoors[listOfDoors.Count - 1].IsLooping = false;


                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Switch1":
                case "Switch2":
                case "Switch3":
                case "Switch4":
                case "Switch5":
                case "Switch6":
                case "Switch7":
                case "Switch8":
                case "Switch9":
                case "Switch10":
                    listOfGameObjects.Add(new GameObject(name, content.Load<Texture2D>("Images/GameObjects/Switch/Switch"), 1, 2, 1f, 0f));
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Up", 1);
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Down", 2);
                    listOfGameObjects[listOfGameObjects.Count - 1].Animation = "Up";
                    listOfGameObjects[listOfGameObjects.Count - 1].position = position;
                    return new Tile(null, TileType.Passable, x, y);
                
                case "Elevator1":
                case "Elevator6":
                case "Elevator7":
                case "Elevator8":
                case "Elevator9":

                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Door"), 1, 1, 1f, 0f, position, DoorType.Elevator, 44));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Elevator2":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Door"), 1, 1, 1f, 0f, position, DoorType.Elevator, 14));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Elevator3":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Door"), 1, 1, 1f, 0f, position, DoorType.Elevator, 15));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Elevator4":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/Door"), 1, 1, 1f, 0f, position, DoorType.SlowDecending, 13));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return LoadTile("egypt tile", TileType.Impassable, x, y);

                case "Elevator5":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/TwoTileDoor"), 1, 1, 1f, 0f, position, DoorType.PushingRight, 45));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return new Tile(null, TileType.Passable, x, y);


                case "Elevator10":
                    listOfDoors.Add(new Door(name, content.Load<Texture2D>("Images/Levels/EgyptianWorld/TwoTileDoor"), 1, 1, 1f, 0f, position, DoorType.Rocket, 75));
                    listOfDoors[listOfDoors.Count - 1].AddAnimation("Idle", 1);
                    listOfDoors[listOfDoors.Count - 1].Animation = "Idle";
                    listOfDoors[listOfDoors.Count - 1].position = position;

                    return new Tile(null, TileType.Passable, x, y);



                case "ESwitch1":
                case "ESwitch2":
                case "ESwitch3":
                case "ESwitch4":
                case "ESwitch5":
                case "ESwitch6":
                case "ESwitch7":
                case "ESwitch8":
                case "ESwitch9":
                case "ESwitch10":
                    listOfGameObjects.Add(new GameObject(name, content.Load<Texture2D>("Images/GameObjects/Switch/Switch"), 1, 2, 1f, 0f));
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Up", 1);
                    listOfGameObjects[listOfGameObjects.Count - 1].AddAnimation("Down", 2);
                    listOfGameObjects[listOfGameObjects.Count - 1].Animation = "Up";
                    listOfGameObjects[listOfGameObjects.Count - 1].position = position;
                    return new Tile(null, TileType.Passable, x, y);
               

                default:
                    break;
            }
            throw new Exception("TrSprite1 to add an object with an incorrect name @ " + x + "," + y);
        }

        #endregion

        //Service Methods

        //Sarcoph Timer
        protected void SarcouphagusTimerUpdate(GameTime gameTime)
        {
            const int TOTALTIME = 10;
            sarcophagus.WaitingTimer += (gameTime.ElapsedGameTime.Milliseconds / 1000);
            if (sarcophagus.WaitingTimer > TOTALTIME)
            {
                sarcophagus.IsActive = false;
                sarcophagus.WaitingTimer = 0;
            }
        }


    }//End of Class
}//End of Namespace
