using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsPlatformer.GameObjects
{
    class Solid : GameObject
    {
        public Solid(GameContext context, Rect frame) : base(context, frame)
        {
            Physics = new PhysicsState(this);
        }

        public override void HandleEnterCollision(Collision collision)
        {
            Player player = collision.Collider as Player;
            if (player != null && collision.Collider.Physics.Velocity.Y > 5)
            {
                player.DealDamage((int)Math.Round(collision.Collider.Physics.Velocity.Y * 10));
            }
        }

        public override void HandleCollision(Collision collision)
        {
            if (Math.Abs(collision.CollisionVector.X) < Math.Abs(collision.CollisionVector.Y))
            {
                collision.Collider.Frame.Center.X += collision.CollisionVector.X;
                collision.Collider.Physics.Velocity.X = 0;
            }
            else
            {
                collision.Collider.Frame.Center.Y += collision.CollisionVector.Y;
                collision.Collider.Physics.Velocity.Y = 0;
            }
        }
    }
}
