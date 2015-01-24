using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Bob_Lob_the_Blob
{
    //Menu Item Struct
    public struct MenuItem
    {
        #region VARIABLES 
        public Texture2D MenuImage { get; set; }
        public Texture2D MenuImageSelected { get; set; }
        public Vector2 Origin { get; set; }

        public Vector2 Size { get; set; }

        public bool IsSelected { get; set; }
        public string OptionText { get; set; }

        //Image Position
        public Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }
        
        //Text Position
        private Vector2 textPosition;
        public Vector2 TextPosition { get { return textPosition; } set { textPosition = value; } }

        #endregion

        //Initialize
        public void Init(Texture2D texture, Texture2D selectedImg, Vector2 origin, Vector2 position) {
            MenuImage = texture;
            MenuImageSelected = selectedImg;
            Origin = origin;
            Position = position;
            Size = new Vector2(MenuImage.Width, MenuImage.Height);
        }


        //Main Menu Overload
        public void Draw(SpriteBatch spriteBatch, bool drawText) {
            if (IsSelected) {
                spriteBatch.Draw(MenuImageSelected, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
                if (drawText)
                    spriteBatch.DrawString(Game1.Font, OptionText, TextPosition, Color.Red);
            }
            else  {
                spriteBatch.Draw(MenuImage, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
                if (drawText)
                    spriteBatch.DrawString(Game1.Font, OptionText, TextPosition, Color.White);
            }
        }

        ////Main Menu Overload - TRANSITION ALPHA
        //public void Draw(SpriteBatch spriteBatch, bool drawText) {
        //    if (IsSelected) {
        //        spriteBatch.Draw(MenuImageSelected, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
        //        if(drawText)
        //            spriteBatch.DrawString(Game1.Font, OptionText, TextPosition, Color.Red);
        //    }
        //    else {
        //        spriteBatch.Draw(MenuImage, Position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
        //        if(drawText)
        //            spriteBatch.DrawString(Game1.Font, OptionText, TextPosition, Color.White);
        //    }
        //}
        //Level Select Overload
        public void Draw(SpriteBatch spriteBatch, SpriteFont font, float Scale)
        {
            if (IsSelected)
            {
                spriteBatch.Draw(MenuImageSelected, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, OptionText, TextPosition, Color.White);
            }
            else {
                spriteBatch.Draw(MenuImage, Position, null, Color.White, 0f, Origin, Scale, SpriteEffects.None, 0);
            }
        }
    } //End of Struct


    //Base Menu Class
    public class Menu
    {
        //Images
        protected Texture2D menuImage;
        protected Texture2D selectedImage;
        protected Texture2D menuBackground;

        //Menu Items
        protected MenuItem[] menuItems;
        
        //Variables
        protected int numOfOptions;
        protected Viewport viewport;
        public int selectedChoice;
        protected Vector2 startingPosition;
        
        //Transition Variables
        public float Alpha { get; set; }
        private bool fading;
        private float ALPHA_CHANGE = 0.0008f;


        public SoundEffect SoundClick { get; set; }

        protected SpriteFont menuFont;
        protected SpriteFont menuFont_Large;

        //Constructor
        public Menu(Texture2D _menuImage, Texture2D _selectedImage, int _numOfOptions, SoundEffect soundEffect, ContentManager Content, GraphicsDevice Device) {
            viewport = Device.Viewport;
            
            //Initialize Menu Item array
            menuItems = new MenuItem[_numOfOptions];

            //NOTE: Menu background will be randomized from Egyptian World, Prehistoric and Jello
            menuBackground = Content.Load<Texture2D>("Images/Menu/menu_bg");

            //Images
            menuImage = _menuImage;
            selectedImage = _selectedImage;

            //Initialize Variables
            numOfOptions = _numOfOptions;
            selectedChoice = 0;
            SoundClick = soundEffect;
            Alpha = 0;
            fading = true;

            //Fonts
            menuFont = Content.Load<SpriteFont>("Fonts/menu_font");
            menuFont_Large = Content.Load<SpriteFont>("Fonts/menu_font_large");
        }

        /* ****************************************
         *            GAME LOOP
         * ************************************* */

        #region GAME LOOP

        //Update
        public virtual void Update(GameTime gameTime) {
            Input.Update(this);
            if (Game1.level != null) {
                Game1.level = null;
            }

            //Update which menu item is selected
            for (int i = 0; i < numOfOptions; i++) {
                if (i == selectedChoice)
                    menuItems[i].IsSelected = true;
                else
                    menuItems[i].IsSelected = false;
            }
        }

        //Draw
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);

            foreach (MenuItem item in menuItems)
                item.Draw(spriteBatch, true);

            spriteBatch.End();
        }
        #endregion


        /* ****************************************
         *            SERVICE METHODS
         * ************************************* */

        //Cursor Movement
        public virtual void CursorMovement(String i) {
            SoundClick.Play();
        }

        //Fading Transition
        public void FadeIn() {
            if (fading) {
                Alpha += ALPHA_CHANGE;
                if (Alpha >= 1) {
                    fading = false;
                }
            }
        }

    }//End of Class
}//End of Namespace
