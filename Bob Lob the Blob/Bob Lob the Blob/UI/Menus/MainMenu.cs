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
    public class MainMenu : Menu
    {
        //Constructor
        public MainMenu(ContentManager Content, GraphicsDevice Device, string[] _options)
            : base(Content.Load<Texture2D>("Images/Menu/menubutton_out"), Content.Load<Texture2D>("Images/Menu/menubutton_over"), _options.Length, Content.Load<SoundEffect>("Audio/menu_dink"), Content, Device)
        {
            startingPosition = new Vector2((viewport.Width / 2), (viewport.Height / 2) - 70);

            for (int i = 0; i < numOfOptions; i++) {
                menuItems[i] = new MenuItem();
                menuItems[i].Init(menuImage, selectedImage, new Vector2(menuImage.Width / 2, menuImage.Height / 2), startingPosition);
                menuItems[i].OptionText = _options[i];
                menuItems[i].TextPosition = new Vector2(((menuItems[i].position.X - menuItems[i].Origin.X) + menuItems[i].Size.X / 2) - menuFont.MeasureString(_options[i]).X + 10, 
                    menuItems[i].position.Y - 15);
                startingPosition.Y += (menuImage.Height + 15);
            }

        } //End of Constructor

        //Cursor Movement
        public override void CursorMovement(string i)
        {
            switch (i)
            {
                case "DOWN": //Moves Down
                    if (selectedChoice >= numOfOptions - 1)
                        selectedChoice = 0;
                    else
                        selectedChoice++;
                    break;

                case "UP": //Moves Up
                    if (selectedChoice <= 0)
                        selectedChoice = numOfOptions - 1;
                    else
                        selectedChoice--;
                    break;

                case "ENTER":
                    switch (selectedChoice) { 
                        case 0:
                            Game1.gameState = GameState.LevelSelect;
                            break;
                        case 1:
                            Game1.gameState = GameState.Tutorial;
                            //Game1.nextLevelToLoad = "Tutorial";
                            break;
                        case 2:
                            Game1.gameState = GameState.OptionsMenu;
                            break;
                        case 3:
                            Game1.gameState = GameState.Credits;
                            break;
                        default:
                            break;
                            
                    }
                    break;

                default:
                    break;
            }
            base.CursorMovement(i);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);

            foreach (MenuItem item in menuItems)
                item.Draw(spriteBatch, true);

            spriteBatch.DrawString(menuFont_Large, Game1.TITLE, new Vector2(viewport.Width / 2 - menuFont_Large.MeasureString(Game1.TITLE).X / 2, 75), Color.White);

            spriteBatch.End();
        }

    } //End of Class
} //End of Namespace
