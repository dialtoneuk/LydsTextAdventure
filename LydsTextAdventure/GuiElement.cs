using System;

namespace LydsTextAdventure
{
    public class GuiElement
    {

        public GuiElement parent;
        public Window window;
        public readonly Position position = new Position(0, 0);

        private int[] dockPadding = new int[]
        {
            1,
            1,
            1,
            1
        };

        private int width = 1;
        private int height = 1;

        public enum Dock
        {

            NO_DOCK,
            LEFT,
            RIGHT,
            TOP,
            BOTTOM,
            FILL
        }

        private string name;

        public bool isVisible = true;
        public bool isDisbled = false;
        public bool isHovering = false;
        public bool isAwaitingOutput = false;

        public Dock DockType
        {
            get;
            set;
        }

        public GuiGroup Group
        {

            get;
            set;
        }

        public int Width
        {
            get => this.width;
        }
        public int Height
        {
            get => this.height;
        }
        public string Name
        {
            get => this.name;
        }

        public GuiElement(Window window = null, GuiElement parent = null, Position position = null)
        {

            this.DockType = Dock.NO_DOCK;

            if (position != null)
                position.SetPosition(position);

            this.parent = parent;
            this.window = window;

            if (window != null)
                window.RegisterElement(this);
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }

        public void DockInsideRectangle(Rectangle container)
        {

            switch (this.DockType)
            {

                case Dock.FILL:
                    this.width = container.Width - (dockPadding[0] + dockPadding[1]);
                    this.height = container.Height - (dockPadding[1] + dockPadding[2]);
                    break;
                case Dock.LEFT:
                    this.width = (container.Width / 2) - (dockPadding[2] + dockPadding[3]);
                    this.height = container.Height - (dockPadding[1] + dockPadding[2]);
                    break;
                case Dock.RIGHT:
                    this.width = (container.Width / 2) - (dockPadding[2]);
                    this.position.x = (container.Width / 2) - dockPadding[2];
                    this.height = container.Height - (dockPadding[1] + dockPadding[2]);
                    break;
                case Dock.BOTTOM:
                    this.width = (container.Width) - (dockPadding[2] + dockPadding[3]);
                    this.position.y = (container.Height / 2) - (dockPadding[0]);
                    this.height = (container.Height / 2) - (dockPadding[1]);
                    break;
                case Dock.TOP:
                    this.width = (container.Width) - (dockPadding[2] + dockPadding[3]);
                    this.height = (container.Height / 2) - (dockPadding[0] + dockPadding[1]);
                    break;
            }

        }

        public void SetDockPadding(int top, int down = 1, int left = 1, int right = 1)
        {

            dockPadding[0] = top;
            dockPadding[1] = down;
            dockPadding[2] = left;
            dockPadding[3] = right;
        }

        public virtual void Update()
        {



        }

        public static bool IsInsideOf(Position position, GuiElement element)
        {

            if (position.x > element.GetX() && position.x < element.GetX() + Math.Max(2, element.width))
                if (position.y < element.GetY() + Math.Max(2, element.height) && position.y > element.GetY())
                    return true;

            return false;
        }

        public virtual void Destroy()
        {


        }

        public void DefaultUpdate()
        {

            if (!this.isVisible)
                return;


        }

        public virtual void OnHover()
        {

        }

        public virtual void OnClick()
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

        public virtual void Draw(int x, int y, Camera camera = null, Window window = null)
        {

        }

        public Rectangle GetRectangle()
        {

            return new Rectangle(this.width, this.height, this.position.x, this.position.y);
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

            return new Position(this.GetX(), this.GetY());
        }
    }
}
