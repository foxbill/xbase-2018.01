using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc.attributes;
using System.IO;
using xbase.tree;
using xbase.data;
using System.Web;

namespace xbase.netdisk
{
    public class NetDisk : HttpWbo
    {
        private static string rootPhyPath = XSite.SitePhysicalPath + "cloud-disk";
        private static string rootVirPath = "/cloud-disk";
        //private static string 

        private string getPhysicalPath(string path)
        {
            return rootPhyPath + path;
        }

        public void deletePath(string path)
        {

        }

        public void deleteFile(string path, string name)
        {

        }

        public string getFileUrl(string path, string name)
        {
            if (string.IsNullOrEmpty(path))
                path = "/";
            path = path.Replace('\\', '/');
            return rootVirPath + path + name;
        }

        public void upload(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = "\\";

            string phyPath = getPhysicalPath(path);

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile file = Request.Files[i];
                //HttpPostedFile file = Request.Files[fld];
                if (file != null)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    fileName = phyPath + fileName;
                    file.SaveAs(fileName);
                }
            }
        }

        public void newFolder(string path, string folderName)
        {
            if (string.IsNullOrEmpty(path))
                path = "\\";
            string phyPath = getPhysicalPath(path);
            phyPath += folderName;
            Directory.CreateDirectory(phyPath);
        }

        public ListData listData(string path)
        {
            ListData ret = new ListData();
            ret.total = 0;
            ret.rows = getFiles(path);
            return ret;
        }

        public List<ListDataRow> getFiles(string path)
        {
            if (string.IsNullOrEmpty(path))
                path = "\\";
            string physicalPath = getPhysicalPath(path);

            List<ListDataRow> ret = new List<ListDataRow>();
            foreach (string file in Directory.GetFiles(physicalPath))
            {
                string fileName = Path.GetFileName(file);

                ListDataRow row = new ListDataRow();
                row.Add("name", fileName);
                row.Add("createTime", File.GetCreationTime(file).ToString());
                row.Add("lastAccess", File.GetLastAccessTime(file).ToString());
                row.Add("lastWrite", File.GetLastWriteTime(file).ToString());
                ret.Add(row);
            }
            return ret;
        }

        private void _GetPathTree(string path, ref TreeNode parent, bool onlyFolder = false)
        {
            if (string.IsNullOrEmpty(path))
                path = "\\";

            string physicalPath = getPhysicalPath(path);

            string[] dirs = Directory.GetDirectories(physicalPath);
            foreach (string dir in dirs)
            {
                FileAttributes fa = File.GetAttributes(dir);

                if ((fa & (FileAttributes.Hidden | FileAttributes.System)) != 0)
                    continue;

                string dirName = Path.GetFileName(dir);
                string id = path + dirName + "\\"; //XSite.SiteVirPath + '/' + dir.Substring(rootDir.Length).Replace('\\', '/');
                //if (parent.HasChild(id)) continue;
                TreeNode dirNode = new TreeNode();

                dirNode.id = id;
                dirNode.name = dirName;
                dirNode.text = dirName;
                dirNode.title = dirName;
                dirNode.iconCls = "icon-save";// "tree-folder";


                dirNode.path = id;
                dirNode.attr.Add("type", "dir");
                parent.children.Add(dirNode);
                _GetPathTree(path + dirName + "\\", ref dirNode, onlyFolder);

            }

            if (onlyFolder)
                return;

            foreach (string file in Directory.GetFiles(physicalPath))
            {
                string fileName = Path.GetFileName(file);//.Remove(file.Length - ext.Length + 1);

                string id = path + fileName;
                TreeNode fileNode = new TreeNode();
                fileNode.id = id;

                fileNode.path = path;
                fileNode.name = fileName;
                fileNode.label = fileName;
                fileNode.title = fileName;
                fileNode.text = fileName;
                fileNode.attr.Add("type", "file");
                parent.children.Add(fileNode);
            }
        }

        /// <summary>
        /// 获取站点文件目录结构
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        [WboMethodAttr(Description = "获取站点文件目录树", Title = "获取站点目录")]
        public TreeNode getFileTree(string path)
        {
            TreeNode tree = new TreeNode();
            tree.attr.Add("type", "dir");
            //            tree.Attributes.Add("NodeType", "dir");
            tree.id = "\\";
            tree.path = "\\";
            tree.name = "企业云盘";
            tree.label = "企业云盘";
            tree.text = "企业云盘";
            tree.title = "企业云盘";
            tree.iconCls = "tree-folder";
            if (!Directory.Exists(rootPhyPath))
                Directory.CreateDirectory(rootPhyPath);
            _GetPathTree(path, ref tree);
            return tree;
        }

        public TreeNode getFolderTree(string path)
        {
            TreeNode tree = new TreeNode();
            tree.attr.Add("type", "dir");
            //            tree.Attributes.Add("NodeType", "dir");
            tree.id = "\\";
            tree.path = "\\";
            tree.name = "企业云盘";
            tree.label = "企业云盘";
            tree.text = "企业云盘";
            tree.title = "企业云盘";
            tree.iconCls = "tree-folder";
            if (!Directory.Exists(rootPhyPath))
                Directory.CreateDirectory(rootPhyPath);
            _GetPathTree(path, ref tree, true);
            return tree;
        }


    }
}
