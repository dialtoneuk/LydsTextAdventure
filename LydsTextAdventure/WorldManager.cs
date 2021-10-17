using System.Collections.Generic;
using System.Linq;

namespace LydsTextAdventure
{
    public class WorldManager
    {

        private static readonly List<World> Worlds = new List<World>();

        public static World CurrentWorld
        {
            get;
            set;
        }

        public static void RegisterWorld(World world)
        {

            Program.DebugLog("world registered: " + world.id, "world_manager");
            WorldManager.Worlds.Add(world);

            if (WorldManager.CurrentWorld == null)
                WorldManager.CurrentWorld = world;
        }

        public static void ClearWorlds()
        {

            WorldManager.Worlds.Clear();
            WorldManager.CurrentWorld = null;
            Program.DebugLog("worlds destroyed", "world_manager");
        }

        public static void DeleteWorld(string id)
        {

            foreach (World world in WorldManager.Worlds)
            {

                if (world.id != id)
                    continue;

                WorldManager.Worlds.Remove(world);
                break;
            }
        }

        public static void SetCurrentWorld(string id)
        {

            foreach (World world in WorldManager.Worlds)
            {

                if (world.id != id)
                    continue;

                WorldManager.CurrentWorld = world;
                break;
            }
        }

        public static void ForceCurrentWorld()
        {

            WorldManager.CurrentWorld = (from l in WorldManager.Worlds
                                         select l).FirstOrDefault();
        }

        public static void UpdateWorlds()
        {

            foreach (World world in WorldManager.Worlds)
            {

                if (world.isWaiting || world.IsDisabled())
                    continue;

                world.Update();
            }
        }
    }
}
