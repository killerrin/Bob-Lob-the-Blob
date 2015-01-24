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
    class ArrowTrap : GameObject
    {
        
        private float shootInterval {  set; get; }
        public ArrowTrap(string _name,Texture2D Texture, int frames, int animations,
           float scale, float rotation)
            : base(_name, Texture, frames, animations, scale, rotation)
        {
            
            shootInterval = 6;
            listOfArrows = new List<GameObject>();
            range = 5;    
        }
        
        
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!targetInRange)
                shootInterval = 0.8f;
            for (int i = 0; i < listOfArrows.Count; i++)
            {
                int speed = 4;
                switch (Name)
                {
                    case "ArrowUp":
                        listOfArrows[i].position.Y -= speed;
                        break;
                    case "ArrowDown":
                        listOfArrows[i].position.Y += speed;
                        break;
                    case "ArrowRight":
                        listOfArrows[i].position.X += speed;
                        break;
                    case "ArrowLeft":
                        listOfArrows[i].position.X -= speed;
                        break;
                    default:
                        break;
                }
                listOfArrows[i].AliveTime += 0.016f;
            }
        }
        public override void ShootArrow(Texture2D texture)
        {
            int MAX_WAIT = 1;
            shootInterval += 0.016f; //just the average deltatime
            if (shootInterval > MAX_WAIT)
            {
                listOfArrows.Add(new GameObject("Arrow", texture, 1, 1, 0.55f, 0f));
                listOfArrows[listOfArrows.Count - 1].AddAnimation("Idle", 1);
                listOfArrows[listOfArrows.Count - 1].Animation = "Idle";
                listOfArrows[listOfArrows.Count - 1].position.X = position.X + width / 2 - 10;
                listOfArrows[listOfArrows.Count - 1].position.Y = position.Y + 5;
                listOfArrows[listOfArrows.Count - 1].IsLooping = false;
                listOfArrows[listOfArrows.Count - 1].FramesPerSecond = 1;
                listOfArrows[listOfArrows.Count - 1].IsActive = true;
                shootInterval = 0;    
            }
            targetInRange = true;
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            for (int i = 0; i < listOfArrows.Count; i++)
            {
                listOfArrows[i].Draw(spriteBatch);
            }
        }
        

    }
}
