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
    public class EndLevelMenu : InGameMenu
    {
        private Texture2D timeCrystal, nullTimeCrystal, checkmark, cross, hat;
        private const int NUM_OPTIONS = 3;

        //Constructor
        public EndLevelMenu(ContentManager Content, GraphicsDevice Device)
            : base(Content, Device)
        {
            //Initialize Textures
            timeCrystal = Content.Load<Texture2D>("Images/Collectibles/TimeCrystal");
            nullTimeCrystal = Content.Load<Texture2D>("Images/Menu/EndLevelMenu/NullTimeCrystal");
            
            checkmark = Content.Load<Texture2D>("Images/Menu/EndLevelMenu/Checkmark");
            cross = Content.Load<Texture2D>("Images/Menu/EndLevelMenu/Cross");
            hat = Content.Load<Texture2D>("Images/Menu/EndLevelMenu/Hat");

            //Initialize Options
            optionList = new List<RenderText>();
            optionList.Add(new RenderText("Next Level", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Next Level").X / 2, 330), Color.White, igmFont));
            optionList.Add(new RenderText("Replay", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Replay").X / 2, 360), Color.White, igmFont));
            optionList.Add(new RenderText("Exit", new Vector2(viewport.Width / 2 - igmFont.MeasureString("Exit").X / 2, 390), Color.White, igmFont));

            currSelection = 0;
        }


        //Cursor Movement 
        public override void CursorMovement(String command)
        {
            switch (command)
            {
                case "DOWN": //Down
                    if (currSelection == 2)
                        currSelection = 0;
                    else
                        currSelection++;
                    break;
                case "UP": //Up
                    if (currSelection == 0)
                        currSelection = 2;
                    else
                        currSelection--;
                    break;
                case "ENTER": //Enter
                    switch (currSelection) {
                        case 0: //Next Level
                            switch (Game1.nextLevelToLoad)
                            {
                                case "Blob":
                                    Game1.nextLevelToLoad = "Egyptian";
                                    break;
                                case "Egyptian":
                                    Game1.nextLevelToLoad = "Prehistoric";
                                    break;
                                case "Prehistoric":
                                    Game1.nextLevelToLoad = "Blob";
                                    break;
                                default:
                                    break;
                            }
                            Game1.level = null;
                            Game1.gameState = GameState.InGame;
                            break;
                        case 1: //Replay Level
                            Game1.level = null;
                            Game1.gameState = GameState.InGame;
                            break;

                        case 2: //Exit
                            Game1.gameState = GameState.LevelSelect;
                            break;
                        default:
                            break;
                    }
                    currSelection = 0;
                    break;

                    default:
                        break;
            }
        }
    
    #region GAME LOOP
        public void Update(GameTime gameTime) {
            base.Update();

            for (int i = 0; i < NUM_OPTIONS; i++) {
                if (i == currSelection)
                    optionList[i].Active = true;
                else
                    optionList[i].Active = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            base.Draw(spriteBatch);

            //Draw the Collected Gems
            for (int i = 0; i < Game1.MAX_GEMS; i++)
            {
                if (Game1.level.listOfGems[i].IsObtained)
                    spriteBatch.Draw(timeCrystal, new Vector2(viewport.Width / 2 - (timeCrystal.Width / 2 * Game1.MAX_GEMS) + 40 * i, 230), Color.White);
                else
                    spriteBatch.Draw(nullTimeCrystal, new Vector2(viewport.Width / 2 - (timeCrystal.Width / 2 * Game1.MAX_GEMS) + 40 * i, 230), Color.White);
            }

            //Draw the Hat, and if it's Collected
            spriteBatch.Draw(hat, new Vector2(viewport.Width / 2 - hat.Width / 2 - 30, 280), Color.White);
            if(Game1.level.HatCollected)
                spriteBatch.Draw(checkmark, new Vector2(viewport.Width / 2 - hat.Width / 2 + 30, 280), Color.White);
            else
                spriteBatch.Draw(cross, new Vector2(viewport.Width / 2 - hat.Width / 2 + 30, 280), Color.White);

            //Draw the Text
            foreach (RenderText txt in optionList) {
                if (txt.Active)
                    txt.Draw(spriteBatch, Color.Crimson);
                else
                    txt.Draw(spriteBatch);
            }

            RenderText.Draw(spriteBatch, "COMPLETED", new Vector2(viewport.Width / 2 - igmFont.MeasureString("COMPLETED").X / 2 - 32, 180f), Color.White, igmFont_Large);

            spriteBatch.End();
        }
    #endregion


        
    }//End of Class
}//End of Namespace
