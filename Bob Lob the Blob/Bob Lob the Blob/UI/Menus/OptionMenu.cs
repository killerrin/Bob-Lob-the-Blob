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
    public class OptionMenu : Menu
    {
        ContentManager content;
        GraphicsDevice device;

        public OptionMenu(ContentManager Content, GraphicsDevice Device, string[] _options)
            : base(Content.Load<Texture2D>("Images/Menu/menubutton_out"), Content.Load<Texture2D>("Images/Menu/menubutton_over"), 3, Content.Load<SoundEffect>("Audio/menu_dink"), Content, Device)
        {
            content = Content;
            device = Device;

            startingPosition = new Vector2((Device.Viewport.Width / 2), (Device.Viewport.Height / 2) - 50);

            for (int i = 0; i < numOfOptions; i++) {
                menuItems[i] = new MenuItem();
                menuItems[i].Init(menuImage, selectedImage, new Vector2(menuImage.Width / 2, menuImage.Height / 2), startingPosition);
                menuItems[i].OptionText = _options[i];
                menuItems[i].TextPosition = new Vector2(((menuItems[i].position.X - menuItems[i].Origin.X) + menuItems[i].Size.X / 2) - menuFont.MeasureString(_options[i]).X + 10,
                    menuItems[i].position.Y - 15);
                startingPosition.Y += (menuImage.Height + 15);
            }

        } //End of Constructor

        //Draw Method
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);

            foreach (MenuItem item in menuItems)
                item.Draw(spriteBatch, true);

            spriteBatch.DrawString(menuFont_Large, "Options", new Vector2(viewport.Width / 2 - menuFont_Large.MeasureString("Options").X / 2, 75), Color.White);

            spriteBatch.End();            
        }

        //Cursor Movement
        public override void CursorMovement(string i)
        {
            switch (i)
            {
                case "DOWN": //Moves Right
                    if (selectedChoice >= numOfOptions - 1)
                        selectedChoice = 0;
                    else
                        selectedChoice++;
                    break;

                case "UP": //Moves Left
                    if (selectedChoice <= 0)
                        selectedChoice = numOfOptions - 1;
                    else
                        selectedChoice--;
                    break;

                case "ENTER":
                    switch (selectedChoice)
	                    {
                            case 0:
                                MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                                break;
                            case 1:
                                if (MediaPlayer.Volume < 1)
                                    MediaPlayer.Volume += 0.1f;
                                break;
                            case 2:
                                if (MediaPlayer.Volume > 0)
                                    MediaPlayer.Volume -= 0.1f;
                                break;
                            default:
                                break;
	                    }
                    break;

                case "EXIT": //Returns to the MainMenu
                    Game1.gameState = GameState.MainMenu;
                    break;

                default:
                    break;
            }
            base.CursorMovement(i);
        }

    } //End of Class
} //End of Namespace
