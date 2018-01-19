using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class ObjectDocket
    {
        private string name = "";
        private string title = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }

    public class DbCategory
    {
        private string err;
        private string database;
        private string databaseTitle;

        private List<ObjectDocket> tables = new List<ObjectDocket>();
        private List<ObjectDocket> views = new List<ObjectDocket>();


        public string Err
        {
            get { return err; }
            set { err = value; }
        } 

        public string DbTitle
        {
            get { return databaseTitle; }
            set { databaseTitle = value; }
        }

        public string DbName
        {
            get { return database; }
            set { database = value; }
        }

        public List<ObjectDocket> Tables
        {
            get { return tables; }
        }

        public List<ObjectDocket> Views
        {
            get { return views; }
        }
    }
}
