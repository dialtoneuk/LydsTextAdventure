using System.Collections.Generic;

namespace LydsTextAdventure
{
    public class WindowManager
    {

        private static readonly List<Window> Windows = new List<Window>();
        private static int Count = 0;

        public static void RegisterWindow(Window window)
        {

            window.SetIndex(WindowManager.Count++);
            Program.DebugLog("initializing window: " + window.ToString(), "window_manager");
            window.Initialize();
            Program.DebugLog("window initialized: " + window.ToString(), "window_manager");
            WindowManager.Windows.Add(window);
            Program.DebugLog("window registered: " + window.ToString(), "window_manager");
        }

        public static void RemoveWindow(string id)
        {

            foreach (Window window in WindowManager.Windows)
            {

                if (window.id == id)
                {

                    WindowManager.Windows.Remove(window);
                    break;
                }
            }
        }

        public static void RemoveWindow(Window window)
        {

            WindowManager.Windows.Remove(window);
        }

        public static Window GetWindowByName(string name)
        {

            foreach (Window window in WindowManager.Windows)
            {

                if (window.GetName().ToLower() == name.ToLower())
                    return window;
            }

            return null;
        }

        public static void DrawWindows()
        {

            foreach (Window window in WindowManager.Windows)
            {

                if (!window.IsVisible())
                    continue;

                window.DefaultDraw();
                window.Draw();
            }
        }

        public static List<Window> GetOpenWindows()
        {

            List<Window> windows = new List<Window>();

            foreach (Window window in WindowManager.Windows)
                if (window.isVisible)
                    windows.Add(window);

            return windows;
        }

        public static void ClearWindows()
        {

            foreach (Window window in WindowManager.Windows)
            {

                window.Destroy();
            }

            WindowManager.Windows.Clear();
        }

        public static void UpdateWindows()
        {

            foreach (Window window in WindowManager.Windows)
            {

                window.Update();
            }
        }
    }
}
