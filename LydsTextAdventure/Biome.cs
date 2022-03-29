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
                }},
                {  new Tuple<float, float>(0, 4), new Type[]{
                    typeof(EntityFallenTree),
                }},
                { new Tuple<float, float>(4, 8), new Type[]{
                    typeof(EntityTree),
                }},
                { new Tuple<float, float>(8, WorldChunks.MAX_NUTRIENTS), new Type[]{
                    typeof(EntityAppleTree),
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
        /// Defines the minDistanceBetweenSeed.
        /// </summary>
        public int minDistanceBetweenSeed = 10;

        /// <summary>
        /// Defines the maxDistanceBetweenSeed.
        /// </summary>
        public int maxDistanceBetweenSeed = 20;

        /// <summary>
        /// Defines the waterLevel.
        /// </summary>
        public float waterLevel = 0.1f;

        /// <summary>
        /// Defines the puddleLevel.
        /// </summary>
        public float puddleLevel = -0.38f;

        /// <summary>
        /// Defines the stoneLevel.
        /// </summary>
        public float stoneLevel = 0.31f;

        /// <summary>
        /// Defines the lavaLevel.
        /// </summary>
        public float lavaLevel = 0.15f;

        /// <summary>
        /// Defines the deepWaterLevel.
        /// </summary>
        public float deepWaterLevel = 0.05f;

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
            PUDDLES,
            /// <summary>
            /// Defines the MOUNTAINS.
            /// </summary>
            MOUNTAINS,

            /// <summary>
            /// Defines the LAVA.
            /// </summary>
            LAVA
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

        /// <summary>
        /// Defines the randomSeedNumber.
        /// </summary>
        public int randomSeedNumber = 0;

        /// <summary>
        /// Defines the frequencies.
        /// </summary>
        public float[] frequencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="Biome"/> class.
        /// </summary>
        /// <param name="randomSeedNumber">The randomSeedNumber<see cref="int"/>.</param>
        public Biome(int randomSeedNumber = 0)
        {

            if (randomSeedNumber == 0)
                this.randomSeedNumber = new Random().Next(1, 9999);
        }

        /// <summary>
        /// Defines the noiseTypes.
        /// </summary>
        public FastNoise.NoiseType[] noiseTypes = new FastNoise.NoiseType[]
        {
            FastNoise.NoiseType.Perlin,
            FastNoise.NoiseType.Cellular,
            FastNoise.NoiseType.Perlin,
            FastNoise.NoiseType.ValueFractal
        };

        /// <summary>
        /// The CreateNoiseControllers.
        /// </summary>
        public void CreateNoiseControllers()
        {

            Random rand = new Random();
            noiseControllers = new List<FastNoise>();
            for (int i = 0; i < Enum.GetNames(typeof(NoiseController)).Length; i++)
            {
                FastNoise noise = new FastNoise(randomSeedNumber);
                noise.SetFrequency(frequencies[i]);
                noise.SetFractalOctaves(12);
                noise.SetNoiseType(noiseTypes[i]);
                noiseControllers.Add(noise);
            }
        }

        /// <summary>
        /// The GetNoiseController.
        /// </summary>
        /// <param name="type">The type<see cref="NoiseController"/>.</param>
        /// <returns>The <see cref="FastNoise"/>.</returns>
        public FastNoise GetNoiseController(NoiseController type)
        {

            return noiseControllers[(int)type];
        }

        /// <summary>
        /// The GenerateMagma.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool GenerateLava()
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
        /// The GetDeepWaterColour.
        /// </summary>
        /// <returns>The <see cref="ConsoleColor"/>.</returns>
        public virtual ConsoleColor GetDeepWaterColour()
        {

            return ConsoleColor.DarkBlue;
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
                    lastDistance = biomeRandom.Next(minDistanceBetweenSeed, maxDistanceBetweenSeed);
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
