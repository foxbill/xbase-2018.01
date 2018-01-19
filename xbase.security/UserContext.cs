using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;

namespace xbase.security
{
    /// <summary>
    /// 用户上下文信息实体
    /// </summary>
    public class UserContext : HttpWbo, IUserContext
    {
        private string userId;
        private string userName;
        private string sessionid;
        private string url;
        private string groupId;
        private string hostLoginName;

        public string HostLoginName
        {
            get { return hostLoginName; }
            set { hostLoginName = value; }
        }

        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        Dictionary<string, object> attributes;

        public UserContext(string userId)
        {
            attributes = new Dictionary<string, object>();
            this.userId = userId;
        }

        public Dictionary<string, object> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id
        {
            get { return this.userId; }
            set { this.userId = value; }
        }
        /// <summary>
        /// 用户显示名
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(userName))
                    return userId;
                return userName;
            }
            set { userName = value; }
        }

        /// <summary>
        /// 会话标志
        /// </summary>
        public string SessionId
        {
            get { return this.sessionid; }
            set { this.sessionid = value; }
        }
        /// <summary>
        /// 资源定位标志
        /// </summary>
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }
    }
    /// <summary>
    /// 用户上下文信息UserContext容器类，静态
    /// </summary>
    public static class UserContextContainer
    {
        //   public static Hashtable UserContextList = new Hashtable();
        /// <summary>
        /// UserContext对象Dictionary
        /// </summary>
        private static Dictionary<string, UserContext> UserContextList = new Dictionary<string, UserContext>();
        /// <summary>
        /// 添加UserContext
        /// </summary>
        /// <param name="objUserContext"></param>
        public static void AddUserContext(UserContext objUserContext)
        {
            if (UserContextList.ContainsKey(objUserContext.SessionId))
                objUserContext = UserContextList[objUserContext.SessionId] = objUserContext;
            else
                UserContextList.Add(objUserContext.SessionId, objUserContext);
        }
        /// <summary>
        /// 根据会话标志sessionId取得用户上下文信息UserContext
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static UserContext GetUserContextBySessionId(string sessionId)
        {
            UserContext objUserContext = null;
            if (UserContextList.ContainsKey(sessionId))
                objUserContext = UserContextList[sessionId];
            return objUserContext;
        }
        /// <summary>
        /// 删除UserContext
        /// </summary>
        /// <param name="sessionId"></param>
        public static void RemoveContextBySessionId(string sessionId)
        {
            if (UserContextList.ContainsKey(sessionId))
                UserContextList.Remove(sessionId);
        }
    }
}
