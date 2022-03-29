using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeMountains : Biome
    {


        public BiomeMountains(int seed) : base(seed)
        {
            this.frequencies = new float[]
           {
                0.005f, //lakeFrequency
                0.015f, //puddleFrequency    
                0.125f, //mountainFrequency    
                0.035f, //spareFrequency    
           };

            this.noiseTypes = new FastNoise.NoiseType[]
            {
                FastNoise.NoiseType.SimplexFractal,
                FastNoise.NoiseType.Perlin,
                FastNoise.NoiseType.CubicFractal,
                FastNoise.NoiseType.Perlin,
            };

            this.nutrientRate = 5;
            this.minDistanceBetweenSeed = 20;
            this.maxDistanceBetweenSeed = 50;
            this.waterLevel = 0.555f;
            this.deepWaterLevel = 0.6f;
            this.puddleLevel = 0.575f;
            this.stoneLevel = 0.205f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityTree),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityPsychedelicMushroom),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityAppleTree),
                    typeof(EntityPsychedelicMushroom),
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
            return ConsoleColor.DarkCyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.DarkGreen;
        }
    }
}
