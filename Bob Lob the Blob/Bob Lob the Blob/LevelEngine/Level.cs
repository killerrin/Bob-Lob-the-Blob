using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using SpriteManager;
using SpriteClasses;
namespace Bob_Lob_the_Blob
{
    public enum CollisionArea
    {
        Top,
        Bottom,
        Left,
        Right
    }
    public class Level
    {
        
    #region VARIABLES
        //Camera Variables
        private float cameraPositionXAxis;
        private float cameraPositionYAxis;
       
        //Level Variables
        public int LevelWidth { get { return tiles.GetLength(0); } }
        public int LevelHeight { get { return tiles.GetLength(1); } }
        private Tile[,] tiles;
        protected Vector2 resetPos; 

        //List Variables
        protected static GameObject[] listOfCheckpoints;
        protected List<GameObject> listOfGameObjects;
        public GameObject[] listOfGems { private set; get; }
        protected List<GameObject> listOfEnemies;
        protected List<GameObject> listOfDoors;
        
        //Hat Variables
        protected int hatIndex;
        protected Hat hatOnLevel;
        protected bool hatTaken;
        
        //Sprite Variables
        protected static Player player; 
        protected Texture2D backgroundImage;
        protected Sprite portal;
        protected static Sarcophagus sarcophagus;
        public MultiBackground scrollBackground;

        //GUI Variables
        private UserInterface ui;
        public bool HatCollected;
        public int currPortal;
        private bool isDead;
        public int deathToll;

        //Time Variables
        protected float portalTime;
        protected float timeElapsed;
        
        //Variables
        public int totalGemsCollected = 0;
        protected int portalCount;
        protected GameObject endPortal;

        //ArrowTrap Variables
        private bool removeArrow = false;

        //Door Variables
        protected bool doorCollide = false;

        //ETC Variables
        public static Debugger debug = new Debugger();
        protected ContentManager content; 
        
    #endregion
        
  
        //constructor
        public Level(ContentManager contentManager, GraphicsDevice Device, Stream fileStream)
        {
            content = contentManager;//so I can use the content from game1
            listOfGameObjects = new List<GameObject>();
            listOfCheckpoints = new GameObject[3];
            listOfGems = new GameObject[3];
            listOfEnemies = new List<GameObject>();
            listOfDoors = new List<GameObject>();

            hatIndex = 0;
            hatOnLevel = Game1.invManager.GetInventoryHat(hatIndex);
            hatTaken = false;
            HatCollected = false;

            currPortal = 1;
            deathToll = 0;
            ui = new UserInterface(contentManager, Device);
            
            portal = new Sprite(content.Load<Texture2D>("Images/GameObjects/Portal/Portal"), 6, 1, 1, 0);
            portal.IsAlive = false;

            sarcophagus = new Sarcophagus(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Sarcophagus"), 1, 1, 0.2f, 0);
            sarcophagus.AddAnimation("Idle", 1);
            sarcophagus.Animation = "Idle";
            sarcophagus.IsLooping = false;
            sarcophagus.FramesPerSecond = 1;
            sarcophagus.IsActive = false;
        }


    #region LOAD TILES
        //Declaring THE ARRAYS
        protected void DeclareTileArray(Stream fileStream)
        {
            int width = 0;
            int lineLength = 0;
            Char tileChar;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))    //Can't explain "using" neccesarily but pretty much creating a StreamReader object and passing the fileStream which loaded the text file back in Game
            {
                //stores a line from the text file into line
                string line = reader.ReadLine();
                //loops through every character in the line that was just stored into line
                foreach (char ch in line)
                    width++;  //in other words, gets x component 
                while (line != null)//cycles through text file till theres no more lins. 
                {
                    lines.Add(line); //adds the current line to Lines List which is a local variable but used for tile initialization. eg. Tile[,] tiles = new Tile[width, line.Count]. We initialize once we get out of this while loop 
                    foreach (char ch in line)
                        lineLength++; //gets u component

                    if (lineLength != width) //makes sure all lines r sale length, returns error telling which line has error
                        throw new Exception(String.Format("The Length of line {0} is different from the preceding line.", lines.Count));

                    line = reader.ReadLine(); // reads next line and during next loop will add to local variable linesList which we use for Tile array initialization
                    lineLength = 0; 
                }
            }
            tiles = new Tile[width, lines.Count]; // creats the size of the array based on the width and number of lines. so X and Y
            //after initializing the tiles array we now loop over every tile position and load an image
            for (int y = 0; y < LevelHeight; y++) 
            {
                for (int x = 0; x < LevelWidth; x++)
                {       //grabs y and x or another way of saSprite1 it is line y char x.
                    tileChar = lines[y][x];    
                    tiles[x, y] = LoadTile(tileChar, x, y); //calls method
                }
            }
        }
        protected virtual Tile LoadTile(char tileChar, int x, int y)
        {
              throw new NotSupportedException("Dont use objects of Level, use derived classes");
        }
        //ALL kinds of tiles go through this function
        protected virtual Tile LoadTile(string fileName, TileType tileType, int x, int y)
        {
            throw new Exception("You shouldnt be using this....... Who there? Use a derived class not the base so we can have a larger index for level");
        }
        //this method returns a texture null tile but grabs the x and y of the screen for player position
        protected virtual Tile LoadPlayerTile(int x, int y)  //Can be used to get enemies objects aswell
        {
            throw new Exception("call me empty ishmail. use a derived class of Level");
        }
    #endregion

