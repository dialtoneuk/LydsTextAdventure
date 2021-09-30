using System;
using System.Threading.Tasks;

namespace LydsTextAdventure
{

    public class EntityMoving : Entity
    {

        public enum MovementType
        {
            VERTICAL,
            HORIZONTAL
        }

        protected bool isVertical = false;
        protected bool flip = false;
        private int speed = 10;

        public override void Update(int tick)
        {

            if (this.isVertical)
                this.VerticalMovement();
            else
                this.HoriziontalMovement();

            int speed = this.speed;
            if (this.speed > Entity.MaxSpeed)
                speed = Entity.MaxSpeed;

            this.Wait(Entity.MaxSpeed / speed); //will wait 1 second before updating again
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

            if (this.position.x < 500 && !flip)
            {
                this.position.x++;
            }
            else
            {

                flip = true;

                if (this.position.x > 0)
                    this.position.x--;
                else
                    flip = false;
            }
        }

        public void VerticalMovement()
        {

            if (this.position.y < 500 && !flip)
            {
                this.position.y++;
            }
            else
            {

                flip = true;

                if (this.position.y > 0)
                    this.position.y--;
                else
                    flip = false;
            }
        }

        public void SetSpeed(int speed)
        {

            this.speed = speed;
        }
    }
}