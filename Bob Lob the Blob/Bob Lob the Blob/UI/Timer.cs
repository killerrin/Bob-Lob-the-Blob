using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bob_Lob_the_Blob
{
    public class Timer
    {
        private float Seconds;
        private float Minutes;
        private float Hours;

        private Color colour;
        private Vector2 timerPosition;

        //Constructor
        public Timer(Vector2 position, Color colour) {
            timerPosition = position;
            this.colour = colour;
        }

        //Update Method
        public void Update(GameTime gameTime) {
            Seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Resets Seconds and adds to Minute
            if (Seconds > 59) {
                Minutes++;
                Seconds = 0;
            }
            //Resets the Minutes and adds to Hour
            if (Minutes > 59) {
                Hours++;
                Minutes = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            spriteBatch.DrawString(font, "Time: " + Hours.ToString("00") + ":" + Minutes.ToString("00") + ":" + Seconds.ToString("00"), timerPosition, colour);
        }

    }//End of Class
}//End of Namespace
