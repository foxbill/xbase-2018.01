using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.data;
using xbase.wbs.wbdl;

namespace wbs.wbap
{
 //   public enum WbapDataType { _String = 0, _List, _Option, _Lookup, _Event }
    public enum WbapDataType { _String = 0, _List, _Option, _Lookup, _Event }

    public class WbapDataBody : Dictionary<string, object>
    {

        public static bool ElementIsDataType(string elementKey, WbapDataType wbapDataType)
        {
            bool ret = elementKey.EndsWith(wbapDataType.ToString());
            if (!ret) ret = WbapDataType._String == wbapDataType;
            return ret;
        }

        public static WbapDataType GetDataType(string elementKey)
        {
            foreach (object dataType in System.Enum.GetValues(typeof(WbapDataType)))
            {
                if (ElementIsDataType(elementKey, (WbapDataType)dataType))
                    return (WbapDataType)dataType;
            }
            return WbapDataType._String;
        }




        /// <summary>
        ///根据tableName参数，为客户端请求，建立Table的框架格式。 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="schema"></param>
        internal void ImportTableSchema(string tableName, WbdlSchema schema)
        {
            DataSourceSchema tableSchema = DataSourceSchemaContainer.Instance().GetItem(tableName);
            string keyField = tableSchema.PrimaryKeys + "_Key";
            foreach (FieldBindSchema fieldBind in schema.FieldBinds)
            {
                if (fieldBind.TableId.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                {
                    if (!this.ContainsKey(fieldBind.Id))
                        this.Add(fieldBind.Id, "");
                    //                    this.
                }
            }

            if (!this.ContainsKey(keyField))
                this.Add(keyField, "");

            foreach (DataListBindSchema listBind in schema.DataListBinds)
            {
                string listKey = listBind.Id +  WbapDataType._List.ToString();
                WbapList dataList = null;
                if (this.ContainsKey(listKey))
                {
                    dataList = this[listKey] as WbapList;
                }
                else
                {

                    dataList = new WbapList();
                    this.Add(listBind.Id +  WbapDataType._List.ToString(), dataList);
                }
                foreach (FieldBindSchema fieldBind in listBind.Columns)
                {
                    if (fieldBind.TableId.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (dataList == null)
                        {
                            dataList = new WbapList();
                            this.Add(listBind.Id +  WbapDataType._List.ToString(), dataList);
                        }
                        dataList.Columns.Add(fieldBind.Id);

                    }
                }

                dataList.Columns.Add(keyField);


            }

        }


        private string GetTypedElementId(string elementId, WbapDataType dataType)
        {
            StringBuilder sb = new StringBuilder(elementId);
            sb.Append(dataType.ToString());
            return sb.ToString();
        }

        internal void AddOption(string elementId, Dictionary<string, string> option)
        {
            string elementTypedId = GetTypedElementId(elementId, WbapDataType._Option);
            this.Add(elementTypedId, option);
        }

        internal void AddEvent(string elementId, WbapEvent wbapEvent)
        {
            string elementTypedId = GetTypedElementId(elementId, WbapDataType._Event);
            this.Add(elementTypedId, wbapEvent);
        }
    }


}


