using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using xbase.Exceptions;

namespace xbase
{
    public static class ContainerConst
    {
        public const char NamePathChar = '/';
    }

    /// <summary>
    /// XContainer
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public class SchemaContainer<T>
          where T : Schema, new()
    {
        protected static SchemaContainer<T> instance;
        private string rootPath;
        private string fileExt = ".xml";
        private Dictionary<string, T> cache = new Dictionary<string, T>();

        /// <summary>
        /// XContainer构造函数
        /// </summary>
        protected SchemaContainer() { }
        /// <summary>
        /// XContainer构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        protected SchemaContainer(string filePath)
        {
            //this.fileExt = fileExt;
            this.rootPath = filePath;
            if (!Directory.Exists(this.rootPath))
                Directory.CreateDirectory(this.rootPath);
        }

        /// <summary>
        /// 初始化（包含扩展名）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileExt">文件扩展名</param>
        public static void Initialize(string filePath, string fileExt)
        {
            Initialize(filePath);
            instance.fileExt = fileExt;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void Initialize(string filePath)
        {
            if (instance == null)
            {
                instance = new SchemaContainer<T>(filePath);
            }
            else
            {
                throw (new EContainerHasInitialized());
            }
        }

        /// <summary>
        /// X容器实例
        /// </summary>
        public static SchemaContainer<T> Instance()
        {
            if (instance == null)
            {
                throw (new EContainerNoInitialize());
            }

            return SchemaContainer<T>.instance;
        }



        /// <summary>
        /// 反序列化获得一个X容器
        /// </summary>
        /// <param name="id">对象ID（xml文件名）</param>
        /// <returns></returns>
        public T GetItem(string id)
        {

            id = BuildId(id);
            if (cache.ContainsKey(id))
            {
                T obj = cache[id];
                return obj;
            }
            string fileName = id;
            string fullName = this.rootPath + fileName + fileExt;
            XmlReader xmlReader = XmlReader.Create(fullName);

            T ret = null;
            try
            {
                if (xmlReader == null) throw (new EContainerCanNotOpenSchameFile());
                XmlSerializer xmls = new XmlSerializer(typeof(T));
                ret = (T)xmls.Deserialize(xmlReader);
                try
                {
                    cache.Add(id, ret);
                }
                catch
                {
                }
            }
            catch (Exception e)
            {
                throw new Exception("打开文件" + fileName + "时发生错误." + e.Message, e);
            }
            finally
            {
                xmlReader.Close();
            }

            return ret;

        }

        private static string BuildId(string id)
        {
            if (id.EndsWith("#"))
                id = id.Remove(id.Length - 1);
            if (string.IsNullOrEmpty(id))
            {
                throw (new EObjectNameCanNotNull());
            }
            id = id.Replace("\\", ContainerConst.NamePathChar + "");
            id = id.TrimStart(ContainerConst.NamePathChar);
            // id = id.ToLower();
            return id;
        }

        /// <summary>
        /// 判断文件是否在目录中
        /// </summary>
        /// <param name="id">对象ID（xml文件名）</param>
        /// <returns></returns>
        public bool Contains(string id)
        {
            id = BuildId(id);

            string fileName = id;
            DirectoryInfo dir = new DirectoryInfo(rootPath);
            bool ret = File.Exists(rootPath + fileName + fileExt);
            return ret;
            //FileInfo[] files = dir.GetFiles("*"+ fileExt);
            //foreach (FileInfo file in files)
            //{
            //    if (fileName.Equals(file.Name))
            //        return true;
            //}
            //return false;
        }

        /// <summary>
        /// 新建一个XML（先判断是否存在后进行序列化生成XML）
        /// </summary>
        /// <param name="id">对象ID（xml文件名）</param>
        /// <returns></returns>
        public T NewItem(string id)
        {
            id = BuildId(id);
            if (Contains(id))
            {
                throw (new EObjectHasExists(id));
            }

            T ret = new T();

            ret.Id = GetFileName(id);
            AddItem(id, ret);

            return ret;
        }

        private string GetFileName(string id)
        {
            id = BuildId(id);
            int idx = id.LastIndexOf(ContainerConst.NamePathChar);
            if (idx > 0)
                return id.Substring(idx + 1, id.Length - idx);
            return id;
        }
        /// <summary>
        /// 反序列化XML
        /// </summary>
        /// <param name="relativePath">相对路径，不含扩展名的XML文件名 Example: folder/filename</param>
        /// <param name="wboSchema">X容器实体</param>
        public void AddItem(string id, T obj)
        {
            id = BuildId(id);
            if (cache.ContainsKey(id))
            {
                throw (new EObjectHasExists(id));
            }

            obj.Id = id;

            if (string.IsNullOrEmpty(obj.Id))
            {
                //                string[] a = relativePath.Split(ContainerConst.NamePathChar);
                //                wboSchema.Id = a.Last<string>();
                throw new EObjectNameCanNotNull();
            }

            if (Contains(id))
            {
                throw (new EObjectHasExists(id));
            }

            BuildFolder(id);


            XmlWriter writer = XmlWriter.Create(this.rootPath + id + fileExt);
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(T));
                xmls.Serialize(writer, obj);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                writer.Close();
            }
            cache.Add(id, obj);


        }

