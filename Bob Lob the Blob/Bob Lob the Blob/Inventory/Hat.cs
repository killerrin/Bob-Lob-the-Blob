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
    public class Hat : Collectable
    {
        public string Name { get; private set; }
        public Hat(Texture2D texture, Vector2 playerPosition, string name)
            : base(texture, playerPosition, 1.0f, 0f)
        {
            position = new Vector2(playerPosition.X + (Texture.Width / 2), playerPosition.Y - (Texture.Height / 2));
            Name = name;
            Collected = false;
        }

        public void FlipHat()
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
        }
        public void ResetHatFlip()
        {
            spriteEffects = SpriteEffects.None;
        }

        //For usage within the player class to have the hat stick to the players head
        //A Vector2 containing the location of the top-left coordinates of the player</param>
        public void Update(Vector2 playerPosition, string direction)
        {
            position = new Vector2(playerPosition.X + ((Texture.Width * Scale) / 25f), playerPosition.Y);// - (Texture.Height / 2));
            if (Name == "Native Head Dress") { if (direction == "left") { position.X += 2; } else if (direction == "right") { position.X -= 5; } position.Y -= 5; }
        }

    } //End of Class
} //End of Namespace
