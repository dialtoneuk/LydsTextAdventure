using System.Collections.Generic;

namespace LydsTextAdventure
{
    public class MovementManager
    {

        public static void MoveEntity(Entity entity, int x, int y, World world = null)
        {

            MovementManager.MoveEntity(entity, new Position(x, y), world);
        }

        public static void MoveEntity(Entity entity, Position position, World world = null)
        {

            if (!MovementManager.CanMove(entity, position, world))
                return;

            entity.position.SetPosition(position);
        }

        public static bool CanMove(Entity entity, Position position, World world = null)
        {

            if (!entity.IsSolid())
                return true;

            if (world == null && entity.world == null)
                world = WorldManager.CurrentWorld;
            else
                world = entity.world;

            Tile tile = world.GetTile(position.x, position.y);
            List<Entity> entities = EntityManager.GetEntitiesAroundPosition(position);

            foreach (Entity ent in entities)
            {

                //solid entity in the way
                //TODO: needs to factor in the entities height and width
                if (ent.position.x == position.x && entity.position.y == position.y)
                    return false;
            }

            if (tile == null)
                return true;

            return !tile.isSolid;
        }
    }
}
