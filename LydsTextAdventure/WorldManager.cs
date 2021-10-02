using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LydsTextAdventure
{
    public class WorldManager
    {

        private static World currentWorld;
        private static List<World> worlds = new List<World>();

        public static World CurrentWorld { get => WorldManager.currentWorld;}

        public static void RegisterWorld(World world)
        {

            Program.DebugLog("world registered: " + world.id, "world_manager");
            WorldManager.worlds.Add(world);

            if (WorldManager.currentWorld == null)
                WorldManager.currentWorld = world;
        }

        public static void ClearWorlds()
        {

            WorldManager.worlds.Clear();
            WorldManager.currentWorld = null;
            Program.DebugLog("worlds destroyed", "world_manager");
        }

        public static void DeleteWorld(string id)
        {

            foreach (World world in WorldManager.worlds)
            {

                if (world.id != id)
                    continue;

                WorldManager.worlds.Remove(world);
                break;
            }
        }

        public static void SetCurrentWorld(string id)
        {

            foreach (World world in WorldManager.worlds)
            {

                if (world.id != id)
                    continue;

                WorldManager.currentWorld = world;
                break;
            }
        }

        public static void ForceCurrentWorld()
        {

            WorldManager.currentWorld = (from l in WorldManager.worlds
                                         select l).FirstOrDefault();
        }

        public static void UpdateWorlds()
        {

            foreach(World world in WorldManager.worlds)
            {

                if (world.isWaiting || world.IsDisabled() )
                    continue;

                world.Update();
            }
        }
    }
}
