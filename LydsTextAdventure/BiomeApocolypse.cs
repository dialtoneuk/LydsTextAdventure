﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{

    public class BiomeApocolypse : Biome
    {


        public BiomeApocolypse()
        {

            this.fractalGain = 15f;
            this.STONE_LEVEL = 0.25f;
            this.LAVA_LEVEL = 0.05f;
            this.biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float,float>(-12, 0), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
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
                }},
                {new Tuple<float,float>(4,8), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {new Tuple<float,float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }}
            };
        }

        public override bool GenerateMagma()
        {

            return true;
        }

        public override ConsoleColor GetWaterColour()
        {
            return ConsoleColor.DarkCyan;
        }

        public override ConsoleColor GetGrassColour()
        {
            return ConsoleColor.DarkYellow;
        }
    }
}
