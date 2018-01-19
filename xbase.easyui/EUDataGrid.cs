using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.data.easyui;
using xbase.data.db;
using xbase.data;

namespace xbase.easyui
{
    public static class EUDataGrid
    {
       public static EasyUiGridData  createGrid(string connName,string tableName){
           DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);
           TableDef tableDef = dba.getTableDef(tableName);
           string title=string.IsNullOrEmpty( tableDef.Title)?tableName:tableDef.Title;
           return EUGridUtils.getGrid(connName, title, tableDef.FieldDefs);
       }
    }
}
