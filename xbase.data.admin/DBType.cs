using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.admin
{
    public class DatabaseType
    {
        private List<SingleDBType> types = new List<SingleDBType>();

        public List<SingleDBType> Types
        {
            get { return types; }
            set { types = value; }
        }
    }

    public class SingleDBType
    {
        private string name;        
        private string title;                
        private string description;
        
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

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
