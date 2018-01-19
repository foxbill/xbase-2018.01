
using System;
using System.Data.Common;
using System.Data;
namespace xbase.data
{
    interface DsAdapter
    {
        void delete(System.Collections.Generic.List<ListDataRow> rows, System.Collections.Generic.Dictionary<string, string> realParams);
        void delete(ListDataRow row, System.Collections.Generic.Dictionary<string, string> realParams);
        void executeCommandSchema(CommandSchema commandSchema, System.Collections.Generic.List<ListDataRow> rows, System.Collections.Generic.Dictionary<string, string> realParams, bool refresh = false);
        void executeCommandSchema(CommandSchema commandSchema, ListDataRow row, System.Collections.Generic.Dictionary<string, string> realParams, bool refresh = false);
        DbCommand getCommand(CommandSchema cmdSchema);
        DataSet getDataSet();
        DataSet getDataSet(System.Collections.Generic.Dictionary<string, string> _queryParams, string where, string orderBy, string groupBy, PaginationInfo pi);
        void insert(System.Collections.Generic.List<ListDataRow> rows, System.Collections.Generic.Dictionary<string, string> realParams);
        void insert(ListDataRow row, System.Collections.Generic.Dictionary<string, string> realParams);
        DataSourceSchema schema { get; }
        void update(System.Collections.Generic.List<ListDataRow> insertRows, System.Collections.Generic.List<ListDataRow> updateRows, System.Collections.Generic.List<ListDataRow> deleteRows, System.Collections.Generic.Dictionary<string, string> realParams);
        void update(System.Collections.Generic.List<ListDataRow> rows, System.Collections.Generic.Dictionary<string, string> realParams);
        void update(ListDataRow row, System.Collections.Generic.Dictionary<string, string> realParams);
    }
}
