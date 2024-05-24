using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsPlatformer
{
    class Animation
    {
        private const int mc_speedScale = 1000;

        public IList<RenderObject> Frames { get; internal set; } = new List<RenderObject>();
        public long StartTick { get; internal set; }
        public int Speed { get; internal set; }

        private bool m_turnedLeft = false;
        public bool TurnedLeft
        {
            get { return m_turnedLeft; }
            set
            {
                m_turnedLeft = value;

                foreach (var frame in Frames)
                    frame.RenderFlip = m_turnedLeft;
            }
        }

        public Animation(int speed, long startTick) : this(new List<RenderObject>(), speed, startTick)
        {
        }

        public Animation(IList<RenderObject> frames, int speed, long startTick)
        {
            Frames = frames;
            Speed = speed;
            StartTick = startTick;
        }

        public RenderObject Animate(long ticks)
        {
            if (ticks - StartTick >= Frames.Count * Speed * mc_speedScale)
                StartTick = ticks;
            int frameIndex = (int)(ticks - StartTick) / (Speed * mc_speedScale);
            return Frames[frameIndex];
        }

        public static Animation WithSingleRenderObject(long ticks, RenderObject renderObject)
        {
            return new Animation(new List<RenderObject> { renderObject }, 1, ticks);
        }

        public static Animation WithSpeedAndColor(long ticks, int speed, Color color)
        {
            var animation = new Animation(speed, ticks);
            animation.Frames.Add(RenderObject.FromColor(color));
            animation.Frames.Add(RenderObject.FromColor(Color.Black));
            return animation;
        }

        public static Animation WithSpeedAndImage(long ticks, int speed, Bitmap image, int width, int height, int frames)
        {
            var animation = new Animation(speed, ticks);
            Rectangle rect = new Rectangle();
            rect.Width = width;
            rect.Height = height;
            rect.X = 0;
            rect.Y = 0;
            for (int i = 0; i < frames; i++)
            {
                Bitmap frameImage = new Bitmap(rect.Width, rect.Height);
                using (var gfx = Graphics.FromImage(frameImage))
                {
                    gfx.DrawImage(image, new Rectangle(0, 0, width, height), rect, GraphicsUnit.Pixel);
                }
                animation.Frames.Add(new RenderObject(frameImage));
                rect.Y = i * rect.Height;
            }
            return animation;
        }
    }
}