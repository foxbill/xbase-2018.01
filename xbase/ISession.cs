using System;
namespace xbase
{
    public interface ISession
    {
        string Id { get; }
        object this[string objKey] { get; set; }
        string getCatchFileFolder();
    }
}
