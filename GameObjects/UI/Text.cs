using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsPlatformer.GameObjects.UI
{
    class Text : Element
    {
        private Font m_font;
        private string m_title;
        private Color m_color;

        public Font Font
        {
            get { return m_font; }
            set
            {
                m_font = value;
                Generate();
            }
        }

        public string Title
        {
            get { return m_title; }
            set
            {
                m_title = value;
                Generate();
            }
        }

        public Color Color
        {
            get { return m_color; }
            set
            {
                m_color = value;
                Generate();
            }
        }

        public Text(GameContext context, Rect frame) : base(context, frame)
        {
            
        }

        public Text(GameContext context, Rect frame, string title, Font font, Color color) : this(context, frame)
        {
            m_title = title;
            m_font = font;
            m_color = color;
            Generate();
        }

        private void Generate()
        {
            if (m_font != null && m_title != null)
            {
                Bitmap texture = new Bitmap((int)Frame.Size.Width, (int)Frame.Size.Height);
                using(var brush = new SolidBrush(m_color))
                using(var gfx = Graphics.FromImage(texture))
                {
                    gfx.DrawString(m_title, m_font, brush, 0, 0);
                }
                RenderObject = new RenderObject(texture);
            }
        }
    }
}
