using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace xbase
{
    /// <summary>
    /// 持续对象（物理文件）
    /// </summary>
    public class Schema
    {
        private string id=Guid.NewGuid().ToString();
        private string title;       
        private string description;

        // Declare the delegate (if using non-generic pattern).
        public delegate void SchemaEventHandler(object sender);

        // Declare the event.
        public event SchemaEventHandler ChangedEvent;

        public void NodiftyChanged()
        {
            if (ChangedEvent != null)
            {
                ChangedEvent(this);
            }
        }

        /// <summary>
        /// 持续对象id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 对象显示标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// 对象描述信息
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }
}
