﻿using System;
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
            window.Initialize();
            Program.DebugLog("window initialized: " + window.ToString());
            WindowManager.windows.Add(window);
            Program.DebugLog("window registered: " + window.ToString());
        }

        public static void RemoveWindow(string id)
        {

            foreach (Window window in WindowManager.windows)
            {

                if(window.id == id)
                {

                    WindowManager.windows.Remove(window);
                    break;
                }
            }
        }

        public static void RemoveWindow(Window window)
        {

            WindowManager.windows.Remove(window);
        }

        public static Window GetWindowByName(string name)
        {

            foreach (Window window in WindowManager.windows)
            {

                if (window.GetName().ToLower() == name.ToLower())
                    return window;
            }

            return null;
        }

        public static void DrawWindows()
        {

            foreach(Window window in WindowManager.windows)
            {

                if (!window.IsVisible())
                    continue;

                window.DefaultDraw();
                window.Draw();
            }
        }

        public static void UpdateWindows()
        {

            foreach (Window window in WindowManager.windows)
            {

                window.Update();
            }
        }
    }
}