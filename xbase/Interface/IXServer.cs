using System;
namespace xbase.Interface
{
    public interface IXServer
    {
        string MapPath(string url);
        string[] MapPath(string[] values);
        void SetHostName(string hostName);
    }
}
