using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class UserInfo
{
    public string userName { get; set; }
    public string macCode { get; set; }
    public DateTime installTime { get; set; }
    public DateTime accreditTime { get; set; }
    
    /// <summary>
    /// 授权级别
    /// accreditLevel=1 限制20天试用期
    /// accreditLevel=1 无限期使用,无登录用户显示
    /// </summary>
    public int accreditLevel { get; set; }
    public int userCount { get; set; }
}

