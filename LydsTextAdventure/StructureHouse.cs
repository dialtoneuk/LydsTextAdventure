using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class StructureHouse : Structure
    {

        public int houseSize = 10;
        public Tile baseTile;

        protected OscMovement movement;

        public StructureHouse()
        {
            this.Name = "House";
            this.houseSize = new Random().Next(12, 24);
            this.Width = this.houseSize;
            this.Height = this.houseSize;

            this.baseTile = new TileConcrete();
            this.baseTile.texture.color = (ConsoleColor)new Random().Next(1, 10);

            this.CreateTiles();
        }

        public override void LoadStructure()
        {

            //door position
            int orientation = new Random().Next(0, 4);
            int doorSize = 2 + new Random().Next(1, this.houseSize / 2);
            for (int x = 0; x < houseSize; x++)
            {
                for (int y = 0; y < houseSize; y++)
                {

                    switch (orientation)
                    {
                        case 0: //left size
                            if (x == 0 && y > 1 && y < doorSize)
                            {
                                this.tiles[x, y] = new TileWoodFloor();
                                continue;
                            }

                            break;
                        case 1: //right size
                            if (x == houseSize - 1 && y > 1 && y < doorSize)
                            {
                                this.tiles[x, y] = new TileWoodFloor();
                                continue;
                            }

                            break;
                        case 2: //bottom size
                            if (y == 0 && x > 1 && x < doorSize)
                            {
                                this.tiles[x, y] = new TileWoodFloor();
                                continue;
                            }

                            break;
                        default:
                        case 3: //top size
                            if (y == houseSize - 1 && x > 1 && x < doorSize)
                            {
                                this.tiles[x, y] = new TileWoodFloor();
                                continue;
                            }

                            break;
                    }

                    if (x == 0 || y == 0 || x == houseSize - 1 || y == houseSize - 1)
                        this.tiles[x, y] = this.baseTile;
                    else
                        this.tiles[x, y] = new TileWoodFloor();
                }
            }
        }
    }
}
