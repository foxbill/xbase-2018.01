using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    /// <summary>
    /// 在Xbase构架中的所有异常的顶层异常，XBase中所有的异常必须继承这个异常
    /// </summary>
    public class XException : Exception
    {
        private int errNo;

        /// <summary>
        /// 构造函数
        /// </summary>
        public XException() : base() { }


        public XException(string messsage, params object[] var) : base(string.Format(messsage, var)) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messsage">错误信息</param>
        public XException(string messsage) : base(messsage) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errNo">错误号</param>
        public XException(int errNo)
            : base()
        {
            this.errNo = errNo;
        }

        /// <summary>
        /// 错误号
        /// </summary>
        public int ErrNo
        {
            get { return errNo; }
        }
    }
    /// <summary>
    /// 错误捕捉：容器没有初始化
    /// </summary>
    public class EContainerNoInitialize : XException { }
    /// <summary>
    /// 错误捕捉：容器已经初始化
    /// </summary>
    public class EContainerHasInitialized : XException { }
    /// <summary>
    /// 错误捕捉：容器不能打开XML文件
    /// </summary>
    public class EContainerCanNotOpenSchameFile : XException { }
    /// <summary>
    /// 错误捕捉：对象名为空
    /// </summary>
    public class EObjectNameCanNotNull : XException
    {
        public EObjectNameCanNotNull()
            : base("保存对象时，不允许对象ID为空")
        {
        }
    }
    /// <summary>
    /// 错误捕捉：对象已存在
    /// </summary>
    public class EObjectHasExists : XException
    {
        public EObjectHasExists(string objectId)
            : base(objectId + "对象已经存在")
        {
        }
    }
    /// <summary>
    /// 错误捕捉：列表对象没有发现ID
    /// </summary>
    public class EPeresisListNoItemOfId : XException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public EPeresisListNoItemOfId(string message) : base(message) { }
    }
}
