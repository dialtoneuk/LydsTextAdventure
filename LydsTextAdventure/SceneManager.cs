using System;
using System.Collections.Generic;

namespace LydsTextAdventure
{
    class SceneManager
    {

        public static Scene CurrentScene;
        private static bool isReady = false;
        public static bool isReadyToDraw = false;

        protected static List<Scene> Scenes = new List<Scene>();

        public static void StartScene(string scene)
        {

            var r = SceneManager.GetScene(scene);

            if (r == null)
                throw new ApplicationException("scene does not exist: " + scene);

            SceneManager.StartScene(r);
        }

        public static void EndScene()
        {

            if (SceneManager.CurrentScene == null)
                throw new ApplicationException("current scene is aready null");

            SceneManager.CurrentScene.Destroy();
            SceneManager.CurrentScene = null;
        }

        public static void StartScene(Scene scene)
        {


            if (SceneManager.CurrentScene != null)
                throw new ApplicationException("scene not ended");

            SceneManager.isReady = false;
            Program.HookManager.CallHook("PreLoad", HookManager.Groups.Scene, scene);
            Program.DebugLog("loading scene " + scene.sceneName, "scene_manager");

            CommandManager.Clear();
            Console.Clear();
            Buffer.Clear();
            WindowManager.ClearWindows();
            Buffer.CleanBuffer();


#if DEBUG
            Program.RegisterDebugCommands();
            Program.DebugLog("registered debug commands");
#endif

            SceneManager.CurrentScene = scene;
            Program.DebugLog("calling scene load", "scene_manager");
            scene.Load();

            Program.HookManager.CallHook("PreStart", HookManager.Groups.Scene, scene);
            Program.DebugLog("calling scene start", "scene_manager");
            scene.Start();
            Program.HookManager.CallHook("Start", HookManager.Groups.Scene, scene);

            //scene is now ready and starting
            SceneManager.isReady = true;
        }

        public static bool IsSceneActive()
        {

            return (SceneManager.isReady == true);
        }

        public static void UpdateScene()
        {

            isReadyToDraw = false;

            Program.HookManager.CallHook("PreUpdate", HookManager.Groups.Scene);

            if (SceneManager.CurrentScene != null)
                SceneManager.CurrentScene.Update();

            Program.HookManager.CallHook("Update", HookManager.Groups.Scene);

            isReadyToDraw = true;
        }

        public static void ThreadedUpdateScene()
        {

            Program.HookManager.CallHook("PreThreadedUpdate", HookManager.Groups.Scene);

            if (SceneManager.CurrentScene != null)
                SceneManager.CurrentScene.ThreadedUpdate();

            Program.HookManager.CallHook("ThreadedUpdate", HookManager.Groups.Scene);
        }


        public static void DrawScene()
        {

            Program.HookManager.CallHook("PreDraw", HookManager.Groups.Scene);

            if (SceneManager.CurrentScene != null)
                SceneManager.CurrentScene.Draw();

            Program.HookManager.CallHook("Draw", HookManager.Groups.Scene);
        }

        public static void AddScene(Scene scene)
        {

            if (SceneManager.GetScene(scene.sceneName) != null)
                throw new ApplicationException();

            Scenes.Add(scene);
            Program.DebugLog("registed scene: " + scene.ToString());
        }

        public static void AddScenes(List<Scene> collection)
        {

            foreach (Scene _scene in collection)
                if (SceneManager.GetScene(_scene.sceneName) != null)
                    throw new ApplicationException();

            List<Scene> _collection = new List<Scene>(collection.Count + SceneManager.Scenes.Count);
            _collection.AddRange(collection);
            _collection.AddRange(SceneManager.Scenes);
            SceneManager.Scenes = _collection;
        }

        public static Scene GetScene(string name)
        {

            foreach (Scene scene in Scenes)
                if (scene.sceneName == name)
                    return scene;

            return null;
        }
    }
}
