namespace WindowsFormsPlatformer.GameObjects
{
    class Consumable : GameObject
    {
        public Consumable(GameContext context, Rect frame) : base(context, frame)
        {
            Physics = new PhysicsState(this);
        }
    }
}