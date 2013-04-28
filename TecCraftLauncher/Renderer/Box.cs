using System;
using System.Drawing;
namespace TecCraftLauncher
{
	public class Box : Object3D
	{
		private TexturePlane top;
		private TexturePlane bottom;
		private TexturePlane front;
		private TexturePlane back;
		private TexturePlane left;
		private TexturePlane right;
		private Rectangle srcTop;
		private Rectangle srcBottom;
		private Rectangle srcFront;
		private Rectangle srcBack;
		private Rectangle srcLeft;
		private Rectangle srcRight;
		private Matrix3D topLocalTransformation = Matrix3D.CreateRotationX(-1.57079637f);
		private Matrix3D bottomLocalTransformation = Matrix3D.CreateRotationX(-1.57079637f);
		private Matrix3D frontLocalTransformation = Matrix3D.Identity;
		private Matrix3D backLocalTransformation = Matrix3D.CreateRotationY(3.14159274f);
		private Matrix3D leftLocalTransformation = Matrix3D.CreateRotationY(-1.57079637f);
		private Matrix3D rightLocalTransformation = Matrix3D.CreateRotationY(1.57079637f);
        public Effects effects;
		public override Image Image
		{
			set
			{
				this.SetImage(value);
			}
		}
		internal override MinecraftModelView Viewport
		{
			set
			{
				base.Viewport = value;
				this.top.Viewport = value;
				this.bottom.Viewport = value;
				this.front.Viewport = value;
				this.back.Viewport = value;
				this.left.Viewport = value;
				this.right.Viewport = value;
				this.Update();
			}
		}
		internal override void Update()
		{
			Matrix3D a = this.globalTransformation * this.localTransformation;
			this.top.LocalTransformation = a * this.topLocalTransformation;
			this.bottom.LocalTransformation = a * this.bottomLocalTransformation;
			this.front.LocalTransformation = a * this.frontLocalTransformation;
			this.back.LocalTransformation = a * this.backLocalTransformation;
			this.left.LocalTransformation = a * this.leftLocalTransformation;
			this.right.LocalTransformation = a * this.rightLocalTransformation;
		}
		public Box(Image image, Rectangle srcTopBottom, Rectangle srcSides, Point3D origin, Effects effects)
		{
			this.effects = effects;
			base.Origin = origin;
			this.SetImage(image, srcTopBottom, srcSides);
		}
		private void SetImage(Image image, Rectangle srcTopBottom, Rectangle srcSides)
		{
			int num = srcTopBottom.Width / 2;
			int height = srcSides.Height;
			int height2 = srcTopBottom.Height;
			this.srcTop = new Rectangle(srcTopBottom.Location, new Size(num, height2));
			this.srcBottom = new Rectangle(srcTopBottom.X + num, srcTopBottom.Y, num, height2);
			this.srcFront = new Rectangle(srcSides.X + height2, srcSides.Y, num, height);
			this.srcBack = new Rectangle(srcSides.X + height2 + num + height2, srcSides.Y, num, height);
			this.srcLeft = new Rectangle(srcSides.Location, new Size(height2, height));
			this.srcRight = new Rectangle(srcSides.X + height2 + num, srcSides.Y, height2, height);
			this.SetImage(image);
		}
		private void SetImage(Image image)
		{
			bool flag = (byte)(this.effects) == 1;
			bool flag2 = (byte)(this.effects) == 2;
			int width = this.srcFront.Width;
			int height = this.srcFront.Height;
			int width2 = this.srcLeft.Width;
			this.top = new TexturePlane(image, flag2 ? this.srcBottom : this.srcTop, new Point3D((float)width * 0.5f, (float)width2 * 0.5f, (float)(-(float)height) * 0.5f), new Point3D(0f, 1f, 0f), this.effects & Effects.FlipHorizontally);
			this.bottom = new TexturePlane(image, flag2 ? this.srcTop : this.srcBottom, new Point3D((float)width / 2f, (float)width2 / 2f, (float)height / 2f), new Point3D(0f, -1f, 0f), this.effects & Effects.FlipHorizontally);
			this.front = new TexturePlane(image, this.srcFront, new Point3D((float)width * 0.5f, (float)height * 0.5f, (float)(-(float)width2) * 0.5f), new Point3D(0f, 0f, 1f), this.effects);
			this.back = new TexturePlane(image, this.srcBack, new Point3D((float)width * 0.5f, (float)height * 0.5f, (float)(-(float)width2) * 0.5f), new Point3D(0f, 0f, -1f), this.effects);
			this.left = new TexturePlane(image, flag ? this.srcRight : this.srcLeft, new Point3D((float)width2 * 0.5f, (float)height * 0.5f, (float)(-(float)width) * 0.5f), new Point3D(-1f, 0f, 0f), this.effects);
			this.right = new TexturePlane(image, flag ? this.srcLeft : this.srcRight, new Point3D((float)width2 * 0.5f, (float)height * 0.5f, (float)(-(float)width) * 0.5f), new Point3D(1f, 0f, 0f), this.effects);
			this.top.Viewport = this.viewport;
			this.bottom.Viewport = this.viewport;
			this.front.Viewport = this.viewport;
			this.back.Viewport = this.viewport;
			this.left.Viewport = this.viewport;
			this.right.Viewport = this.viewport;
		}
		public override float HitTest(PointF location)
		{
			float num = -1000f;
			float num2 = this.top.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = this.bottom.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = this.front.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = this.back.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = this.left.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			num2 = this.right.HitTest(location);
			if (num2 > num)
			{
				num = num2;
			}
			return num;
		}
	}
}
