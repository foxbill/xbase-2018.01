using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using xbase.umc;
using Newtonsoft.Json;
using wbs;
using xbase.wbs.wbdl;
using xbase.umc.attributes;
using System.Web;
using System.Web.SessionState;
using xbase.tree;
using xbase.local;


namespace xbase.sdk
{
    /// <summary>
    /// 站点管理工具
    /// </summary>
    [WboAttr(Id = "SiteAdmin", LifeCycle = LifeCycle.Session, Description = "站点管理工具")]
    public static class SiteAdmin 
    {
        private static string[] limitedFolders;
        private static string[] enableFileTypes;
        public const string SESSION_FOLDER = "session-catch";
        public const int DIR_TYPE = 1;
        public const int File_TYPE = 2;

        static SiteAdmin()
        {
            try
            {
                string s = ConfigurationManager.AppSettings["limitedFolders"].ToString();

                if (!string.IsNullOrEmpty(s))
                {
                    s = s.ToLower();
                    limitedFolders = s.Split('|');
                }

                s = ConfigurationManager.AppSettings["enableFileType"].ToString();
                if (!string.IsNullOrEmpty(s))
                {
                    s = s.ToLower();
                    enableFileTypes = s.Split('|');
                }
            }
            catch
            {
                throw new Exception("WebConfig AppSettings not config enableFileType or limitedFolders");
            }
        }

        //private static void ExcludeEmptyDir(ref XTreeNode tree, ref XTreeNode parent, string filter)
        //{

        //    if (tree.Nodes.Count == 0 && string.Equals(tree.Data.ToString(), filter, StringComparison.OrdinalIgnoreCase))
        //        parent.Nodes.Remove(tree);
        //    for (int i = 0; i < tree.Nodes.Count; i++)
        //    {
        //        XTreeNode node = tree.Nodes[i];
        //        if (string.Equals(node.Data.ToString(), filter, StringComparison.OrdinalIgnoreCase))
        //        {
        //            ExcludeEmptyDir(ref node, ref tree, filter);
        //            if (node.Nodes.Count == 0)
        //            {
        //                tree.Nodes.Remove(node);
        //                i--;
        //            }
        //        }
        //    }

        //}

        private static bool isHideFolder(string virPath)
        {
            virPath = virPath.Replace('\\', '/').TrimEnd('/');
            if (!virPath.StartsWith("/"))
                virPath = "/" + virPath;

            return limitedFolders.Contains(virPath.ToLower());
        }
        /// <summary>
        /// 根据制定路径，文件扩展名返目录树结构
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileExt"></param>
        /// <param name="parent"></param>
        //private void _GetPathTree(string path, string fileExt, ref TreeNode parent)
        //{
        //    string rootDir = XSite.SitePhysicalPath;
        //    int fileNameStartIndex = (rootDir + path).Length;
        //    string[] dirs = Directory.GetDirectories(rootDir + path);
        //    foreach (string dir in dirs)
        //    {
        //        FileAttributes fa = File.GetAttributes(dir);

        //        if ((fa & (FileAttributes.Hidden | FileAttributes.System)) != 0)
        //            continue;

        //        string dirName = dir.Substring(fileNameStartIndex).Trim('\\');
        //        string id = XSite.SiteVirPath + '/' + dir.Substring(rootDir.Length).Replace('\\', '/');
        //        //if (parent.HasChild(id)) continue;
        //        TreeNode dirNode = new TreeNode();

        //        dirNode.id = id;
        //        dirNode.text = dirName;
        //        dirNode.nodeType = DIR_TYPE;
        //        dirNode.path = id;
        //        dirNode.attr.Add("NodeType", "dir");
        //        if (!isHideFolder(dirNode.path))
        //            parent.children.Add(dirNode);
        //        if (path == "")
        //            _GetPathTree(dirName, fileExt, ref dirNode);
        //        else
        //            _GetPathTree(path + "\\" + dirName, fileExt, ref dirNode);

