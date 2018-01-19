using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.message
{

    internal sealed class MsgSqlScript
    {
        internal const string ReceiveSQL =
          @"SELECT TOP 20 [id]
              ,[type]
              ,[m_from]
              ,[m_to]
              ,[text]
              ,[url]
              ,[send_time]
              ,[receive_time]
            FROM [message]
            WHERE m_to=@to or m_to='all' ";
    }
}
