using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase
{
    public class DataSchema
    {
        private List<DataSchema> subFolder;
//        private string  
    }

    public interface ObjSerialor
    {
        void SetData(string dataName,string data);
        string  GetData(string dataName, string data);
        string GetSchema(); 
    }
}
  