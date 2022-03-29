using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{
    public class MovementManager
    {

        public const int BASE_MOVE_COST = 35;
        public const int COOLDOWN = 10; //10 ticks

        private static int canMove;

        public static void MoveEntity(Entity entity, int x, int y, World world = null)
        {

            MovementManager.MoveEntity(entity, new Position(x, y), world);
        }

        public static void MoveEntity(Entity entity, Position position, World world = null)
        {



            if (!MovementManager.CanMove(entity, position, world))
                return;

            //movement cooldown
            if (canMove != 0 && Program.GetTick() < canMove)
                return;


            if (entity.GetType() == typeof(Player))
                ((Player)entity).stanima -= BASE_MOVE_COST;

            entity.position.SetPosition(position);
            canMove = Program.GetTick() + COOLDOWN;
        }

        public static bool CanMove(Entity entity, Position position, World world = null)
        {

            if (world == null && entity.World == null)
                world = WorldManager.CurrentWorld;
            else
                world = entity.World;


            if (!entity.isSolid)
                return true;

            if (SceneManager.CurrentScene.player.stanima - BASE_MOVE_COST <= 0)
                return false;

            List<Entity> entities = EntityManager.GetVisibleEntitiesAroundPosition(position);

            foreach (Entity ent in entities)
            {

                if (ent.GetType().IsSubclassOf(typeof(Structure)))
                {

                    int actualX = ent.position.x - position.x;
                    int actualY = ent.position.y - position.y;



                    Structure structure = (Structure)ent;
                    try
                    {
                        return !structure.GetTiles()[Math.Abs(actualX), Math.Abs(actualY)].isSolid;
                    }
                    catch
                    {
                        //poke the tile instead
                    }
                }

                //solid entity in the way
                //TODO: needs to factor in the entities height and width
                if (ent.position.x == position.x && entity.position.y == position.y)
                    return false;
            }

            Tile tile = world.GetTile(position.x, position.y);

            if (tile == null)
                return true;

            if (tile.GetType() == typeof(TileWorldBorder))
                return false;


            return tile.isSolid == false;
        }
    }
}
