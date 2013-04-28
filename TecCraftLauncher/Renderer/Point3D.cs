using System;
namespace TecCraftLauncher
{
	public struct Point3D
	{
		public float X;
		public float Y;
		public float Z;
		public static Point3D Zero
		{
			get
			{
				return default(Point3D);
			}
		}
		public Point3D(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.X,
				";",
				this.Y,
				";",
				this.Z,
				")"
			});
		}
		public static Point3D operator +(Point3D a, Point3D b)
		{
			return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		public static Point3D operator -(Point3D a, Point3D b)
		{
			return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		public static Point3D operator *(Point3D p, float s)
		{
			return new Point3D(p.X * s, p.Y * s, p.Z * s);
		}
		public static Point3D operator /(Point3D p, float s)
		{
			return new Point3D(p.X / s, p.Y / s, p.Z / s);
		}
	}
}
