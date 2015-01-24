using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Bob_Lob_the_Blob
{
    public class PlayerInventory
    {
        //Will hold the Hats collected
        //The Time crystals collected

        //Dictionary Variables
        public Dictionary<int, Hat> CollectedHats { get; private set; } //Dictionary of Hats

        //Dictionary of Time Crystals. (string: Worldname, TimeCrystal[]:The time crystals for that world)
        public Dictionary<int, TimeCrystal[]> TimeCrystals { get; private set; }

        //Temporary Variables
        public int totalCollectedCrystals { get; set; } //Total Collected Crystals
        public int totalCollectedHats { get; private set; } //Total Collected Hats


        //Constructor
        public PlayerInventory() {
            //Initialize();
        }

    #region Service Methods

        //Reinitiliaze Inventory (For New game, will set all the Details to 0)
        public void Initialize() {
            CollectedHats = new Dictionary<int, Hat>();
            TimeCrystals = new Dictionary<int, TimeCrystal[]>();

            totalCollectedCrystals = 0;
            totalCollectedHats = 0;
        }

        //Initialize using a File
        public void Initialize(int TMP_PARAMETER)
        {
            CollectedHats = new Dictionary<int, Hat>();
            TimeCrystals = new Dictionary<int, TimeCrystal[]>();

            totalCollectedCrystals = 0;
            totalCollectedHats = 0;
        }

        /* ADD TO HATS
         * Param:   int Index - Hat index.
         *          Hat hat - An object of the Hat Struct
         * Return:  Void
         * Add to the Dictionary of Hats */
        public void AddToHats(int index, Hat hat)
        {
            totalCollectedHats++;
            CollectedHats.Add(index, hat);
        }

        /* GET HAT
         * Param:   int Index - Hat index.
         * Return:  Hat - Returns the specified hat
         * Get the hat at the specified index */
        public Hat GetHat(int index)
        {
            return CollectedHats[index];
        }

        /* ADD TO TIME CRYSTALS
         * Param:   int Index - World index.
         *          TimeCrystal[] tcArr - An array of the Time Crystals for that level
         * Return:  Void
         * Add to the Dictionary of TimesCrystals */
        public void AddToTC(int index, TimeCrystal[] tcArr)
        {
            TimeCrystals.Add(index, tcArr);
        }

        /* GET TCARRAY
         * Param:   String Index - Hat index.
         * Return:  Returns the array of Timecrystals for that world
         * Get the hat at the specified index */
        public TimeCrystal[] GetTC(int index)
        {
            return TimeCrystals[index];
        }

        public void SetTC(int index, TimeCrystal[] tcArr)
        {
            TimeCrystals[index] = tcArr;
        }

        public void DrawTC(int index, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < TimeCrystals[index].Length; i++)
            {
                TimeCrystals[index][i].Draw(spriteBatch);
            }
        }

    #endregion

    }//End of Class
}//End of Namespace
