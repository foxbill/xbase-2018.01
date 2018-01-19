using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.data.db;
using xbase.data;
using System.Data.Common;
using System.Data;

namespace xbase.message
{
    public class MessageService : HttpWbo
    {

        public void sendMessage(Message msg)
        {
            if (!Security.isFriend(msg.To))
                return;

            DatabaseAdmin dba = DatabaseAdmin.getInstance();
            ListDataRow row = new ListDataRow();
            row.Add("m_from", msg.From);
            row.Add("m_to", msg.To);
            row.Add("type", msg.Type.ToString());
            row.Add("url", msg.Url);
            row.Add("text", msg.Text);
            row.Add("send_time", DateTime.Now.ToString());
            dba.insertTableRow("message", row);
        }

        public List<Message> receiveMsg()
        {
            DatabaseAdmin dba = DatabaseAdmin.getInstance();
            DbCommand cmd = dba.getSqlStringCommand(MsgSqlScript.ReceiveSQL);
            dba.addInParameter(cmd, "@to", DbType.String, this.Security.user);
            IDataReader read = cmd.ExecuteReader();
            //            IDataReader read = dba.executeReader(cmd);
            List<Message> ret = new List<Message>();
            while (read.Read())
            {
                Message msg = new Message();
                msg.Id = read.GetInt32(0);
                msg.Type = read.GetInt16(1);
                msg.From = read.GetString(2);
                msg.To = read.GetString(3);
                msg.Text = read.GetString(4);
                msg.Url = read.GetString(5);
                msg.SendTime = read.GetString(4);
                msg.ReceiveTime = DateTime.Now.ToString();
                ret.Add(msg);
            }
            return ret;
        }

    }
}
