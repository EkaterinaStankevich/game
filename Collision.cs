namespace WindowsFormsPlatformer
{
    struct Collision
    {
        public Vector CollisionVector;
        public GameObject Collider;

        public Collision(GameObject collider, Vector collisionVector)
        {
            Collider = collider;
            CollisionVector = collisionVector;
        }
    }
}