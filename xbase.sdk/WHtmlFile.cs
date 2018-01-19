using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.wbs.wbdl;

namespace xbase.sdk
{
    public class WHtmlFile
    {
        private string fileName;
        private string data;
        private string charset = "utf-8";
        private WbdlSchema wbdl = new WbdlSchema();

        public WbdlSchema Wbdl
        {
            get { return wbdl; }
            set { wbdl = value; }
        }

        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        public bool IsNew { get; set; }

    }
}
