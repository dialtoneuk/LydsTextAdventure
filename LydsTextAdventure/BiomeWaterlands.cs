using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeWaterlands : Biome
    {


        public BiomeWaterlands(int seed) : base(seed)
        {
            this.frequencies = new float[]
            {
                    0.075f, //lakeFrequency
                    0.240f, //puddleFrequency    
                    0.1f, //mountainFrequency    
                    0.035f, //spareFrequency    
            };

            this.nutrientRate = 3;
            this.minDistanceBetweenSeed = 10;
            this.maxDistanceBetweenSeed = 30;
            this.waterLevel = 0.05f;
            this.deepWaterLevel = 0.1f;
            this.puddleLevel = 0.025f;
            this.stoneLevel = 0.5f;

            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityBush),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityTree),
                    typeof(EntityPineTree),
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
            return ConsoleColor.Yellow;
        }
    }
}
