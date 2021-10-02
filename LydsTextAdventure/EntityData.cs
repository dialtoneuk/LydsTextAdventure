using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{

    [Serializable]
    public class EntityData
    {


        public Type type;
        public int x;
        public int y;
        public int health;
        public string name;

        public EntityData(Entity entity)
        {

            this.x = entity.position.x;
            this.y = entity.position.y;
            this.name = entity.GetName();
            this.health = entity.GetHealth();
            this.type = entity.GetType();
        }
    }
}
