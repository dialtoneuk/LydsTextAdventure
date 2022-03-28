using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class Biome
    {

        public Dictionary<Tuple<float, float>, Type[]> biomeFoliage;
        public Random biomeRandom = new Random();

        public const int SEED_CHANCE = 2;
        public const int SEED_MAX_DISTANCE = 60;
        public const int SEED_MIN_DISTANCE = 20;

        public int lastDistance = 0;

        public Biome()
        {

            if (this.biomeFoliage == null)
                this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityBush)
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityAppleTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree)
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityAppleTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                }}
            };
        }

        public virtual bool GenerateMagma()
        {

            return true;
        }

        public virtual ConsoleColor GetGrassColour()
        {

            return ConsoleColor.Green;
        }

        public virtual ConsoleColor GetWaterColour()
        {

            return ConsoleColor.Blue;
        }

        public bool CanSeed(int nutrientRate, int chanceModifier = 10)
        {

            bool canSeed = false;

            //if last distance is greater than or equal to zero or we hit probability
            if (lastDistance <= 0)
            {

                //more of a chance
                int value = biomeRandom.Next(0, 100 + nutrientRate);

                //if we hit prob then can seed and reset distance
                if (value > 100 - chanceModifier)
                {
                    canSeed = true;
                    lastDistance = biomeRandom.Next(SEED_MIN_DISTANCE, SEED_MAX_DISTANCE);
                }
            }

            lastDistance--;
            return canSeed;
        }

        public Type[] GetFoliageTypes(int nutrientLevel)
        {

            List<Type> types = new List<Type>();
            foreach (KeyValuePair<Tuple<float, float>, Type[]> pair in this.biomeFoliage)
            {

                if (nutrientLevel > pair.Key.Item1 && nutrientLevel <= pair.Key.Item2)
                    types.AddRange(pair.Value);
            }

            return types.ToArray();
        }
    }
}