        //    }
        //    fileNameStartIndex = (rootDir + path).Length;

        //    string[] fileExts = fileExt.Split('|');

        //    foreach (string ext in fileExts)
        //    {
        //        foreach (string file in Directory.GetFiles(rootDir + path, ext, SearchOption.TopDirectoryOnly))
        //        {
        //            if (!file.EndsWith(ext.Substring(1), StringComparison.OrdinalIgnoreCase)) continue;
        //            string fileName = file;//.Remove(file.Length - ext.Length + 1);
        //            fileName = fileName.Substring(fileNameStartIndex).Trim('\\');
        //            string id = XSite.SiteVirPath + '/' + file.Substring(rootDir.Length).Replace('\\', '/');
        //            //if (parent.HasChild(id)) continue;
        //            TreeNode fileNode = new TreeNode();
        //            fileNode.id = id;
        //            fileNode.text = fileName;
        //            fileNode.nodeType = File_TYPE;
        //            fileNode.attr.Add("NodeType", "file");
        //            if (!isHideFolder(fileNode.path))
        //                parent.children.Add(fileNode);
        //        }
        //    }
        //}

        /// <summary>
        /// 获取站点文件目录结构
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        //[WboMethodAttr(Description = "获取站点文件目录树", Title = "获取站点目录")]
        //public TreeNode GetSiteFileTree(string path, string fileExt)
        //{
        //    //            string path = parama.ParaValue;
        //    //            string fileExt = parama.ParaObj.ToString().Trim('\"');
        //    if (string.IsNullOrEmpty(path))
        //        path = "";

        //    if (string.IsNullOrEmpty(fileExt))
        //        fileExt = ConfigurationManager.AppSettings["filterExtName"].ToString();

        //    TreeNode tree = new TreeNode();
        //    tree.nodeType = 1;
        //    tree.attr.Add("NodeType", "dir");
        //    tree.id = XSite.SiteVirPath + path;
        //    if (path == "/")
        //        tree.text = "站点";
        //    else
        //        tree.text = Path.GetDirectoryName(path);

        //    _GetPathTree(path, fileExt, ref tree);
        //    //        ExcludeSpecialDir(ref tree, ref tree, sb.ToString());
        //    //       ExcludeEmptyDir(ref tree, ref tree, "dir");
        //    return tree;
        //}


        //public TreeNode getSiteFiles()
        //{
        //    return GetSiteFileTree(null, null);
        //}

        public static bool fileExists(string path)
        {
            string filePhysicalPath = HttpContext.Current.Server.MapPath(path);
            return File.Exists(filePhysicalPath);
        }


        /// <summary>
        /// 保存站点html文件
        /// </summary>
        /// <param name="file"></param>
        public static void saveFile(WHtmlFile file)
        {
            string fileName = HttpContext.Current.Server.MapPath(file.FileName);
            System.IO.File.WriteAllText(fileName, file.Data, Encoding.GetEncoding(file.Charset));
        }

        /// <summary>
        /// 保存web业务页面
        /// </summary>
        /// <param name="hFile"></param>
        public static string saveWbdlHtmlFile(WHtmlFile hFile)
        {
            if (string.IsNullOrEmpty(hFile.FileName))
                throw new Exception(Lang.NoFileName);

            string fileName = HttpContext.Current.Server.MapPath(hFile.FileName);
            //if (hFile.IsNew)
            //    fileName = getCatchFileFolder() + "\\" + Path.GetFileName(fileName);

            System.IO.File.WriteAllText(fileName, hFile.Data, Encoding.GetEncoding(hFile.Charset));
            //SaveWbdl(hFile.FileName, hFile.Wbdl);

            //   fileName = fileName.Remove(fileName.IndexOf(XSite.SitePhysicalPath), XSite.SitePhysicalPath.Length);
            //   fileName = fileName.Replace('\\', '/');
            //   if (!fileName.StartsWith("/"))
            //       fileName = "/" + fileName;
            return fileName;
        }

