using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Exceptions
{
    public class PermissionException : Exception
    {
        private string loginUrl;

        public string LoginUrl
        {
            get { return loginUrl; }
        }

        public PermissionException(string loginUrl, string userId, string objectId) :
            base(userId + "用户无权访问" + objectId)
        {
            this.loginUrl = loginUrl;
        }

        public PermissionException(string loginUrl, string message)
            : base(message)
        {
            this.loginUrl = loginUrl;
        }


    }
}
