using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeRadioactive : Biome
    {

        public new const int SEED_MIN_DISTANCE = 40;
        public new const int SEED_MAX_DISTANCE = 60;


        public BiomeRadioactive()
        {

            this.nutrientRate = 1;
            this.fractalGain = 20f;
            this.STONE_LEVEL = 0.30f;
            this.LAVA_LEVEL = 0.08f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityDeadTree),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }}
            };
        }

        public override bool GenerateMagma()
        {

            return false;
        }

        public override ConsoleColor GetWaterColour()
        {
            return ConsoleColor.DarkBlue;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.Red;
        }
    }
}
