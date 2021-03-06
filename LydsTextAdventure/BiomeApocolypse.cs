using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeApocolypse : Biome
    {


        public BiomeApocolypse(int seed) : base(seed)
        {

            this.frequencies = new float[]
            {
                    0.045f, //lakeFrequency
                    0.075f, //puddleFrequency    
                    0.025f, //mountainFrequency    
                    0.055f, //lavaFrequency  
            };

            this.minDistanceBetweenSeed = 30;
            this.maxDistanceBetweenSeed = 60;
            this.waterLevel = 0.65f;
            this.deepWaterLevel = 0.7f;
            this.puddleLevel = 0.6f;
            this.lavaLevel = 0.225f;
            this.stoneLevel = 0.5f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityOakTree),
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
            return ConsoleColor.Cyan;
        }

        public override ConsoleColor GetDeepWaterColour()
        {
            return ConsoleColor.DarkCyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.DarkYellow;
        }
    }
}
