﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class World
    {

  
        public Tile[,] world;
        public readonly int width;
        public readonly int height;

        private readonly FastNoise noise = new FastNoise();

        public World(int width = 1024, int height = 2014)
        {

            this.world = new Tile[width, height];

            this.width = width;
            this.height = height;
           
        }

        //returns true if a chunk exists

        public bool HasChunkAtPosition()
        {


            return true;
        }

        public char[,] Draw( int startx, int starty, int width, int height)
        {

            char[,] result = new char[width, height];

            int actualx = 0;
            int actualy = 0;

            for(int x = 0; x < width; x++)
            {
                actualx = x + startx;

                for (int y = 0; y < height; y++)
                {
                    actualy = y + starty;

                    if(actualy < 0 || actualx < 0 || actualy > this.height || actualx > this.width){
                        result[x, y] = ' ';
                    } 
                    else
                    {
                        result[x, y] = this.world[actualx, actualy].texture.character;
                    }
                }
            }
           
            return result;
        }

        public void Generate()
        {

            Texture water = new Texture('~', ConsoleColor.Blue);
            Texture ground = new Texture(',', ConsoleColor.Gray);

            for(int x = 0; x < this.width; x++ )
            {

                for(int y = 0; y < this.height; y++)
                {


                    float noiseValue = this.noise.GetNoise(x, y);
                    Tile tile;

                    if (noiseValue < 0.1)
                        tile = new Tile(water);
                    else
                        tile = new Tile(ground);

                    this.world[x, y] = tile;
                }
            }

        }
    }
}
