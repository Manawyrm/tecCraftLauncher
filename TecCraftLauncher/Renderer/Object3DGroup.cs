using System;
using System.Collections.Generic;
using System.Drawing;
namespace TecCraftLauncher
{
	public class Object3DGroup : Object3D
	{
		private List<Object3D> object3DList = new List<Object3D>();
		internal override MinecraftModelView Viewport
		{
			set
			{
				base.Viewport = value;
				foreach (Object3D current in this.object3DList)
				{
					current.Viewport = value;
				}
			}
		}
		public override Image Image
		{
			set
			{
				foreach (Object3D current in this.object3DList)
				{
					current.Image = value;
				}
			}
		}
		internal override void Update()
		{
			Matrix3D globalTransformation = this.globalTransformation * this.localTransformation;
			for (int i = 0; i < this.object3DList.Count; i++)
			{
				this.object3DList[i].GlobalTransformation = globalTransformation;
			}
		}
		public override float HitTest(PointF location)
		{
			float num = -1000f;
			foreach (Object3D current in this.object3DList)
			{
				float num2 = current.HitTest(location);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}
		public void Add(Object3D object3D)
		{
			if (object3D == this)
			{
				throw new ArgumentException("Cannot add Object3D into itself.");
			}
			this.object3DList.Add(object3D);
		}
	}
}
