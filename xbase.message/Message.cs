using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.message
{
    public class Message
    {
        public string From { get; set; }

        public string To { get; set; }

        public short Type { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }

        public int Id { get; set; }

        public string SendTime { get; set; }

        public string ReceiveTime { get; set; }

        public string ReceiveDevice { get; set; }
    }
}
