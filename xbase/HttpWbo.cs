using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.security;
using xbase.umc;
using System.Web;
using System.Web.SessionState;

namespace xbase
{
    public class HttpWbo : Wbo, IHttpWbo
    {
        private ISecurity security;
        private HttpContext context;
        private Umc umc;

        protected ISecurity Security
        {
            get { return umc.Security; }
        }


        protected Umc Umc
        {
            get { return umc; }

        }


        protected HttpContext Context
        {
            get { return umc.Context; }
        }


        protected HttpRequest Request
        {
            get
            {
                return umc.Context.Request;
            }
        }
        protected HttpServerUtility Server
        {
            get
            {
                return umc.Context.Server;
            }
        }

        protected HttpSessionState Session
        {
            get
            {
                return umc.Context.Session;
            }
        }

        protected HttpResponse Response
        {
            get
            {
                return umc.Context.Response;
            }
        }


        public virtual void setContext(Umc umc)
        {
            this.umc = umc;
        }
    }
}
