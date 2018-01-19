using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using xbase;

namespace xbase.data
{
    public class WebTableSchema : Schema
    {
        private string updateCommand;

        public string UpdateCommand
        {
            get { return updateCommand; }
            set { updateCommand = value; }
        }
        private string selectCommand;

        public string SelectCommand
        {
            get { return selectCommand; }
            set { selectCommand = value; }
        }
        private string insertCommand;

        public string InsertCommand
        {
            get { return insertCommand; }
            set { insertCommand = value; }
        }
    //    private string deleteCommand;
     //   private int pageSize=50;
    }

    public class WebDataSchema
    {
        private Dictionary<string, WebDataSchema> tabls = new Dictionary<string, WebDataSchema>();
    }


    public class WebDataSet
    {
        private DataSet ds;
      //  private DataRelation dl;
        public WebDataSet()
        {
         //   dl = new DataRelation("r1", "ptable", "dtable", "pf1,pf2,", "df1,df2", false);
             //dataSet.Relations.Add(
            ds.Tables[1].Rows[1].GetChildRows("r1");
        }
        
    }
}
