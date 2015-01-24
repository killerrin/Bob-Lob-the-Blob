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


namespace Bob_Lob_the_Blob
{
    public static class Input
    {
        private static bool keyPressed;
        private static KeyboardState keyState;
        public static KeyboardState prevKeyState;
        private static GamePadState padState;
        public static GamePadState prevPadState;
        private static float WaitingTime;

        //Update Input for the Player
        public static void Update(Player player)
        {
            float timeSpan = 2.5f;
            WaitingTime += 0.1f;
            keyPressed = false;
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);


            //Pause Menu
            if ((keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape)) 
                || (padState.IsButtonDown(Buttons.Start) && !prevPadState.IsButtonDown(Buttons.Start)))
                    Game1.gameState = GameState.Pause;


            //Swap Between Hats - Right
            if (WaitingTime >= timeSpan
                && keyState.IsKeyDown(Keys.Q)
                || padState.IsButtonDown(Buttons.LeftTrigger))
            {
                if (!prevPadState.IsButtonDown(Buttons.LeftTrigger))
                {
                    while (true) {
                        if (Game1.invManager.CurrentHatIndex < 0)
                            Game1.invManager.CurrentHatIndex = Game1.invManager.totalInventoryHats;
                        else
                            Game1.invManager.CurrentHatIndex--;
                        if (Game1.invManager.CollectedHats.ContainsKey(Game1.invManager.CurrentHatIndex)) { break; }
                    }
                    WaitingTime = 0;
                }
            }

            //Swap Between Hats - Left
            if (WaitingTime >= timeSpan
                && keyState.IsKeyDown(Keys.E)
                || padState.IsButtonDown(Buttons.RightTrigger))
            {
                if (!prevPadState.IsButtonDown(Buttons.RightTrigger))
                {
                    if (Game1.invManager.CurrentHatIndex > Game1.invManager.totalCollectedHats) { Game1.invManager.CurrentHatIndex = 0; }
                    else
                    {
                        while (true)
                        {
                            Game1.invManager.CurrentHatIndex++;
                            if (Game1.invManager.CollectedHats.ContainsKey(Game1.invManager.CurrentHatIndex)) { break; }
                        }
                    }
                    WaitingTime = 0;
                }
            }



            //Right
            if (keyState.IsKeyDown(Keys.Right)
            || keyState.IsKeyDown(Keys.D)
            || padState.DPad.Right == ButtonState.Pressed
            || padState.ThumbSticks.Left.X > 0.5f)
            {
                player.Right();
                keyPressed = true;
            }

            //Left
            else if (keyState.IsKeyDown(Keys.Left)
                    || keyState.IsKeyDown(Keys.A)
                    || padState.DPad.Left == ButtonState.Pressed
                    || padState.ThumbSticks.Left.X < -0.5f)
            {
                player.Left();
                keyPressed = true;
            }


            //If Not Charging
            if (!player.IsPlayerCharging)
            {
                //Running
                if ((keyState.IsKeyDown(Keys.LeftShift))
                  || (padState.Buttons.X == ButtonState.Pressed))
                    player.Run();
                else 
                    player.ResetRun();
            }
            
            //Jump
            if ((keyState.IsKeyDown(Keys.Up))
              || (padState.Buttons.A == ButtonState.Pressed))
                player.Jump();

            //Idle
            if (!keyPressed)
                player.Idle();

            //Cycle Checkpoints
            if (keyState.IsKeyDown(Keys.C) && prevKeyState != keyState
                    || (padState.Buttons.RightShoulder == ButtonState.Pressed && prevPadState.Buttons.RightShoulder == ButtonState.Released))
                Level.CycleCheckPoints(player.CurrentCheckpoint);
            
            //Ability Key
            if (keyState.IsKeyDown(Keys.LeftControl) && prevKeyState != keyState
                    || (padState.Buttons.Y == ButtonState.Pressed 
                    && prevPadState.Buttons.Y == ButtonState.Released))

                switch (Game1.nextLevelToLoad)
                {
                    case "Egyptian":
                        Level.DropSarcoughagus();
                        break;
                    case "Prehistoric":
                        if (!player.IsPlayerCharging)
                            player.Charge();
                        break;
                    default:
                        break;
                }
                
            
            
