﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    class SceneManager
    {

        private static Scene currentScene;

        protected static List<Scene> scenes = new List<Scene>()
        {
            new SceneMenu("menuScene")
        };

        public static void StartScene(string scene)
        {

            var r = SceneManager.GetScene( scene );

            if (r == null)
                throw new ApplicationException("scene does not exist");

            SceneManager.StartScene( r );
        }

        public static void StartScene(Scene scene)
        {

            Program.DebugLog("loading scene " + scene.sceneName, "scene_manager");

            if (SceneManager.currentScene != null)
                throw new ApplicationException("scene not ended");

            Program.DebugLog("calling scene load", "scene_manager");
            scene.Load();

            Program.DebugLog("calling scene start", "scene_manager");
            scene.Start();

            SceneManager.currentScene = scene;
        }

        public static bool IsSceneActive()
        {

            return (SceneManager.currentScene != null);
        }

        public static void UpdateScene()
        {

            if (SceneManager.currentScene is null)
                throw new ApplicationException("scene not started");

            SceneManager.currentScene.Update();
            SceneManager.currentScene.Draw();
        }

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
