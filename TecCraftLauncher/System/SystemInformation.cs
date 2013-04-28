using System;
using System.Collections.Generic;
using System.Text;


namespace TecCraftLauncher
{
    class SystemInformation
    {
       public static int GetArchitecture()
       {
           return System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) * 8;
       }

    }
}
