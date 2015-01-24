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


namespace Bob_Lob_the_Blob
{
    //Enumeration for the GameState
    public enum GameState
    {
        MainMenu,
        Tutorial,
        LevelSelect,
        OptionsMenu,
        InGame,
        Pause,
        Credits,
        EndLevelMenu,
        HatMenu
    }

    public enum LevelID { 
        Blob,
        Egypt,
        Prehistoric
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {

    #region VARIABLES

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //ETC Variables
        public static GameState gameState;
        public static Random random = new Random();
        public static bool debug = false;

        //Game Asset Variables
        public static SpriteFont Font;
        private Texture2D blankTexture;  
        
        //Inventory Variables
        public static InventoryManager invManager;
        public bool saveFileExists;

        //Level Variables
        public static Level level;
        public static string nextLevelToLoad;

        //Menu Objects
        Menu menu;
        Menu levelSelect;
        Menu options;
        Menu hatMenu;
        EndLevelMenu endLevelMenu;
        PauseMenu pauseMenu;
        CreditManager credits;
        Tutorial tutorial;

        //Music
        public Song BackgroundMusic { get; set; }

        //Constants
        public const int NUM_OF_HATS = 4;
        public const int MAX_GEMS = 3;
        public const string TITLE = "Bob Lob The Blob";

    #endregion

    #region LOADING

        //Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 670;
            graphics.PreferredBackBufferWidth = 1200;
        }

        
        //Initialize
        protected override void Initialize()
        {
            //MAKE A MAIN FONT
            Font = Content.Load<SpriteFont>("Fonts/igm_font");
            invManager = new InventoryManager();
            invManager.InitializeInvHats(Content, GraphicsDevice);
            invManager.Initialize(Content);

            base.Initialize();
        }

        
        //Load Content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            blankTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });

           
            //Initialize the Menus
            menu = new MainMenu(Content, GraphicsDevice, new string[] { "Level Select", "Tutorial", "Options", "Credits" });
            levelSelect = new LevelSelect(Content, GraphicsDevice, new string[] { "Blob Era", "Egyptian Era", "Prehistoric Era", "Hats" });
            options = new OptionMenu(Content, GraphicsDevice, new string[] { "Mute", "Volume Up", "Volume Down" });
            hatMenu = new HatMenu(GraphicsDevice, Content);
            
            tutorial = new Tutorial(Content, GraphicsDevice);

            pauseMenu = new PauseMenu(Content, GraphicsDevice);
            endLevelMenu = new EndLevelMenu(Content, GraphicsDevice);

            //Background Music
            BackgroundMusic = Content.Load<Song>("Audio/menu_background");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
            MediaPlayer.Play(BackgroundMusic);
            
        }


        //Unload Content
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

    #endregion

    #region GAME LOOP
        //Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (gameState) { 
                case GameState.MainMenu:
                    menu.Update(gameTime);
                    break;

                case GameState.Tutorial:
                    tutorial.Update(gameTime);
                    break;

                case GameState.LevelSelect:
                    levelSelect.Update(gameTime);
                    break;

                case GameState.OptionsMenu:
                    options.Update(gameTime);
                    break;

                case GameState.InGame:
                    if (level == null) {
                        LoadLevel();
                    }
                    level.Update(gameTime);
                    break;

                case GameState.Pause:
                    pauseMenu.Update(gameTime);
                    break;

                case GameState.Credits:
                    if (credits == null)
                        credits = new CreditManager(Content, GraphicsDevice);
                    credits.Update(gameTime);
                    break;

                case GameState.EndLevelMenu:
                    endLevelMenu.Update(gameTime); 
                    break;

                case GameState.HatMenu:
                    hatMenu.Update(gameTime);
                    break;

                default:
                    break;
            }

            base.Update(gameTime);
        }
        
        //Draw Method
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (gameState)
            {
                case GameState.MainMenu:
                    menu.Draw(gameTime, spriteBatch);
                    break;

                case GameState.Tutorial:
                    tutorial.Draw(gameTime, spriteBatch);
                    break;

                case GameState.LevelSelect:
                    levelSelect.Draw(gameTime, spriteBatch);
                    break;

                case GameState.OptionsMenu:
                    options.Draw(gameTime, spriteBatch);
                    break;

                case GameState.InGame:
                    if (level != null)
                        level.Draw(gameTime, spriteBatch, Font);    
                    break;

                case GameState.Pause:
                    level.Draw(gameTime, spriteBatch, Font);
                    pauseMenu.Draw(gameTime, spriteBatch);
                    break;

                case GameState.Credits:
                    if (credits != null)
                        credits.Draw(spriteBatch);
                    break;

                case GameState.EndLevelMenu:
                    level.Draw(gameTime, spriteBatch, Font);
                    endLevelMenu.Draw(gameTime, spriteBatch);
                    break;

                case GameState.HatMenu:
                    hatMenu.Draw(gameTime, spriteBatch);
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }

    #endregion


        //Load Level Method
        public void LoadLevel()
        {
            if (nextLevelToLoad == "Blob")
            {
                string levelPath = "Content/Levels/" + nextLevelToLoad + ".txt";
                using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                    level = new JelloWorld(Content, GraphicsDevice, fileStream);    
            }
           
            if (nextLevelToLoad == "Egyptian")
            {
                string levelPath = "Content/Levels/" + nextLevelToLoad + ".txt";
                using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                    level = new Egyptian(Content, GraphicsDevice, fileStream);
            }
            if (nextLevelToLoad == "Prehistoric")
            {
                string levelPath = "Content/Levels/" + nextLevelToLoad + ".txt";
                using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                    level = new Prehistoric(Content, GraphicsDevice, fileStream);
            }
        }
        
        
    }
}
