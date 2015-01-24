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
using SpriteManager;

namespace Bob_Lob_the_Blob
{
    public class Player : GameObject
    {
        private float chargeTimer = 0;
        private Hat hat;
        public string direction;

        public Player(Texture2D Texture, int frames, int animations,
            float scale, float rotation) 
            : base("Player",Texture, frames, animations, scale, rotation)
        {
            hat = Game1.invManager.GetHat(0);
            mass = 5; 
            MAX_VELOCITY = new Vector2(100, 0);
            Name = "Bob";
            acceleration = Vector2.Zero;
            Force = Vector2.Zero;
            IsPlayerJumping = false;
            IsPlayerFalling = false;
            IsAlive = true;
            IsPlayerCharging = false;
            CurrentCheckpoint = 0;

            direction = "right";
        }


        /* *******************************************
         *              MOVEMENT
         * **************************************** */

    #region MOVEMENT

        //Right
        public void Right()
        {
            this.SpriteEffect = SpriteEffects.None;
            hat.ResetHatFlip();
            direction = "right";
            if (InitialVelocity.X < MAX_VELOCITY.X) ////REVISE****need to play around with forces, Game.gravity, and mass to get our player looking more realistic
                Force.X = 5000; //add 500 Newtons to the right.
        }

        //Left
        public void Left()
        {
            this.SpriteEffect = SpriteEffects.FlipHorizontally;
            hat.FlipHat();
            direction = "left";
            if (InitialVelocity.X > -(MAX_VELOCITY.X))      // this is negative cause his MAX_VELOCITY.X going left would be a negative number
               Force.X = -5000;  //we havent used any drag or anything yet  //Adds 500 Newtons to the Left *******Revise******* make this force a variable, something called WalkingSpeed
        }

        //Idle
        public void Idle()
        {
            if (direction == "right") { hat.ResetHatFlip(); }
            if (direction == "left") { hat.FlipHat(); }

            if (InitialVelocity.X > 0) //Walking Right
            {
                InitialVelocity.X *= .95f; //REVISE****need to play around with forces, Game.gravity, and mass to get our player looking more realistic
            }
            if (InitialVelocity.X < 0) //Walking Left
            {
                InitialVelocity.X *= .95f;   //REVISE****need to play around with forces, Game.gravity, and mass to get our player looking more realistic
            }
        }
        
        //Jump
        public void Jump()
        {
            if (!IsPlayerJumping && !IsPlayerFalling)
            {
                IsPlayerJumping = true;
                //AppliedImpulse = false;   
                Force.Y = -35000;  //so he jumps
            }
         }

        //Run
        public void Run() {
            MAX_VELOCITY = new Vector2(250, 0);
        }

        //Reset Run
        public void ResetRun() {
            MAX_VELOCITY = new Vector2(100, 0);
        }

    #endregion

        /* *******************************************
         *              ABILITIES
         * **************************************** */
    
    #region ABILITIES
        //Charge Ability
        public void Charge()
        {
            MAX_VELOCITY = new Vector2(600, 0);
            if (this.SpriteEffect == SpriteEffects.None)  //Will need to change if we put in right and left sprites
                Force.X += 50000;
            else
                Force.X -= 50000;
            IsPlayerCharging = true;
        }

        //Update Charge
        public void UpdateCharge(float timeLapse)
        {
            const float MAXCHARGETIME = 0.3f;
            const int CHARGECASTTIME = 1;
            chargeTimer += timeLapse;
            if (chargeTimer < MAXCHARGETIME)
            {
                if (this.SpriteEffect == SpriteEffects.None) //Will need to change if we put in right and left sprites 
                    Force.X += 50000;
                else
                    Force.X -= 50000;
            }
            else
            {
                const int originalVelocity = 250;
                if (MAX_VELOCITY.X > originalVelocity)
                    MAX_VELOCITY.X = 250;
                else if(chargeTimer > CHARGECASTTIME)
                {
                    chargeTimer = 0;
                    IsPlayerCharging = false;
                }
            }
        }

    #endregion



        /* *******************************************
         *              GAME LOOP
         * **************************************** */

    #region GAME LOOP

        //Update v01
        public override void Update(GameTime gameTime)
        {
            //Update Hat Position
            ChangeHat();

            base.Update(gameTime);
            ////REAL PHYSICS..... I think......
            // time between frames
            
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            //fake physics, may revise my isAlive condition statements between Level and Player, will implement Patricks once i get around to that stage.
            if (IsAlive)
            {
                if (IsPlayerCharging)
                    UpdateCharge(timeLapse);
                if (InitialVelocity.X > MAX_VELOCITY.X) // this is to control if player is letting go of run button
                    InitialVelocity.X = MAX_VELOCITY.X;
                else if (InitialVelocity.X < -MAX_VELOCITY.X) // this is to control if player is letting go of run button
                    InitialVelocity.X = -MAX_VELOCITY.X;

                Force.Y += Physics.applyGravityForce(mass);

                Level.debug.force = Force;

                //acceleration.X = Force.X / mass; //Fnet=ma    only use force.X from moving for this. dont have any other forces yet but will add friction soon
                //acceleration.Y = (Force.Y + 825) / mass;   //just apply gravity to the Y and we increase Force.Y when we jump only. ******Object Ideal***** a fan that pushes the player in a specific direction using a force 
                //s= Vit + (0.5 * acceleration * t^2) ///OR/// S = Vit + at^2/2

                Position += Physics.getPosition(InitialVelocity, Force, timeLapse, mass);//2nd law 

                Level.debug.position = Position;
                
                InitialVelocity = Physics.getVelocity(InitialVelocity, Force, timeLapse, mass);
                Level.debug.initialvelocity = InitialVelocity;

                Force = Vector2.Zero;

                hat.Scale = Scale;
                hat.Update(Position, direction);
            }
            
        }

        //Update v02
        public void Update(GameTime gameTime, GraphicsDevice Device)
        {
            //Update Hat Position
            ChangeHat();

            //call overload to do rotation and basic movement
            Update(gameTime);
            hat.Scale = Scale;
            hat.Update(Position, direction);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            hat.Draw(spriteBatch);
        }

    #endregion

        /* *******************************************
         *              SERVICE METHODS
         * **************************************** */

    #region SERVICE METHODS

        //Change Hat
        private void ChangeHat()
        {
            hat = Game1.invManager.GetHat(Game1.invManager.CurrentHatIndex);
        }

        //Reset Player Postion
        public void resetPlayerPos(Vector2 deadVector)
        {
            if (deadVector.X < 0)
                position.X -= 15;
            else if (deadVector.X > 0)
                position.X += 15;
            if (deadVector.Y < 0)
                position.Y -= 15;
            else if (deadVector.Y > 0)
                position.Y += 15;

        }

    #endregion
    }
}
