using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class SceneManager
    {

        protected static List<Scene> scenes = new List<Scene>()
        {
            new SceneMenu("menuScene")
        };

        public static void AddScene(Scene scene)
        {

            if (SceneManager.GetScene(scene.sceneName) != null )
                throw new ApplicationException();

            scenes.Add(scene);
        }

        public static void AddScenes(List<Scene> collection)
        {

            foreach(Scene _scene in collection)
                if(SceneManager.GetScene( _scene.sceneName ) != null )
                    throw new ApplicationException();

            List<Scene> _collection = new List<Scene>(collection.Count + SceneManager.scenes.Count);
            _collection.AddRange(collection);
            _collection.AddRange(SceneManager.scenes);
            SceneManager.scenes = _collection;
        }

        public static Scene GetScene(string name)
        {

            foreach (Scene scene in scenes)
                if (scene.sceneName == name)
                    return scene;

            return null;
        }
    }
}
