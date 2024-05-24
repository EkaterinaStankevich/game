using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class Camera : GameObject
    {
        public Size OriginalSize { get; internal set; }

        public Camera(GameContext context, Rect frame) : base(context, frame)
        {
            OriginalSize = new Size(frame.Size.Width / 2, frame.Size.Height / 2);
        }

        public override void HandleKeyboardState(IList<Keys> keys)
        {
            if (keys.Contains(Keys.Z))
            {
                Frame.Size = new Size(OriginalSize.Width * 2, OriginalSize.Height * 2);
            }
            else
            {
                Frame.Size = OriginalSize;
            }
        }
    }
}