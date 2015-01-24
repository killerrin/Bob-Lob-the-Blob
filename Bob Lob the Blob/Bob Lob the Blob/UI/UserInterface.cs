using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Bob_Lob_the_Blob
{
    //Element ENUM
    public enum eType { 
        WorldID,
        Gem,
        DT,
        Checkpoint,
        Hat
    }

    //Element Class
    public class gui_element {
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }

        private eType type; 

        public bool Active { get; set; }
        private Color colour;
        private float Scale { get; set; }

        public string Text { get; set; }
        public Vector2 TextPosition { get; set; }
        private Color textColour;

        private ContentManager Content;

        //Essentially the 'Constructor'
        public gui_element(Texture2D _image, string _text, bool _active, float _scale, Vector2 _position, Vector2 _textPosition, eType _type) {
            //Initialize Image Variables
            colour = Color.White;
            Text = _text;
            Active = _active;
            Image = _image;
            Scale = _scale;
            type = _type;

            //Initialize the Positioning
            Position = _position;
            TextPosition = _textPosition;

            //Initialize Text Variables
            textColour = Color.White;
        }

        //Essentially the 'Constructor'
        public gui_element(ContentManager _Content, Texture2D _image, string _text, bool _active, float _scale, Vector2 _position, Vector2 _textPosition, eType _type)
        : this(_image, _text, _active, _scale, _position, _textPosition, _type)
        {
            Content = _Content;
        }

        //Update Method
        public void Update(GameTime gameTime) {

            switch (type)
            {
                case eType.Gem:
                    break;
                case eType.DT:
                    Text = "Deaths: " + Game1.level.deathToll;
                    break;
                case eType.Checkpoint: //Check if the Current Checkpoint
                    if (Text == Game1.level.currPortal.ToString())
                        Active = true;
                    else
                        Active = false;
                    break;
                case eType.Hat:
                    if (Game1.level.HatCollected) {
                        Active = true;
                        Image = Content.Load<Texture2D>("Images/GUI/hat_collected");
                    }
                    break;
                default:
                    break;
            }


            //Update the Colour
            if (!Active) {
                colour = Color.Gray;
                textColour = Color.White;
            }
            else {
                colour = Color.White;
                textColour = Color.Crimson;
            }

        }

        //Draw Method
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (Image != null) {
                if (Text != null) {
                    //Draw the Text and Image
                    spriteBatch.Draw(Image, Position, null, colour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    spriteBatch.DrawString(UserInterface.guiFont, Text, TextPosition, textColour);
                }
                else
                    //Draw only the images
                    spriteBatch.Draw(Image, Position, null, colour, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            }
            else { 
                //Draw the Text
                spriteBatch.DrawString(UserInterface.guiFont, Text, TextPosition, textColour);
            }
        }
    }//End of Class



    //UI Class
    public class UserInterface
    {
        #region VARIABLES
        //Static Variables
        public static SpriteFont guiFont;

        //Essential Variables
        public List<gui_element> ElementList;
        public Timer timer;

        //Variables
        private Rectangle guiRect;
        private Texture2D guiBG;
        private Rectangle deviceRect;

        private int id = 0;

        //ETC Variables
        private Viewport viewport;

        #endregion


        //Constructor
        public UserInterface(ContentManager Content, GraphicsDevice Device) {
            viewport = Device.Viewport;

            ElementList = new List<gui_element>();

            //Game1.level.HatCollected = false;

            //Initialize the Rectangle for the GUI
            guiBG = Content.Load<Texture2D>("Images/GUI/gui_bg");
            guiRect = new Rectangle(Device.Viewport.X, Device.Viewport.Height - guiBG.Height, guiBG.Width, guiBG.Height);
            deviceRect = new Rectangle(Device.Viewport.X, Device.Viewport.Y, Device.Viewport.Width, Device.Viewport.Height);

            //Initialize
            try {
                //Fonts
                guiFont = Content.Load<SpriteFont>("Fonts/gui_font");
            }
            catch (Exception ex) {
                throw ex;
            }

            //Set the ID
            switch (Game1.nextLevelToLoad)
            {
                case "Blob":
                    id = 1;
                    break;
                case "Egyptian":
                    id = 2;
                    break;
                case "Prehistoric":
                    id = 3;
                    break;
            }
            InitializeGUI(Content);
        }

        public void InitializeGUI(ContentManager Content) {
            //Elements
            //World ID
            ElementList.Add(new gui_element(null, "W: 0" + id, false, 1f, Vector2.Zero, new Vector2(50, viewport.Height - guiRect.Height / 2 - 15), eType.WorldID));

            //TIME CRYSTALS - SET TO TRUE OR FALSE DEPENDING ON WHICH IS ALREADY COLLECTED
            ElementList.Add(new gui_element(Content.Load<Texture2D>("Images/GUI/element05"), null, false, 0.3f, new Vector2(180, viewport.Height - guiRect.Height / 2 - 20), Vector2.Zero, eType.Gem));
            ElementList.Add(new gui_element(Content.Load<Texture2D>("Images/GUI/element05"), null, false, 0.3f, new Vector2(220, viewport.Height - guiRect.Height / 2 - 20), Vector2.Zero, eType.Gem));
            ElementList.Add(new gui_element(Content.Load<Texture2D>("Images/GUI/element05"), null, false, 0.3f, new Vector2(260, viewport.Height - guiRect.Height / 2 - 20), Vector2.Zero, eType.Gem));

            //Timer
            timer = new Timer(new Vector2(viewport.Width / 2 - 250, viewport.Height - guiRect.Height / 2 - 15), Color.White);
            ElementList.Add(new gui_element(null, "Deaths: ", false, 1f, Vector2.Zero, new Vector2(viewport.Width / 2 + 50, viewport.Height - guiRect.Height / 2 - 15), eType.DT));
            //ElementList.Add(new gui_element(Content.Load<Texture2D>("Images/GUI/map"), null, true, 1f, new Vector2(viewport.Width / 2 - 250, viewport.Height - guiRect.Height / 2 - 29), Vector2.Zero, eType.Map));

            //Death Counter

            //CHECK POINTS  - ONLY SET TO TRUE IF IT'S THE CURRENT CHECK POINT
            ElementList.Add(new gui_element(null, "1", true, 1f, Vector2.Zero, new Vector2(900, viewport.Height - guiRect.Height / 2 - 15), eType.Checkpoint));
            ElementList.Add(new gui_element(null, "2", false, 1f, Vector2.Zero, new Vector2(930, viewport.Height - guiRect.Height / 2 - 15), eType.Checkpoint));
            ElementList.Add(new gui_element(null, "3", false, 1f, Vector2.Zero, new Vector2(960, viewport.Height - guiRect.Height / 2 - 15), eType.Checkpoint));

            //Hat Collected
            ElementList.Add(new gui_element(Content, Content.Load<Texture2D>("Images/GUI/hat"), null, true, 0.47f, new Vector2(1060, viewport.Height - guiRect.Height / 2 - 25), Vector2.Zero, eType.Hat));
        }

        public void Update(GameTime gameTime) {
            timer.Update(gameTime);
            for (int i = 0; i < ElementList.Count; i++) {
               ElementList[i].Update(gameTime);           
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(guiBG, guiRect, Color.White);

            timer.Draw(spriteBatch, guiFont);

            foreach (gui_element e in ElementList) {
                e.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }



    }//End of Class
}//End of Namespace
