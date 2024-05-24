using System;
using System.Collections.Generic;

namespace WindowsFormsPlatformer
{
    internal class PhysicsState
    {
        public GameObject GameObject { get; internal set; }
        public bool Gravity { get; set; } = false;
        public bool Still { get; set; } = true;
        public double GravityForce { get; set; } = 0;
        public List<GameObject> Colliders { get; internal set; } = new List<GameObject>();

        public Vector Velocity;
        public PhysicsState(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public void Change()
        {
            if (Gravity)
            {
                Velocity += new Vector(0, GravityForce);
            }
            GameObject.Frame.Center += Velocity;
        }

        public void Clean()
        {
            Colliders.RemoveAll(c => c.IsRemoved);
        }

        public void DetectCollision(PhysicsState c)
        {
            if (Still && c.Still)
                return;

            double x1 = GameObject.GlobalPosition().X - GameObject.Frame.Size.Width / 2;
            double x2 = c.GameObject.GlobalPosition().X - c.GameObject.Frame.Size.Width / 2;
            double X1 = x1 + GameObject.Frame.Size.Width;
            double X2 = x2 + c.GameObject.Frame.Size.Width;
            double y1 = GameObject.GlobalPosition().Y - GameObject.Frame.Size.Height / 2;
            double y2 = c.GameObject.GlobalPosition().Y - c.GameObject.Frame.Size.Height / 2;
            double Y1 = y1 + GameObject.Frame.Size.Height;
            double Y2 = y2 + c.GameObject.Frame.Size.Height;

            double diffX1 = X1 - x2;
            double diffX2 = x1 - X2;
            double diffY1 = Y1 - y2;
            double diffY2 = y1 - Y2;

            bool alreadyCollided = (
                Colliders.Contains(c.GameObject)
                ||
                c.Colliders.Contains(GameObject)
                );
            if (diffX1 > 0 &&
                diffX2 < 0 &&
                diffY1 > 0 &&
                diffY2 < 0)
            {
                Vector overlapArea = new Vector(
                    (Math.Abs(diffX1) < Math.Abs(diffX2) ? diffX1 : diffX2),
                    (Math.Abs(diffY1) < Math.Abs(diffY2) ? diffY1 : diffY2)
                    );
                if (!alreadyCollided)
                {
                    Colliders.Add(c.GameObject);
                    c.Colliders.Add(GameObject);

                    GameObject.HandleEnterCollision(new Collision(c.GameObject, overlapArea));
                    c.GameObject.HandleEnterCollision(new Collision(GameObject, overlapArea * -1));
                }
                GameObject.HandleCollision(new Collision(c.GameObject, overlapArea));
                c.GameObject.HandleCollision(new Collision(GameObject, overlapArea * -1));
            }
            else if (alreadyCollided)
            {
                Colliders.Remove(c.GameObject);
                c.Colliders.Remove(GameObject);
                GameObject.HandleExitCollision(c.GameObject);
                c.GameObject.HandleExitCollision(GameObject);
            }
        }
    }
}