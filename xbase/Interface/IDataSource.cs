using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.Interface
{
    /// <summary>
    /// 字段基本摘要信息
    /// </summary>
    public class FieldInfo
    {
        public string Title = "";
        public string Name = "";
    }

    /// <summary>
    ///对象通知请求数据句柄 
    /// </summary>
    /// <param name="sender"></param>
    /// <returns></returns>
    public delegate bool DataSourceSetDataNotifty(IDataSource sender);

    /// <summary>
    /// 通知外部程序更新指定字段的数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    public delegate void DataSourceSetFieldNotify(IDataSource sender, string fieldFolder);

    /// <summary>
    /// 数据源接口类型
    /// </summary>
    public interface IDataSource
    {

        void SetFieldData(string fieldName, string[] fieldData);
        string[] GetFieldData(string fieldName);

        void SaveData();
        void RefreshData();

        Dictionary<string, WboFieldDef> GetFieldInfos();
        event DataSourceSetDataNotifty OnSetData;
        event DataSourceSetFieldNotify OnSetField;

    }

    /// <summary>
    /// 回话数据对象，可接收到Session信息
    /// </summary>
    public interface SessionDataSource
    {
    }

}
