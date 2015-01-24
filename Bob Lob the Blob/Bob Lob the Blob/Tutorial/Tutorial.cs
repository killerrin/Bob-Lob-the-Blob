using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bob_Lob_the_Blob
{
    public class TutorialImage
    {
        private Texture2D image;
        private string text;
        private SpriteFont tu_font;
        private Vector2 textPosition;

        public TutorialImage(Texture2D _image, string _text, SpriteFont font, GraphicsDevice Device)
        {
            image = _image;
            text = _text;
            tu_font = font;

            textPosition = new Vector2(Device.Viewport.Width - tu_font.MeasureString(text).X - 20, Device.Viewport.Height - tu_font.MeasureString(text).Y - 20);
        }

        public void Draw(SpriteBatch spriteBatch, float alphaValue)
        {
            spriteBatch.Draw(image, Vector2.Zero, Color.White * alphaValue);
            spriteBatch.DrawString(tu_font, text, textPosition, Color.White * alphaValue);
        }
    }

    public class Tutorial
    {
    #region VARIABLES
        //States for the Tutorial
        private enum State
        {
            Intro,
            Tutorial
        }
        private State state;


        //States for the Transition
        private enum Transition
        {
            FadingIn,
            FadeInComplete,
            FadingOut,
            FadeOutComplete
        }

        private Transition transState;

        //Intro
        private List<TutorialImage> IntroImages;
        private const int INTRO_IMAGES = 7;
        private int currImage;

        //Tutorial Images
        private List<TutorialImage> TutImages;
        private const int TU_IMAGES = 2;

        //Fading
        private Texture2D blankTexture;
        private float timer;
        private float alphaValue;

        //Fonts
        private SpriteFont tu_font;

    #endregion


    #region INITIALIZATION
        //Constructor
        public Tutorial(ContentManager Content, GraphicsDevice Device) {
            state = State.Intro;

            tu_font = Content.Load<SpriteFont>("Fonts/tu_font");

            blankTexture = new Texture2D(Device, 1, 1);
            blankTexture.SetData(new Color[] { Color.Black });

            //Transition Variables
            timer = 0;
            alphaValue = 0;
            currImage = 0;

            InitIntro(Content, Device);
            InitTutorial(Content, Device);
        }

        //Initialize Introduction Sequence
        public void InitIntro(ContentManager Content, GraphicsDevice Device) {
            IntroImages = new List<TutorialImage>();
            try {
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/00"), "50 A.E (After Earth) - Nuclear Power Plant", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/01"), "1200 A.E - Current Era in the Blob Empire", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/02"), "1240 A.E - Bob Lob on his way to the Royal Hat Fitting", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/03"), "1240 A.E - Attack of the energy thieves", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/04"), "1240 A.E - Energy decreasing!", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/05"), "1240 A.E - Time energy flux forms!", tu_font, Device));
                IntroImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Intro/06"), "XXXX A.E - Current Era Unknown.", tu_font, Device));
            }
            catch (ContentLoadException ex) {
                System.Windows.Forms.MessageBox.Show("An image for the Introduction cannot be found: " + ex.Message);
            }
        }

        //Initialize Tutorial Sequence
        public void InitTutorial(ContentManager Content, GraphicsDevice Device) {
            TutImages = new List<TutorialImage>();
            try {
                TutImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Tu/00"), "", tu_font, Device));
                TutImages.Add(new TutorialImage(Content.Load<Texture2D>("Tutorial/Tu/01"), "", tu_font, Device));
            }
            catch (ContentLoadException ex) {
                System.Windows.Forms.MessageBox.Show("An image for the Introduction cannot be found: " + ex.Message);
            }
        }
    #endregion

    #region GAME LOOP
        //Update Method
        public void Update(GameTime gameTime) {
            switch (transState) {
                case Transition.FadingIn:
                    alphaValue += 0.25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (alphaValue > 1) {
                        transState = Transition.FadeInComplete;
                        alphaValue = 1;
                        
                        if (state == State.Tutorial)
                            timer = 2;
                        else
                            timer = 1;
                    }
                    break;
                case Transition.FadeInComplete:
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer < 0) {
                        transState = Transition.FadingOut;
                        timer = 0;
                    }
                    break;
                case Transition.FadingOut:
                    alphaValue -= 0.25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (alphaValue < 0) {
                        transState = Transition.FadeOutComplete;
                        alphaValue = 0;

                        if (state == State.Tutorial)
                            timer = 2;
                        else
                            timer = 1;
                    }
                    break;
                case Transition.FadeOutComplete:
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (timer < 0) {
                        transState = Transition.FadingIn;
                        timer = 0;

                        currImage++;
                    }
                    break;
            }

            switch (state) {
                case State.Intro:
                    if (transState == Transition.FadingIn)
                        if (currImage > INTRO_IMAGES - 1) {
                            state = State.Tutorial;
                            currImage = 0;
                        }
                    break;
                case State.Tutorial:
                    if (transState == Transition.FadingIn)
                        if (currImage > TU_IMAGES - 1) {
                            Game1.gameState = GameState.MainMenu;
                            currImage = 0;
                        }
                    break;
                default:
                    break;
            }


            //Input
            KeyboardState keyState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Escape)
                || padState.IsButtonDown(Buttons.B))
            {
                state = State.Intro;
                transState = Transition.FadingIn;
                currImage = 0;
                alphaValue = 0;

                Game1.gameState = GameState.MainMenu;
            }
        }

        //Draw Method
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            switch (state) {
                case State.Intro:
                    IntroImages[currImage].Draw(spriteBatch, alphaValue);
                    break;
                case State.Tutorial:
                    TutImages[currImage].Draw(spriteBatch, alphaValue);
                    break;
                default:
                    break;
            }

            spriteBatch.End();
        }
    #endregion

    }//End of Class
}//End of Namespace