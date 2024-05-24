using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WindowsFormsPlatformer
{
    class RenderObject
    {
        public Bitmap Texture { get; internal set; }

        private bool m_renderFlip;
        public bool RenderFlip
        {
            get { return m_renderFlip; }
            set
            {
                if ((value && !m_renderFlip) || (!value && m_renderFlip))
                {
                    Texture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    m_renderFlip = value;
                }
            }
        }

        public RenderObject(Bitmap texture)
        {
            Texture = texture;
        }

        public void Render(Graphics renderer, GameContext context, Vector position, Size size, Vector cameraPosition, Size cameraSize)
        {
            Vector renderPosition = position + new Vector(-size.Width / 2, -size.Height / 2) - cameraPosition - new Vector(-cameraSize.Width / 2, -cameraSize.Height / 2);
            Rectangle targetRect = new Rectangle();
            targetRect.X = (int)Math.Round(context.WindowSize.Width * (renderPosition.X / cameraSize.Width));
            targetRect.Y = (int)Math.Round(context.WindowSize.Height * (renderPosition.Y / cameraSize.Height));
            targetRect.Width = (int)Math.Round(context.WindowSize.Width * (size.Width / cameraSize.Width));
            targetRect.Height = (int)Math.Round(context.WindowSize.Height * (size.Height / cameraSize.Height));
            using (ImageAttributes wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                renderer.DrawImage(Texture, targetRect, 0, 0, Texture.Width, Texture.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        public static RenderObject FromColor(Color color)
        {
            Bitmap texture = new Bitmap(1, 1);
            texture.SetPixel(0, 0, color);
            return new RenderObject(texture);
        }
    }
}
