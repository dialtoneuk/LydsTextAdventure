using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeWaterlands : Biome
    {


        public BiomeWaterlands()
        {

            this.fractalGain = 0.25f;
            this.WATER_LEVEL = 0.01f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(0, 4), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityPsychedelicMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityMushroom),
                    typeof(EntityBush),
                    typeof(EntityBush),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
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
            return ConsoleColor.DarkGray;
        }
    }
}
