using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace SpriteClasses
{
    public class BackgroundLayer
    {
        //each layer is declared as an object of this class
        //-- only used in MultiBackground class
        public Texture2D Image { get; set; }
        public Vector2 position;
        //allows you to draw a layer further up/down or
        //left/right on the screen so you can get the 
        //layers positioned exactly the way you want them
        //without changing the image itself
        public Vector2 Offset { get; set; }
        public float Depth { get; set; }
        public float MoveRate { get; set; }
        public Vector2 ImageSize { get; set; }
        public Color Color { get; set; }

        public BackgroundLayer()
        {
            position = Vector2.Zero;
            Offset = Vector2.Zero;
            Depth = 0.0f;
            MoveRate = 0.0f;
            ImageSize = Vector2.Zero;
            Color = Color.White;
        }
    }

    public class MultiBackground
    {
        //these booleans control all movement
        private bool moving = false;
        private bool moveLeftRight = true;
        public int LayerCount { get; private set; }

        private Vector2 windowSize;
        //each layer gets added to this list--changed it to static
        public static List<BackgroundLayer> layerList = new List<BackgroundLayer>();

        //draw method creates its own batch
        private SpriteBatch batch;

        public MultiBackground(GraphicsDevice device)
        {
            windowSize.X = device.Viewport.Width;
            windowSize.Y = device.Viewport.Height;
            batch = new SpriteBatch(device);
        }

        //creates a layer from the data passed in and adds it to the list
        public void AddLayer(Texture2D picture, float depth, float moveRate)
        {
            BackgroundLayer layer = new BackgroundLayer();
            layer.Image = picture;
            //depth of 0 is closest, depth of 1 is furthest away
            layer.Depth = depth;
            //if moveRate is positive, it will move right or down
            //if its negative, it will move left or up
            layer.MoveRate = moveRate;
            layer.ImageSize = new Vector2(picture.Width, picture.Height*0.5f);//scaled the height might change later

            layerList.Add(layer);
            LayerCount++;
        }

        //Remove Layers - Every one that is in the LayerList
        public void RemoveLayers() {
            for (int i = layerList.Count - 1; i >= 0; i--) {
                layerList.RemoveAt(i);
            }
            LayerCount = 0;
        }

        //used to sort layers in Draw method
        public int CompareDepth(BackgroundLayer layer1, BackgroundLayer layer2)
        {
            //depth of 0 is closest, depth of 1 is furthest away
            if (layer1.Depth < layer2.Depth)
                return 1;
            if (layer1.Depth > layer2.Depth)
                return -1;
            if (layer1.Depth == layer2.Depth)
                return 0;
            return 0;
        }

        //similar to update 
        //but used to move one time based on a rate, 
        //rather than update, which is based on gametime
        public void Move(float rate)
        {
            float moveRate = rate / 60.0f;
            //loop through all layers in list
            foreach (BackgroundLayer layer in layerList)
            {
                //calculate distance to move 
                float moveDistance = layer.MoveRate * moveRate;
                //only do something if moving is false
                //(update method works if moving is true)
                if (!moving)
                {
                    //if moving left to right, change X
                    if (moveLeftRight)
                    {
                        layer.position.X += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.X = layer.position.X % layer.ImageSize.X;
                    }
                    //if moving up and down, change Y
                    else
                    {
                        layer.position.Y += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.Y = layer.position.Y % layer.ImageSize.Y;
                    }
                }
            }
        }

        //update should be called from the game1.cs update method
        public void Update(GameTime gameTime)
        {
            //loop through layers in list
            foreach (BackgroundLayer layer in layerList)
            {
                //this method will execute 60 times/second so divide by 60
                //to calculate how much to move each time
                float moveDistance = layer.MoveRate / 60.0f;
                //do something only if moving is true
                if (moving)
                {
                    //if moving left to right, change X
                    if (moveLeftRight)
                    {
                        layer.position.X += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.X = layer.position.X % layer.ImageSize.X;
                    }
                    //if moving up and down, change Y
                    else
                    {
                        layer.position.Y += moveDistance;
                        //reset to 0 once whole image has been drawn
                        layer.position.Y = layer.position.Y % layer.ImageSize.Y;
                    }
                }
            }
        }

        public void Draw()
        {

            //sort layers by depth
            layerList.Sort(CompareDepth);
            //draw uses its own batch
            batch.Begin();
            
            //loop through layers
            for (int i = 0; i < layerList.Count; i++)
            {
                //move left or right
                if (moveLeftRight)
                {
                    //draw1 - always done
                    batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X, layerList[i].position.Y - layerList[i].ImageSize.Y) + layerList[i].Offset, layerList[i].Color);
                    //draw2 - done only when image moving off screen
                    //use if statement so code works for both left to right 
                    //and right to left scrolling
                    if (layerList[i].position.X > 0.0f)  //used when scrolling from left to right
                    {
                        batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X - layerList[i].ImageSize.X, layerList[i].position.Y-layerList[i].ImageSize.Y) + layerList[i].Offset, layerList[i].Color);
                    }
                    else  //used when scrolling from right to left
                    {
                        if (Math.Abs(layerList[i].position.X) > layerList[i].Image.Width - windowSize.X)
                        {
                            batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X + layerList[i].ImageSize.X, layerList[i].position.Y-layerList[i].ImageSize.Y) + layerList[i].Offset, layerList[i].Color);
                        }
                    }
                }
                else  //move up or down
                {
                    //draw1 - always done
                    batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X, layerList[i].position.Y) + layerList[i].Offset, layerList[i].Color);
                    //draw2 - done only when image moving off screen
                    //use if statement so code works for both top to bottom 
                    //and bottom to top scrolling
                    if (layerList[i].position.Y > 0.0f)
                    {
                        //used when scrolling from top to bottom          
                        batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X, layerList[i].position.Y - layerList[i].ImageSize.Y) + layerList[i].Offset, layerList[i].Color);
                    }
                    else //used when scrolling from bottom to top 
                    {
                        if (Math.Abs(layerList[i].position.Y) > layerList[i].Image.Height - windowSize.Y)
                        {
                            batch.Draw(layerList[i].Image, new Vector2(layerList[i].position.X, layerList[i].position.Y + layerList[i].ImageSize.Y) + layerList[i].Offset, layerList[i].Color);
                        }
                    }
                }
            }
            batch.End();
        }

        //methods to set the booleans to true and false
        public void SetMoveUpDown()
        {
            moveLeftRight = false;
        }

        public void SetMoveLeftRight()
        {
            moveLeftRight = true;

        }

        public void Stop()
        {
            moving = false;
        }

        public void StartMoving()
        {
            moving = true;
        }

        // to switch the direction of the scrolling based on which direction the player is moving
        public void SwitchDirection(string direction)
        {
            if (direction == "left")
            {
                for(int i=0;i<layerList.Count;i++)
                {
                    if(layerList[i].MoveRate<0)
                    {
                        layerList[i].MoveRate *= -1;
                    }
                }
            }
            if (direction == "right")
            {
                for (int i = 0; i < layerList.Count; i++)
                {
                    if (layerList[i].MoveRate > 0)
                    {
                        layerList[i].MoveRate *= -1;
                    }
                }

            }
            if (direction == "up")
            {
                for (int i = 0; i < layerList.Count; i++)
                {
                    if (layerList[i].MoveRate < 0)
                    {
                        layerList[i].MoveRate *= -1;
                    }
                }
            }
            if (direction == "down")
            {
                for (int i = 0; i < layerList.Count; i++)
                {
                    if (layerList[i].MoveRate > 0)
                    {
                        layerList[i].MoveRate *= -1;
                    }
                }
            }
        }

        public void SetLayerPosition(int layerNumber, Vector2 offset)
        {
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            //change offset (start position) of a layer
            layerList[layerNumber].Offset = offset;
            layerList[layerNumber].position += offset;
        }

        public void SetLayerAlpha(int layerNumber, float percent)
        {
            //used for transparency
            if (layerNumber < 0 || layerNumber >= layerList.Count) return;

            float alpha = (percent / 100.0f);

            layerList[layerNumber].Color = new Color(new Vector4(0.0f, 0.0f, 0.0f, alpha));
        }
    }
}
