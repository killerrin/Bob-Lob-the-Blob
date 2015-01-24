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
    public class TimeCrystal : Collectable
    {
        //For the Level Select Screen
        public TimeCrystal(Texture2D texture, Vector2 position, float scale, bool collected)
            : base(texture, position, scale, 0f)
        {
            Collected = collected;

            if (!Collected)
                Colour = Color.Gray;
        }

        public TimeCrystal(Texture2D texture, Vector2 position)
            : base(texture, position, 1.0f, 0f)
        {
            // Preset to grey to indicate that it is not picked up yet
            Colour = Color.Gray;
        }

        public TimeCrystal(Texture2D texture, Vector2 position, float scale, float rotation)
            : base(texture, position, scale, rotation)
        {
            Colour = Color.Plum;
        }
       
        // Used to change the colour of the crystal
        public void ChangeColour(Color color)
        {
            Colour = color;
        }

        //Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Activated) {
                spriteBatch.Draw(Texture, Position, null, Colour, Rotation, spriteOrigin, Scale, spriteEffects, 0f);
            }
        }

    } //End of Class
} //End of Namespace
