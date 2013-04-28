using System;
using System.Drawing;
namespace TecCraftLauncher
{
	public abstract class Object3D
	{
		private delegate void RotateMethod(float deltaX, float deltaY);
		private delegate void UpdateRotationMethod();
		private const float PIby180 = 0.0174532924f;
		protected Matrix3D originTranslation = Matrix3D.Identity;
		protected Matrix3D positionTranslation = Matrix3D.Identity;
		protected Matrix3D scaleTransformation = Matrix3D.Identity;
		protected Matrix3D localRotation = Matrix3D.Identity;
		protected Matrix3D localTransformation = Matrix3D.Identity;
		protected Matrix3D globalTransformation = Matrix3D.Identity;
		private float angle1;
		private float angle2;
		private float maxAngle1 = 3.14159274f;
		private float minAngle1 = -3.14159274f;
		private float maxAngle2 = 3.14159274f;
		private float minAngle2 = -3.14159274f;
		private RotationOrders order;
		protected MinecraftModelView viewport;
		private Object3D.RotateMethod Rotate;
		private Object3D.UpdateRotationMethod UpdateRotation;
		public abstract Image Image
		{
			set;
		}
		public float Angle1
		{
			get
			{
				return this.angle1;
			}
			set
			{
				this.angle1 = value;
				this.UpdateRotation();
			}
		}
		public float Angle2
		{
			get
			{
				return this.angle2;
			}
			set
			{
				this.angle2 = value;
				this.UpdateRotation();
			}
		}
		public float MinDegrees1
		{
			get
			{
				return this.minAngle1 / 0.0174532924f;
			}
			set
			{
				this.minAngle1 = value * 0.0174532924f;
			}
		}
		public float MinDegrees2
		{
			get
			{
				return this.minAngle2 / 0.0174532924f;
			}
			set
			{
				this.minAngle2 = value * 0.0174532924f;
			}
		}
		public float MaxDegrees1
		{
			get
			{
				return this.maxAngle1 / 0.0174532924f;
			}
			set
			{
				this.maxAngle1 = value * 0.0174532924f;
			}
		}
		public float MaxDegrees2
		{
			get
			{
				return this.maxAngle2 / 0.0174532924f;
			}
			set
			{
				this.maxAngle2 = value * 0.0174532924f;
			}
		}
		public float Scale
		{
			get
			{
				return this.scaleTransformation.M11;
			}
			set
			{
				this.scaleTransformation = Matrix3D.CreateScale(value);
				this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
			}
		}
		public RotationOrders RotationOrder
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
				switch (this.order)
				{
				case RotationOrders.XY:
					this.Rotate = new Object3D.RotateMethod(this.RotateXY);
					this.UpdateRotation = new Object3D.UpdateRotationMethod(this.UpdateRotationXY);
					return;
				case RotationOrders.YX:
					this.Rotate = new Object3D.RotateMethod(this.RotateYX);
					this.UpdateRotation = new Object3D.UpdateRotationMethod(this.UpdateRotationYX);
					return;
				case RotationOrders.XZ:
					this.Rotate = new Object3D.RotateMethod(this.RotateXZ);
					this.UpdateRotation = new Object3D.UpdateRotationMethod(this.UpdateRotationXZ);
					return;
				case RotationOrders.ZX:
					this.Rotate = new Object3D.RotateMethod(this.RotateZX);
					this.UpdateRotation = new Object3D.UpdateRotationMethod(this.UpdateRotationZX);
					return;
				default:
					return;
				}
			}
		}
		internal virtual MinecraftModelView Viewport
		{
			set
			{
				this.viewport = value;
			}
		}
		public Point3D Origin
		{
			get
			{
				return new Point3D(-this.originTranslation.M14, -this.originTranslation.M24, -this.originTranslation.M34);
			}
			set
			{
				this.originTranslation = Matrix3D.CreateTranslation(-value.X, -value.Y, -value.Z);
				this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
			}
		}
		public Point3D Position
		{
			get
			{
				return new Point3D(this.positionTranslation.M14, this.positionTranslation.M24, this.positionTranslation.M34);
			}
			set
			{
				this.positionTranslation = Matrix3D.CreateTranslation(value);
				this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
				this.Update();
			}
		}
		public Matrix3D GlobalTransformation
		{
			get
			{
				return this.globalTransformation;
			}
			set
			{
				this.globalTransformation = value;
				this.Update();
			}
		}
		public Matrix3D LocalTransformation
		{
			get
			{
				return this.localTransformation;
			}
			set
			{
				this.localTransformation = value;
				this.Update();
			}
		}
		internal abstract void Update();
		public void SetRotation(float angle1, float angle2)
		{
			this.angle1 = angle1;
			this.angle2 = angle2;
			this.UpdateRotation();
		}
		public void RotateByMouse(float deltaX, float deltaY)
		{
			if (this.Rotate != null)
			{
				this.Rotate(deltaX, deltaY);
				this.Update();
			}
		}
		private void CorrectAngles()
		{
			if (this.angle1 > this.maxAngle1)
			{
				this.angle1 = this.maxAngle1;
			}
			else
			{
				if (this.angle1 < this.minAngle1)
				{
					this.angle1 = this.minAngle1;
				}
			}
			if (this.angle2 > this.maxAngle2)
			{
				this.angle2 = this.maxAngle2;
				return;
			}
			if (this.angle2 < this.minAngle2)
			{
				this.angle2 = this.minAngle2;
			}
		}
		public abstract float HitTest(PointF location);
		private void RotateXY(float delta1, float delta2)
		{
			this.angle1 += delta1 * 0.0174532924f;
			this.angle2 += delta2 * 0.0174532924f * (float)Math.Cos((double)(this.viewport.RotationY * 0.0174532924f));
			this.UpdateRotationXY();
		}
		private void RotateYX(float delta1, float delta2)
		{
			this.angle1 += delta1 * 0.0174532924f;
			this.angle2 += delta2 * 0.0174532924f * (float)Math.Cos((double)this.viewport.RotationY * 3.1415926535897931 / 180.0);
			this.UpdateRotationYX();
		}
		private void RotateXZ(float delta1, float delta2)
		{
			this.angle1 += delta1 * 0.0174532924f * (float)Math.Cos((double)(this.viewport.RotationY * 0.0174532924f)) + delta2 * 0.0174532924f * (float)Math.Sin((double)(this.viewport.RotationY * 0.0174532924f));
			this.angle2 += delta2 * 0.0174532924f * (float)Math.Cos((double)(this.viewport.RotationY * 0.0174532924f)) - delta1 * 0.0174532924f * (float)Math.Sin((double)(this.viewport.RotationY * 0.0174532924f));
			this.UpdateRotationXZ();
		}
		private void RotateZX(float delta1, float delta2)
		{
			this.angle1 += delta1 * 0.0174532924f * (float)Math.Cos((double)(this.viewport.RotationY * 0.0174532924f)) + delta2 * 0.0174532924f * (float)Math.Sin((double)(this.viewport.RotationY * 0.0174532924f));
			this.angle2 += delta2 * 0.0174532924f * (float)Math.Cos((double)(this.viewport.RotationY * 0.0174532924f)) - delta1 * 0.0174532924f * (float)Math.Sin((double)(this.viewport.RotationY * 0.0174532924f));
			this.UpdateRotationZX();
		}
		private void UpdateRotationXY()
		{
			this.CorrectAngles();
			this.localRotation = Matrix3D.CreateRotationY(this.angle1) * Matrix3D.CreateRotationX(this.angle2);
			this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
		}
		private void UpdateRotationYX()
		{
			this.CorrectAngles();
			this.localRotation = Matrix3D.CreateRotationX(this.angle2) * Matrix3D.CreateRotationY(this.angle1);
			this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
		}
		private void UpdateRotationXZ()
		{
			this.CorrectAngles();
			this.localRotation = Matrix3D.CreateRotationZ(this.angle1) * Matrix3D.CreateRotationX(this.angle2);
			this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
		}
		private void UpdateRotationZX()
		{
			this.CorrectAngles();
			this.localRotation = Matrix3D.CreateRotationX(this.angle2) * Matrix3D.CreateRotationZ(this.angle1);
			this.localTransformation = this.positionTranslation * this.localRotation * this.originTranslation * this.scaleTransformation;
		}
	}
}
