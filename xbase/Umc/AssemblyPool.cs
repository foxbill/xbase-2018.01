using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.local;
using System.IO;
using xbase.Exceptions;

namespace xbase.umc
{
    public static class AssemblyPool
    {
        private static Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();

        public static Assembly getAssembly(WboSchema wboSchema)
        {
            if (!string.IsNullOrEmpty(wboSchema.AssemblyFile))
                return getAssembly(wboSchema.AssemblyFile);
            else
                return getAssembly(wboSchema.AssemblyName);
        }

        public static Assembly getAssembly(string assemblyName)
        {
            if (_assemblies.ContainsKey(assemblyName))
                return _assemblies[assemblyName];

            AssemblyProxy ap =new AssemblyProxy(assemblyName);
            Assembly asbly = ap.Assembly;
            try
            {
                _assemblies.Add(assemblyName, asbly);
            }
            catch { }
            return asbly;
            //Assembly newAssembly =
        }
    }
}
