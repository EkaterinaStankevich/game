using System.Drawing;

namespace WindowsFormsPlatformer.GameObjects.UI
{
    class UI : Element
    {
        public UI(GameContext context, Rect frame) : base(context, frame) {}

        public Element HealthBarHolder { get; set; }
        public Bar HealthBar { get; set; }
        public Element PowerBarHolder { get; set; }
        public Bar PowerBar { get; set; }
        public Text DeathText { get; set; }
        public Text WinText { get; set; }

        public void Render(Graphics renderer)
        {
            base.Render(renderer, Frame.Center, Vector.Zero, Context.World.Camera.OriginalSize);
        }
    }
}
