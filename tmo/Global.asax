<%@ Application Language="C#" %>
<%@ Import Namespace="xbase.data" %>
<%@ Import Namespace="xbase.wbs" %>
<%@ Import Namespace="xbase.wbs.wbdl" %>
<%@ Import Namespace="xbase.umc" %>
<%@ Import Namespace="xbase.bi.schema" %>
<%@ Import Namespace="xbase" %>
<%@ Import Namespace="xbase.security" %>
<%@ Import Namespace="xbase.web.app.xj_service" %>
<%@ Import Namespace="XSecurityHandler" %>
<%@ Import Namespace="xbase.Interface" %>
<%@ Import Namespace="xbase.sdk" %>
<%@ Import Namespace="xbase.web.app.pdfdoc" %>
<%@ Import Namespace="xbase.Validation" %>
<%@ Import Namespace="xbase.data.Validation" %>
<%@ Import Namespace="xbase.security" %>
<%@ Import Namespace="xbase.math" %>
<%@ Import Namespace="Common.Logging" %>
<%@ Import Namespace="xbase.web" %>
<%@ Import Namespace="xbase.admin" %>
<%@ Import Namespace="xbase.data.wbc" %>
<%@ Import Namespace="xbase.data.transfer" %>
<script Language="C#" RunAt="server">
    private static ILog log = LogManager.GetLogger("Logger");

    private void Session_Start(object sender, EventArgs e)
    {

        //   HttpApplication application = sender as HttpApplication;
        //  HttpApplicationState appState = application.Application;
        //  HttpSessionState Session = application.Context.Session;
        //   HttpRequest Request = application.Request;
        // Session["Started"] = "started";
        // log.Debug("XSessionModule Session_Start SessionID:" + Session.SessionID);
        //   XSite.GetSession(Session.SessionID);
        // XSite.SetHostName(Request.Url.Scheme + "://" + Request.Url.Authority);
    }

    private void Session_End(object sender, EventArgs e)
    {
        //  HttpApplication application = sender as HttpApplication;

        //  if (application.Context.Session != null)
        // {
        //   HttpSessionState Session = application.Context.Session;

        //     Umc.RemoveSession(Session.SessionID);
        //  XSite.RemoveSession(Session.SessionID);
        //  log.Debug("XSessionModule Session_End SessionID:" + Session.SessionID);
        //  }


    }

    protected void Application_Start(object sender, EventArgs e)
    {
        //  XBaseRuntime.initialize(HttpRuntime.AppDomainAppPath, HttpRuntime.AppDomainAppVirtualPath);
        // string commanyPath = ConfigurationManager.AppSettings["SchemaPath"].ToString();
        //string commanyPath = ConfigurationSettings.AppSettings["SchemaPath"].ToString();
        string sitePath = HttpRuntime.AppDomainAppPath;
        //            string app_DataPath = Server.MapPath(APP_DATA);
        string app_DataPath = sitePath + "App_Data";
        string reportPath = app_DataPath;


        //            string sitePath = Server.MapPath("/");
        // IXServer iXServer = new XHttpServer(Server);
        XSite.Open(Server);

        //   commanyPath = app_DataPath + commanyPath;
        log.Debug("XSite Open");
        log.Debug("SitePath is " + sitePath);

        string commanyPath = XSite.AppSchemaPath;
        log.Debug("commanyPath is " + commanyPath);
        //    string app_DataPath = XSite.AppDataServerPath;
        //    string sitePath = XSite.SitePhysicalPath;
        DataSourceSchemaContainer.Initialize(commanyPath + "Table\\");
        WbdlSchemaContainer.Initialize(commanyPath + "Form\\");
        DataDocSchemaContainer.Initialize(commanyPath + "DataDoc\\");
        ChartShemaContainer.Initialize(commanyPath + "chart\\");
        // Umc.Initialize(commanyPath + "umc\\");
        OperatorFactory.InitualFile(app_DataPath + "\\operator.xml");

        ValidatorSchemaContainer.Initialize(app_DataPath + "\\Validators\\");

        // XMenuSchemaContainer.Initialize(schemaPath + "Menu\\");
        string dataVirPath = HttpRuntime.AppDomainAppVirtualPath;
        if (dataVirPath.EndsWith("/"))
            dataVirPath += "DataFile";
        else
            dataVirPath += "/DataFile";
        //     XDatabaseFactory.Initialize(commanyPath + XDatabaseFactory.XML_FILE, sitePath + "DataFile", dataVirPath);
        //     XDIOParser.Initialize(sitePath, HttpRuntime.AppDomainAppVirtualPath);

        //bill
        //      XDataSetSchemaContainer.Initialize(app_DataPath + "DataSet\\");

        //     _Loger = new XLogging.XLoggingService();

        //XReportManager.Initialize(reportPath + "\\ReportingServer\\");
        //     Umc.RegisterClass(typeof(XMenu.XMenu), "xMenu");
        WboRegService.RegisterClass(typeof(DataSource), "Ds");
        //    Umc.RegisterClass(typeof(xbase.data.admin.DBManager), "DbManager");
        WboRegService.RegisterClass(typeof(xbase.bi.DataDoc), "DataDoc");
        WboRegService.RegisterClass(typeof(xbase.bi.ChartAdmin), "ChartAdmin");
        WboRegService.RegisterClass(typeof(xbase.bi.XChart), "XChart");
        WboRegService.RegisterClass(typeof(xbase.sdk.WboAdmin), "WboAdmin");
        WboRegService.RegisterClass(typeof(UserAccount), "UserAccount");
        WboRegService.RegisterClass(typeof(RoleManager), "RoleManager");
        WboRegService.RegisterClass(typeof(xbase.wbs.Page), "Page");
        WboRegService.RegisterClass(typeof(SiteAdmin), "SiteAdmin");
        WboRegService.RegisterClass(typeof(DatadocPdf), "DatadocPdf");
        WboRegService.RegisterClass(typeof(xbase.sdk.ValidationAdmin), "ValidationAdmin");
        WboRegService.RegisterClass(typeof(sms.SSMPort), "SSMPort");
        WboRegService.RegisterClass(typeof(xbase.web.controls.DataChart), "DataChart");
        WboRegService.RegisterClass(typeof(xbase.sdk.controls.WbcBox), "WbcBox");
        WboRegService.RegisterClass(typeof(xbase.admin.WboMan), "WboMan");
        WboRegService.RegisterClass(typeof(xbase.data.DataExplore), "DataExplore");
        WboRegService.RegisterClass(typeof(xbase.data.DsExplore), "DsExplore");
        WboRegService.RegisterClass(typeof(xbase.web.UploadHandler), "UploadHandler");
        WboRegService.RegisterClass(typeof(Security));
        WboRegService.RegisterClass(typeof(VDataTable));
        WboRegService.RegisterClass(typeof(ParameterDirectionCaptions));
        WboRegService.RegisterClass(typeof(DbTypeCaptions));
        WboRegService.RegisterClass(typeof(ExcelTransfer));
        WboRegService.RegisterClass(typeof(xbase.data.ui.DataForm));
        WboRegService.RegisterClass(typeof(xbase.weixin.MpMenuManage));
        WboRegService.RegisterClass(typeof(xbase.weixin.ContractTest));

        ValidatorFactory.RegisterClass(typeof(NullValidator));
        ValidatorFactory.RegisterClass(typeof(ScopeValidator));
        ValidatorFactory.RegisterClass(typeof(DateTimeValidator));
        ValidatorFactory.RegisterClass(typeof(DbExpressionValidator));
        xbase.data.db.DatabaseAdmin dba = xbase.data.db.DatabaseAdmin.getInstance();
    }
    
</script>
