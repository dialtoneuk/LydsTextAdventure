using System;

namespace LydsTextAdventure
{
    public class Command
    {

        private readonly string commandCode;
        protected string commandName;
        protected string commandShortName = "";
        protected ConsoleKey commandKey;
        protected Action commandAction;

        public Command(string name, Action action = null, string shortname = "", ConsoleKey key = ConsoleKey.Tab)
        {

            this.commandCode = Guid.NewGuid().ToString();

            if (name.Length < 2)
                throw new ArgumentException();

            this.commandName = name;

            if (shortname.Length != 0)
                this.commandShortName = shortname;

            if (action != null)
                this.commandAction = action;

            if (this.commandShortName.Length == 0)
                this.SetShortName();

            if (key == ConsoleKey.Tab)
                this.commandKey = this.GetKeyFromShortname();
            else
                this.commandKey = key;
        }

        public ConsoleKey GetKeyFromShortname()
        {

            foreach (ConsoleKey key in Enum.GetValues(typeof(ConsoleKey)))
            {

                if (this.commandShortName[0].ToString() == key.ToString().ToLower())
                    return key;
            }

            throw new SystemException("cannot get key from shortname:" + this.commandShortName[0].ToString());
        }

        //returns true if both of the classes are equal by their GUID
        public bool IsEqual(Command obj)
        {

            if (this.commandCode != obj.commandCode)
                return false;

            return true;
        }

        //executes the function
        public virtual bool Execute()
        {

            if (this.HasAction())
                this.commandAction.Invoke();

            return true;
        }

        public override string ToString()
        {
            return this.commandShortName + " : " + this.commandName;
        }

        //sets the short name of the command
        public void SetShortName(string shortname = "")
        {

            if (shortname.Length == 0)
                this.commandShortName = this.commandName.Substring(0, 2);
            else
                this.commandShortName = shortname;
        }

        //return true if the search term is the command
        public bool SearchTermMatches(string search_term)
        {

            search_term = search_term.ToLower();

            if (search_term.Length == 0)
                return false;

            if (!this.commandName.Equals(search_term))
                if (!this.commandShortName.Equals(search_term))
                    return false;
                else
                    return true;
            else
                return true;
        }

        public bool SearchKeyMatches(ConsoleKey key)
        {

            return this.commandKey.ToString() == key.ToString();
        }

        //returns true if a command is similar
        public bool IsSimilar(Command command)
        {

            if (this.commandName == command.commandName)
                return true;

            if (this.commandShortName == command.commandShortName)
                return true;

            return false;
        }

        //returns true if we have a valid mapped action
        protected bool HasAction()
        {

            return (this.commandAction != null && this.commandAction.GetType() == typeof(Action));
        }
    }
}
