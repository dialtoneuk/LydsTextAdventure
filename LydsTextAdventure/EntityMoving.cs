using System;

namespace LydsTextAdventure
{

    public class EntityMoving : Entity
    {

        public EntityMoving(string name = "") : base(name)
        {

            this.texture = new Texture(' ');
        }

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

            if (res > 0)
                this.Wait(res); //will wait 1 second before updating again
        }

        public void SetDistance(int distance)
        {

            this.distance = distance;
        }
        public int GetDistance()
        {

            return this.distance;
        }

        public void SetMovementType(MovementType type)
        {

            if (type == MovementType.VERTICAL)
            {
                this.isVertical = true;
            }
            else if (type == MovementType.HORIZONTAL)
            {
                this.isVertical = false;
            }
        }


        public void HoriziontalMovement()
        {

            if (!this.hasGoal)
            {

                if (!this.flip)
                    this.goal = this.position.x + this.distance;
                else
                    this.goal = this.position.x - this.distance;

                if (this.goal < 0)
                    this.goal = 0;

                this.hasGoal = true;
            }

            if (!this.flip)
            {

                if (this.position.x != this.goal)
                    MovementManager.MoveEntity(this, this.position.x + 1, this.position.y);
                else
                {
                    this.flip = true;
                    this.hasGoal = false;
                }
            }
            else
            {

                if (this.position.x != this.goal)
                    MovementManager.MoveEntity(this, this.position.x - 1, this.position.y);
                else
                {
                    this.flip = false;
                    this.hasGoal = false;
                }
            }
        }

        public void VerticalMovement()
        {

            if (this.position.y < this.distance && !this.flip)
            {
                MovementManager.MoveEntity(this, this.position.x, this.position.y + 1);
            }
            else
            {

                this.flip = true;

                if (this.position.y > 0)
                    MovementManager.MoveEntity(this, this.position.x, this.position.y - 1);
                else
                    this.flip = false;
            }
        }

        //will move speed + distance in a second
        public void SetSpeed(int speed)
        {

            this.speed = speed;
        }
    }
}