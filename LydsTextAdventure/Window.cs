using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Window
    {

        public readonly Position position = new Position(0, 0);
        public readonly List<GuiElement> guiElements = new List<GuiElement>();
        public Camera camera = null;

        public string title = "Default Window";
        private string name = "default_window";
        public readonly string id = Guid.NewGuid().ToString();
        public int index = 0;
        protected int height = 0;
        protected int width = 0;
        public bool isVisible = false;
        private bool drawDefault = true;

        public virtual bool IsVisible()
        {

            return this.isVisible;
        }

        public void SetPosition(Position position)
        {

            this.position.SetPosition(position);
        }


        public void SetPosition(int x, int y)
        {

            this.position.x = x;
            this.position.y = y;
        }

        public void SetSize(int w, int h)
        {

            this.width = w;
            this.height = h;
        }

        public void SetTitle(string title)
        {

            this.title = title;
        }

        public void SetName(string name)
        {

            this.name = name;
        }

        public string GetName()
        {

            return this.name;
        }

        public string GetTitle()
        {

            return this.title;
        }

        public void Show()
        {

            this.isVisible = true;
        }

        public void Remove()
        {

            WindowManager.RemoveWindow(this);
        }

        public void Hide()
        {

            this.isVisible = false;
        }

        public Window(string title = "")
        {

            if (title != "")
                this.title = title;

            this.camera = EntityManager.GetMainCamera();
            WindowManager.RegisterWindow(this);
        }

        public override string ToString()
        {

            return this.id + ":" + this.name + "[" + this.index + "]";
        }

        public virtual void Destroy()
        {

            foreach (GuiElement element in this.guiElements)
                element.Destroy();

            this.guiElements.Clear();
        }

        public void SetIndex(int index)
        {

            this.index = index;
        }

        public virtual void Update()
        {

            foreach (GuiElement element in this.guiElements)
            {

                element.Update();

                //is hovering over
                if (GuiElement.IsInsideOf(InputController.GetMousePosition(), element))
                {

                    element.isHovering = true;
                    element.OnHover();
                } else {
                    element.isHovering = false;
                }
            }
        }

       
        public virtual void Initialize()
        {


        }

        public void RegisterElement(GuiElement element)
        {

            Program.DebugLog("element registered: " + element.ToString(), "window");
            element.window = this;
            guiElements.Add(element);
        }

        public void RegisterElements(params GuiElement[] elements)
        {


            for (int i = 0; i < elements.Length; i++)
                this.RegisterElement(elements[i]);
        }
        
        public virtual void ShouldDrawDefault(bool val)
        {

            this.drawDefault = val;
        }

        public void DefaultDraw()
        {


            if (this.drawDefault)
            {

                Surface.DrawBox(this.position.x, this.position.y, this.width, this.height);
                Surface.Write(this.position.x + 2, this.position.y, "[" + this.title + "]");
            }
       
            foreach (GuiElement element in this.guiElements)
                element.Draw(element.GetX(), element.GetY(), this.camera, this);
        }

        public Rectangle GetRectangle()
        {

            return new Rectangle(this.width, this.height);
        }


        public virtual void Draw()
        {


        }
    }
}
