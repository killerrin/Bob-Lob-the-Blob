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
    //Class
    public class CreditManager
    {
        public bool Active;

        private enum CreditState {
            Scrolling,
            FadingOut
        }
        private CreditState state;

        private Texture2D background;

        private Color textColor;
        private SpriteFont smallFont;
        private SpriteFont largeFont;

        private List<RenderText> creditText;
        private List<CreditImage> creditImage;

        private Vector2 creditScrollSpeed;
        private Vector2 lastPosition;

        private int width;
        private int height;

        const float TEXT_SEPERATION = 50f;
        const float SIDE_BY_SIDE_IMAGE_SEPERATION = 25f;

        Texture2D blankTexture;
        private bool sceneIsFading = false;
        private bool sceneIsUnFading = false;
        private float currentTransparency = 0f;
        private const float TRANSPARENCY_RATE_OF_CHANGE = 0.0008f; //For optimal faster speed, 0.003f;

        //Cosntructor
        public CreditManager(ContentManager content, GraphicsDevice graphics)
        {
            Active = true;

            blankTexture = new Texture2D(graphics, 1, 1);
            blankTexture.SetData(new Color[] { Color.White });

            //background = content.Load<Texture2D>("Credits Dependancies/Credit Background");
            background = blankTexture;

            width = graphics.Viewport.Bounds.Width;
            height = graphics.Viewport.Bounds.Height;

            state = CreditState.Scrolling;

            creditScrollSpeed = new Vector2(0f, -0.06f);

            textColor = Color.White;
            lastPosition = new Vector2(width / 2f, height + TEXT_SEPERATION);

            smallFont = content.Load<SpriteFont>("Credits Dependancies/FontSmall");
            largeFont = content.Load<SpriteFont>("Credits Dependancies/FontLarge");

            creditText = new List<RenderText> { };
            creditImage = new List<CreditImage> { };

            //-- Set up the Credits Now --\\
            // Add the Text
            AddHeader("Bob Lob the Blob");
            AddContent("GAME 220 - Game Dynamics 01");
            AddSpacing();
            AddSpacing();

            //Programmers
            AddHeader("Programmers");
            AddContent("Patrick Barahona-Griffiths");
            AddContent("Sarah Childs");
            AddContent("Andrew Godfroy");
            AddContent("Brian Lefrancois");
            AddContent("Cassandra Siewert");
            AddSpacing();

            //Artists
            AddHeader("Artists");
            AddContent("Cassandra Siewert");
            AddSpacing();

            //Designers
            AddHeader("Level Designers");
            AddContent("Logan Holzwarth");
            AddSpacing();

            //Leads
            AddHeader("Project Lead");
            AddContent("Sarah Childs");
            AddSpacing();

            AddHeader("Technical Leads");
            AddContent("Andrew Godfroy");
            AddContent("Brian Lefrancois");
            AddSpacing();

            AddHeader("Creative Lead");
            AddContent("Sarah Childs");
            AddSpacing();

            AddHeader("Special Thanks");
            AddContent("Justin Kan for Motivational Support");

            //Add Images

            //Finally, put in the Humber College Logo
            AddSpacing();
            AddSpacing();
            AddSpacing();
            AddSpacing();
            AddSpacing();
            AddCenteredImage(content.Load<Texture2D>("Credits Dependancies/Humber College Logo Small"));
        }

        public void AddHeader(string text)
        {
            Vector2 textSize = largeFont.MeasureString(text);

            creditText.Add(new RenderText(text, new Vector2((width / 2f) - (textSize.X / 2f), lastPosition.Y + TEXT_SEPERATION), textColor, largeFont));
            lastPosition.Y += TEXT_SEPERATION;
        }

        public void AddContent(string text)
        {
            Vector2 textSize = smallFont.MeasureString(text);

            creditText.Add(new RenderText(text, new Vector2((width / 2f) - (textSize.X / 2f), lastPosition.Y + TEXT_SEPERATION), textColor, smallFont));
            lastPosition.Y += TEXT_SEPERATION;
        }

        public void AddSpacing(float seperation)
        {
            lastPosition.Y += seperation;
        }
        public void AddSpacing()
        {
            lastPosition.Y += TEXT_SEPERATION;
        }

        public void AddCenteredImage(Texture2D texture)
        {
            Vector2 position = new Vector2(width / 2f, lastPosition.Y + TEXT_SEPERATION);
            position.X -= (texture.Width / 2f);

            creditImage.Add(new CreditImage(texture, position));
            lastPosition.Y += texture.Height + TEXT_SEPERATION;
        }

        public void AddTwoImage(Texture2D texture1, Texture2D texture2)
        {
            creditImage.Add(new CreditImage(texture1, new Vector2(50f, lastPosition.Y + TEXT_SEPERATION)));
            creditImage.Add(new CreditImage(texture2, new Vector2(50f + texture1.Width + SIDE_BY_SIDE_IMAGE_SEPERATION, lastPosition.Y + TEXT_SEPERATION)));
            lastPosition.Y += (texture1.Height > texture2.Height) ? texture1.Height : texture2.Height + TEXT_SEPERATION;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Escape) 
                || padState.IsButtonDown(Buttons.B))
            {
                Active = false;
                Game1.gameState = GameState.MainMenu;
            }


            switch (state)
            {
                case CreditState.Scrolling:
                    if (creditImage[creditImage.Count() - 1].position.Y <= ((height / 2) - (creditImage[creditImage.Count() - 1].image.Height / 2)))
                    {
                        sceneIsFading = true;
                        state = CreditState.FadingOut;
                        break;
                    }

                    float elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
                    foreach (RenderText i in creditText)
                    {
                        i.Location += creditScrollSpeed * elapsedTime;
                    }
                    foreach (CreditImage i in creditImage)
                    {
                        i.position += creditScrollSpeed * elapsedTime;
                    }
                    break;
                case CreditState.FadingOut:
                    //Active = false;
                    FadingTransition(gameTime);
                    break;
            }
            Input.prevKeyState = keyState;
            Input.prevPadState = padState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // uncomment if we want to use a specific image. Since code is already set up to support images, just replace the image inside of Content/CreditDependancies/
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);

            foreach (RenderText i in creditText) { i.Draw(spriteBatch); }
            foreach (CreditImage i in creditImage) { i.Draw(spriteBatch); }

            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, width, height), Color.Black * currentTransparency);//fadeColour);

            spriteBatch.End();
        }

        private void FadingTransition(GameTime gameTime)
        {
            if (sceneIsFading)
            {
                currentTransparency += TRANSPARENCY_RATE_OF_CHANGE * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentTransparency >= 1)
                {
                    sceneIsFading = false;
                    //sceneIsUnFading = true;

                    // Skip unfading and go directly to main menu.
                    Game1.gameState = GameState.MainMenu; 
                    Active = false;
                }
            }
            if (sceneIsUnFading)
            {
                currentTransparency -= TRANSPARENCY_RATE_OF_CHANGE * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentTransparency <= 0)
                {
                    currentTransparency = 0;
                    sceneIsFading = false;
                    sceneIsUnFading = false;
                    Active = false;
                }
            }
        }
    }//End of Class

    //Credit Image Class
    class CreditImage
    {
        public Texture2D image;
        public Color color;
        public Vector2 position;

        public CreditImage(Texture2D texture, Vector2 location)
        {
            image = texture;
            position = location;
            color = Color.White;
        }

        public void Update(GameTime gameTime) {  }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, color);
        }

    }
}//End of Namespace
