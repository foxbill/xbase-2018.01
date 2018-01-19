using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace xbase.Exceptions
{

    public class JsonResponse
    {
        private JRespErr err;

        public JRespErr Err
        {
            get { return err; }
            set { err = value; }
        }

        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class JRespErr
    {
        int no;
        string text;
        string url;

        public int No
        {
            get { return no; }
            set { no = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string ErrStack { get; set; }
    }


}
