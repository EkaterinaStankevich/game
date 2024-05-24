using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsPlatformer.GameObjects;
using WindowsFormsPlatformer.GameObjects.UI;
using Timer = System.Timers.Timer;

namespace WindowsFormsPlatformer
{
    public partial class MainForm : Form
    {
        IList<Keys> KeysPressed = new List<Keys>();
        GameContext Context;

        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            CreateContext();

            //var PhysicsTimer = new Timer(1);
            //PhysicsTimer.Elapsed += new ElapsedEventHandler(PhysicsTick);
            //PhysicsTimer.Start();

            //var ContextTimer = new Timer(10);
            //ContextTimer.Elapsed += new ElapsedEventHandler(ContextTick);
            //ContextTimer.Start();

            //var RenderTimer = new Timer(10);
            //RenderTimer.Elapsed += new ElapsedEventHandler(RenderTick);
            //RenderTimer.AutoReset = true;
            //RenderTimer.Start();

            //var AnimationTimer = new Timer(34);
            //AnimationTimer.Elapsed += new ElapsedEventHandler(AnimationTick);
            //AnimationTimer.Start();

            var GameTimer = new Timer(1);
            GameTimer.Elapsed += new ElapsedEventHandler(GameTick);
            GameTimer.Start();

            //TODO: thread sync

            ResizeEnd += new EventHandler(CreateBackBuffer);
            Load += new EventHandler(CreateBackBuffer);
            base.Paint += new PaintEventHandler(Paint);
            base.KeyDown += new KeyEventHandler(KeyDown);
            base.KeyUp += new KeyEventHandler(KeyUp);
        }

        new void KeyDown(object sender, KeyEventArgs e)
        {
            if (!KeysPressed.Contains(e.KeyCode))
                KeysPressed.Add(e.KeyCode);
            Context.World.KeyDown(e.KeyCode);
        }

        new void KeyUp(object sender, KeyEventArgs e)
        {
            KeysPressed.Remove(e.KeyCode);
        }

        new void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

