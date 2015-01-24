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
    public class InventoryManager
    {
        //Dictionary of Hats
        private Dictionary<int, Hat> hats;

        /* ADD TO HATS
         * Param:   int Index - Hat index.
         *          Hat hat - An object of the Hat Struct
         * Return:  Void
         * Add to the Dictionary of Hats */
        public void AddToHats(int index, Hat hat)
        {
            totalCollectedHats++;
            hats.Add(index, hat);
        }

        /* GET HAT
         * Param:   int Index - Hat index.
         * Return:  Void
         * Get the hat at the specified index */
        public Hat GetHat(int index)
        {
            return hats[index];
        }

        public int totalCollectedCrystals { get; set; }
        public int totalCollectedHats { get; private set; }

        //Constructor
        public InventoryManager()
        {
            hats = new Dictionary<int, Hat>();
            totalCollectedHats = 0;
            totalCollectedCrystals = 0;
        }

    } //End of Class
} //End of Namespace
