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

            if (world == null && entity.World == null)
                world = WorldManager.CurrentWorld;
            else
                world = entity.World;

            Tile tile = world.GetTile(position.x, position.y);

            if (tile == null)
                return true;

            if (tile.GetType() == typeof(TileWorldBorder))
                return false;

            if (!entity.isSolid)
                return true;

            List<Entity> entities = EntityManager.GetVisibleEntitiesAroundPosition(position);

            foreach (Entity ent in entities)
            {

                //solid entity in the way
                //TODO: needs to factor in the entities height and width
                if (ent.position.x == position.x && entity.position.y == position.y)
                    return false;
            }

            return tile.isSolid == false;
        }
    }
}
