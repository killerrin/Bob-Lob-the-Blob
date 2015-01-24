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
    public class EnemyAI:GameObject
    {
        bool chasing = false;
        bool moving = false;
        Vector2 currNormalDirection;
        public EnemyAI(string _name,Texture2D Texture, int frames, int animations,
           float scale, float rotation)
            : base(_name, Texture, frames, animations, scale, rotation)
        {
            this.IsAlive = true;
            range = 5;    
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (chasing)
            {
                Chase(gameTime,currNormalDirection);
            }
            if (moving)
            {
                Move();
            }

            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            Force.Y += Physics.applyGravityForce(mass);

            Level.debug.force = Force;

                //acceleration.X = Force.X / mass; //Fnet=ma    only use force.X from moving for this. dont have any other forces yet but will add friction soon
                //acceleration.Y = (Force.Y + 825) / mass;   //just apply gravity to the Y and we increase Force.Y when we jump only. ******Object Ideal***** a fan that pushes the player in a specific direction using a force 
                //s= Vit + (0.5 * acceleration * t^2) ///OR/// S = Vit + at^2/2

             //Position += Physics.getPosition(InitialVelocity, Force, timeLapse, mass);//2nd law 

                Level.debug.position = Position;

             InitialVelocity = Physics.getVelocity(InitialVelocity, Force, timeLapse,mass);
             Level.debug.initialvelocity = InitialVelocity;
            
        }
        public override void Chase(GameTime gameTime, Vector2 normalDirection)
        {
            chasing = true;
            moving = false;
            if (normalDirection.X < 0)
            {
                this.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            currNormalDirection = normalDirection;
            this.position.X += normalDirection.X*2; //2 for speed
        }
        public override void Move()
        {
            chasing = false;
            moving = true;
            //this.SpriteEffect = SpriteEffects.None;
            this.position.X += 2;
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //this.Draw(spriteBatch);
        }
    }
}
