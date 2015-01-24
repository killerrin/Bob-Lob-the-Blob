using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Bob_Lob_the_Blob
{
    public class PauseMenu : InGameMenu
    {
        private const int NUM_OPTIONS = 3;

        //Constructor
        public PauseMenu(ContentManager Content, GraphicsDevice Device)
            : base(Content, Device)
        {
            viewport = Device.Viewport;

            optionList = new List<RenderText>();
            optionList.Add(new RenderText("Mute", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Mute").X / 2, 300), Color.White, igmFont));
            optionList.Add(new RenderText("Resume", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Resume").X / 2, 330), Color.White, igmFont));
            optionList.Add(new RenderText("Exit", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Exit").X / 2, 360), Color.White, igmFont));
        }

        #region GAME LOOP

        //Pause Update
        public void Update(GameTime gameTime)
        {
            base.Update();

            for (int i = 0; i < NUM_OPTIONS; i++) {
                if (i == currSelection)
                    optionList[i].Active = true;
                else
                    optionList[i].Active = false;
            }
        }

        //Pause Draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            base.Draw(spriteBatch);

            RenderText.Draw(spriteBatch, "PAUSED", new Vector2(viewport.Width / 2.3f, 250f), Color.White, igmFont_Large);

            foreach (RenderText txt in optionList) {
                if (txt.Active)
                    txt.Draw(spriteBatch, Color.Crimson);
                else
                    txt.Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        #endregion


        //Cursor Movement
        public override void CursorMovement(String i)
        {
            switch (i)
            {
                //Down
                case "DOWN":
                    if (currSelection == 2)
                        currSelection = 0;
                    else
                        currSelection++;
                    break;
                //Up
                case "UP":
                    if (currSelection == 0)
                        currSelection = 2;
                    else
                        currSelection--;
                    break;
                //Enter
                case "ENTER":
                    switch (currSelection)
                    {
                        case (0):
                            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
                            break;
                        case (1):
                            Game1.gameState = GameState.InGame;
                            break;
                        case (2):
                            Game1.gameState = GameState.MainMenu;
                            break;
                    }
                    currSelection = 0;
                    break;
                case "EXIT":
                    Game1.gameState = GameState.InGame;
                    break;
                //Default
                default:
                    break;
            }
        }

    }//End of Class
}//End of Namespace
