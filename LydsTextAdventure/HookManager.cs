using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class HookManager
    {


        public class HookCluster
        {

            public string Group
            {
                get;
                set;
            }

            public List<Hook> hooks = new List<Hook>();
        }

        private readonly HookCluster[] clusters;


        public enum Groups : int
        {
            Scene,
            World,
            Entity,
            Global,
            Window,
            Default,
            Console,
            Mod,
            Input,
            Game,
            Envirionment,
            Base,
            _
        }

        public HookManager()
        {


            this.clusters = new HookCluster[Enum.GetValues(typeof(Groups)).Length];

            for (int i = 0; i < clusters.Length; i++)
            {
                this.clusters[i] = new HookCluster
                {
                    Group = ((Groups)i).ToString()
                };
            }
        }


        public void RegisterHook(Hook hook)
        {

            foreach (HookCluster cluster in clusters)
                if (cluster.Group == hook.Group)
                {
                    cluster.hooks.Add(hook);
                    Program.DebugLog(String.Format("hook registered n: {0} g: {1} id: {2} callname: {3}", hook.Name, hook.Group, hook.id, hook.CallName));
                    break;
                }
        }

        public void CallHook(string name, Groups group, params object[] vs)
        {

            this.CallHook(name, group.ToString(), vs);
        }

        public void CallHook(string name, string group = "Default", params object[] vs)
        {

            foreach (HookCluster cluster in clusters)
                if (cluster.Group == group)
                {
                    foreach (Hook hook in cluster.hooks)
                    {

                        if (this.GetCallName(name, group) == hook.CallName)
                        {
                            //Program.DebugLog(String.Format("hook called n: {0} g: {1} id: {2} callname: {3}", hook.Name, hook.Group, hook.id, hook.CallName));
                            hook.hookAction.Invoke(vs);
                        }
                    }
                }
        }

        private string GetCallName(string name, string group)
        {

            return (group + "." + name);
        }
    }
}
