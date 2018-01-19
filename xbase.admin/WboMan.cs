using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using xbase.tree;
using xbase.umc;
using xbase.Exceptions;
using xbase.local;
namespace xbase.admin
{
    public class WboMan
    {
        private static void RegComponent(string dllFile)
        {
            AssemblyProxy ap = new AssemblyProxy(dllFile);
            ap.regAssembly();
        }

        public static void RegWbo(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new XException("RegWbo src param " + Lang.CanNotIsNull);
            if (url.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                url = XSite.MapPath(url);
            RegComponent(url);
        }

        public static void RegWSDL(string url)
        {
            string file = XAssemblyBuilder.buildWSDL(url);
            RegComponent(file);
        }

        public static void RegDllFile()
        {
            HttpRequest req = HttpContext.Current.Request;
            for (int i = 0; i < req.Files.Count; i++)
            {
                HttpPostedFile file = req.Files[i];
                string savePath = XAssemblyBuilder.wboDllPath + Path.GetFileName(file.FileName);
                if (savePath.EndsWith(".dll"))
                {
                    file.SaveAs(savePath);
                    RegComponent(savePath);
                }
                else if (savePath.EndsWith(".cs"))
                {
                    file.SaveAs(savePath);
                    string dllPath = XAssemblyBuilder.CompileFile(savePath);
                    if (!dllPath.EndsWith(XAssemblyBuilder.errFileExt))
                        RegComponent(savePath);
                }
                else if (savePath.EndsWith(".xml"))
                {
                    file.SaveAs(savePath);
                }
                else
                    throw new XException(Lang.RegWboMustIsDll);
            }

        }


        public static void saveWboSchema(string json)
        {
            WboExplore.saveWboSchema(json);
        }

        public static void updateWboSchema(string id, WboSchema wboSchema)
        {

            if (WboSchemaContainer.Instance().Contains(id))
            {
                WboSchemaContainer.Instance().UpdateItem(id, wboSchema);
            }

        }

        public static WboSchema getWboSchema(string id)
        {
            return WboExplore.getWboSchema(id);
        }

        public static List<TreeNode> getWboNodes()
        {
            return WboExplore.getWboNodes();
        }

        public static Dictionary<string, string> getWboIds()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] xmls = WboSchemaContainer.Instance().GetIDsByFolder("");
            foreach (string id in xmls)
            {
                WboSchema ws = getWboSchema(id);
                if (ws.IsPublish)
                    ret.Add(id, ws.Title);
            }
            return ret;
        }

        public static List<TextValue> getWboTextValues()
        {
            List<TextValue> ret = new List<TextValue>();
            string[] xmls = WboSchemaContainer.Instance().GetIDsByFolder("");
            foreach (string id in xmls)
            {

                WboSchema ws = getWboSchema(id);
                if (ws.IsPublish)
                    ret.Add(new TextValue() { text = ws.Title, value = id });
            }
            return ret;
        }


        public static Dictionary<object, string> getLifeCycleOptions()
        {
            Dictionary<object, string> ret = new Dictionary<object, string>();

            LifeCycle[] values = Enum.GetValues(typeof(LifeCycle)) as LifeCycle[];
            foreach (LifeCycle value in values)
            {
                ret.Add((int)value, LifeCycleLabels.get(value));
            }
            return ret;
        }

        public static Dictionary<string, WboFieldDef> getWboSchemaFieldMap()
        {
            Dictionary<string, WboFieldDef> ret = new Dictionary<string, WboFieldDef>();
            WboFieldDef fi = new WboFieldDef();
            fi.Options = getLifeCycleOptions();
            ret.Add("LifeCycle", fi);
            return ret;
        }

        public static WboForm getWboSchemaForm(string id)
        {
            WboForm ret = new WboForm();
            ret.FieldInfos = getWboSchemaFieldMap();
            ret.Data = getWboSchema(id);
            return ret;
        }
    }
}
