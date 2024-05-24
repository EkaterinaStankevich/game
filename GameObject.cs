using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsPlatformer
{
    class GameObject
    {
        public GameContext Context { get; internal set; }
        public List<GameObject> Children { get; internal set; } = new List<GameObject>();
        public GameObject Parent { get; set; }
        public RenderObject RenderObject { get; set; }
        public PhysicsState Physics { get; set; }
        public Animation Animation { get; set; }
        public Rect Frame;
        public bool Visible { get; set; } = true;
        private bool m_removed = false;
        public bool IsRemoved { get { return m_removed; } }

        public void Remove()
        {
            m_removed = true;
        }

        public GameObject(GameContext context)
        {
            Context = context;
        }

        public GameObject(GameContext context, Rect frame) : this(context)
        {
            Frame = frame;
        }

        public virtual void KeyDown(Keys key)
        {
            foreach (var child in Children)
                child.KeyDown(key);
        }

        public virtual void HandleKeyboardState(IList<Keys> keys)
        {
            foreach (var child in Children)
                child.HandleKeyboardState(keys);
        }

        public void ProcessPhysics()
        {
            if (Physics != null)
                Physics.Change();

            foreach (var child in Children)
                child.ProcessPhysics();
        }

        public void DetectCollisions()
        {
            var allColliders = new List<GameObject>();
            DetectCollisions(allColliders);
            int size = allColliders.Count;
            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    allColliders[i].Physics.DetectCollision(allColliders[j].Physics);
                }
            }
        }

        protected void DetectCollisions(IList<GameObject> allColliders)
        {
            if (Physics != null)
                allColliders.Add(this);
            foreach (var child in Children)
            {
                child.DetectCollisions(allColliders);
            }
        }

        public virtual void HandleEnterCollision(Collision collision) { }

        public virtual void HandleExitCollision(GameObject collider) { }

        public virtual void HandleCollision(Collision collision) { }

        public void Animate(long ticks)
        {
            if (Animation != null)
                RenderObject = Animation.Animate(ticks);

            foreach (var child in Children)
                child.Animate(ticks);
        }

        public virtual void Render(Graphics renderer, Vector localBasis, Vector cameraPosition, Size cameraSize)
        {
            if (Visible && RenderObject != null)
            {
                Vector globalPosition = Frame.Center;
                globalPosition += localBasis;
                RenderObject.Render(renderer, Context, globalPosition, Frame.Size, cameraPosition, cameraSize);
            }
            foreach (var child in Children)
            {
                child.Render(renderer, localBasis + Frame.Center, cameraPosition, cameraSize);
            }
        }

        public void AddChild(GameObject child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void Clean()
        {
            foreach (var child in Children)
                if (child.Physics != null)
                    child.Physics.Clean();
            Children.RemoveAll(c => c.IsRemoved);
        }

        public Vector GlobalPosition()
        {
            if (Parent == null)
            {
                return Frame.Center;
            }
            else
            {
                return Frame.Center + Parent.GlobalPosition();
            }
        }
    }
}
