using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wbs;
using System.Security.Policy;
using xbase.security;
using xbase.Exceptions;

namespace xbase.wbs
{
    /// <summary>
    /// Wbps 服务驱动
    /// </summary>
    public class Wbps
    {
        public const string SESSION_KEY = "";

        private Dictionary<string, Page> pages;
        private ISecurity security;
        private Page currentPage;
        private bool flowing;
        private ISession session;



        public Wbps(ISecurity security, ISession session)
        {
            this.security = security;
            this.session = session;
            pages = new Dictionary<string, Page>();
        }

        private Page getPage(WbpsResquest request, string sessionId)
        {

            string pageId = Page.getPageId(request.PageId);
            string url = request.Url;

            if (!string.IsNullOrEmpty(request.FlowId))
            {
                if (currentPage != null && string.IsNullOrEmpty(currentPage.ExecutingFlow))
                {
                    pages.Clear();
                }

                if (currentPage != null && currentPage.PageId.Equals(pageId, StringComparison.OrdinalIgnoreCase))
                    return currentPage;

                if (pages.ContainsKey(pageId))
                    return pages[pageId];
            }



            Page ret = new Page(request, sessionId, this.security,  session);

            if (currentPage == null || string.IsNullOrEmpty(currentPage.ExecutingFlow))
            {
                currentPage = ret;
            }
            else if (currentPage != null)
            {
                if (pages.ContainsKey(pageId))
                    pages[pageId] = ret;
                else
                    pages.Add(pageId, ret);
            }


            return ret;
        }

        private void removePage(string pageId)
        {
            if (pages.ContainsKey(pageId))
                pages.Remove(pageId);
        }

        public WbpsResponse invoke(WbpsResquest request, string sessionId)
        {
            WbpsResponse ret = new WbpsResponse();

            //    if (string.IsNullOrEmpty(request.FlowId))
            //        this.page = null;

            Page page = getPage(request, sessionId);

            if (!security.CheckObjectPermission("ListData", request.PageId, PermissionTypes.Read))
            {
                ret.Err = JsonExceptionUtils.ThrowErr(SecErrs.NotLogin, security.LoginPageUrl).Err;
                return ret;
            }

            ret = page.InvokeRequest(request);

            return ret;
        }

    }

    public class WbpsEvent
    {
        private string elementName;
        private string actionFlow;
        private string eventName;
        private WbpsResquest eventRequest = new WbpsResquest();

        public WbpsResquest EventRequest   //事件发生时像服务器发出的请求
        {
            get { return eventRequest; }
            set { eventRequest = value; }
        }

        private Dictionary<string, string> vars = new Dictionary<string, string>();//暂时未用

        public Dictionary<string, string> Vars
        {
            get { return vars; }
        }

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        public string ActionFlow
        {
            get { return actionFlow; }
            set { actionFlow = value; }
        }
    }

    public class WbpsResponse : JsonResponse
    {
        //客户端数据
        private Dictionary<string, string[]> elementDatas = new Dictionary<string, string[]>();

        //客户需要执行的端脚本
        private string clientScript;

        public string ClientScript
        {
            get { return clientScript; }
            set { clientScript = value; }
        }

        //客户回发的请求
        private WbpsResquest backRequest;

        //客户端需要绑定的事件
        private List<WbpsEvent> events;



        public List<WbpsEvent> Events
        {
            get { return events; }
            set { events = value; }
        }

        public WbpsResquest BackRequest
        {
            get { return backRequest; }
            set { backRequest = value; }
        }

        public Dictionary<string, string[]> ElementDatas
        {
            get { return elementDatas; }
            set { elementDatas = value; }
        }

    }


    public class WbpsResquest
    {
        private string flowId;
        private string pageId;
        private Dictionary<string, string[]> elementDatas = new Dictionary<string, string[]>();
        private Dictionary<string, string> flowVars;
        private WbapRequestSender sender = new WbapRequestSender();
        private int step = 0;
        private string url;
        private Dictionary<string, string> queryStrings = new Dictionary<string, string>();
        private string query;
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        /// <summary>
        /// 用于解析URL上面的查询字符如：http://xxx/xxxx.html?aaa=122,bbb=233中的aaa和bbb
        /// </summary>
        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        private string jsRet = "";

        public string JsRet
        {
            get { return jsRet; }
            set { jsRet = value; }
        }

        public WbapRequestSender Sender
        {
            get { return sender; }
            set { sender = value; }
        }



        public Dictionary<string, string> FlowVars
        {
            get { return flowVars; }
            set { flowVars = value; }
        }

        public Dictionary<string, string[]> ElementDatas
        {
            get { return elementDatas; }
            set { elementDatas = value; }
        }

        public string PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }

    }

}
