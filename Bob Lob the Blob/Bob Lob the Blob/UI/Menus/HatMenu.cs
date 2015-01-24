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
    public class HatMenu : Menu
    {
        //Constructor
        public HatMenu(GraphicsDevice Device, ContentManager Content)
            : base(Content.Load<Texture2D>("Images/Menu/hat_bg"), Content.Load<Texture2D>("Images/Menu/hat_bg_over"), Game1.NUM_OF_HATS, Content.Load<SoundEffect>("Audio/menu_dink"), Content, Device) 
        {
            viewport = Device.Viewport;
        }


        //Draw
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 drawOffset = new Vector2(250, 150);

            spriteBatch.Begin();
            spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
            RenderText.Draw(spriteBatch, "Hats", new Vector2(viewport.Width / 2 - menuFont_Large.MeasureString("Hats").X / 2, 50f), Color.White, menuFont_Large);

            for (int i = 0; i < Game1.invManager.InventoryHats.Count; i++) {
                drawOffset.X = 250; // Reset X back to normal

                if (i == selectedChoice)
                    spriteBatch.Draw(selectedImage, drawOffset, Color.White);
                else
                    spriteBatch.Draw(menuImage, drawOffset, Color.White);

                Hat hat = Game1.invManager.InventoryHats[i];

                //Draw the Hat's Image
                if (hat.Collected) {
                    hat.Colour = Color.White;
                    RenderText.Draw(spriteBatch, hat.Name, new Vector2(drawOffset.X + (180 * 0.5f) + 10f, drawOffset.Y + ((90 * 0.5f) / 3)), Color.White, menuFont);
                }
                else {
                    hat.Colour = Color.DarkGray;
                    RenderText.Draw(spriteBatch, hat.Name, new Vector2(drawOffset.X + (180 * 0.5f) + 10f, drawOffset.Y + ((90 * 0.5f) / 3)), Color.DarkGray, menuFont);
                }
                spriteBatch.Draw(hat.Texture, drawOffset, null, hat.Colour, hat.Rotation, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);


                drawOffset.Y += (120 * 0.5f) + 10f; //h.Texture.Height == 120 // Go to new line
            }
            spriteBatch.End();
        }

        //Cursor Movement
        public override void CursorMovement(string i)
        {
            switch (i)
            {
                case "DOWN": //Moves Right
                    if (selectedChoice >= numOfOptions)
                        selectedChoice = 0;
                    else
                        selectedChoice++;
                    break;

                case "UP": //Moves Left
                    if (selectedChoice <= 0)
                        selectedChoice = numOfOptions;
                    else
                        selectedChoice--;
                    break;

                case "EXIT":
                    Game1.gameState = GameState.LevelSelect;
                    break;

                default:
                    break;
            }
            base.CursorMovement(i);
        }

    }//End of Clas
}//End of Namespace
