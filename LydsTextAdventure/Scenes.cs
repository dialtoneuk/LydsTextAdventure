using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class Scenes
    {

        /**
         * Contains all of the scenes used by the game
         */

        public static void RegisterScenes()
        {

            SceneManager.AddScene(new SceneGame("sceneGame"));
            SceneManager.AddScene(new SceneMenu("sceneMenu"));
            Program.DebugLog("all scenes registered");
        }
    }
}
