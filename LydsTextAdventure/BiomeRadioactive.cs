using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeRadioactive : Biome
    {

        public BiomeRadioactive(int seed) : base(seed)
        {

            this.frequencies = new float[]
            {
                0.030f, //lakeFrequency
                0.040f, //puddleFrequency    
                0.020f, //mountainFrequency    
                0.015f, //lavaFrequency      
            };

            this.minDistanceBetweenSeed = 30;
            this.maxDistanceBetweenSeed = 60;
            this.waterLevel = 0.38f;
            this.deepWaterLevel = 0.5f;
            this.puddleLevel = 0.40f;
            this.lavaLevel = 0.55f;
            this.stoneLevel = 0.45f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityDeadTree),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                }}
            };

            this.CreateNoiseControllers();
        }

        public override bool GenerateLava()
        {

            return true;
        }

        public override ConsoleColor GetWaterColour()
        {
            return ConsoleColor.DarkCyan;
        }

        public override ConsoleColor GetDeepWaterColour()
        {
            return ConsoleColor.Cyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.Red;
        }
    }
}
