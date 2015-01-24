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
    public class Collectable
    {

        protected Vector2 position;
        protected Vector2 spriteOrgin;
        protected Color colour;
        public Texture2D Texture { get; private set; }
        public bool Activated;
        public float Scale { get; set; }
        public float Rotation { get; set; }
        SpriteEffects spriteEffects;

        // These 4 variables are used for floating the collectible up and down on the map
        private Vector2 originalPosition;
        private bool floatingUp;
        private const int FLOAT_SPEED = 2;
        private const int FLOAT_MAX = 25;

        //Constructor
        public Collectable(Texture2D texture, Vector2 pposition, float scale, float rotation)
        {
            Texture = texture;
            spriteOrgin = Vector2.Zero;
            position = pposition;
            colour = Color.White;
            Scale = scale;
            Rotation = rotation;
            spriteEffects = SpriteEffects.None;
            Activated = true;

            floatingUp = true;
            originalPosition = position;
        }


        //For usage when the collectible is on the map. This causes the object to float up and down
        public virtual void Update(GameTime gameTime)
        {
            if (Activated)
            {
                if (floatingUp)
                {
                    position.Y -= FLOAT_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                    if (position.Y <= (originalPosition.Y - FLOAT_MAX)) { floatingUp = false; }
                }
                else
                {
                    position.Y += FLOAT_SPEED * gameTime.ElapsedGameTime.Milliseconds;
                    if (position.Y >= (originalPosition.Y + FLOAT_MAX)) { floatingUp = true; }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Activated)
            {
                spriteBatch.Draw(Texture, position, null, colour, Rotation, spriteOrgin, Scale, spriteEffects, 0f);
            }
        }
    } //End of Class
} //End of Namespace
