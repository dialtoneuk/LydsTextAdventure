using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeFlatlands : Biome
    {

        public new const int SEED_MIN_DISTANCE = 5;
        public new const int SEED_MAX_DISTANCE = 20;


        public BiomeFlatlands()
        {

            this.nutrientRate = 4;
            this.fractalGain = 0.8f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityFallenTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityFallenTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityFallenTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityOakTree),
                    typeof(EntityFallenTree),
                    typeof(EntityOakTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                }}
            };
        }

        public override bool GenerateMagma()
        {

            return false;
        }

        public override ConsoleColor GetWaterColour()
        {
            return ConsoleColor.Cyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.DarkGreen;
        }
    }
}
