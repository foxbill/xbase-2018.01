using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.bi.schema
{


    public class DataDocSchema:xbase.Schema
    {
        private string author;
        private string editor;
        private string version;
        private DateTime createDate;
        private xbase.SchemaList<SubjectSchema> subjects=new xbase.SchemaList<SubjectSchema>();

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public xbase.SchemaList<SubjectSchema> Subjects
        {
            get { return subjects; }
            set { subjects = value; }
        } 
    }
}
