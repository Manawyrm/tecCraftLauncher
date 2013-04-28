using System;
using System.Drawing;
namespace TecCraftLauncher
{
	internal struct Texel
	{
		internal TexturePlane TexturePlane;
		internal int X;
		internal int Y;
		private Color color;
		private Brush brush;
		private Pen pen;
		internal double Z
		{
			get
			{
				return this.TexturePlane.ZOrder[this.X + 1, this.Y + 1];
			}
		}
		internal Texel(TexturePlane texturePlane, int x, int y, Color color)
		{
			this.TexturePlane = texturePlane;
			this.X = x;
			this.Y = y;
			this.color = color;
			this.brush = new SolidBrush(color);
			this.pen = new Pen(Color.White, 0.01f);
		}
		internal void Draw(Graphics g)
		{
			PointF[] points = new PointF[]
			{
				this.TexturePlane.Points[this.X, this.Y],
				this.TexturePlane.Points[this.X + 1, this.Y],
				this.TexturePlane.Points[this.X + 1, this.Y + 1],
				this.TexturePlane.Points[this.X, this.Y + 1]
			};
			g.FillPolygon(this.brush, points);
		}
	}
}
