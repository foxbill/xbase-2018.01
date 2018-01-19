using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace xbase.umc
{
    public  class ComponentLoader
    {
        public void Load() {
            byte[] assemblyInfo = File.ReadAllBytes("dllpath");
            Assembly asm = Assembly.Load(assemblyInfo);
            Assembly asm1= Assembly.LoadFile("aa");
            object obj = asm.CreateInstance("namespace.comId", true);
        }
        
    }
}