    #region COLLISION DETECTION
        //CHECKS FOR COLLISIONS BETWEEN TWO SPRITES
        public void CollisionCheckSpriteVsSprite(GameObject Sprite1, GameObject Sprite2)
        {
           
            if (Sprite1.Name == "Bob")
            {
                #region Blob Vs Arrow Traps
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite2.Name == "ArrowUp" || Sprite2.Name == "ArrowDown" || Sprite2.Name == "ArrowLeft" || Sprite2.Name == "ArrowRight") && Sprite1.IsAlive)
                {
                    switch (Sprite2.Name)
                    {
                            case "ArrowUp":
                                Sprite2.ShootArrow(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Arrows/ArrowUp"));
                                break;
                            case "ArrowDown":
                                Sprite2.ShootArrow(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Arrows/ArrowDown"));
                                break;
                            case "ArrowRight":
                                Sprite2.ShootArrow(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Arrows/ArrowRight"));
                                break;
                            case "ArrowLeft":
                                Sprite2.ShootArrow(content.Load<Texture2D>("Images/Levels/EgyptianWorld/Arrows/ArrowLeft"));
                                break;


                            default:
                                break;
                        }
                        
                    }
                    else
                        Sprite2.targetInRange = false;

                #endregion

                #region Blob Vs Enemy or Saws
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite2.Name == "Enemy" || Sprite2.Name == "SawBladeLeft" || Sprite2.Name == "SawBladeUp"))
                {
                    Sprite1.IsAlive = false;
                    Sprite1.AppliedImpulse = false;
                    isDead = true;
                }
                #endregion

                #region Blob Vs Collectables
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && Sprite2.IsActive && Sprite1.IsAlive && Sprite1.Name == "Bob" && (Sprite2.Name == "Gem1" || Sprite2.Name == "Gem2" || Sprite2.Name == "Gem3"))
                {
                    Sprite2.IsActive = false;
                    switch (Sprite2.Name)
                    {
                        case "Gem1":
                            ui.ElementList[1].Active = true;
                            listOfGems[0].IsObtained = true;
                            totalGemsCollected++;
                            break;
                        case "Gem2":
                            ui.ElementList[2].Active = true;
                            listOfGems[1].IsObtained = true;
                            totalGemsCollected++;
                            break;
                        case "Gem3":
                            ui.ElementList[3].Active = true;
                            listOfGems[2].IsObtained = true;
                            totalGemsCollected++;
                            break;
                        default:
                            break;
                    }
                    if (totalGemsCollected >= 2)
                    {
                        endPortal.IsActive = true;
                    }
                }
                #endregion

