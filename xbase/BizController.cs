using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.Exceptions;

namespace xbase
{

    /// <summary>
    /// 业务对象适配器
    /// </summary>
    /// <typeparam name="S">Schame 对象</typeparam>
    /// <typeparam name="C">Schame 容器</typeparam>
    /// <typeparam name="B">业务对象</typeparam>
    public abstract class BizController<S, C, B>
        where S : Schema, new()
        where C : SchemaContainer<S>
        where B : BizObject, new()
    {
        private S schema = null;
        private B buzObject = null;

        /// <summary>
        ///通过SchemaIdID构造控制器 
        /// </summary>
        /// <param name="schemaId"></param>
        protected BizController() { }
        protected void SetSchema(S schema)
        {
            this.schema = schema;
        }

        public BizController(string schemaId)
        {
            this.schema = SchemaContainer<S>.Instance().GetItem(schemaId);

            B obj = new B();
            obj.Id = schemaId;
            SetObj(obj);
            //    Initailize(buzObject);
        }

        /// <summary>
        /// 由BuzObject对象构造控制器
        /// </summary>
        /// <param name="wboSchema"></param>
        public BizController(B obj)
        {
            this.schema = SchemaContainer<S>.Instance().GetItem(obj.Id);
            SetObj(obj);
        }

        /// <summary>
        /// 用Schema构造控制器
        /// </summary>
        /// <param name="schema"></param>
        public BizController(S schema)
        {
            this.schema = schema;

            B obj = new B();
            obj.Id = schema.Id;
            SetObj(obj);
            //    Initailize(buzObject);
        }

        /// <summary>
        /// 虚方法，供子类初始化业务对象用
        /// </summary>
        /// <param name="wboSchema"></param>
        protected virtual void SetObj(B obj)
        {
            this.buzObject = obj;
        }

        /// <summary>
        /// 业务对象
        /// </summary>
        public B BuzObject
        {
            get { return buzObject; }
        }
        /// <summary>
        /// 对象的Schema
        /// </summary>
        public S Schame
        {
            get { return schema; }
        }


    }
    /// <summary>
    /// 异常类:无法打开文件
    /// </summary>
    public class Ex_CanntOpenSchameFile : XException { Ex_CanntOpenSchameFile(string schameId) { } }

}
