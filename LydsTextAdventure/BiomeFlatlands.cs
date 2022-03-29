using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeFlatlands : Biome
    {

        public BiomeFlatlands(int seed) : base(seed)
        {

            this.frequencies = new float[]
            {
                0.055f, //lakeFrequency
                0.075f, //puddleFrequency    
                0.045f, //mountainFrequency    
                0.035f, //spareFrequency    
            };

            this.nutrientRate = 4;
            this.minDistanceBetweenSeed = 30;
            this.maxDistanceBetweenSeed = 60;
            this.waterLevel = 0.275f;
            this.deepWaterLevel = 0.425f;
            this.puddleLevel = 0.35f;
            this.stoneLevel = 0.35f;

            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                     typeof(EntityOakTree),
                    typeof(EntityPineTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityMushroom),
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
