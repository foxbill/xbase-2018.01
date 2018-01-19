using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace xbase.umc.com
{
    public class ComAppProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFullName">"要加载的dll文件的全路径"</param>
        public static void loadApp(string assemblyFullName)
        {

            System.Reflection.Assembly outlookAssembly = System.Reflection.Assembly.LoadFile(assemblyFullName);

            Type appType1 = outlookAssembly.GetType("Microsoft.Office.Interop.Outlook.Application");
            Type appType = appType1.GetInterface("_Application");

            var coClassAttr = (System.Runtime.InteropServices.CoClassAttribute)appType1.GetCustomAttributes(typeof(System.Runtime.InteropServices.CoClassAttribute), false)[0];
            var coClass = coClassAttr.CoClass;

            // Create outlook application.
            var app = Activator.CreateInstance(coClassAttr.CoClass);
            //自此创建_Application接口成功。

        }

        private static void CreateReader(string rdsrvName)
        {
            throw new NotImplementedException();
            //CoRDSrv result = new CoRDSrv();
            //string clsid = getGUIDbyDrvname(rdsrvName);
            //string progid = getProgIDbyDrvname(rdsrvName);
            //result.getCardName(rdsrvName);
            //object objDoc = null;

            //clsid = clsid.Substring(1, clsid.Length - 2);
            //try
            //{
            //    try
            //    {
            //        objDoc = Marshal.GetActiveObject(progid);
            //        IntPtr ip = Marshal.GetIDispatchForObject(objDoc);
            //        ComInst = Marshal.AddRef(ip);
            //        System.Windows.Forms.MessageBox.Show(ComInst.ToString());
            //        result._FReader = Marshal.GetObjectForIUnknown(ip) as IIDReader;
            //        //result._FReader = objDoc as IIDReader）;


            //    }
            //    catch (Exception ex) { objDoc = null; }
            //    if (objDoc == null)
            //    {
            //        Type m_Com_Document = Type.GetTypeFromCLSID(new Guid(clsid), false);
            //        objDoc = Activator.CreateInstance(m_Com_Document);
            //        result._FReader = objDoc as IIDReader;
            //        //isNew = true;
            //    }
            //}
            //catch { }
            //return result;

        }

        static void test(string[] args)
        {
            object[] oParams = new object[] { "leafyoung" };

            object oComObj = Activator.CreateInstance(
              Type.GetTypeFromProgID("ReflectionCOM.TestObj"));

            object rez = oComObj.GetType().InvokeMember("SayHello",
               BindingFlags.InvokeMethod,
               null,
               oComObj,
               oParams);

            Console.WriteLine(rez);
        }

        private void Invoke()
        {
            //try
            //{
            //    DataSet ds = new DataSet();

            //    Type objType;
            //    object objBinding;

            //    objType = Type.GetTypeFromProgID("CSGPDBAccess.CSGPDBAccess");
            //    objBinding = Activator.CreateInstance(objType);

            //    Type[] paramTypes = new Type[] { Type.GetType("System.String"), Type.GetType("System.Int32"), Type.GetType("System.String[]&") };

            //    MethodInfo m = objType.GetMethod("openProcedure", paramTypes);
            //    object[] args = new object[3];
            //    args[0] = "Test";
            //    args[1] = 1;
            //    args[2] = new string[] { "0052005" };

            //    ds = (DataSet)m.Invoke(objBinding, args);

            //    if (ds.Tables.Count > 0)
            //    {
            //        dataGrid1.DataSource = ds.Tables[0].DefaultView;
            //    }
            //}
            //catch (TargetInvocationException ee)
            //{
            //    MessageBox.Show(ee.Message);
            //}

        }

    }
}
