using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class WindowManager
    {

        private static List<Window> windows = new List<Window>();
        private static int count = 0;

        public static void RegisterWindow(Window window)
        {

            window.SetIndex(WindowManager.count++);
            WindowManager.windows.Add(window);
            Program.DebugLog("window registered: " + window.ToString());
        }
    }
}
