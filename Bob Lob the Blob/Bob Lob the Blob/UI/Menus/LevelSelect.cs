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
    public enum Select { 
        Level,
        Hats
    }

    public class LevelSelect : Menu
    {
        private Texture2D[] worldBackgrounds;
        SpriteFont ls_font;
        TimeCrystal[] tmp;


        //Cosntructor
        public LevelSelect(ContentManager Content, GraphicsDevice Device, string[] _options)
            : base(Content.Load<Texture2D>("Images/Menu/levelSelect_out"), Content.Load<Texture2D>("Images/Menu/levelSelect_over"), 4, Content.Load<SoundEffect>("Audio/menu_ting"), Content, Device) 
        {
            startingPosition = new Vector2(100, 250);
            worldBackgrounds = new Texture2D[numOfOptions];
            ls_font = Content.Load<SpriteFont>("Fonts/ls_Font");

            

            //Set up the Menu Options
            for (int i = 0; i < numOfOptions; i++) {
                menuItems[i] = new MenuItem();
                if (i == 3) {
                    worldBackgrounds[i] = Content.Load<Texture2D>("Images/Menu/hat_option");
                    menuItems[i].Init(worldBackgrounds[i], Content.Load<Texture2D>("Images/Menu/hat_option_over"), Vector2.Zero, new Vector2(Device.Viewport.Width - worldBackgrounds[i].Width, Device.Viewport.Height - worldBackgrounds[i].Height));
                }
                else {
                    worldBackgrounds[i] = Content.Load<Texture2D>("Images/Levels/Thumb/world0" + (i + 1));
                    menuItems[i].Init(menuImage, selectedImage, Vector2.Zero, startingPosition);
                    startingPosition.X += (menuImage.Width + ((Device.Viewport.Width - (menuImage.Width * 3))) / 3);
                }
                Vector2 textPosition = new Vector2((Device.Viewport.Width / 2) - ls_font.MeasureString(_options[i]).X / 2, 80);
                menuItems[i].TextPosition = textPosition;
                menuItems[i].OptionText = _options[i];
             }

            //Set up the Positions for the Crystals
            tmp = new TimeCrystal[3];
            for (int i = 0; i < numOfOptions - 1; i++)
            {
                tmp = Game1.invManager.GetTC(i);
                for (int j = 0; j < tmp.Length; j++)
                {
                    if (j != 0)
                        tmp[j].Position = new Vector2(tmp[j - 1].Position.X + (menuImage.Width - ((tmp[j].Texture.Width * tmp[j].Scale) * tmp.Length)) / 3 + 5, menuItems[i].Position.Y + menuImage.Height);

                    else
                        tmp[j].Position = new Vector2(menuItems[i].Position.X + (menuImage.Width - ((tmp[j].Texture.Width * tmp[j].Scale) * tmp.Length)) / 3 + 5, menuItems[i].Position.Y + menuImage.Height);
                }
                Game1.invManager.SetTC(i, tmp);
            }
            

        }

        //Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);

            for (int i = 0; i < numOfOptions; i++) {
                if(!menuItems[i].IsSelected)
                    spriteBatch.Draw(worldBackgrounds[i], menuItems[i].Position, Color.Gray);
                else
                    spriteBatch.Draw(worldBackgrounds[i], menuItems[i].Position, Color.White);
                menuItems[i].Draw(spriteBatch, ls_font, 1f);

                for (int k = 0; k < numOfOptions - 1; k++) {
                    Game1.invManager.DrawTC(k, spriteBatch);
                }
            }

            spriteBatch.End();
        }


        //Cursor Movement
        public override void CursorMovement(string i)
        {
            switch (i)
            {
                case "RIGHT": //Moves Right
                    if (selectedChoice >= numOfOptions - 1)
                        selectedChoice = 0;
                    else
                        selectedChoice++;
                    break;

                case "LEFT": //Moves Left
                    if (selectedChoice <= 0)
                        selectedChoice = numOfOptions - 1;
                    else
                        selectedChoice--;
                    break;

                case "ENTER":
                    Game1.gameState = GameState.InGame;
                    if (selectedChoice == 0)
                        Game1.nextLevelToLoad = "Blob";
                    else if (selectedChoice == 1)
                        Game1.nextLevelToLoad = "Egyptian";
                    else if (selectedChoice == 2)
                        Game1.nextLevelToLoad = "Prehistoric";
                    else if (selectedChoice == 3)
                        Game1.gameState = GameState.HatMenu;
                    selectedChoice = 0;
                    break;

                case "EXIT":
                    Game1.gameState = GameState.MainMenu;
                    selectedChoice = 0;
                    break;

                default:
                    break;
            }
            base.CursorMovement(i);
        }

    } //End of Class
}//End of Namespace
