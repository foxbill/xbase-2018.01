using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.math
{
    public class ExpNode
    {

        private ExpNodeType nodeType;
        private int level;
        private string text;

        public ExpNode(string text, ExpNodeType nodeType, int level)
        {
            this.text = text;
            this.nodeType = nodeType;
            this.level = level;
        }

        public ExpNodeType NodeType
        {
            get { return nodeType; }
        }

        public int Level
        {
            get { return level; }
        }

        public string Text
        {
            get { return text; }
        }
    }
}
