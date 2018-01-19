#define DEBUG
#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    /// <summary>
    /// 日志事件声明
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="category"></param>
    /// <param name="priority"></param>
    /// <param name="evType"></param>
    public delegate void LoggingEvent(object sender, Exception e, LogCategry category, LogPriority priority, System.Diagnostics.TraceEventType evType);

    /// <summary>
    /// 日志类别
    /// </summary>
    public enum LogCategry
    {
        /// <summary>
        /// 一般
        /// </summary>
        General = 0,
        /// <summary>
        /// 信息输出
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warnning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal
    }
    /// <summary>
    /// 日志优先级
    /// </summary>
    public enum LogPriority
    {
        /// <summary>
        /// 最低
        /// </summary>
        Lowest = 0,
        /// <summary>
        /// 低
        /// </summary>
        Low,
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 高
        /// </summary>
        High,
        /// <summary>
        /// 最高
        /// </summary>
        Highest        
    }

    /// <summary>
    /// 异常发出者基类接口
    /// </summary>
    public interface IXBaseExceptionSender
    {
        /// <summary>
        /// 日志事件
        /// </summary>
        event LoggingEvent OnException;
        /// <summary>
        /// 日志记录者接口类
        /// </summary>
        IXLogger Logger
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 日志记录者接口
    /// </summary>
    public interface IXLogger
    {        
        /// <summary>
        /// 注册异常发出者
        /// </summary>
        /// <param name="sender"></param>
        void RegistService(IXBaseExceptionSender sender);
        /// <summary>
        /// 反注册异常发出者
        /// </summary>
        /// <param name="sender"></param>
        void UnRegistService(IXBaseExceptionSender sender);
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="category"></param>
        /// <param name="priority"></param>
        /// <param name="evType"></param>
        void Log(object sender, Exception e, LogCategry category, LogPriority priority, System.Diagnostics.TraceEventType evType);
        /// <summary>
        /// 取得最新的错误信息
        /// </summary>
        /// <returns></returns>
        object GetLastError();

    }
    /// <summary>
    /// XBase 异常发出者基类
    /// </summary>
    public class XBaseExceptionSenderss:IXBaseExceptionSender
    {
        private static IXLogger _Logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        public XBaseExceptionSenderss()
        {


            
        }
        /// <summary>
        /// 抛出异常并记录日志
        /// </summary>
        /// <param name="ex"></param>
        public void Throw(Exception ex)
        {
            Exception exx;            
#if DEBUG
            exx = new Exception(ex.Message + "\r\n" + ex.StackTrace);
#else
            exx = ex;
#endif
            if (OnException != null)
                OnException(this, exx, LogCategry.Error, LogPriority.High, System.Diagnostics.TraceEventType.Error);            
            //if (_Logger != null)
            //    _Logger.Log(this, exx, LogCategry.Error, LogPriority.High, System.Diagnostics.TraceEventType.Error);
            throw exx;
        }


        #region IXBaseExceptionSender 成员
        /// <summary>
        /// 异常事件
        /// </summary>
        public event LoggingEvent OnException;

        #endregion

        #region IXBaseExceptionSender 成员

        /// <summary>
        /// 日志记录者
        /// </summary>
        public IXLogger Logger
        {
            get
            {
                return _Logger;
            }
            set
            {
                _Logger = value as IXLogger;
                if (_Logger != null)
                    _Logger.RegistService(this);
            }
        }

        #endregion
    }


    /// <summary>
    /// 请求资源实体
    /// </summary>
    public class XRequest
    {
        Uri url;
        string hostaddress;
        string hostname;
        /// <summary>
        /// 资源定位标志
        /// </summary>
        public Uri Uri
        {
            get { return this.url; }
            set { this.url = value; }
        }
        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostAddress
        {
            get { return this.hostaddress; }
            set { this.hostaddress = value; }
        }
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName
        {
            get { return this.hostname; }
            set { this.hostname = value; }
        }
        /// <summary>
        /// 构造XRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="hostaddress"></param>
        /// <param name="hostname"></param>
        public XRequest(Uri url, string hostaddress, string hostname)
        {
            this.Uri = url;
            this.HostAddress = hostaddress;
            this.HostName = hostname;
        }
    }


    /// <summary>
    /// 树结构
    /// </summary>
    public class XTreeNode
    {
        private string id;
        private string name;
        private object data;
        private List<XTreeNode> nodes;
        private Dictionary<string, string> attributes = new Dictionary<string, string>();

        public Dictionary<string, string> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public XTreeNode()
        {
            nodes = new List<XTreeNode>();
        }

        /// <summary>
        /// 节点ID
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        /// <summary>
        /// 节点数据
        /// </summary>
        public object Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<XTreeNode> Nodes
        {
            get
            {
                return nodes;
            }
            set
            {
                nodes = value;
            }
        }
        /// <summary>
        /// 检测是否已包含子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasChild(string id)
        {
            foreach (XTreeNode node in Nodes)
            {
                if (string.Equals(node.ID, id, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Schema 描述对象
    /// </summary>
    public class SchemaObjectBreif
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 对象显示名称
        /// </summary>
        public string Title;
        /// <summary>
        /// 对象描述
        /// </summary>
        public string Description;

    }

}
