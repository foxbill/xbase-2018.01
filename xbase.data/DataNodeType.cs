using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public enum DataNodeType
    {
        NONE,//0
        TABLE,//1
        VIEW,//2
        SP,//3
        TRIGGER,//4
        CONSTRAINT,//5
        DB,//6
        TABLE_FOLDER,//7
        VIEW_FOLDER,//8
        SP_FOLDER,//9
        TRIGGER_FOLDER,//10
        CONSTRAINT_FOLDER//11
    }
}
