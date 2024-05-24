using WindowsFormsPlatformer.GameObjects;
using WindowsFormsPlatformer.GameObjects.UI;

namespace WindowsFormsPlatformer
{
    class GameContext
    {
        public World World { get; internal set; }
        public UI UI { get; internal set; }
        public Size WindowSize { get; set; }

        private bool m_quit = false;
        public bool IsQuit
        {
            get { return m_quit; }
        }

        public void Quit()
        {
            m_quit = true;
        }

        public GameContext(Size windowSize)
        {
            WindowSize = windowSize;
            World = new World(this, new Rect(0, 0, windowSize.Width / 2, windowSize.Height / 2));
            UI = new UI(this, new Rect(Vector.Zero, World.Camera.OriginalSize));
        }
    }
}
