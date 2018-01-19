using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class PaginationInfo
    {
        public int total = 100;
        public int pageCount = 1;
        public int page = 1;
        public int pageSize = 15;

        public bool isStoreProcessPagination { get; set; }
    }
}
