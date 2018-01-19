using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase
{

    /// <summary>
    /// 持续列表对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BizObjectList<T> : List<T>
        where T : BizObject, new()
    {
        /// <summary>
        /// 寻找对象
        /// </summary>
        /// <param name="id">对象ID（XML文件名）</param>
        /// <returns></returns>
        public T FindObject(string id)
        {
            foreach (T obj in this)
            {
                if (obj.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return obj;
            }
            return null;
        }
        /// <summary>
        /// 获得对象
        /// 寻找对象，如未找到抛出异常
        /// </summary>
        /// <param name="id">对象ID（XML文件名）</param>
        /// <returns></returns>
        public T GetObject(string id)
        {
            T obj = FindObject(id);
            if (obj == null)
            {
                throw (new EPeresisListNoItemOfId(id));
            }
            return obj;
        }

        private void CheckIdErr(string id)
        {
            if (id == null || id.Equals(""))
            {
                throw (new E_ItemIdCanNotNull(""));
            }

            if (FindObject(id) != null)
            {
                throw (new E_ItemIdHasExsists(id));
            }


        }
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="wboSchema"></param>
        public new void Add(T obj)
        {
            if (obj == null) return;
            CheckIdErr(obj.Id);
            base.Add(obj);
        }
        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T NewObject(string id)
        {
            CheckIdErr(id);
            T obj = new T();
            obj.Id = id;
            this.Add(obj);
            return obj;
        }
    }
}
