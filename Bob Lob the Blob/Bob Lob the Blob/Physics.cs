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
    //added debugger, FIX PLAYER AND PHYSICS DUDE

    public static class Physics
    {
        private const float GRAVITY = 350;
        private const float CO_EFF_FRIC = 10.00f * 100000;
        public static float prevVelX = 0;

        //public Vector2 finalVelocity, 
        //               acceleration; 


        //Might have to figure out which direction to return.

        //public float applyFriction(){
        //    Vector2 frictionForce;

        //    return frictionForce;
        //}

        //not returning the right force atm

        public static float applyImpulseForce(int mass, Vector2 velocityI, float timeLapse, bool bounce, Vector2 force)
        {
            float impulse;

            if (bounce)
            {
                float epsilon = 0.8f;
                impulse = mass * (epsilon * velocityI.Y) - (mass * velocityI.Y);
            }
            else
                impulse = -(mass * velocityI.Y);


            return impulse / timeLapse;
        }

        private static Vector2 getAcceleration(Vector2 force, int mass)
        {

            return force / mass;
        }

        public static Vector2 getVelocity(Vector2 velocityI, Vector2 force, float timeLapse, int mass)
        {

            //debug only
            Level.debug.acceleration = getAcceleration(force, mass);

            return velocityI + getAcceleration(force, mass) * timeLapse;
        }

        public static Vector2 getPosition(Vector2 velocityI, Vector2 force, float timeLapse, int mass)
        {

            return velocityI * timeLapse + (0.5f * getAcceleration(force, mass) * timeLapse * timeLapse);

        }

        //========================================v SARA LOOK HERE FOR CHANGED CODE v=====================//
        public static float applyNormalForce(CollisionArea collision, int mass, Vector2 force)
        {

            switch (collision)
            {
                case CollisionArea.Top:
                    return -(applyGravityForce(mass));

                case CollisionArea.Bottom:
                    return force.Y;

                case CollisionArea.Left:
                    return -force.X;

                case CollisionArea.Right:
                    return -force.X;

                default:
                    return applyGravityForce(mass);
            }
        }
        //========================================^ SARA LOOK HERE FOR CHANGED CODE ^=====================//

        public static float applyGravityForce(int mass)
        {

            return mass * GRAVITY;
        }

        //========================================v SARA LOOK HERE FOR CHANGED CODE v=====================//
        public static float getFrictionForce(int mass, Vector2 force, Vector2 velocityI)
        {
            float frictionForce = 0;

            if (velocityI.X > 0)
            {
                frictionForce = CO_EFF_FRIC / force.Y;
            }
            else if (velocityI.X < 0)
            {
                frictionForce = -(CO_EFF_FRIC / force.Y);
            }

            return frictionForce;
        }
        //========================================^ SARA LOOK HERE FOR CHANGED CODE ^=====================//
    }
}
