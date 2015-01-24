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

namespace SpriteManager
{
    public class Sprite : SpriteManager
    {
        protected float timeElapsed; // Total time passing
        public bool IsLooping = false; // Is the animation looping.
        public string Name { protected set; get; }
        public bool IsPlayerJumping { set; get; }
        public bool IsPlayerFalling { set; get; }
        public bool IsAlive { set; get; }
        public bool IsActive { set; get; }
        public bool IsBouncing { set; get; }
        public bool IsPlayerCharging { set; get; }
        public bool AppliedImpulse { get; set; }
        public bool IsColliding { get; set; }
        public int CurrentCheckpoint { set; get; }
        public int mass { set; get; }

        public Vector2 InitialVelocity;
        public Vector2 finalVelocity,
                        acceleration, //Basic Physic variables.
                        MAX_VELOCITY,
                        Force;
        //  private Vector2 gravity; // gravity imposed on the entity.

        // default to 20 frames per second
        protected float timeToUpdate = 0.05f;
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }
       
        public Sprite(Texture2D Texture, int frames, int animations, float scale, float rotation)
            : base(Texture, frames, animations, scale, rotation)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (FrameIndex < Frames - 1)
                    FrameIndex++;
                else if (IsLooping)
                    FrameIndex = 0;
               
            }
        }
       
    }
}

// code to add to Game1  
// Sprite loading;


// Draw Method
// spriteBatch.Begin();
// loading.Draw(spriteBatch);
// spriteBatch.End();  

// Load Content
// loading = new Sprite(
//          Content.Load<Texture2D>("LoadingCircle"), 3, 2, 1f, 0f);
// loading.Position = new Vector2(100, 100);
// loading.IsLooping = true;
// loading.FramesPerSecond = 30;

// loading.Update(gameTime);