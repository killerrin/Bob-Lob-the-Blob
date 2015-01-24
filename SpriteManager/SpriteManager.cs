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
    public class SpriteManager
    {

        protected Texture2D Texture;
        public Vector2 position;                        //***UPDATE Made public aswell for level collision detection. there must be a simpler way.
        protected Color Color = Color.White;
        public Vector2 spriteOrigin;
        public float Rotation;
        public float Scale;
        public SpriteEffects SpriteEffect;

        protected Dictionary<string, Rectangle[]> Animations =
                new Dictionary<string, Rectangle[]>(); // works similarily to lists. Stores a key and a rectangle. 

        public int FrameIndex { set; get; } // current frame.
        public string Animation; // name of the animation plaSprite1

        protected int Frames; // number of frames in all of the animation?
        public int height { set; get; }
        public int width { set; get; }

        public virtual Rectangle CollisionRectangle
        {           //just setting one rectangle for now for basic colision detection, can be done more ifficiently by using the GetBounds Method in Level I think.//BRIAN
            get
            {
                return new Rectangle((int)(Position.X), (int)(Position.Y),
                                     (int)(width * Scale), (int)(height * Scale));
            }
        }
        public Rectangle TopRectangle
        {
            get
            {
                return new Rectangle((int)(Position.X + 5), CollisionRectangle.Top, (int)(width * Scale - 10), 3);
            }
        }
        public Rectangle BottomRectangle
        {
            get
            {
                return new Rectangle((int)(position.X + 5), CollisionRectangle.Bottom - 3, (int)(width * Scale - 10), 3);
            }
        }
        public Rectangle RightRectangle
        {
            get
            {
                return new Rectangle(CollisionRectangle.Right - 2, (int)(position.Y + 2), 2,(int)(height * Scale - 4));
            }
        }
        public Rectangle LeftRectangle
        {
            get
            {
                return new Rectangle(CollisionRectangle.Left, (int)(position.Y + 2), 2, (int)(height * Scale - 4 ));
            }
        }
        
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //Constructor
        public SpriteManager(Texture2D Texture, int frames, int animations, float scale, float rotation)
        {
            this.Texture = Texture;
            Frames = frames;
            Scale = scale;
            Rotation = rotation;
            width = Texture.Width / frames; // gets the width of a single image 
            height = Texture.Height / animations; // gets hieght of a single image

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Animations[Animation][FrameIndex],
                Color, Rotation, spriteOrigin, Scale, SpriteEffect, 0f);

        }

        public void AddAnimation(string name, int rows)
        {
            Rectangle[] rectangles = new Rectangle[Frames];
            for (int i = 0; i < Frames; i++)
            {
                rectangles[i] = new Rectangle(i * width,
                    (rows - 1) * height, width, height);
            }
            Animations.Add(name, rectangles);
        } // take the name of the animation, and the row it is on.

    }
}


// code to add to Game1  
// SpriteManager loading;

// add in Load Content Method 
//loading = new SpriteManager(
//      Content.Load<Texture2D>("LoadingCircle"), 8);
//loading.Position = new Vector2 (100,100);

// Draw Method
// spriteBatch.Begin();
// loading.Draw(spriteBatch);
// spriteBatch.End();  