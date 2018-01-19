using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Validation;
using xbase.Exceptions;

namespace xbase.Exceptions
{
    /// <summary>
    /// 服务器错误编号
    /// </summary>
    public enum ServerErrs
    {
        ServerException = 1
    }

    /// <summary>
    /// 安全访问错误编号
    /// </summary>
    public enum SecErrs
    {
        NotLogin = 100,
        NotPemission
    }

    /// <summary>
    ///安全访问错误信息 
    /// </summary>
    public static class SecErrMsgs
    {
        static string[] msgs =
        {
            "用户没登录，无法访问这个资源",
            "用户没有权限访问资源"
        };

        public static string GetMessage(SecErrs index)
        {
            return msgs[(int)index - 100];
        }
    }

    /// <summary>
    /// 错误处理工具类
    /// </summary>
    public static class JsonExceptionUtils
    {

        public static JsonResponse ThrowErr(SecErrs errNo, string loginUrl)
        {
            JRespErr err = new JRespErr();
            err.No = (int)errNo;
            err.Text = SecErrMsgs.GetMessage(errNo);
            err.Url = loginUrl;

            JsonResponse ret = new JsonResponse();
            ret.Err = err;
            return ret;
        }


        public static JsonResponse ThrowErr(int no, string message, string errUrl)
        {
            JRespErr err = new JRespErr();
            err.No = no;
            err.Text = message;
            err.Url = errUrl;

            JsonResponse ret = new JsonResponse();
            ret.Err = err;
            return ret;
        }

        public static JsonResponse ThrowErr(Exception exception)
        {
            JRespErr err = new JRespErr();
            err.No = (int)ServerErrs.ServerException;
            JsonResponse ret = new JsonResponse();

            if (exception == null)
            {
                err.Text = "未知错误";
                err.Url = null;

                ret.Err = err;
                return ret;

            }
            if (exception is XUserException)
            {
                err.Text = exception.Message;
                err.Url = null;

                ret.Err = err;
                return ret;
            }



            if (exception is EValidateException)
            {
                err.Text = "输入验证错误：" + exception.Message;
                err.Url = null;

                ret.Err = err;
                return ret;
            }

            string s = exception.Message + "\n";
            if (exception.InnerException != null)
                s += exception.InnerException.Message;

            if (exception.StackTrace != null)
                err.ErrStack = exception.StackTrace.ToString() + "\n";

            if (exception.InnerException != null)
            {
                if (exception.InnerException.StackTrace != null)
                    err.ErrStack += exception.InnerException.StackTrace.ToString() + "\n";
            }

            err.Text = s;
            err.Url = null;


            ret.Err = err;
            return ret;
        }

    }

}