            prevPadState = padState;
            prevKeyState = keyState;
        }

        //Update Input for the Level
        public static void Update(Level level)
        {
            padState = GamePad.GetState(PlayerIndex.One);
            keyState = Keyboard.GetState();

            

            prevKeyState = keyState;
            prevPadState = padState;
        }


        /* ****************************************
         *            MENU UPDATES
         * ************************************* */
        #region MENU

        //Update Input for the Menu
        public static void Update(Menu menu)
        {
            float totalTimeToWait = 1;
            WaitingTime += 0.1f;//this is so I can cycle the menu one at a time when held down like FF 
            keyPressed = false;
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);

            if (WaitingTime > totalTimeToWait && (keyState.IsKeyDown(Keys.Down)
              || padState.DPad.Down == ButtonState.Pressed
              || padState.ThumbSticks.Left.Y < -0.5f))
            {
                menu.CursorMovement("DOWN");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Up)
              || padState.DPad.Up == ButtonState.Pressed
              || padState.ThumbSticks.Left.Y > 0.5f)))
            {
                menu.CursorMovement("UP");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Right)
            || padState.DPad.Right == ButtonState.Pressed
            || padState.ThumbSticks.Left.X > 0.5f)))
            {
                menu.CursorMovement("RIGHT");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Left)
                     || padState.DPad.Left == ButtonState.Pressed
                     || padState.ThumbSticks.Left.X < -0.5f)))
            {
                menu.CursorMovement("LEFT");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Enter) && WaitingTime > totalTimeToWait)
              || (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A == ButtonState.Released)))
            {
                menu.CursorMovement("ENTER");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Escape) && prevKeyState != keyState)
                  || (padState.Buttons.B == ButtonState.Pressed && prevPadState.Buttons.B == ButtonState.Released)))
            {
                menu.CursorMovement("EXIT");
                WaitingTime = 0;
            }

            prevKeyState = keyState;
            prevPadState = padState;
        }

        //Update Input for the Pause Menu
        public static void Update(InGameMenu menu)
        {
            float totalTimeToWait = 1;
            WaitingTime += 0.1f;
            keyPressed = false;
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);

            //EXIT the Pause Menu
            if ((keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
               || (padState.IsButtonDown(Buttons.Start) && !prevPadState.IsButtonDown(Buttons.Start)))
                Game1.gameState = GameState.InGame;



            //Move the Selection
            //RIGHT
            if (WaitingTime > totalTimeToWait
                && (keyState.IsKeyDown(Keys.Up)
                || padState.IsButtonDown(Buttons.DPadUp)
                || padState.ThumbSticks.Left.Y > 0.5f))
            {
                if (prevKeyState != keyState || prevPadState != padState)
                {
                    menu.CursorMovement("UP");
                    WaitingTime = 0;
                }
            }

            //LEFT
            if (WaitingTime > totalTimeToWait
                && (keyState.IsKeyDown(Keys.Down)
                || padState.IsButtonDown(Buttons.DPadDown)
                || padState.ThumbSticks.Left.Y < -0.5f))
            {
                if (prevKeyState != keyState || prevPadState != padState)
                {
                    menu.CursorMovement("DOWN");
                    WaitingTime = 0;
                }
            }


            //Selection
            if (WaitingTime > totalTimeToWait
                && (keyState.IsKeyDown(Keys.Enter)
                || padState.Buttons.A == ButtonState.Pressed))
            {
                if (prevKeyState != keyState || prevPadState != padState)
                {
                    menu.CursorMovement("ENTER");
                    WaitingTime = 0;
                }
            }

            prevKeyState = keyState;
            prevPadState = padState;

        }

        //Update Input for the EndLevelMenu
        public static void Update(EndLevelMenu menu)
        {
            float totalTimeToWait = 1;
            WaitingTime += 0.1f;//this is so I can cycle the menu one at a time when held DOWN like FF 

            keyPressed = false;
            keyState = Keyboard.GetState();
            padState = GamePad.GetState(PlayerIndex.One);

            if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Right)
            || keyState.IsKeyDown(Keys.D)
            || padState.DPad.Right == ButtonState.Pressed
            || padState.ThumbSticks.Left.X > 0.5f)))
            {
                menu.CursorMovement("RIGHT");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Left)
                     || keyState.IsKeyDown(Keys.A)
                     || padState.DPad.Left == ButtonState.Pressed
                     || padState.ThumbSticks.Left.X < -0.5f)))
            {
                menu.CursorMovement("LEFT");
                WaitingTime = 0;
            }
            else if (WaitingTime > totalTimeToWait
                && ((keyState.IsKeyDown(Keys.Enter) && WaitingTime > totalTimeToWait)
              || (padState.Buttons.A == ButtonState.Pressed && prevPadState.Buttons.A == ButtonState.Released)))
            {
                menu.CursorMovement("ENTER");
                WaitingTime = 0;
            }
            prevKeyState = keyState;
        }

        #endregion

    }//End of Class
 }//End of Namespace
