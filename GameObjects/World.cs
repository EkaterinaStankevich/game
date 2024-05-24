using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class World : GameObject
    {
        public Camera Camera { get; internal set; }

        public World(GameContext context, Rect frame) : base(context, frame)
        {
            Camera = new Camera(context, frame);
        }

        public void Render(Graphics renderer)
        {
            base.Render(renderer, Frame.Center, Camera.GlobalPosition(), Camera.Frame.Size);
        }

        public override void KeyDown(Keys key)
        {
            base.KeyDown(key);
            if (key == Keys.Q)
            {
                Context.Quit();
            }
        }
    }
}
