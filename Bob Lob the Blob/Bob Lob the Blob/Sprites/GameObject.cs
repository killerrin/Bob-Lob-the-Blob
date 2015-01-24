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
using SpriteManager;

namespace Bob_Lob_the_Blob
{
    public class GameObject : Sprite
    {

        public bool IsObtained { set; get; }
        public override Rectangle CollisionRectangle
        {           //just setting one rectangle for now for basic colision detection, can be done more ifficiently by using the GetBounds Method in Level I think.//BRIAN
            get
            {
                switch (Name)
                {
                    case "SawBladeUp":
                        return new Rectangle((int)(Position.X + 5), (int)(Position.Y + 5),
                                     (int)(width * Scale) - 10, (int)(height * Scale));
                    case "SawBladeLeft":
                        return new Rectangle((int)(Position.X + 10), (int)(Position.Y + 10),
                                     (int)(width * Scale - 20), (int)(height * Scale - 30));
                    case "Checkpoint1":
                        return new Rectangle((int)(Position.X), (int)(Position.Y),
                                     (int)(width * Scale), (int)(height * Scale));
                    case "Checkpoint2":
                        return new Rectangle((int)(Position.X), (int)(Position.Y),
                                     (int)(width * Scale), (int)(height * Scale));
                    case "Checkpoint3":
                        return new Rectangle((int)(Position.X), (int)(Position.Y),
                                     (int)(width * Scale), (int)(height * Scale));


                    case "ArrowUp":
                        return new Rectangle((int)(Position.X), (int)(Position.Y - (height * Scale) * range),
                                     (int)(width * Scale), (int)(height * Scale * range));
                    case "ArrowDown":
                        return new Rectangle((int)(Position.X), (int)(Position.Y + height * Scale),
                                     (int)(width * Scale), (int)(height * Scale * range));
                    case "ArrowRight":
                        return new Rectangle((int)(Position.X + width * Scale), (int)(Position.Y),
                                     (int)(width * Scale * range), (int)(height * Scale));
                    case "ArrowLeft":
                        return new Rectangle((int)(Position.X - width * Scale * range), (int)(Position.Y),
                                     (int)(width * Scale * range), (int)(height * Scale));
                    case "Switch1":
                    case "Switch2":
                    case "Switch3":
                    case "Switch4":
                    case "Switch5":
                    case "Switch6":
                    case "Switch7":
                    case "Switch8":
                    case "Switch9":
                    case "Switch10":
                    case "ESwitch1":
                    case "ESwitch2":
                    case "ESwitch3":
                    case "ESwitch4":
                    case "ESwitch5":
                    case "ESwitch6":
                    case "ESwitch7":
                    case "ESwitch8":
                    case "ESwitch9":
                    case "ESwitch10":
                    
                        return new Rectangle((int)(Position.X), (int)(Position.Y + height * Scale / 2),
                                     (int)(width * Scale), (int)(height * Scale / 2));        



                    default:
                        return new Rectangle((int)(Position.X), (int)(Position.Y),
                                     (int)(width * Scale), (int)(height * Scale));

                }

            }
        }
        public List<GameObject> listOfArrows { set; get; }
        public DoorType doorType { set; get; }
        public float AliveTime { set; get; }
        //ArrowTrap Variables
        protected int range;
        public bool targetInRange = false;
        public bool RemoveArrow { set; get; }

        

        public GameObject(string name, Texture2D Texture, int frames, int animations,
           float scale, float rotation)
            : base(Texture, frames, animations, scale, rotation)
        {
            Name = name;
        }

        public virtual void ShootArrow(Texture2D texture)
        {
            throw new Exception("Skeleton Method");
        }
        public virtual void Chase(GameTime gameTime, Vector2 normalDirection)
        {
            throw new Exception("Skeleton Method");
        }
        public virtual void Move()
        {
            throw new Exception("Skeleton Method");
        }
    }
}
