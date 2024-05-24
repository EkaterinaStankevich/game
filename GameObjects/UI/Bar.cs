namespace WindowsFormsPlatformer.GameObjects.UI
{
    class Bar : Element
    {
        private Rect m_originalFrame;
        private double m_value = 100;

        public Bar(GameContext context, Rect frame) : base(context,frame)
        {
            m_originalFrame = frame;
        }

        public double Value
        {
            get { return m_value; }
            set
            {
                if (value > 100) value = 100;
                if (value < 0) value = 0;
                m_value = value;

                Frame.Center.X = m_originalFrame.Center.X + m_originalFrame.Size.Width * ((value - 100) / 200);
                Frame.Size.Width = m_originalFrame.Size.Width / 100 * value;
            }
        }
    }
}
