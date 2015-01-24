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
    public class InventoryManager
    {
        //Variables
        public Dictionary<int, Hat> CollectedHats { get; private set; } //Dictionary of Hats
        public Dictionary<int, Hat> InventoryHats { get; private set; } //Dictionary of Hats
        public int totalCollectedCrystals { get; set; } //Total Collected Crystals
        public int totalCollectedHats { get; private set; } //Total Collected Hats
        public int totalInventoryHats { get; private set; }
        public int CurrentHatIndex { get; set; } // Used when cycling the hat on the player

        //Dictionary of Time Crystals. (string: Worldname, TimeCrystal[]:The time crystals for that world)
        public Dictionary<int, TimeCrystal[]> TimeCrystals { get; private set; }


    #region SERVICE METHODS

        #region Hats
            //Add to Collected Hats
            public void AddToCollectedHats(int index, Hat hat) {
                totalCollectedHats++;

                hat.Collected = true;
                InventoryHats[index].Activated = true;
                CollectedHats.Add(index, hat);
            }
        
            //Add to Inventory Hats
            public void AddToInventoryHats(int index, Hat hat) {
                totalInventoryHats++;
                InventoryHats.Add(index, hat);
                InventoryHats[index].Collected = false;
            }

            //Get Collected Hats
            public Hat GetCollectedHat(int index) {
                return CollectedHats[index];
            }

            //Get Inventory Hats
            public Hat GetInventoryHat(int index) {
                return InventoryHats[index];
            }
        
            //Get Current Hat
            public Hat GetHat(int index) {
                return CollectedHats[index];
            }
        #endregion

        #region Time Crystals
        //Add to TC Array
            public void AddToTC(int index, TimeCrystal[] tcArr) {
                TimeCrystals.Add(index, tcArr);
            }

            //Get TC Array
            public TimeCrystal[] GetTC(int index) {
                return TimeCrystals[index];
            }
        
            //Set TC Array - With Array
            public void SetTC(int index, TimeCrystal[] tcArr) {
                TimeCrystals[index] = tcArr;
            }

            //Draw TC Array
            public void DrawTC(int index, SpriteBatch spriteBatch)
            {
                for (int i = 0; i < TimeCrystals[index].Length; i++) {
                    TimeCrystals[index][i].Draw(spriteBatch);
                }
            }
        #endregion
    #endregion


    #region INITIALIZATION METHODS
        public void Initialize(ContentManager Content) {
                if (Game1.debug) //IN DEBUG STATE
                {
                    //ADD THE COLLECTED HATS
                    AddToCollectedHats(1, GetInventoryHat(1));
                    AddToCollectedHats(2, GetInventoryHat(2));
                }
                
                //Add the Blank Hat
                AddToCollectedHats(0, GetInventoryHat(0));

                //ADD THE TIME CRYSTAL ARRAYS FOR WHICH ARE COLLECTED
                AddToTC((int)LevelID.Blob, new TimeCrystal[3] { new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true) });

                AddToTC((int)LevelID.Egypt, new TimeCrystal[3] { new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true) });

                AddToTC((int)LevelID.Prehistoric, new TimeCrystal[3] { new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true),
                    new TimeCrystal(Content.Load<Texture2D>("Images/Collectibles/tc_large"), Vector2.Zero, 0.25f, true) });

                //Initialize it all to Zero for a new Game
                totalCollectedHats = -1;
                CurrentHatIndex = 0;
                totalCollectedCrystals = 0;
        }

        //Initialize All Hats in the Game Inventory (NOT PLAYERS)
        public void InitializeInvHats(ContentManager Content, GraphicsDevice Device) {
            Texture2D invisibleHatTexture = new Texture2D(Device, 140, 100);
            AddToInventoryHats(0, new Hat(invisibleHatTexture, Vector2.Zero, "No Hat"));
            AddToInventoryHats(1, new Hat(Content.Load<Texture2D>("Images/Collectibles/Hats/Hat4"), Vector2.Zero, "Baseball Cap"));
            AddToInventoryHats(2, new Hat(Content.Load<Texture2D>("Images/Collectibles/Hats/Hat2"), Vector2.Zero, "Sombrero"));
            AddToInventoryHats(3, new Hat(Content.Load<Texture2D>("Images/Collectibles/Hats/Hat6"), Vector2.Zero, "Brown Bag"));
            AddToInventoryHats(4, new Hat(Content.Load<Texture2D>("Images/Collectibles/Hats/Hat5"), Vector2.Zero, "Native Head Dress"));
        }
    #endregion

        //Constructor
        public InventoryManager()
        {
            CollectedHats = new Dictionary<int, Hat>();
            InventoryHats = new Dictionary<int, Hat>();
            TimeCrystals = new Dictionary<int, TimeCrystal[]>();
        }

    } //End of Class
} //End of Namespace