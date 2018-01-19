using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Exceptions;
using xbase.Interface;
using System.Web;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using xbase.local;
using xbase.umc;

namespace xbase
{
    public static class XSite
    {
        private const string APP_DATA_DIR = "App_Data";
        private const string BIN_DIR = "bin";
        private const string DATA_FILES = "datafiles";

        private static string _sitePhysicalPath;
        private static string _siteVirPath;
        private static Dictionary<string, object> objects = new Dictionary<string, object>();
        private static bool isStart = false;
        private static string schemaPath;
        //       private static Dictionary<string, XSession> sessions = new Dictionary<string, XSession>();
        private static HttpServerUtility _server;


        public static string DataFileVirPath
        {
            get { return "/" + DATA_FILES + "/"; }
        }
        public static string DataFilePath
        {
            get
            {
                string path = SitePhysicalPath + "\\" + DATA_FILES + "\\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }


        /// <summary>
        /// 打开站点
        /// </summary>
        /// <param name="sitePath"></param>
        /// <param name="server"></param>
        public static void Open(HttpServerUtility server)
        {

            //            string app_DataPath = Server.MapPath(APP_DATA);


            //            string sitePath = Server.MapPath("/");
            if (isStart) return;

            WboRegService.RegisterClass(typeof(Register));

            _server = server;

            _siteVirPath = HttpRuntime.AppDomainAppVirtualPath;


            _sitePhysicalPath = HttpRuntime.AppDomainAppPath;
            if (!_sitePhysicalPath.EndsWith("\\"))
                _sitePhysicalPath += "\\";


            schemaPath = _sitePhysicalPath + "App_Data\\";

            if (!_sitePhysicalPath.EndsWith("\\"))
                _sitePhysicalPath += "\\";

            _server = server;

            isStart = true;
        }


        //public static ISession GetSession(string sessionId)
        //{
        //    XSession ret = null;
        //    if (!sessions.ContainsKey(sessionId))
        //    {
        //        try
        //        {
        //            sessions.Add(sessionId, new XSession(sessionId));
        //        }
        //        catch { }
        //    }
        //    ret = sessions[sessionId];
        //    return ret;
        //}

        //public static void RemoveSession(string sesssionId)
        //{
        //    sessions.Remove(sesssionId);
        //}

        public static string SitePhysicalPath
        {
            get
            {
                if (!isStart) throw new EAppNotStart("SystemNotStart");
                if (!_sitePhysicalPath.EndsWith("\\"))
                    _sitePhysicalPath += "\\";
                return XSite._sitePhysicalPath;
            }
        }

        public static string SiteVirPath
        {
            get
            {
                if (!isStart) throw new EAppNotStart("SystemNotStart");
                return XSite._siteVirPath;
            }
        }


        public static Dictionary<string, object> Objects
        {
            get { return XSite.objects; }
        }

        public static string AppSchemaPath
        {
            get
            {
                return schemaPath;
            }
        }

        public static string AppDataServerPath
        {
            get
            {
                return SitePhysicalPath + APP_DATA_DIR + "\\";
            }
        }

        public static string BinServerPath
        {
            get
            {
                return SitePhysicalPath + BIN_DIR + "\\";
            }
        }

        public static string MapPath(string VirPath)
        {
            return _server.MapPath(VirPath);
        }

        public static string DataSourceTypeId
        {
            get
            {
                return "ds";
            }
        }


        public static string WboPath
        {
            get
            {
                return AppDataServerPath + "wbo\\";
            }
        }
    }
}
