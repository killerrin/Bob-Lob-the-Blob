using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Bob_Lob_the_Blob
{
    public class InGameMenu
    {
        //Textures
        protected Texture2D blankTexture;

        //Fonts
        protected SpriteFont igmFont;
        protected SpriteFont igmFont_Large;

        //Variables
        protected int currSelection;
        protected List<RenderText> optionList;

        //Viewport
        protected Viewport viewport;

        //Constructor
        public InGameMenu(ContentManager Content, GraphicsDevice Device) {
            viewport = Device.Viewport;

            //Initialize Fonts
            igmFont = Content.Load<SpriteFont>("Fonts/igm_font");
            igmFont_Large = Content.Load<SpriteFont>("Fonts/igm_font_large");

            //Initialize Textures
            blankTexture = new Texture2D(Device, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });
        }

        //Update Method
        public void Update() {
            Input.Update(this);
        }

        //Draw Method
        public virtual void Draw(SpriteBatch spriteBatch) {
            //Call base.Draw from within the SpriteBatch Begin/End
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * 0.5f);
        }

        //Overriden in the Inheriting Classes
        public virtual void CursorMovement(String i)
        {  }

    }//End of Class
}//End of Namespace
