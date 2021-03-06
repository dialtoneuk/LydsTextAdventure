using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeMushrooms : Biome
    {


        public BiomeMushrooms(int seed) : base(seed)
        {
            this.frequencies = new float[]
           {
                0.0515f, //lakeFrequency
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
            this.stoneLevel = 0.6f;
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
            return ConsoleColor.Cyan;
        }
    }
}
