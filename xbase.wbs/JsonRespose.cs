using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs
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
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(this);
        }
    }

    public class JRespErr
    {
        int errNo;
        string errText;
        string errUrl;

        public int ErrNo
        {
            get { return errNo; }
            set { errNo = value; }
        }

        public string ErrText
        {
            get { return errText; }
            set { errText = value; }
        }

        public string ErrUrl
        {
            get { return errUrl; }
            set { errUrl = value; }
        }
    }


}
