using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestClass
{
    public class ClassEcho
    {
        private string name = "孙凰翔";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string company = "甜蜜窝";

        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private int age = 22;

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        private string sex = "男";

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        private string department = "研发部";

        public string Department
        {
            get { return department; }
            set { department = value; }
        }

        public string echo(string name)
        {
            this.name = name;
            return "I'm " + name;
        }

    }
}
