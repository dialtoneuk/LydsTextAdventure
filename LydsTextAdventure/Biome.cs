namespace LydsTextAdventure
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Biome" />.
    /// </summary>
    public class Biome
    {
        /// <summary>
        /// Defines the biomeFoliage.
        /// </summary>
        public Dictionary<Tuple<float, float>, Type[]> biomeFoliage = new Dictionary<Tuple<float, float>, Type[]>
            {
                {new Tuple<float, float>(-12, 0), new Type[]{
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityDeadTree),
                    typeof(EntityFallenTree),
                    typeof(EntityBush),
                    typeof(EntityBush),
                }},
                {  new Tuple<float, float>(0, 4), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityBush)
                }},
                { new Tuple<float, float>(4, 8), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityAppleTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree)
                }},
                { new Tuple<float, float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityTree),
                    typeof(EntityOakTree),
                    typeof(EntityAppleTree),
                    typeof(EntityOakTree),
                    typeof(EntityBush),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                    typeof(EntityPineTree),
                }}
            };

        /// <summary>
        /// Defines the noiseControllers.
        /// </summary>
        public List<FastNoise> noiseControllers;

        /// <summary>
        /// Defines the biomeRandom.
        /// </summary>
        public Random biomeRandom = new Random();

        /// <summary>
        /// Defines the SEED_MAX_DISTANCE.
        /// </summary>
        public const int SEED_MAX_DISTANCE = 60;

        /// <summary>
        /// Defines the SEED_MIN_DISTANCE.
        /// </summary>
        public const int SEED_MIN_DISTANCE = 4;

        /// <summary>
        /// Defines the fractalGain.
        /// </summary>
        public float fractalGain = 1.0f;

        /// <summary>
        /// Defines the WATER_LEVEL.
        /// </summary>
        public float WATER_LEVEL = 0.05f;

        /// <summary>
        /// Defines the STONE_LEVEL.
        /// </summary>
        public float STONE_LEVEL = 0.31f;

        /// <summary>
        /// Defines the LAVA_LEVEL.
        /// </summary>
        public float LAVA_LEVEL = 0.15f;

        /// <summary>
        /// Defines the DEEP_WATER_LEVEL.
        /// </summary>
        public float DEEP_WATER_LEVEL = 0.0005f;

        /// <summary>
        /// Defines the NoiseController.
        /// </summary>
        public enum NoiseController
        {
            /// <summary>
            /// Defines the LAKES.
            /// </summary>
            LAKES,

            /// <summary>
            /// Defines the RIVERS.
            /// </summary>
            RIVERS,

            /// <summary>
            /// Defines the MOUNTAINS.
            /// </summary>
            MOUNTAINS
        }

        /// <summary>
        /// Defines the nutrientRate.
        /// </summary>
        public int nutrientRate = 0;

        /// <summary>
        /// Defines the oreRate.
        /// </summary>
        public int oreRate = 0;

        /// <summary>
        /// Defines the dangerRate.
        /// </summary>
        public int dangerRate = 0;

        /// <summary>
        /// Defines the lastDistance.
        /// </summary>
        public int lastDistance = 0;

        public float[] frequencies = new float[]
        {
            0.015f, //lakeFrequency
            0.015f, //riverFrequency    
            0.015f, //mountainFrequency    
            0.015f, //spareFrequency    
        };

        public FastNoise.NoiseType[] noiseTypes = new FastNoise.NoiseType[]
        {
            FastNoise.NoiseType.Perlin,
            FastNoise.NoiseType.Perlin,
            FastNoise.NoiseType.Perlin,
            FastNoise.NoiseType.Perlin
        };


        /// <summary>
        /// The CreateNoiseControllers.
        /// </summary>
        public void CreateNoiseControllers()
        {

            Random rand = new Random();
            this.noiseControllers = new List<FastNoise>();
            for (int i = 0; i < Enum.GetNames(typeof(NoiseController)).Length; i++)
            {
                FastNoise noise = new FastNoise(rand.Next(1337, 1337 * 1337));
                noise.SetFrequency(frequencies[i]);
                noise.SetNoiseType(noiseTypes[i]);
                this.noiseControllers.Add(noise);
            }
        }

        public FastNoise GetNoiseController(NoiseController type)
        {

            return this.noiseControllers[(int)type];
        }

        /// <summary>
        /// The GenerateMagma.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool GenerateMagma()
        {

            return true;
        }

        /// <summary>
        /// The GetGrassColour.
        /// </summary>
        /// <returns>The <see cref="ConsoleColor"/>.</returns>
        public virtual ConsoleColor GetGrassColour()
        {

            return ConsoleColor.Green;
        }

        /// <summary>
        /// The GetWaterColour.
        /// </summary>
        /// <returns>The <see cref="ConsoleColor"/>.</returns>
        public virtual ConsoleColor GetWaterColour()
        {

            return ConsoleColor.Blue;
        }

        /// <summary>
        /// The CanSeed.
        /// </summary>
        /// <param name="nutrientRate">The nutrientRate<see cref="int"/>.</param>
        /// <param name="chanceModifier">The chanceModifier<see cref="int"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CanSeed(int nutrientRate, int chanceModifier = 10)
        {

            bool canSeed = false;

            //if last distance is greater than or equal to zero or we hit probability
            if (lastDistance <= 0)
            {

                //more of a chance
                int value = biomeRandom.Next(0, 100 + nutrientRate);

                //if we hit prob then can seed and reset distance
                if (value > 100 - chanceModifier)
                {
                    canSeed = true;
                    lastDistance = biomeRandom.Next(SEED_MIN_DISTANCE, SEED_MAX_DISTANCE);
                }
            }

            lastDistance--;
            return canSeed;
        }

        /// <summary>
        /// The GetFoliageTypes.
        /// </summary>
        /// <param name="nutrientLevel">The nutrientLevel<see cref="int"/>.</param>
        /// <returns>The <see cref="Type[]"/>.</returns>
        public Type[] GetFoliageTypes(int nutrientLevel)
        {

            List<Type> types = new List<Type>();
            foreach (KeyValuePair<Tuple<float, float>, Type[]> pair in biomeFoliage)
            {

                if (nutrientLevel > pair.Key.Item1 && nutrientLevel <= pair.Key.Item2)
                    types.AddRange(pair.Value);
            }

            return types.ToArray();
        }
    }
}