                #region Blob Vs Arrow
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && Sprite2.Name == "Arrow" && Sprite1.IsAlive)
                {
                    Sprite1.IsAlive = false;
                    removeArrow = true;
                    isDead = true;
                }
                
                #endregion

                #region Blob Vs Checkpoints
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && !Sprite2.IsActive && Sprite1.IsAlive && (Sprite2.Name == "Checkpoint1" || Sprite2.Name == "Checkpoint2" || Sprite2.Name == "Checkpoint3"))
                {
                    if (!Sprite2.IsActive)
                    {
                        Sprite2.IsActive = true;
                        Sprite2.FrameIndex = 1;
                        Sprite2.IsLooping = true;
                        switch (Sprite2.Name)
                        {
                            case "Checkpoint1":
                                player.CurrentCheckpoint = 1;
                                break;
                            case "Checkpoint2":
                                player.CurrentCheckpoint = 2;
                                break;
                            case "Checkpoint3":
                                player.CurrentCheckpoint = 3;
                                break;
                            default:
                                break;
                        }
                    }

                }
                #endregion

                #region Blob Vs Sarcough
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && Sprite1.IsAlive && Sprite2.IsActive && Sprite2.Name == "Sarcophagus")
                {
                    bool isNForce = false;
                    bool vertCollision = false;
                    CollisionArea CollisionState;

                    if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle))
                    {//Top side of Sarcough
                        if (Sprite1.CollisionRectangle.Intersects(Sprite2.TopRectangle))
                        {
                            CollisionState = CollisionArea.Top;
                            Sprite1.position.Y = Sprite2.CollisionRectangle.Top - Sprite1.height * Sprite1.Scale + 1.5f;

                            if (!isNForce)
                            {
                                Sprite1.Force.Y += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);

                                if (Sprite1.InitialVelocity.X != 0)
                                {
                                    Sprite1.Force.X += Physics.getFrictionForce(Sprite1.mass, Sprite1.Force, Sprite1.InitialVelocity);
                                }
                                isNForce = true;
                            }
                            if (!Sprite1.AppliedImpulse)
                            {
                                Sprite1.Force.Y += Physics.applyImpulseForce(Sprite1.mass, Sprite1.InitialVelocity, timeElapsed, Sprite1.IsBouncing, Sprite1.Force);
                                Sprite1.AppliedImpulse = true;
                            }
                            Sprite1.IsPlayerFalling = false;
                            Sprite1.IsPlayerJumping = false;
                            vertCollision = true;

                        }//Bottom side of Sarcough
                        if (Sprite1.CollisionRectangle.Intersects(Sprite2.BottomRectangle))
                        {
                            Sprite2.position.Y = Sprite1.CollisionRectangle.Top - Sprite2.height * Sprite2.Scale;
                            CollisionState = CollisionArea.Bottom;

                            if (!isNForce)
                            {
                                Sprite1.Force.Y += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                                isNForce = true;
                            }
                            if (!Sprite1.AppliedImpulse)
                            {
                                Sprite1.Force.Y += Physics.applyImpulseForce(Sprite1.mass, Sprite1.InitialVelocity, timeElapsed, Sprite1.IsBouncing, Sprite1.Force);
                                Sprite1.AppliedImpulse = true;
                            }
                            vertCollision = true;
                        }
                        if (!vertCollision)
                        {//Right side of the Sarcough
                            if (Sprite1.CollisionRectangle.Intersects(Sprite2.RightRectangle))
                            {
                                Sprite2.position.X = Sprite1.CollisionRectangle.Left - Sprite2.width * Sprite2.Scale;
                                CollisionState = CollisionArea.Right;

                                if (!isNForce)
                                {
                                    Sprite1.Force.X += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                                    isNForce = true;
                                }
                                Sprite2.InitialVelocity.X = 0;
                            }// Left side of Sarcough
                            if (Sprite1.CollisionRectangle.Intersects(Sprite2.LeftRectangle))
                            {
                                Sprite2.position.X = Sprite1.CollisionRectangle.Right;
                                CollisionState = CollisionArea.Left;
                                Sprite2.InitialVelocity.X = 0;
                            }
                        }
                    }
                }
                #endregion

                #region Blob Vs Exit
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite2.Name == "Exit") && Sprite2.IsActive && Sprite1.Name == "Bob")
                {
                    player.IsAlive = false;
                    Game1.gameState = GameState.EndLevelMenu;
                }
                #endregion
            }

            

            #region Arrow Vs Sarcouphagus
            if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite1.Name == "Arrow") && (Sprite2.Name == "Sarcophagus") && Sprite2.IsActive)
            {
                removeArrow = true;
            }
            #endregion

            #region Blob or Sarcough Vs Switches
            if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite1.IsActive || Sprite1.IsAlive) && (Sprite2.Name == "Switch1" || Sprite2.Name == "Switch2" || Sprite2.Name == "Switch3" || Sprite2.Name == "Switch4" || Sprite2.Name == "Switch5" || Sprite2.Name == "Switch6" || Sprite2.Name == "Switch7" || Sprite2.Name == "Switch8" || Sprite2.Name == "Switch9" || Sprite2.Name == "Switch10"
                                                                                    ||  Sprite2.Name == "ESwitch1" || Sprite2.Name == "ESwitch2" || Sprite2.Name == "ESwitch3" || Sprite2.Name == "ESwitch4" || Sprite2.Name == "ESwitch5" || Sprite2.Name == "ESwitch6" && Sprite2.Name == "ESwitch7" || Sprite2.Name == "ESwitch8" || Sprite2.Name == "ESwitch9" || Sprite2.Name == "ESwitch10"))
                
            {               
                Sprite2.Animation = "Down";
                string tempName = "";
                switch (Sprite2.Name)
                {
                    case "Switch1":
                        tempName = "Door1";
                        break;
                    case "Switch2":
                        tempName = "Door2";
                        break;
                    case "Switch3":
                        tempName = "Door3";
                        break;
                    case "Switch4":
                        tempName = "Door4";
                        break;
                    case "Switch5":
                        tempName = "Door5";
                        break;
                    case "Switch6":
                        tempName = "Door6";
                        break;
                    case "Switch7":
                        tempName = "Door7";
                        break;
                    case "Switch8":
                        tempName = "Door8";
                        break;
                    case "Switch9":
                        tempName = "Door9";
                        break;
                    case "Switch10":
                        tempName = "Door10";
                        break;

                    case "ESwitch1":
                        tempName = "Elevator1";
                        break;
                    case "ESwitch2":
                        tempName = "Elevator2";
                        break;
                    case "ESwitch3":
                        tempName = "Elevator3";
                        break;
                    case "ESwitch4":
                        tempName = "Elevator4";
                        break;
                    case "ESwitch5":
                        tempName = "Elevator5";
                        break;
                    case "ESwitch6":
                        tempName = "Elevator6";
                        break;
                    case "ESwitch7":
                        tempName = "Elevator7";
                        break;
                    case "ESwitch8":
                        tempName = "Elevator8";
                        break;
                    case "ESwitch9":
                        tempName = "Elevator9";
                        break;
                    case "ESwitch10":
                        tempName = "Elevator10";
                        break;

                    default:
                        break;
                }
                for (int i = 0; i < listOfDoors.Count; i++)
                {
                    if (listOfDoors[i].Name == tempName)
                    {
                        listOfDoors[i].IsActive = true;
                            
                    }

                }

            }
            #endregion

            #region Blob or Sarcough or Arrow Vs Doors & Elevators
            if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle) && (Sprite1.IsActive || Sprite1.IsAlive) && (Sprite2.Name == "Door1" || Sprite2.Name == "Door2" || Sprite2.Name == "Door3" || Sprite2.Name == "Door4" || Sprite2.Name == "Door5" || Sprite2.Name == "Door6" || Sprite2.Name == "Door7" || Sprite2.Name == "Door8" || Sprite2.Name == "Door9" || Sprite2.Name == "Door10"
                                                                                    || Sprite2.Name == "Elevator1" || Sprite2.Name == "Elevator2" || Sprite2.Name == "Elevator3" || Sprite2.Name == "Elevator4" || Sprite2.Name == "Elevator5" || Sprite2.Name == "Elevator6" || Sprite2.Name == "Elevator7" || Sprite2.Name == "Elevator8" || Sprite2.Name == "Elevator9" || Sprite2.Name == "Elevator10"))
            {
                bool isNForce = false;
                CollisionArea CollisionState;
                    
                if (Sprite1.CollisionRectangle.Intersects(Sprite2.CollisionRectangle))
                {
                    if (Sprite1.Name == "Arrow")
                    {
                        removeArrow = true;
                    }
                    if (Sprite1.Name == "Bob"
                        || Sprite1.Name == "Sarcophagus")
                    {    // this is used for egyptian elevators ive set up
                        if (Sprite2.doorType == DoorType.PushingRight)
                        {
                            //Right side of the Tile
                            if (Sprite1.CollisionRectangle.Intersects(Sprite2.RightRectangle))
                            {
                                Sprite1.IsColliding = true;
                                CollisionState = CollisionArea.Right;
                                Sprite1.position.X = Sprite2.RightRectangle.Right;

                                if (!isNForce)
                                {
                                    Sprite1.Force.X += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                                    isNForce = true;
                                }

                                Sprite1.InitialVelocity.X = 0;

                            }
                            if (Sprite1.CollisionRectangle.Intersects(Sprite2.LeftRectangle))
                            {
                                Sprite1.IsColliding = true;
                                CollisionState = CollisionArea.Left;
                                Sprite1.position.X = Sprite2.LeftRectangle.Left - Sprite1.width * Sprite1.Scale + 0.9f;

                                if (!isNForce)
                                {
                                    Sprite1.Force.X += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                                    isNForce = true;
                                }
                                //if (!sprite.AppliedImpulse)
                                //{
                                //    sprite.Force.X += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                                //    sprite.AppliedImpulse = true;
                                //} //
                                Sprite1.InitialVelocity.X = 0;
                            }
                            if (Sprite1.CollisionRectangle.Contains(new Point((int)(Sprite2.position.X + Sprite2.width * Sprite2.Scale / 2), (int)(Sprite2.position.Y + Sprite2.height * Sprite2.Scale / 2))))
                            {
                                if (!Sprite2.IsActive)
                                {
                                    Sprite1.IsAlive = false;
                                    Sprite1.IsActive = false;
                                }
                            }
                
                        }
                        //top of rectangle
                        if (Sprite1.CollisionRectangle.Intersects(Sprite2.TopRectangle))
                        {
                            Sprite1.IsColliding = true;
                            CollisionState = CollisionArea.Top;
                            Sprite1.position.Y = Sprite2.CollisionRectangle.Top - Sprite1.height * Sprite1.Scale + 1.5f;

                            if (!isNForce)
                            {
                                 Sprite1.Force.Y += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);

                                if (Sprite1.InitialVelocity.X != 0)
                                {
                                    Sprite1.Force.X += Physics.getFrictionForce(Sprite1.mass, Sprite1.Force, Sprite1.InitialVelocity);
                                }
                                isNForce = true;
                            }
                            if (!Sprite1.AppliedImpulse)
                            {
                                Sprite1.Force.Y += Physics.applyImpulseForce(Sprite1.mass, Sprite1.InitialVelocity, timeElapsed, Sprite1.IsBouncing, Sprite1.Force);
                                Sprite1.AppliedImpulse = true;
                            }
                            Sprite1.IsPlayerFalling = false;
                            Sprite1.IsPlayerJumping = false;

                        }       
                    }
                    //bottom
                    if (Sprite1.CollisionRectangle.Intersects(Sprite2.BottomRectangle) 
                        && !Sprite2.IsActive 
                        && Sprite2.doorType != DoorType.PushingRight
                        && Sprite1.IsColliding)
                    {
                        Sprite1.IsAlive = false;
                        Sprite1.IsActive = false;
                    }
                    //Right side of the Tile
                    if (Sprite1.CollisionRectangle.Intersects(Sprite2.RightRectangle))
                    {
                        Sprite1.IsColliding = true;
                        CollisionState = CollisionArea.Right;
                        Sprite1.position.X = Sprite2.RightRectangle.Right;

                        if (!isNForce)
                        {
                            Sprite1.Force.X += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                            isNForce = true;
                        }
                        //if (!sprite.AppliedImpulse)
                        //{
                        //    sprite.Force.X += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                        //    sprite.AppliedImpulse = true;
                        //} //Impulse doesn't work. No idea why.

                        Sprite1.InitialVelocity.X = 0;

                        if (Sprite1.Name == "Blob" && Sprite1.IsPlayerCharging)
                        {
                            Sprite1.IsPlayerCharging = false;
                            Sprite1.InitialVelocity.X = 0;
                        }
                    }// Left side of Tile
                    if (Sprite1.CollisionRectangle.Intersects(Sprite2.LeftRectangle))
                    {
                        Sprite1.IsColliding = true;
                        CollisionState = CollisionArea.Left;
                        Sprite1.position.X = Sprite2.LeftRectangle.Left - Sprite1.width * Sprite1.Scale + 0.9f;

                        if (!isNForce)
                        {
                            Sprite1.Force.X += Physics.applyNormalForce(CollisionState, Sprite1.mass, Sprite1.Force);
                            isNForce = true;
                        }
                        //if (!sprite.AppliedImpulse)
                        //{
                        //    sprite.Force.X += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                        //    sprite.AppliedImpulse = true;
                        //} //
                        Sprite1.InitialVelocity.X = 0;
                    }



                }
                if (!Sprite1.IsColliding)
                {
                    Sprite1.AppliedImpulse = false;
                }
            }
