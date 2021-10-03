using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class GuiElement
    {

        public GuiElement parent;
        public Window window;
        public readonly Position position = new Position(0, 0);

        private int width;
        private int height;

        private string name;

        public bool isVisible = true;
        public bool isDisbled = false;
        public bool isAwaitingOutput = false;

        public int Width { get => width; }
        public int Height { get => height; }
        public string Name { get => name; }

        public GuiElement(Window window = null, GuiElement parent = null, Position position = null)
        {

            if (position != null)
                position.SetPosition(position);

            this.parent = parent;
            this.window = window;

            if(window!=null)
                window.RegisterElement(this);
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }

        public virtual void Update()
        {


        }

        public void SetName(string name)
        {

            this.name = name;
        }

        public void SetSize(int width, int height)
        {

            this.width = width;
            this.height = height;
        }

        public virtual void Draw(int x, int y, Camera camera=null)
        {


        }

        public void SetPosition(int x, int y)
        {

            this.position.x = x;
            this.position.y = y;
        }

        public int GetLeft()
        {

            int x = this.position.x;

            if (this.parent != null)
                x = this.parent.position.x + x;

            return x;
        }

        public int GetTop()
        {

            int y = this.position.y;

            if (this.parent != null)
                y = this.parent.position.y + y;

            return y;
        }

        public int GetX()
        {

            if (this.window == null)
                return this.GetLeft();

            return this.window.position.x + this.GetLeft();
        }

        public int GetY()
        {

            if (this.window == null)
                return this.GetTop();

            return this.window.position.y + this.GetTop();
        }

        public Position GetPosition()
        {

            if (this.parent == null)
                return new Position(this.position.x, this.position.y);


            if (this.window == null)
                return new Position(this.GetLeft(), this.GetTop());

            return new Position( this.GetX(), this.GetY());
        }
    }
}
