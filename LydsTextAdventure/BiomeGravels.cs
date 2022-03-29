using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeGravels : Biome
    {

        public BiomeGravels(int seed) : base(seed)
        {

            this.frequencies = new float[]
           {
                0.115f, //lakeFrequency
                0.075f, //puddleFrequency    
                0.045f, //mountainFrequency    
                0.035f, //spareFrequency    
           };

            this.noiseTypes = new FastNoise.NoiseType[]
            {
                FastNoise.NoiseType.SimplexFractal,
                FastNoise.NoiseType.Perlin,
                FastNoise.NoiseType.Perlin,
                FastNoise.NoiseType.Perlin,
            };

            this.nutrientRate = 5;
            this.minDistanceBetweenSeed = 20;
            this.maxDistanceBetweenSeed = 50;
            this.waterLevel = 0.205f;
            this.deepWaterLevel = 0.40f;
            this.puddleLevel = 0.175f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityTree),
                    typeof(EntityBush),
                }}
            };

            this.CreateNoiseControllers();
        }

        public override bool GenerateLava()
        {

            return false;
        }


        public override ConsoleColor GetWaterColour()
        {
            return ConsoleColor.Cyan;
        }

        public override ConsoleColor GetDeepWaterColour()
        {
            return ConsoleColor.Cyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.Gray;
        }
    }
}