#endregion
        }
        
        //Checks for Collisions between tiles and sprites
        public void CollisionCheckTileVsSprite(GameObject sprite)
        {   //the genius at work here
            float tempPosX = sprite.position.X - sprite.position.X % Tile.Size.X;
            float tempPosY = sprite.position.Y - sprite.position.Y % Tile.Size.Y;

            int tempIndexforX = (int)(tempPosX / Tile.Size.X);
            int tempIndexforY = (int)(tempPosY / Tile.Size.Y);
           
            if (sprite.IsAlive
                || sprite.IsActive)
            {
                int row = 1;
                while (row <= 4)
                {
                    int yOffSet = 0;
                    switch (row)
                    {
                        case 1:
                            yOffSet = -1;
                            break;
                        case 2:
                            yOffSet = 0;
                            break;
                        case 3:
                            yOffSet = 1;
                            break;
                        case 4:
                            yOffSet = 2;
                            break;
                        default:
                            break;
                    }
                    CollidingWithTiles(sprite, tempIndexforX, tempIndexforY, yOffSet);
                    row++;
                }
            }
        }
        
        //Tile Collision check 
        public void CollidingWithTiles(GameObject sprite, int x, int y, int yOffset)
        {
            if (x <= 0)
                x = 1;
            if (y <= 0)
                y = 1;
            if (y > LevelHeight - 3)
                y = LevelHeight - 4;
            if (x > LevelWidth - 2)
                x = LevelWidth - 3;
            bool isNForce = false;
            int xOffSet = x - 1;
            int count = 0;
            while (count < 3)
            {
                sprite.IsColliding = false;

                #region Blob Vs Breakable
                if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].Bounds) && tiles[xOffSet, y + yOffset].TileType == TileType.Breakable && sprite.Name == "Bob")
                {
                    sprite.InitialVelocity.X = 0;
                    sprite.IsPlayerCharging = false;
                    sprite.position.X = tiles[xOffSet, y + yOffset].Bounds.Left - sprite.width * sprite.Scale;
                }
                #endregion
                
                #region any sprite Vs Tiles
                if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].Bounds) && tiles[xOffSet, y + yOffset].TileType == TileType.Impassable)
                {
                    if (sprite.Name == "Arrow")
                    {
                        removeArrow = true;
                        break;
                    }
                    bool vertCollision = false;
                    CollisionArea CollisionState;

                    //Top side of Tile
                    if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].TopRectangle))
                    {
                        sprite.IsColliding = true;
                        CollisionState = CollisionArea.Top;
                        sprite.position.Y = tiles[xOffSet, y + yOffset].Bounds.Top - sprite.height * sprite.Scale + 1.5f;
                        sprite.InitialVelocity.Y = 0;
                        Level.debug.position = sprite.Position;

                        if (!isNForce)
                        {

                            sprite.Force.Y += Physics.applyNormalForce(CollisionState, sprite.mass, sprite.Force);

                            if (sprite.InitialVelocity.X != 0)
                            {
                                sprite.Force.X += Physics.getFrictionForce(sprite.mass, sprite.Force, sprite.InitialVelocity);
                            }
                            isNForce = true;
                        }
                        if (!sprite.AppliedImpulse)
                        {
                            sprite.Force.Y += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                            sprite.AppliedImpulse = true;
                        }


                        //sprite.InitialVelocity.Y = 0;
                        sprite.IsPlayerFalling = false;
                        sprite.IsPlayerJumping = false;
                        vertCollision = true;
                    }
                    //Bottom side of Tile
                    else if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].BottomRectangle) && !vertCollision)
                    {
                        sprite.IsColliding = true;
                        CollisionState = CollisionArea.Bottom;
                        sprite.position.Y = tiles[xOffSet, y + yOffset].Bounds.Bottom;

                        if (!isNForce)
                        {
                            sprite.Force.Y += Physics.applyNormalForce(CollisionState, sprite.mass, sprite.Force);
                            isNForce = true;
                        }
                        if (!sprite.AppliedImpulse)
                        {
                            sprite.Force.Y += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                            sprite.AppliedImpulse = true;
                        }

                        //sprite.InitialVelocity.Y = 0;
                        vertCollision = true;
                        
                    }
                    if (!vertCollision)
                    {//Right side of the Tile
                        if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].RightRectangle))
                        {
                            sprite.IsColliding = true;
                            CollisionState = CollisionArea.Right;
                            sprite.position.X = tiles[xOffSet, y + yOffset].Bounds.Right;

                            if (!isNForce)
                            {
                                sprite.Force.X += Physics.applyNormalForce(CollisionState, sprite.mass, sprite.Force);
                                isNForce = true;
                            }
                            //if (!sprite.AppliedImpulse)
                            //{
                            //    sprite.Force.X += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                            //    sprite.AppliedImpulse = true;
                            //} //Impulse doesn't work. No idea why.

                            sprite.InitialVelocity.X = 0;

                            if (sprite.Name == "Blob" && sprite.IsPlayerCharging)
                            {
                                sprite.IsPlayerCharging = false;
                                sprite.InitialVelocity.X = 0;
                            }
                        }// Left side of Tile
                        if (sprite.CollisionRectangle.Intersects(tiles[xOffSet, y + yOffset].LeftRectangle))
                        {
                            sprite.IsColliding = true;
                            CollisionState = CollisionArea.Left;
                            sprite.position.X = tiles[xOffSet, y + yOffset].Bounds.Left - sprite.width * sprite.Scale + 0.9f;

                            if (!isNForce)
                            {
                                sprite.Force.X += Physics.applyNormalForce(CollisionState, sprite.mass, sprite.Force);
                                isNForce = true;
                            }
                            //if (!sprite.AppliedImpulse)
                            //{
                            //    sprite.Force.X += Physics.applyImpulseForce(sprite.mass, sprite.InitialVelocity, timeElapsed, sprite.IsBouncing, sprite.Force);
                            //    sprite.AppliedImpulse = true;
                            //} //
                            sprite.InitialVelocity.X = 0;

                            if (sprite.Name == "Blob" && sprite.IsPlayerCharging)
                            {
                                sprite.IsPlayerCharging = false;
                                sprite.InitialVelocity.X = 0;
                            }
                        }
                    }

                }
                #endregion

                xOffSet++;
                count++;
            }
            if (!sprite.IsColliding)
            {
                sprite.AppliedImpulse = false;
            }

        }

        //Check for Hat Collision vs Bob Lob
        private void CheckHatCollision()
        {
            Rectangle hatRect = new Rectangle((int)hatOnLevel.Position.X, (int)hatOnLevel.position.Y, (int)(hatOnLevel.Texture.Width * hatOnLevel.Scale), (int)(hatOnLevel.Texture.Height * hatOnLevel.Scale));
            if (player.CollisionRectangle.Intersects(hatRect))
            {
                HatCollected = true;
                hatTaken = true;
                if (Game1.invManager.CollectedHats.ContainsKey(hatIndex)) { return; }
                else
                {
                    Game1.invManager.AddToCollectedHats(hatIndex, Game1.invManager.GetInventoryHat(hatIndex));
                    return;
                }
            }
        }
    #endregion

    #region GAME LOOP
        //MAIN UPDATE FOR LEVEL
        public void Update(GameTime gameTime)
        {
            timeElapsed = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0F);
            Input.Update(player);
            //resets switches animation
            for (int i = 0; i < listOfGameObjects.Count; i++) {  
                if (listOfGameObjects[i].Name == "Switch1" || listOfGameObjects[i].Name == "Switch2" || listOfGameObjects[i].Name == "Switch3" || listOfGameObjects[i].Name == "Switch4" || listOfGameObjects[i].Name == "Switch5" || listOfGameObjects[i].Name == "Switch6" || listOfGameObjects[i].Name == "Switch7" || listOfGameObjects[i].Name == "Switch8" || listOfGameObjects[i].Name == "Switch9" || listOfGameObjects[i].Name == "Switch10"
                                           || listOfGameObjects[i].Name == "ESwitch1" || listOfGameObjects[i].Name == "ESwitch2" || listOfGameObjects[i].Name == "ESwitch3" || listOfGameObjects[i].Name == "ESwitch4" || listOfGameObjects[i].Name == "ESwitch5" || listOfGameObjects[i].Name == "ESwitch6" || listOfGameObjects[i].Name == "ESwitch7" || listOfGameObjects[i].Name == "ESwitch8" || listOfGameObjects[i].Name == "ESwitch9" || listOfGameObjects[i].Name == "ESwitch10")
                {
                    listOfGameObjects[i].Animation = "Up";
                }

                //update enemy movement
                if (listOfGameObjects[i].Name == "Enemy") {
                    Vector2 distance = new Vector2(player.position.X - listOfGameObjects[i].position.X, player.position.Y - listOfGameObjects[i].position.Y);
                    if (distance.X >= -200) {
                        distance.Normalize();
                        listOfGameObjects[i].Chase(gameTime, distance);
                    }
                    else
                        listOfGameObjects[i].Move();
                }
            }

            //Keyboard Input for Scrolling Background
            #region Background Input
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Left))
            {
                scrollBackground.SwitchDirection("left");
                scrollBackground.SetMoveLeftRight();
                scrollBackground.Update(gameTime);

            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                scrollBackground.SwitchDirection("right");
                scrollBackground.SetMoveLeftRight();
                scrollBackground.Update(gameTime);
            }
            else if (player.IsPlayerFalling)
            {
                scrollBackground.SwitchDirection("down");
                scrollBackground.SetMoveUpDown();
                scrollBackground.Update(gameTime);
            }
            #endregion

            //Dead Player Update
            if (!player.IsAlive) {
                if (isDead) {
                    deathToll++;
                    isDead = false;
                }
                UpdateDeadPlayer();
            }
            else
                CollisionCheckTileVsSprite(player);
            
            //Portal Update
            if (portal.IsAlive)
                PortalTimeUpdate(gameTime);             

            //Sarcophagus Update
            if (sarcophagus.IsActive) {
                sarcophagus.Update(gameTime);
                CollisionCheckSpriteVsSprite(player, sarcophagus);
            }

            //Run through Checkpoints and Gems
            for (int i = 0; i < listOfCheckpoints.Length; i++)
            { 
                CollisionCheckSpriteVsSprite(player, listOfCheckpoints[i]);
                if (listOfCheckpoints[i].IsActive)
                    listOfCheckpoints[i].Update(gameTime);

                CollisionCheckSpriteVsSprite(player, listOfGems[i]);
                listOfGems[i].Update(gameTime); 
            }

            //Enemy and Arrow Update
            #region ARROWS AND DOORS
            for (int i = 0; i < listOfEnemies.Count; i++)
            {
                switch (listOfEnemies[i].Name)
                {
                    case "ArrowUp":
                    case "ArrowLeft":
                    case "ArrowRight":
                    case "ArrowDown":
                        for (int j = 0; j < listOfEnemies[i].listOfArrows.Count; j++)
                        {
                            float tempTime = 0.5f;
                            if (listOfEnemies[i].listOfArrows[j].AliveTime > tempTime)
                            {
                                removeArrow = false;
                                CollisionCheckSpriteVsSprite(listOfEnemies[i].listOfArrows[j], sarcophagus);
                                if (!removeArrow)
                                    CollisionCheckTileVsSprite(listOfEnemies[i].listOfArrows[j]);
                                if (!removeArrow)
                                    CollisionCheckSpriteVsSprite(player, listOfEnemies[i].listOfArrows[j]);
                                if (!removeArrow)
                                    for (int k = 0; k < listOfDoors.Count; k++)
                                    {
                                        switch (listOfDoors[k].Name)
                                        {
                                            case "Door1":
                                            case "Door2":
                                            case "Door3":
                                            case "Door4":
                                            case "Door5":
                                            case "Door6":
                                            case "Door7":
                                            case "Door8":
                                            case "Door9":
                                            case "Door10":                   ////FUTURE NOTE, if there is severe memory issues, this may be the culprit, 3 looped for loops of decent size. Only way i could think of to do it properly
                                                if (!removeArrow)
                                                {
                                                    if (listOfEnemies[i].listOfArrows.Count > 0)
                                                        CollisionCheckSpriteVsSprite(listOfEnemies[i].listOfArrows[j], listOfDoors[k]);
                                                }
                                                //if (!removeArrow)
                                                //    CollisionCheckTileVsSprite(listOfEnemies[i].listOfArrows[j]);
                                                if (removeArrow)
                                                    if (listOfEnemies[i].listOfArrows.Count > 0)
                                                        listOfEnemies[i].listOfArrows.RemoveAt(j);
                                                break;

                                            default:
                                                break;
                                        }
                                    }
                                else if (removeArrow)
                                    listOfEnemies[i].listOfArrows.RemoveAt(j);
                            }                                
                        }
                        break;
                    
                    default:
                        break;
                }
                CollisionCheckSpriteVsSprite(sarcophagus, listOfEnemies[i]);
                listOfEnemies[i].Update(gameTime);
                CollisionCheckSpriteVsSprite(player, listOfEnemies[i]);
            }
            #endregion

            //Collision Detection for Game Objects
            for (int i = 0; i < listOfGameObjects.Count; i++)
            {
                listOfGameObjects[i].Update(gameTime);
                CollisionCheckSpriteVsSprite(player, listOfGameObjects[i]);
                CollisionCheckSpriteVsSprite(sarcophagus, listOfGameObjects[i]);
                //collision detection for enemy
                CollisionCheckTileVsSprite(listOfGameObjects[i]);
            }

            //Door Update
            for (int i = 0; i < listOfDoors.Count; i++)
            {
                listOfDoors[i].Update(gameTime);
                CollisionCheckSpriteVsSprite(player, listOfDoors[i]);
                CollisionCheckSpriteVsSprite(sarcophagus, listOfDoors[i]);
            }

            //Collision Detection for Sarcoph + Player
            CollisionCheckTileVsSprite(sarcophagus);
            CollisionCheckSpriteVsSprite(player, endPortal);

            //UI Updates
            currPortal = player.CurrentCheckpoint;
            ui.Update(gameTime);

            //Sprite Update
            player.Update(gameTime);

            //Hat Updates
            if (!hatTaken)
            {
                hatOnLevel.Scale = player.Scale;
                CheckHatCollision();
            }
        }
   
        //UPDATE THE DEAD PLAYER
        private void UpdateDeadPlayer()
        {
            player.InitialVelocity = Vector2.Zero;
            Vector2 deadVector = Vector2.Zero;
            if (player.CurrentCheckpoint == 0)
                deadVector = resetPos - player.Position;
            else if (player.CurrentCheckpoint == 1)
                deadVector = listOfCheckpoints[0].position - player.Position;
            else if (player.CurrentCheckpoint == 2)
                deadVector = listOfCheckpoints[1].position - player.Position;
            else if (player.CurrentCheckpoint == 3)
                deadVector = listOfCheckpoints[2].position - player.Position;


            player.resetPlayerPos(deadVector);
            if (player.CurrentCheckpoint == 0)
            {

                if (player.CollisionRectangle.Contains((int)resetPos.X + 30, (int)resetPos.Y + 30) && portalCount == 1)
                {
                    portal = new Sprite(content.Load<Texture2D>("Images/GameObjects/Portal/Portal"), 4, 1, 0.5f, 0);
                    portal.IsAlive = true;
                    portal.position = new Vector2((int)(player.position.X - 20), (int)(player.position.Y - 20));
                    portal.AddAnimation("Spin", 1);
                    portal.Animation = "Spin";
                    portal.IsLooping = true;
                    portal.FramesPerSecond = 7;
                    player.IsAlive = true;
                    portalCount = 0;
                }

                else if (!portal.IsAlive && portalCount == 0)
                {
                    portal = new Sprite(content.Load<Texture2D>("Images/GameObjects/Portal/Portal"), 4, 1, 0.5f, 0);
                    portal.IsAlive = true;
                    portal.position = new Vector2((int)(player.position.X - 20), (int)(player.position.Y - 20));
                    portal.AddAnimation("Spin", 1);
                    portal.Animation = "Spin";
                    portal.IsLooping = true;
                    portal.FramesPerSecond = 7;
                    portalCount++;
                }
            }

            else if (player.CollisionRectangle.Contains((int)listOfCheckpoints[player.CurrentCheckpoint - 1].position.X + 30, (int)listOfCheckpoints[player.CurrentCheckpoint - 1].position.Y + 30) && portalCount == 1)
            {
                portal = new Sprite(content.Load<Texture2D>("Images/GameObjects/Portal/Portal"), 4, 1, 0.5f, 0);
                portal.IsAlive = true;
                portal.position = new Vector2((int)(player.position.X - 20), (int)(player.position.Y - 20));
                portal.AddAnimation("Spin", 1);
                portal.Animation = "Spin";
                portal.IsLooping = true;
                portal.FramesPerSecond = 7;
                player.IsAlive = true;
                portalCount = 0;

            }
            else if (!portal.IsAlive && portalCount == 0)
            {
                portal = new Sprite(content.Load<Texture2D>("Images/GameObjects/Portal/Portal"), 4, 1, 0.5f, 0);
                portal.IsAlive = true;
                portal.position = new Vector2((int)(player.position.X - 20), (int)(player.position.Y - 20));
                portal.AddAnimation("Spin", 1);
                portal.Animation = "Spin";
                portal.IsLooping = true;
                portal.FramesPerSecond = 7;
                portalCount++;
            }
        }

        //MAIN DRAW FOR LEVEL
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont Font)
        {
            ScrollCamera(spriteBatch.GraphicsDevice.Viewport);
            Matrix cameraTransform = Matrix.CreateTranslation(-cameraPositionXAxis, -cameraPositionYAxis, 0.0f);
            scrollBackground.Draw();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, cameraTransform);
            DrawTiles(spriteBatch);

            if (!hatTaken) { hatOnLevel.Draw(spriteBatch); }
            spriteBatch.End();

            ui.Draw(gameTime, spriteBatch);
        }

        //DRAW THE TILES
        public void DrawTiles(SpriteBatch spritebatch)
        {
            int left = (int)Math.Floor(cameraPositionXAxis / Tile.Size.X);
            int right = (int)(left + spritebatch.GraphicsDevice.Viewport.Width / Tile.Size.Y); //hmmm
            right = Math.Min(right, LevelWidth - 1);
            for (int y = 0; y < LevelHeight; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    Texture2D texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        spritebatch.Draw(texture, position, Color.White);
                    }
                }
            }
            if (portal.IsAlive)
                portal.Draw(spritebatch);
            if (sarcophagus.IsActive)
                sarcophagus.Draw(spritebatch);


            for (int i = 0; i < listOfGameObjects.Count; i++)
            {
                listOfGameObjects[i].Draw(spritebatch);
            }
            
            for (int i = 0; i < listOfCheckpoints.Length; i++)
            {
                listOfCheckpoints[i].Draw(spritebatch);
                if (listOfGems[i].IsActive)
                {
                    listOfGems[i].Draw(spritebatch);
                }
            }
            if (endPortal.IsActive)
                endPortal.Draw(spritebatch);
            for (int i = 0; i < listOfEnemies.Count; i++)
            {
                listOfEnemies[i].Draw(spritebatch);
            }
            
            for (int i = 0; i < listOfDoors.Count; i++)
            {
                listOfDoors[i].Draw(spritebatch);
            }
            if (player.IsAlive)
                player.Draw(spritebatch);
            
        }

        //UPDATE THE PORTAL
        private void PortalTimeUpdate(GameTime gameTime)
        {
            const float MaxPortalWait = 1.5f;
            portal.Update(gameTime);
            portalTime += (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
            if (portalTime > MaxPortalWait)
            {
                portal.IsAlive = false;
                portalTime = 0;

            }
        }

        //Scroll the Camera
        private void ScrollCamera(Viewport viewport)    
         {                                                                                 
            const float ViewMargin = 0.35f;
            const float TopMargin = 0.3f;
            const float BottomMargin = 0.5f;

            //  Calculate the edges of the screen.
            float marginWidth = viewport.Width * ViewMargin;
            float marginLeft = cameraPositionXAxis + marginWidth;
            float marginRight = cameraPositionXAxis + viewport.Width - marginWidth;

            float marginTop = cameraPositionYAxis + viewport.Height * TopMargin;
            float marginBottom = cameraPositionYAxis + viewport.Height - viewport.Height * BottomMargin;
            //  Calculate how far to scroll when the player is near the edges of the screen.
            float cameraMovementX = 0.0f;
            float cameraMovementY = 0.0f;

            if (player.Position.X < marginLeft)
                cameraMovementX = player.Position.X - marginLeft;

            else if (player.Position.X > marginRight)
                cameraMovementX = player.Position.X - marginRight;

            if (player.Position.Y < marginTop)
                cameraMovementY = player.Position.Y - marginTop;

            else if (player.Position.Y > marginBottom)
                cameraMovementY = player.Position.Y - marginBottom;

            //updates the camera position, but prevents scrolling off the ends of the level.
            float maxCameraPositionXOffset = Tile.Size.X * LevelWidth - viewport.Width;
            cameraPositionXAxis = MathHelper.Clamp(cameraPositionXAxis + cameraMovementX, 0.0f, maxCameraPositionXOffset);

            float maxCameraPositionYOffset = Tile.Size.Y * LevelHeight - viewport.Height;
            cameraPositionYAxis = MathHelper.Clamp(cameraPositionYAxis + cameraMovementY, 0.0f, maxCameraPositionYOffset);
        }

    #endregion

    #region SERVICE METHODS
        //Drops the Sarcoph
        public static void DropSarcoughagus()
        {
            sarcophagus.IsActive = true;
            if (player.SpriteEffect == SpriteEffects.None)
                sarcophagus.position = new Vector2(player.position.X - sarcophagus.width * sarcophagus.Scale, player.position.Y);
            
            if (player.SpriteEffect == SpriteEffects.FlipHorizontally)
                sarcophagus.position = new Vector2(player.position.X + player.width * player.Scale, player.position.Y);

            sarcophagus.InitialVelocity = Vector2.Zero;
            
            
            
        }

        //Cycles through the Checkpoints
        public static void CycleCheckPoints(int CurrentCheckpoint)
        {
            if (player.IsAlive)
            {
                if (CurrentCheckpoint == 1)
                {
                    if (listOfCheckpoints[CurrentCheckpoint].IsActive)
                        player.CurrentCheckpoint++;
                    else if (listOfCheckpoints[CurrentCheckpoint + 1].IsActive)
                        player.CurrentCheckpoint = 2;
                }
                else if (CurrentCheckpoint == 2)
                {
                    if (listOfCheckpoints[CurrentCheckpoint].IsActive)
                        player.CurrentCheckpoint++;
                    else if (listOfCheckpoints[CurrentCheckpoint - 2].IsActive)
                        player.CurrentCheckpoint = 1;
                }
                else if (CurrentCheckpoint == 3)
                {
                    if (listOfCheckpoints[CurrentCheckpoint - 3].IsActive)
                        player.CurrentCheckpoint = 1;
                    else if (listOfCheckpoints[CurrentCheckpoint - 2].IsActive)
                        player.CurrentCheckpoint = 2;

                }
            }

        }
    #endregion

    }//End of Class
}//End of Namespace
