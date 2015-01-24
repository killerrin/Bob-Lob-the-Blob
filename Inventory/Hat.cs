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

namespace Inventory
{
    public class Hat : Collectable
    {
        public Hat(Texture2D texture, Vector2 playerPosition)
            :base(texture, playerPosition, 1.0f, 0f)
        {
            position = new Vector2(playerPosition.X + (Texture.Width / 2), playerPosition.Y - (Texture.Height / 2));
        }

        
        //For usage within the player class to have the hat stick to the players head
        //A Vector2 containing the location of the top-left coordinates of the player</param>
        public override void Update(Vector2 playerPosition)
        {
            position = new Vector2(playerPosition.X + (Texture.Width / 2), playerPosition.Y - (Texture.Height / 2));
        }

    } //End of Class
} //End of Namespace
