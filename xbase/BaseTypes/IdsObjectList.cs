using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;

namespace xbase.BaseTypes
{
    public class IdsObjectList<T> : List<T>
          where T : IdsObject, new()
    {
        /// <summary>
        /// 寻找对象
        /// </summary>
        /// <param name="id">对象ID（XML文件名）</param>
        /// <returns></returns>
        public T FindItem(string id)
        {
            foreach (T item in this)
            {
                if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            return null;
        }
        /// <summary>
        /// 获得对象
        /// 寻找对象，如未找到抛出异常
        /// </summary>
        /// <param name="id">对象ID（XML文件名）</param>
        /// <returns></returns>
        public T GetItem(string id)
        {
            T obj = FindItem(id);
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

            if (FindItem(id) != null)
            {
                throw (new E_ItemIdHasExsists(id));
            }


        }
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="item"></param>
        public new void Add(T item)
        {
            if (item == null) return;
            CheckIdErr(item.Id);
            base.Add(item);
        }
        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T NewItem(string id)
        {
            CheckIdErr(id);
            T schema = new T();
            schema.Id = id;
            this.Add(schema);
            return schema;
        }

        public bool ContainsId(string id)
        {
            return FindItem(id) != null;
        }
    }
}
