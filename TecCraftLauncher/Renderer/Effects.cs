using System;
namespace TecCraftLauncher
{
	[Flags]
	public enum Effects : byte
	{
		None = 0,
		FlipHorizontally = 1,
		FlipVertically = 2
	}
}
