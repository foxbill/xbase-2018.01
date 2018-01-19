using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using xbase;

namespace xbase.security
{
    //public static class UserContextContainer
    //{
    //    //   public static Hashtable UserContextList = new Hashtable();
    //    public static Dictionary<string, UserContext> UserContextList = new Dictionary<string, UserContext>();

    //    public static void AddUserContext(UserContext objUserContext)
    //    {
    //        if (UserContextList.ContainsKey(objUserContext.SessionId))
    //            objUserContext = UserContextList[objUserContext.SessionId] = objUserContext;
    //        else
    //            UserContextList.Add(objUserContext.SessionId, objUserContext);
    //    }

    //    public static UserContext GetUserContextBySessionId(string sessionId)
    //    {
    //        UserContext objUserContext = null;
    //        if (UserContextList.ContainsKey(sessionId))
    //            objUserContext = UserContextList[sessionId];
    //        return objUserContext;
    //    }
    //}
}
