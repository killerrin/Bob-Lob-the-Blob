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
    public class Sarcophagus : GameObject
    {
        public float WaitingTimer = 0;


        public Sarcophagus(Texture2D Texture, int frames, int animations,
           float scale, float rotation)
            : base("Sarcophagus", Texture, frames, animations, scale, rotation)
        {
            mass = 5;
            acceleration = Vector2.Zero;
            Force = Vector2.Zero;
            InitialVelocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            // time between frames

            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            //fake physics, may revise my isAlive condition statements between Level and Player, will implement Patricks once i get around to that stage.
            Force.Y += Physics.applyGravityForce(mass);

            Position += Physics.getPosition(InitialVelocity, Force, timeLapse, mass);//2nd law 
            InitialVelocity = Physics.getVelocity(InitialVelocity, Force, timeLapse, mass);
            Force = Vector2.Zero;

        }
    }
}
