using System;
namespace xbase.security
{
    public interface IUserContext
    {
        string SessionId { get; set; }
        string Url { get; set; }
        string HostLoginName { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string GroupId { get; set; }

        string HeadPhoto { get; set; }

        string IP { get; set; }
    }
}
