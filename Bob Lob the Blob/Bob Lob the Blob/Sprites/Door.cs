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
using SpriteManager;


namespace Bob_Lob_the_Blob
{
    public enum DoorType
    {
        Normal = 0,
        Elevator = 1,
        SlowDecending = 2,
        PushingRight = 3,
        Rocket = 4, ////so yeah dont need to keep this but......
    }
    public class Door : GameObject
    {
        private Vector2 origPos;
        private float doorSize; 
        public Door(string _name, Texture2D Texture, int frames, int animations,
           float scale, float rotation, Vector2 _position, DoorType _doorType, int _doorSize)
            : base(_name, Texture, frames, animations, scale, rotation)
        {
            origPos = _position;
            doorType = _doorType;
            switch (doorType)
            {
                case DoorType.Normal:
                case DoorType.Elevator:
                case DoorType.SlowDecending:
                case DoorType.Rocket:
                    doorSize = (int)(_doorSize * Tile.Size.Y);
                    break;
                case DoorType.PushingRight:
                    doorSize = (int)(_doorSize * Tile.Size.X);
                    break;
                default:
                    break;
            }
            
            
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            switch (doorType)
            {
                case DoorType.Normal:
                    if (IsActive && position.Y > origPos.Y - doorSize)
                        position.Y -= 4;
                    else if (!IsActive && position.Y < origPos.Y)
                        position.Y += 4;
                    break;
                case DoorType.Elevator:
                    if (IsActive && position.Y > origPos.Y - doorSize)
                        position.Y -= 10;
                    
                    else if (!IsActive && position.Y < origPos.Y)
                        position.Y += 10;
                    break;
                case DoorType.SlowDecending:
                    if (IsActive && position.Y >= origPos.Y - doorSize)
                    {
                        position.Y -= 30;
                        if (position.Y < origPos.Y - doorSize)
                            position.Y = origPos.Y - doorSize;
                    }
                    else if (!IsActive && position.Y <= origPos.Y)
                        position.Y += .7f;
                    if (position.Y > origPos.Y)
                        position.Y = origPos.Y;
                    break;
                case DoorType.PushingRight:
                    if (IsActive && position.X < origPos.X + doorSize)
                        position.X += 25;
                    else if (!IsActive && position.X > origPos.X)
                        position.X -= 25;
                    break;
                case DoorType.Rocket:
                    if (IsActive && position.Y > origPos.Y - doorSize)
                        position.Y -= 10;

                    else if (!IsActive && position.Y < origPos.Y)
                        position.Y += 10;
                    break;
                
                default:
                    break;
            }
            
            
                
            
            
            IsActive = false;
        }
    }
}
