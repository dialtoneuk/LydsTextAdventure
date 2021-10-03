﻿using System;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    public class EntityMoving : Entity
    {

        public EntityMoving(string name = "") : base(name) { }

        public enum MovementType
        {
            VERTICAL,
            HORIZONTAL
        }

        protected bool isVertical = false;
        protected bool flip = false;
        protected bool hasGoal = false;
        private int speed = 10;
        private int distance = 500;
        private int goal = 0;

        public override void Update(int tick)
        {

            if (this.isVertical)
                
                this.VerticalMovement();
            else
                this.HoriziontalMovement();

            int speed = this.speed;
            if (this.speed > Entity.MaxSpeed)
                speed = Entity.MaxSpeed;

            int res = Entity.MaxSpeed / Math.Max(1, Math.Min(Entity.MaxSpeed, speed));

            if(res > 0 )
                this.Wait(res); //will wait 1 second before updating again
        }

        public void SetDistance(int distance)
        {

            this.distance = distance;
        }

        public override Texture GetTexture()
        {

            return new Texture('F');
        }

        public int GetDistance()
        {

            return this.distance;
        }

        public void SetMovementType(MovementType type)
        {

            if(type == MovementType.VERTICAL)
            {
                isVertical = true;
            } 
            else if ( type == MovementType.HORIZONTAL )
            {
                isVertical = false;
            }
        }


        public void HoriziontalMovement()
        {

            if (!this.hasGoal)
            {

                if (!flip)
                    goal = this.position.x + this.distance;
                else
                    goal = this.position.x - this.distance;

                if (goal < 0)
                    goal = 0;

                this.hasGoal = true;
            }

            if (!flip)
            {

                if (this.position.x != goal)
                    MovementManager.MoveEntity(this, this.position.x + 1, this.position.y);
                else
                {
                    flip = true;
                    this.hasGoal = false;
                }
            }
            else
            {

                if (this.position.x != goal)
                    MovementManager.MoveEntity(this, this.position.x - 1, this.position.y);
                else
                {
                    flip = false;
                    this.hasGoal = false;
                }
            }
        }

        public void VerticalMovement()
        {

            if (this.position.y < this.distance && !flip)
            {
                MovementManager.MoveEntity(this, this.position.x, this.position.y + 1);
            }
            else
            {

                flip = true;

                if (this.position.y > 0)
                    MovementManager.MoveEntity(this, this.position.x, this.position.y - 1);
                else
                    flip = false;
            }
        }

        //will move speed + distance in a second
        public void SetSpeed(int speed)
        {

            this.speed = speed;
        }
    }
}