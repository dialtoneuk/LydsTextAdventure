using System;

namespace LydsTextAdventure
{
    public class GuiButton : GuiElement
    {

        protected string text = "Button";
        protected Action<GuiButton> onHover;
        protected Action<GuiButton> onClick;

        public void SetOnHover(Action<GuiButton> onHover)
        {

            this.onHover = onHover;
        }

        public void SetOnClick(Action<GuiButton> onClick)
        {

            this.onClick = onClick;
        }

        public void SetText(string text)
        {

            this.text = text;
        }

        public override void OnHover()
        {

            this.onHover?.Invoke(this);
            base.OnHover();
        }

        public override void OnClick()
        {

            this.onClick?.Invoke(this);
            base.OnClick();
        }

        public override void Draw(int x, int y, Camera camera = null, Window window = null)
        {

            Surface.DrawBox(x, y, this.Width, this.Height, window.GetRectangle(), Buffer.Types.GUI_BUFFER, false);

            if (isHovering)
                Surface.DrawText(x + this.Width / 4, y + this.Height / 2, "[" + this.text + "]", window.GetRectangle());
            else
                Surface.DrawText(x + this.Width / 4, y + this.Height / 2, this.text, window.GetRectangle());
        }
    }
}
