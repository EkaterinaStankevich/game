namespace WindowsFormsPlatformer.GameObjects
{
    class Frame : GameObject
    {
        public double Width { get; internal set; }
        public Solid Floor { get; internal set; }
        public Solid WallLeft { get; internal set; }
        public Solid WallRight { get; internal set; }
        public Solid Ceiling { get; internal set; }

        public Frame(GameContext context, Rect frame, double width) : base(context, frame)
        {
            Width = width;
            Ceiling = new Solid(context, new Rect(
                0,
                -Frame.Size.Height / 2 + Width / 2,
                Frame.Size.Width,
                Width));
            AddChild(Ceiling);
            WallLeft = new Solid(context, new Rect(
                -Frame.Size.Width / 2 + Width / 2,
                0,
                Width,
                Frame.Size.Height - Width * 2));
            AddChild(WallLeft);
            WallRight = new Solid(context, new Rect(
                Frame.Size.Width / 2 - Width / 2,
                0,
                Width,
                Frame.Size.Height - Width * 2));
            AddChild(WallRight);
            Floor = new Solid(context, new Rect(
                0,
                Frame.Size.Height / 2 - Width / 2,
                Frame.Size.Width,
                Width));
            AddChild(Floor);
        }
    }
}
