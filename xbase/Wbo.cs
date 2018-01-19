using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    public class Wbo
    {
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;
        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

    }

    public class ObjCatelog
    {
        private string id;
        private string path;


        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string objType;

        public string ObjType
        {
            get { return objType; }
            set { objType = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private List<ObjCatelog> children = new List<ObjCatelog>();

        public List<ObjCatelog> Children
        {
            get { return children; }
            set { children = value; }
        }
    }

}