        private void BuildFolder(string id)
        {
            id = BuildId(id);

            string[] paths = id.Split(ContainerConst.NamePathChar);
            if (paths.Length > 0)
            {
                string dir = this.rootPath;
                for (int i = 0; i < paths.Length - 1; i++)
                {
                    dir += (string.IsNullOrEmpty(paths[i]) || paths[i].Length == 0) ? paths[i] : ContainerConst.NamePathChar + paths[i];
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                }
            }
        }

        /// <summary>
        /// 更新实体及XML
        /// </summary>
        /// <param name="originalId"></param>
        /// <param name="wboSchema"></param>
        public void UpdateItem(string originalId, T obj)
        {
            //originalId = originalId.ToLower();
            originalId = BuildId(originalId);
            if (Contains(originalId))
            {
                T item = GetItem(originalId);
                item.NodiftyChanged();
            }

            if (string.IsNullOrEmpty(originalId))
                throw new XException("在更新对象配置时，必须提供对象的原始Id");

            if (!obj.Id.Equals(originalId, StringComparison.OrdinalIgnoreCase))
                if (Contains(obj.Id))
                    throw new XException("在更新对象配置时，新的Id已经被其他对象使用，请重新输入新的Id");


            T oldObj = null;

            if (!cache.ContainsKey(originalId))
            {
                cache.Add(originalId, obj);
            }
            else
            {
                oldObj = cache[originalId];
                cache[originalId] = obj;
            }



            DeleteItem(originalId);
            AddItem(obj.Id, obj);


            if (oldObj != null)
            {
                oldObj.NodiftyChanged();
            }

            //obj.Id = originalId;

            //if (!cache.ContainsKey(originalId))
            //{
            //    cache.Add(originalId, obj);
            //}
            //else
            //{
            //    oldObj = cache[originalId];
            //    cache[originalId] = obj;
            //}
            //string file = this.rootPath + originalId.ToLower() + fileExt;

            //if (Contains(originalId))
            //{
            //    File.Copy(file, file + ".bak", true);
            //    File.Delete(file);
            //}

            //try
            //{
            //    XmlWriter writer = XmlWriter.Create(file);
            //    try
            //    {
            //        XmlSerializer xmls = new XmlSerializer(typeof(T));
            //        xmls.Serialize(writer, obj);
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
            //    finally
            //    {
            //        writer.Close();
            //    }
            //    if (File.Exists(file + ".bak"))
            //        File.Delete(file + ".bak");
            //}
            //catch (Exception e)
            //{
            //    if (File.Exists(file + ".bak"))
            //        File.Copy(file + ".bak", file, true);
            //    throw (e);
            //}
            //if (oldObj != null)
            //{
            //    oldObj.NodiftyChanged();
            //}
        }

        /// <summary>
        /// 删除实体及XML
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="wboSchema"></param>
        public void DeleteItem(string id, T obj)
        {
            id = BuildId(id);
            if (string.IsNullOrEmpty(obj.Id))
                throw (new EObjectNameCanNotNull());
            string file = this.rootPath + id + fileExt;
            if (Contains(id))
                File.Delete(file);
            if (!cache.ContainsKey(id))
                return;
            try
            {
                cache.Remove(id);
            }
            catch (Exception)
            {
                throw (new EContainerCanNotOpenSchameFile());
            }
        }

