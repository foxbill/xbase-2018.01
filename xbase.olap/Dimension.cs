using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.olap
{
    public class Dimension
    {
        public string name { get; set; }
        public string foreignKey { get; set; }
        public DimensionType type { get; set; }
        public List<Hierarchy> Hierarchies;
    }
}
