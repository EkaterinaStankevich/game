using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsPlatformer.GameObjects
{
    class Player : GameObject
    {
        public double Speed { get; set; }
        public double JumpSpeed { get; set; }
        public int Power { get; internal set; }
        public int MaxPower { get; set; }
        public bool Jumped { get; internal set; }
        public int Health { get; internal set; } = 100;
        public bool Dead { get; internal set; }
        public bool Won { get; internal set; }
        public Size OriginalSize { get; internal set; }
        public Animation IdleAnimation { get; set; }
        public Animation MoveAnimation { get; set; }
        public Animation JumpAnimation { get; set; }
        public Animation CrouchAnimation { get; set; }
        public Animation CrouchMoveAnimation { get; set; }

        private bool m_crouched;
        public bool Crouched
        {
            get { return m_crouched; }
            set
            {
                if (value && !m_crouched)
                {
                    m_crouched = true;
                    Frame.Center.Y += OriginalSize.Height / 4;
                    Frame.Size.Height = OriginalSize.Height / 2;
                }
                else if (!value && m_crouched)
                {
                    m_crouched = false;
                    Frame.Center.Y -= OriginalSize.Height / 4;
                    Frame.Size.Height = OriginalSize.Height;
                }
            }
        }


        public Player(GameContext context, Rect frame) : base(context, frame)
        {
            Physics = new PhysicsState(this);
            Physics.Gravity = true;
            Physics.Still = false;
            OriginalSize = frame.Size;
        }

        public override void KeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.G:
                    Physics.Gravity = !Physics.Gravity;
                    if (!Physics.Gravity)
                    {
                        Jumped = true;
                        Physics.Velocity *= 0;
                    }
                    break;
            }
            base.KeyDown(key);
        }

        public override void HandleKeyboardState(IList<Keys> keys)
        {
            if (!Dead)
            {
                bool sitDown = false;
                bool moveLeft = false;
                bool moveRight = false;
                Vector moveVector = new Vector();
                if (keys.Contains(Keys.Left) || keys.Contains(Keys.A))
                {
                    moveVector += new Vector(-Speed, 0);
                    moveLeft = true;
                }
                if (keys.Contains(Keys.Right) || keys.Contains(Keys.D))
                {
                    moveVector += new Vector(Speed, 0);
                    moveRight = true;
                }
                if (keys.Contains(Keys.Up) || keys.Contains(Keys.W) || keys.Contains(Keys.Space))
                {
                    if (!Physics.Gravity)
                    {
                        moveVector += new Vector(0, -Speed);
                    }
                    else
                    {
                        if (!Jumped)
                        {
                            Physics.Velocity += new Vector(0, -JumpSpeed);
                            Jumped = true;
                        }
                    }
                }
                if (keys.Contains(Keys.Down) || keys.Contains(Keys.S) || keys.Contains(Keys.ControlKey))
                {
                    if (!Physics.Gravity)
                    {
                        moveVector += new Vector(0, Speed);
                    }
                    else
                    {
                        sitDown = true;
                    }
                }
                Crouched = sitDown;

                if (moveLeft && !moveRight)
                {
                    MoveAnimation.TurnedLeft = true;
                    CrouchAnimation.TurnedLeft = true;
                    CrouchMoveAnimation.TurnedLeft = true;
                }
                if (moveRight && !moveLeft)
                {
                    MoveAnimation.TurnedLeft = false;
                    CrouchAnimation.TurnedLeft = false;
                    CrouchMoveAnimation.TurnedLeft = false;
                }

                if (!moveLeft && !moveRight && !Jumped && !Crouched)
                    Animation = IdleAnimation;
                if (!moveLeft && !moveRight && !Jumped && Crouched)
                    Animation = CrouchAnimation;
                if ((moveLeft || moveRight) && !Jumped && !Crouched)
                    Animation = MoveAnimation;
                if ((moveLeft || moveRight) && !Jumped && Crouched)
                    Animation = CrouchMoveAnimation;
                if (Jumped && Crouched)
                    Animation = CrouchAnimation;
                if (Jumped && !Crouched)
                    Animation = JumpAnimation;

                Frame.Center += moveVector;
            }
            base.HandleKeyboardState(keys);
        }

        public override void HandleEnterCollision(Collision collision)
        {
            var consumable = collision.Collider as Consumable;
            if (consumable != null)
            {
                Power += 1;
                Context.UI.PowerBar.Value = (double)Power/MaxPower*100.0;
                consumable.Remove();
                Speed += 0.01;
                JumpSpeed += 0.01;
                if (Power >= MaxPower)
                {
                    Win();
                }
            }
        }

        public override void HandleExitCollision(GameObject collider)
        {
            if (Physics.Colliders.Count == 0)
            {
                Jumped = true;
            }
        }

        public override void HandleCollision(Collision collision)
        {
            if (Math.Abs(collision.CollisionVector.X) > Math.Abs(collision.CollisionVector.Y))
            {
                if (collision.CollisionVector.Y > 0 && Jumped && Physics.Gravity)
                {
                    Jumped = false;
                }
            }
        }

        public void DealDamage(int damage)
        {
            if (!Won)
            {
                Health -= damage;
                Context.UI.HealthBar.Value = Health;
                if (Health < 0)
                {
                    Die();
                }
            }
        }

        public void Die()
        {
            Dead = true;
            Context.UI.DeathText.Visible = true;
        }

        public void Win()
        {
            Won = true;
            Context.UI.WinText.Visible = true;
        }
    }
}