        /// <summary>
        /// 保存页面的Wbdl配置信息
        /// </summary>
        /// <param name="pageFullName"></param>
        /// <param name="wbdl"></param>
        public static void saveWbdl(string pageFullName, WbdlSchema wbdl)
        {
            string pageFile = XSite.MapPath(pageFullName);
            string pageId = pageFile.Remove(0, XSite.SitePhysicalPath.Length);
            wbdl.Id = pageId;
            WbdlSchemaContainer.Instance().UpdateItem(pageId, wbdl);
        }

        public static WbdlSchema getWbdl(string pageFullName)
        {
            string pageFile = XSite.MapPath(pageFullName);

            string pageId = pageFile.Remove(0, XSite.SitePhysicalPath.Length);
            if (WbdlSchemaContainer.Instance().Contains(pageId))
                return WbdlSchemaContainer.Instance().GetItem(pageId);

            string oldPageId = Path.ChangeExtension(pageFile, "").TrimEnd('.');
            oldPageId = oldPageId.Remove(0, XSite.SitePhysicalPath.Length);

            if (WbdlSchemaContainer.Instance().Contains(oldPageId))
                return WbdlSchemaContainer.Instance().GetItem(oldPageId);

            return new WbdlSchema();
        }

        public static  string getCatchFileFolder()
        {
            string folder = XSite.SitePhysicalPath + SESSION_FOLDER + "\\" +HttpContext.Current.Session.SessionID;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        public static string getCatchFileUrl()
        {
            string folder = XSite.SiteVirPath + SESSION_FOLDER + "/" + HttpContext.Current.Session.SessionID;
            return folder;
        }

        public static List<TreeNode> getFileNodes()
        {
            return getFileNodes("/");
        }
        /// <summary>
        /// 获取Id指定路径的子目录和文件
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <returns></returns>
        public static List<TreeNode> getFileNodes(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = "/";
            else
                path = path.Replace("***", "/");//防止用户传入node.Id作为path

            string phyPath = (XSite.SitePhysicalPath + path).Replace('/', '\\');
            string virPath = path.Replace('\\', '/');
            if (!virPath.EndsWith("/"))
                virPath = virPath + "/";


            List<TreeNode> ret = new List<TreeNode>();
            if (isHideFolder(virPath)) return ret;

            string[] dirs = Directory.GetDirectories(phyPath);
            foreach (string dir in dirs)
            {
                string dirName = Path.GetFileName(dir);
                string dirVirPath = virPath + dirName;
                if (isHideFolder(dirVirPath)) continue;

                FileAttributes fa = File.GetAttributes(dir);
                if ((fa & (FileAttributes.Hidden | FileAttributes.System)) != 0)
                    continue;

                TreeNode dirNode = new TreeNode();
                dirNode.text = dirName;
                dirNode.path = dirVirPath;
                dirNode.id = dirVirPath.Replace("/", "***");
                dirNode.nodeType = DIR_TYPE;
                dirNode.attr.Add("NodeType", "dir");
                dirNode.state = "closed";
                ret.Add(dirNode);
            }

            foreach (string extType in enableFileTypes)
            {
                string[] files = Directory.GetFiles(phyPath, extType, SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    //if (parent.HasChild(id)) continue;
                    TreeNode fileNode = new TreeNode();
                    fileNode.text = Path.GetFileName(file);
                    fileNode.path = virPath + fileNode.text;
                    fileNode.id = fileNode.path.Replace("/", "***");
                    fileNode.nodeType = File_TYPE;
                    //fileNode.state = "open";
                    fileNode.attr.Add("NodeType", "file");
                    if (!isHideFolder(fileNode.id))
                        ret.Add(fileNode);
                }
            }

            return ret;
        }
    }

}