            //render
            Context.World.Render(e.Graphics);
            Context.UI.Render(e.Graphics);
        }

        void CreateBackBuffer(object sender, EventArgs e)
        {
            Context.WindowSize = new Size(ClientSize.Width, ClientSize.Height);
        }

        void PhysicsTick(object sender, ElapsedEventArgs e)
        {
            Context.World.Clean();
            Context.World.ProcessPhysics();
            Context.World.DetectCollisions();
        }

        void ContextTick(object sender, ElapsedEventArgs e)
        {
            if (Context.IsQuit)
            {
                Application.Exit();
            }

            Context.World.HandleKeyboardState(KeysPressed);
        }

        void RenderTick(object sender, ElapsedEventArgs e)
        {
            //redraw
            Invalidate();
        }

        void AnimationTick(object sender, ElapsedEventArgs e)
        {
            Context.World.Animate(e.SignalTime.Ticks);
        }

        void GameTick(object sender, ElapsedEventArgs e)
        {
            if (Context.IsQuit)
            {
                Application.Exit();
            }

            Context.World.HandleKeyboardState(KeysPressed);
            Context.World.Clean();
            Context.World.ProcessPhysics();
            Context.World.DetectCollisions();
            Context.World.Animate(e.SignalTime.Ticks);

            Invalidate();
        }

        void CreateContext()
        {
            //TODO: fine tune
            const int gameWidth = 400;
            const int gameHeight = 300;
            const int gridSquareSize = 10;
            const double gravityForce = 0.1;
            const double itemChance = 0.16;


            //-----------------------------------------------------------


            Context = new GameContext(new Size(ClientSize.Width, ClientSize.Height));
            Context.World.Frame.Size = new Size(gameWidth, gameHeight);

            var player = new Player(Context, new Rect(0, 0, gridSquareSize, 2* gridSquareSize));
            player.Speed = 1.3;
            player.JumpSpeed = 2.5;
            player.Physics.GravityForce = gravityForce;
            player.AddChild(Context.World.Camera);
            player.IdleAnimation = Animation.WithSingleRenderObject(DateTime.Now.Ticks, new RenderObject(Properties.Resources.idle));
            player.JumpAnimation = Animation.WithSingleRenderObject(DateTime.Now.Ticks, new RenderObject(Properties.Resources.jump));
            player.CrouchAnimation = Animation.WithSingleRenderObject(DateTime.Now.Ticks, new RenderObject(Properties.Resources.crouch));
            player.CrouchMoveAnimation = Animation.WithSingleRenderObject(DateTime.Now.Ticks, new RenderObject(Properties.Resources.crouch));
            player.MoveAnimation = Animation.WithSpeedAndImage(DateTime.Now.Ticks, 700, Properties.Resources.move, 40, 80, 6);

            Context.World.AddChild(player);

            var frame = new Frame(Context, new Rect(0, 0, Context.World.Frame.Size.Width, Context.World.Frame.Size.Height), gridSquareSize);
            frame.Ceiling.RenderObject = RenderObject.FromColor(Color.Black);
            frame.WallLeft.RenderObject = RenderObject.FromColor(Color.Black);
            frame.WallRight.RenderObject = RenderObject.FromColor(Color.Black);
            frame.Floor.RenderObject = RenderObject.FromColor(Color.Black);
            Context.World.AddChild(frame);
            
            var rnd = new Random();
            int count = (int)(Context.World.Frame.Size.Width * Context.World.Frame.Size.Height * itemChance / (gridSquareSize * gridSquareSize));
            int powerCount = count/2;
            player.MaxPower = powerCount;
            int x = (int)(Context.World.Frame.Size.Width / gridSquareSize - 2);
            int y = (int)(Context.World.Frame.Size.Height / gridSquareSize - 2);
            int rndX, rndY;
            int[] takenX = new int[count];
            int[] takenY = new int[count];
            for (int i = 0; i < count; i++)
            {
                bool taken;
                do
                {
                    taken = false;
                    rndX = rnd.Next() % x;
                    rndY = rnd.Next() % y;
                    for (int j = 0; j <= i; j++)
                    {
                        if (rndX == takenX[j] && rndY == takenY[j])
                        {
                            taken = true;
                            break;
                        }
                    }
                } while (taken);

                takenX[i] = rndX;
                takenY[i] = rndY;

                var rect = new Rect(
                        Context.World.Frame.Size.Width / 2 - gridSquareSize * 1.5 - rndX * gridSquareSize,
                        Context.World.Frame.Size.Height / 2 - gridSquareSize * 1.5 - rndY * gridSquareSize,
                        gridSquareSize,
                        gridSquareSize);

                if (powerCount > 0)
                {
                    var gameObject = new Consumable(Context, rect);
                    gameObject.RenderObject = RenderObject.FromColor(Color.FromArgb(0x00, 0xFF, 0x00));
                    Context.World.AddChild(gameObject);
                    powerCount--;

                }
                else
                {
                    var gameObject = new Solid(Context, rect);
                    gameObject.RenderObject = new RenderObject(Properties.Resources.brick);
                    Context.World.AddChild(gameObject);
                }
            }

            Context.UI.DeathText = new Text(Context, new Rect(0, 0, 150, 40), "You died! Game over!", new Font("Times New Roman", 12), Color.Red);
            Context.UI.DeathText.Visible = false;
            Context.UI.AddChild(Context.UI.DeathText);

            Context.UI.WinText = new Text(Context, new Rect(0, 0, 150, 40), "Congratulations! You won!", new Font("Times New Roman", 12), Color.Green);
            Context.UI.WinText.Visible = false;
            Context.UI.AddChild(Context.UI.WinText);

            Context.UI.HealthBarHolder = new Element(Context, new Rect(-Context.World.Camera.OriginalSize.Width / 2 + 16, -Context.World.Camera.OriginalSize.Height / 2 + 2.5, 30, 3));
            Context.UI.HealthBarHolder.RenderObject = RenderObject.FromColor(Color.Black);
            Context.UI.AddChild(Context.UI.HealthBarHolder);

            Context.UI.HealthBar = new Bar(Context, new Rect(-Context.World.Camera.OriginalSize.Width / 2 + 16, -Context.World.Camera.OriginalSize.Height / 2 + 2.5, 29, 2));
            Context.UI.HealthBar.RenderObject = RenderObject.FromColor(Color.Red);
            Context.UI.AddChild(Context.UI.HealthBar);

            Context.UI.PowerBarHolder = new Element(Context, new Rect(Context.World.Camera.OriginalSize.Width / 2 - 16, -Context.World.Camera.OriginalSize.Height / 2 + 2.5, 30, 3));
            Context.UI.PowerBarHolder.RenderObject = RenderObject.FromColor(Color.Black);
            Context.UI.AddChild(Context.UI.PowerBarHolder);

            Context.UI.PowerBar = new Bar(Context, new Rect(Context.World.Camera.OriginalSize.Width / 2 - 16, -Context.World.Camera.OriginalSize.Height / 2 + 2.5, 29, 2));
            Context.UI.PowerBar.RenderObject = RenderObject.FromColor(Color.Green);
            Context.UI.PowerBar.Value = 0;
            Context.UI.AddChild(Context.UI.PowerBar);
        }
    }
}
