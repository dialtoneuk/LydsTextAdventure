namespace LydsTextAdventure
{
    public class EntityMovingText : EntityMoving
    {

        protected string text;
        protected bool writeOver = false;

        public EntityMovingText()
        {

            this.SetSpeed(5);
            this.SetDistance(32);
        }

        public void SetWriteOver(bool writeOver)
        {

            this.writeOver = writeOver;
        }

        public override bool ShouldDrawTexture()
        {

            return false;
        }

        public override void Draw(int x, int y, Camera camera)
        {

            Surface.DrawText(x, y, this.text, camera.GetViewRectangle());

            base.Draw(x, y, camera);
        }

        public void SetText(string str)
        {

            this.text = str;
        }
    }
}
