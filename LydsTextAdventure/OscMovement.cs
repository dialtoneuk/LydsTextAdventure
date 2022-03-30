using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class OscMovement
    {

        public int angle = -180;
        public int aDirection = -1;
        public readonly int scale;
        public readonly bool absolute;
        public readonly int speed;

        public enum Direction
        {
            Horizontal,
            Vertical
        }

        protected Entity entity;
        protected Position startingPosition;
        protected Direction direction;

        public OscMovement(Entity entity, int scale = 4, int speed = 2, Direction direction = Direction.Horizontal, bool absolute = true)
        {

            this.entity = entity;
            this.scale = scale;
            this.speed = speed;
            this.direction = direction;
            this.absolute = absolute;
        }

        public int GetNumber()
        {

            if (startingPosition == null)
                startingPosition = new Position(this.entity.position.x, this.entity.position.y);

            if (angle < 180 && angle > -180)
                angle = angle + (speed * aDirection);
            else
            {
                aDirection = aDirection * -1;
                angle = angle + (speed * aDirection);
            }

            int final = (int)(Math.Max(1, scale) * (Math.Sin((angle * (Math.PI)) / 180.0)));

            if (absolute)
                final = Math.Abs((int)final);

            if (direction == Direction.Horizontal)
                return this.startingPosition.x + final;
            else
                return this.startingPosition.y + final;
        }

        public Position GetPosition()
        {

            if (startingPosition == null)
                startingPosition = new Position(this.entity.position.x, this.entity.position.y);

            if (direction == Direction.Horizontal)
                return new Position(this.GetNumber(), this.startingPosition.y);
            else
                return new Position(this.startingPosition.x, this.GetNumber());
        }
    }
}
