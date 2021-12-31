using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Hook
    {

        public string Name
        {
            get;
            protected set;
        }

        public Type OriginalType
        {
            get;
            protected set;
        }

        public string CallName
        {
            get;
            protected set;
        }


        public readonly string id = Guid.NewGuid().ToString();
        public Action<object[]> hookAction;

        public string Group
        {
            get;
            protected set;
        }

        public Hook(string name, HookManager.Groups group, Action<object[]> action) : this(name, group.ToString())
        {

            this.hookAction = action;
            Program.HookManager.RegisterHook(this);
        }

        public Hook(string name, string group)
        {

            this.OriginalType = this.GetType();
            this.Name = name;
            this.Group = group;
            this.CallName = this.Group + "." + this.Name;
        }

        public virtual void SetAction(Action<object[]> action)
        {
            this.hookAction = action;
        }
    }
}