        /// <summary>
        /// 删除实体及XML
        /// </summary>
        /// <param name="id"></param>
        public void DeleteItem(string id)
        {
            id = BuildId(id);
            if (Contains(id))
            {
                T item = GetItem(id);
                item.NodiftyChanged();
            }

            if (string.IsNullOrEmpty(id))
                throw (new EObjectNameCanNotNull());
            if (cache.ContainsKey(id))
                cache.Remove(id);

            try
            {
                string file = this.rootPath + id + fileExt;
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch (Exception e)
            {
                throw (new EContainerCanNotOpenSchameFile());
            }
        }

        /// <summary>
        /// 返回所有的ID
        /// </summary>
        /// <returns></returns>
        public string[] GetSchemaIds()
        {
            try
            {
                string[] Ids = Directory.GetFiles(this.rootPath, "*" + fileExt, SearchOption.AllDirectories);
                int fileNameStartIndex = this.rootPath.Length;

                for (int i = 0; i < Ids.Length; i++)
                {
                    Ids[i] = Ids[i].Substring(fileNameStartIndex);
                    Ids[i] = Ids[i].Remove(Ids[i].Length - 4);
                }

                return Ids;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回指定路径下的一级子目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetSchemaFolders(string path)
        {
            try
            {
                string[] Ids = Directory.GetDirectories(this.rootPath + path);
                return Ids;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回指定路径下(不含子目录)的所有xml文件名（ID）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetIDsByFolder(string path)
        {
            try
            {
                string[] Ids = Directory.GetFiles(this.rootPath + path, "*" + fileExt, SearchOption.TopDirectoryOnly);
                int fileNameStartIndex = this.rootPath.Length;

                for (int i = 0; i < Ids.Length; i++)
                {
                    Ids[i] = Ids[i].Substring(fileNameStartIndex);
                    Ids[i] = Ids[i].Remove(Ids[i].Length - 4);
                }

                return Ids;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据目录返回对象描述列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SchemaObjectBreif[] GetObjectBreifByFolder(string path)
        {
            try
            {
                string[] Ids = Directory.GetFiles(this.rootPath + path, "*" + fileExt, SearchOption.TopDirectoryOnly);
                int fileNameStartIndex = this.rootPath.Length;
                List<SchemaObjectBreif> schemaBrfList = new List<SchemaObjectBreif>();
                for (int i = 0; i < Ids.Length; i++)
                {
                    Ids[i] = path + Ids[i].Substring(fileNameStartIndex);
                    Ids[i] = Ids[i].Remove(Ids[i].Length - 4);
                    Schema schema = GetItem(Ids[i]);
                    if (schema == null) continue;
                    SchemaObjectBreif sf = new SchemaObjectBreif();
                    sf.ID = schema.Id;
                    sf.Title = schema.Title;
                    sf.Description = schema.Description;
                    schemaBrfList.Add(sf);
                }
                return schemaBrfList.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 返回指定目录下的目录树结构
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        public void GetSchemaTree(string path, ref XTreeNode parent)
        {
            int fileNameStartIndex = (this.rootPath + path).Length;
            foreach (string dir in Directory.GetDirectories(this.rootPath + path))
            {
                string dirName = dir.Substring(fileNameStartIndex).Trim('\\');
                XTreeNode dirNode = new XTreeNode();
                dirNode.ID = dir.Substring(this.rootPath.Length);
                dirNode.Name = dirName;
                dirNode.Data = "dir";
                parent.Nodes.Add(dirNode);
                if (path == "")
                    GetSchemaTree(dirName, ref dirNode);
                else
                    GetSchemaTree(path + "\\" + dirName, ref dirNode);
            }

            fileNameStartIndex = (this.rootPath + path).Length;

            foreach (string file in Directory.GetFiles(this.rootPath + path, "*.xml", SearchOption.TopDirectoryOnly))
            {
                string fileName = file.Remove(file.Length - 4);
                fileName = fileName.Substring(fileNameStartIndex).Trim('\\');
                XTreeNode fileNode = new XTreeNode();
                fileNode.ID = file.Substring(this.rootPath.Length);
                fileNode.Name = fileName;
                fileNode.Data = "file";
                parent.Nodes.Add(fileNode);
            }
        }

    }
}
