using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
namespace TecCraftLauncher
{
	public class MinecraftModelView : Control
	{
		private const int IsometricFOV = 46;

		private List<Texel> texelList = new List<Texel>();
		private TexelComparer texelComparer = new TexelComparer();
		private List<Object3D> object3DList = new List<Object3D>();
		private List<Object3D> dynamicObject3DtList = new List<Object3D>();
		internal float cameraZ = 31.4285717f;
		private int fov = 70;
		private float scale = 1f;
		internal float RotationY;
		internal Matrix3D GlobalTransformation = Matrix3D.Identity;
		private bool perspective = true;
		private Object3D rightHand;
		private Object3D leftHand;
		private Object3D body;
		private Object3D rightLeg;
		private Object3D leftLeg;
		private Object3DGroup head;
		private Image skin;
		private Image setSkin;
		private Color backgroundColor;
		private float PiBy180 = 0.0174532924f;
		private Brush backgroundBrush = new SolidBrush(Color.SkyBlue);
        private Timer tmrRotate;
        
		private IContainer components;
	
		public new Color BackColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				this.backgroundColor = value;
				this.UpdateBackgroundBrush();
				base.Invalidate();
			}
		}

        public int RotationSpeed
        {
            get
            {
                return this.tmrRotate.Interval;
            }
            set
            {
                this.tmrRotate.Interval = value;
            }
        }
		
		[Browsable(false)]
		public Image Skin
		{
			get
			{
				return this.setSkin;
			}
			set
			{
				
				if (this.skin != null)
				{
					this.skin.Dispose();
				}
                this.skin = value;
				this.texelList.Clear();
				this.head.Image = this.skin;
				this.rightHand.Image = this.skin;
				this.leftHand.Image = this.skin;
				this.leftLeg.Image = this.skin;
				this.rightLeg.Image = this.skin;
				this.body.Image = this.skin;
				foreach (Object3D current in this.object3DList)
				{
					current.Update();
				}
				base.Invalidate();
			}
		}



		private static Image EmptySkin
		{
			get
			{
				Image image = new Bitmap(64, 32, PixelFormat.Format32bppArgb);
				return image;
			}
		}
	

		public MinecraftModelView()
		{
			base.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.InitializeComponent();
			this.texelComparer = new TexelComparer();
			this.InitializeModel();
		}
	
		
		private void InitializeModel()
		{
            this.skin = MinecraftModelView.EmptySkin;
            Object3D object3D = new Box(this.skin, new Rectangle(8, 0, 16, 8), new Rectangle(0, 8, 32, 8), new Point3D(0f, 0f, 0f), Effects.None);
            Object3D object3D2 = new Box(this.skin, new Rectangle(40, 0, 16, 8), new Rectangle(32, 8, 32, 8), new Point3D(0f, 0f, 0f), Effects.None);
            this.leftHand = new Box(this.skin, new Rectangle(44, 16, 8, 4), new Rectangle(40, 20, 32, 12), new Point3D(0f, 4f, 0f), Effects.FlipHorizontally);
            this.rightHand = new Box(this.skin, new Rectangle(44, 16, 8, 4), new Rectangle(40, 20, 32, 12), new Point3D(0f, 4f, 0f), Effects.None);
            this.leftLeg = new Box(this.skin, new Rectangle(4, 16, 8, 4), new Rectangle(0, 20, 16, 12), new Point3D(0f, 6f, 0f), Effects.FlipHorizontally);
            this.rightLeg = new Box(this.skin, new Rectangle(4, 16, 8, 4), new Rectangle(0, 20, 16, 12), new Point3D(0f, 6f, 0f), Effects.None);
            this.body = new Box(this.skin, new Rectangle(20, 16, 16, 4), new Rectangle(16, 20, 24, 12), new Point3D(0f, 0f, 0f), Effects.None);
			this.head = new Object3DGroup();
			object3D2.Scale = 1.16f;
			this.head.RotationOrder = RotationOrders.XY;
			this.head.MinDegrees1 = -80f;
			this.head.MaxDegrees1 = 80f;
			this.head.MinDegrees2 = -57f;
			this.head.MaxDegrees2 = 57f;
			this.head.Add(object3D);
			this.head.Add(object3D2);
			this.head.Position = new Point3D(0f, 8f, 0f);
			this.head.Origin = new Point3D(0f, -4f, 0f);
			this.head.RotationOrder = RotationOrders.XY;
			this.body.Position = new Point3D(0f, 2f, 0f);
			this.leftHand.Position = new Point3D(6f, 6f, 0f);
			this.leftHand.RotationOrder = RotationOrders.ZX;
			this.leftHand.MinDegrees1 = 0f;
			this.leftHand.MaxDegrees1 = 160f;
			this.leftHand.MinDegrees2 = -170f;
			this.leftHand.MaxDegrees2 = 60f;
			this.rightHand.Position = new Point3D(-6f, 6f, 0f);
			this.rightHand.RotationOrder = RotationOrders.ZX;
			this.rightHand.MinDegrees1 = -160f;
			this.rightHand.MaxDegrees1 = 0f;
			this.rightHand.MinDegrees2 = -170f;
			this.rightHand.MaxDegrees2 = 60f;
			this.leftLeg.Position = new Point3D(2f, -4f, 0f);
			this.leftLeg.RotationOrder = RotationOrders.ZX;
			this.leftLeg.MinDegrees1 = 0f;
			this.leftLeg.MaxDegrees1 = 70f;
			this.leftLeg.MinDegrees2 = -110f;
			this.leftLeg.MaxDegrees2 = 60f;
			this.rightLeg.Position = new Point3D(-2f, -4f, 0f);
			this.rightLeg.RotationOrder = RotationOrders.ZX;
			this.rightLeg.MinDegrees1 = -70f;
			this.rightLeg.MaxDegrees1 = 0f;
			this.rightLeg.MinDegrees2 = -110f;
			this.rightLeg.MaxDegrees2 = 60f;
			this.AddDynamic(this.head);
			this.AddStatic(this.body);
			this.AddDynamic(this.rightHand);
			this.AddDynamic(this.leftHand);
			this.AddDynamic(this.rightLeg);
			this.AddDynamic(this.leftLeg);
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			base.OnPaintBackground(pevent);
			Graphics graphics = pevent.Graphics;
			graphics.CompositingQuality = CompositingQuality.HighSpeed;
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			graphics.SmoothingMode = SmoothingMode.HighSpeed;
			graphics.CompositingMode = CompositingMode.SourceCopy;
			graphics.FillRectangle(this.backgroundBrush, base.ClientRectangle);
			graphics.CompositingMode = CompositingMode.SourceOver;
			
		
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			this.texelList.Sort(this.texelComparer);
			Graphics graphics = pe.Graphics;
			graphics.TranslateTransform((float)(base.Width / 2), (float)(base.Height / 2));
			graphics.ScaleTransform(this.scale, -this.scale);
			graphics.CompositingQuality = CompositingQuality.HighSpeed;
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			graphics.SmoothingMode = SmoothingMode.HighSpeed;
			graphics.CompositingMode = CompositingMode.SourceCopy;
			for (int i = 0; i < this.texelList.Count; i++)
			{
				this.texelList[i].Draw(graphics);
			}
		}
	
	
		private void UpdateBackgroundBrush()
		{
               this.backgroundBrush = ((Brush) new SolidBrush(this.backgroundColor));
		}
		public Brush GetBackgroundBrush(Size size)
		{
				return new SolidBrush(this.backgroundColor);
		}
		
		private void SetupProjection()
		{
			this.cameraZ = 2400f / (float)this.fov;
			this.scale = (float)Math.Min(base.Width, base.Height) * 0.01f / (float)Math.Tan((double)(this.perspective ? this.fov : 46) * 3.1415926535897931 / 360.0);
		}
		protected override void OnResize(EventArgs e)
		{
			this.SetupProjection();
			this.UpdateBackgroundBrush();
			base.OnResize(e);
		}
		internal void RemoveTexelsOf(TexturePlane texturePlane)
		{
			for (int i = 0; i < this.texelList.Count; i++)
			{
				if (this.texelList[i].TexturePlane == texturePlane)
				{
					this.texelList.RemoveAt(i);
					i--;
				}
			}
		}
		internal void AddTexel(Texel texel)
		{
			this.texelList.Add(texel);
		}

		private void AddStatic(Object3D object3D)
		{
			object3D.Viewport = this;
			this.object3DList.Add(object3D);
			foreach (Object3D current in this.object3DList)
			{
				current.Update();
			}
		}
		private void AddDynamic(Object3D object3D)
		{
			object3D.Viewport = this;
			this.object3DList.Add(object3D);
			this.dynamicObject3DtList.Add(object3D);
			foreach (Object3D current in this.object3DList)
			{
				current.Update();
			}
		}
		internal PointF Point3DTo2D(Point3D point3D)
		{
			if (this.perspective)
			{
				return new PointF(point3D.X * (-50f / (point3D.Z - this.cameraZ)), point3D.Y * (-50f / (point3D.Z - this.cameraZ)));
			}
			return new PointF(point3D.X, point3D.Y);
		}
		internal float GetZOrder(Point3D point3D)
		{
			if (this.perspective)
			{
				return point3D.X * point3D.X + point3D.Y * point3D.Y + (this.cameraZ - point3D.Z) * (this.cameraZ - point3D.Z);
			}
			return -point3D.Z;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tmrRotate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrRotate
            // 
            this.tmrRotate.Enabled = true;
            this.tmrRotate.Interval = 10;
            this.tmrRotate.Tick += new System.EventHandler(this.tmrRotate_Tick);
            this.ResumeLayout(false);

		}

        private void tmrRotate_Tick(object sender, EventArgs e)
        {
            this.RotationY = (float)this.RotationY + 1;
            Matrix3D globalTransformation = Matrix3D.CreateRotationX(0 * this.PiBy180) * Matrix3D.CreateRotationY(this.RotationY * this.PiBy180);
            foreach (Object3D current in this.object3DList)
            {
                current.GlobalTransformation = globalTransformation;
            }
            base.Invalidate();
        }
	}
}
