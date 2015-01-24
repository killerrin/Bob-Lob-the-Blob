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
    public enum TileType
	{
	    Passable = 0,
        Impassable = 1,
        Checkpoint = 2,
        Exit = 3,
        Breakable = 4,
    }
    
    public class Tile
    {  //fields
        private Texture2D texture; //Image
        private TileType tileType; //to decide if tile is passable or whatever
        private int x, y;   //To store where in the array it is on the screen
        //public  int width;  //stores demensions of the tile
        //public  int height;
        public static Vector2 Size { private set; get; }
        public int NumberOfHits { set; get; }
        //props
        public TileType TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        //other
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(x * Size.X), (int)(y * Size.Y), (int)(Size.X), (int)(Size.Y));
            }
        }
        public Rectangle TopRectangle
        {
            get
            {
                return new Rectangle((int)(x * Size.X + 5), Bounds.Top, 54, 3);
            }
        }
        public Rectangle BottomRectangle
        {
            get
            {
                return new Rectangle((int)(x * Size.X + 5), Bounds.Bottom, 54, 3);
            }
        }
        public Rectangle RightRectangle
        {
            get
            {
                return new Rectangle(Bounds.Right - 2, (int)(y * Size.Y + 2), 2, 32);
            }
        }
        public Rectangle LeftRectangle
        {
            get
            {
                return new Rectangle(Bounds.Left, (int)(y * Size.Y + 2), 2, 32);
            }
        }
        //public static readonly Vector2 Size = new Vector2(width, height); //currently setting this up results in not being able to change the size of the tiles during runtime
      

        public Tile(Texture2D Texture, TileType TileType, int X, int Y)
        {
            texture = Texture;
            tileType = TileType;
            x = X;
            y = Y;
            Size = new Vector2(64, 36); //HARDCODED NOW< Dont know what I was thinking allowing us to control the size of the tile otherwise.
            NumberOfHits = 0;
        }
    

    }
}
