using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace TecCraftLauncher
{
    public class TexturePlane : Object3D
    {
  
            private List<Texel> texelList = new List<Texel>();
            internal PointF[,] Points;
            internal double[,] ZOrder;
            internal bool IsVisible = true;
            private bool[,] visibility;
            private Bitmap bitmap;
            private bool flipHorizontally;
            private bool flipVertically;
            private int width;
            private int height;
            private Point3D normal;
            public override Image Image
            {
                set
                {
                    this.Bitmap = (Bitmap)value;
                }
            }
            internal override MinecraftModelView Viewport
            {
                set
                {
                    base.Viewport = value;
                    if (this.bitmap != null && value != null)
                    {
                        this.UpdateBitmap();
                    }
                }
            }
            private Bitmap Bitmap
            {
                set
                {
                    if (this.viewport == null)
                    {
                        this.bitmap = value;
                        return;
                    }
                    this.texelList.Clear();
                    if (this.bitmap != null)
                    {
                        this.viewport.RemoveTexelsOf(this);
                        this.Points = null;
                    }
                    this.bitmap = value;
                    if (this.bitmap != null)
                    {
                        this.UpdateBitmap();
                        this.Update();
                    }
                }
            }
            internal override void Update()
            {
                if (this.Points == null || this.viewport == null)
                {
                    return;
                }
                Matrix3D m = this.globalTransformation * this.localTransformation * this.originTranslation;
                for (int i = 0; i <= this.width; i++)
                {
                    for (int j = 0; j <= this.height; j++)
                    {
                        Point3D point3D = m * new Point3D((float)i, (float)j, 0f);
                        this.Points[i, j] = this.viewport.Point3DTo2D(point3D);
                        double num = (double)this.viewport.GetZOrder(point3D);
                        this.ZOrder[i, j] += num;
                        this.ZOrder[i + 1, j] += num;
                        this.ZOrder[i, j + 1] += num;
                        this.ZOrder[i + 1, j + 1] = num;
                    }
                }
            }
            private void UpdateBitmap()
            {
                this.width = this.bitmap.Width;
                this.height = this.bitmap.Height;
                this.visibility = new bool[this.width, this.height];
                for (int i = 0; i < this.width; i++)
                {
                    for (int j = 0; j < this.height; j++)
                    {
                        Color pixel = this.bitmap.GetPixel(i, j);
                        int num = this.flipHorizontally ? (this.width - i - 1) : i;
                        int num2 = this.flipVertically ? j : (this.height - j - 1);
                        if (pixel.A == 0)
                        {
                            this.visibility[num, num2] = false;
                        }
                        else
                        {
                            this.visibility[num, num2] = true;
                            Texel texel = new Texel(this, num, num2, pixel);
                            this.viewport.AddTexel(texel);
                            this.texelList.Add(texel);
                        }
                    }
                }
                this.Points = new PointF[this.width + 1, this.height + 1];
                this.ZOrder = new double[this.width + 2, this.height + 2];
            }
            public TexturePlane(Image bitmap, Rectangle srcRect, Point3D origin, Point3D normal, Effects effects)
            {
                base.Origin = origin;
                this.normal = normal;
                if (bitmap == null)
                {
                    this.Bitmap = null;
                    return;
                }
                Bitmap bitmap2 = new Bitmap(srcRect.Width, srcRect.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), srcRect, GraphicsUnit.Pixel);

                }
                this.flipHorizontally = ((byte)(effects & Effects.FlipHorizontally) == 1);
                this.flipVertically = ((byte)(effects & Effects.FlipVertically) == 2);
                this.Bitmap = bitmap2;
            }
            public override float HitTest(PointF location)
            {
                if (this.Points == null)
                {
                    return -1000f;
                }
                GraphicsPath graphicsPath = new GraphicsPath();
                graphicsPath.AddPolygon(new PointF[]
			{
				this.Points[0, 0],
				this.Points[this.Points.GetLength(0) - 1, 0],
				this.Points[this.Points.GetLength(0) - 1, this.Points.GetLength(1) - 1],
				this.Points[0, this.Points.GetLength(1) - 1]
			});
                Region region = new Region(graphicsPath);
                if (region.IsVisible(location))
                {
                    for (int i = 0; i < this.Points.GetLength(0) - 1; i++)
                    {
                        for (int j = 0; j < this.Points.GetLength(1) - 1; j++)
                        {
                            if (this.visibility[i, j])
                            {
                                graphicsPath.Reset();
                                graphicsPath.AddPolygon(new PointF[]
							{
								this.Points[i, j],
								this.Points[i + 1, j],
								this.Points[i + 1, j + 1],
								this.Points[i, j + 1]
							});
                                if (graphicsPath.IsVisible(location))
                                {
                                    return (this.globalTransformation * this.localTransformation * this.originTranslation * new Point3D((float)i, (float)j, 0f)).Z;
                                }
                            }
                        }
                    }
                }
                return -1000f;
            }
        }
    }
