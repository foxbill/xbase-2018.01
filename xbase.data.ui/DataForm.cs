using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.local;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Data;


namespace xbase.data.ui
{
    public class DataForm : HttpWbo
    {
        private string _dsName;
        private DataSource _ds;
        private int _rowFieldCount = 4;
        private int _colWidth = 200;

        private ListDataRow _row;
        private List<FieldSchema> fromItems;
        private List<FieldSchema> imageFields;
        private List<FieldSchema> memoFields;


        private DataSource getDataSourse()
        {
            if (_ds != null)
                return _ds;

            if (string.IsNullOrEmpty(_dsName))
            {
                //if (!DataSourceSchemaContainer.Instance().Contains(name))
                    _dsName = name;
            }

            _ds = new DataSource(_dsName);
            return _ds;
        }

        public DataForm(string name)
        {
            this.name = name;
        }

        public string dsName
        {
            get { return _dsName; }
            set { _dsName = value; }
        }

        private void getFormItems(DataSourceSchema dss)
        {
            fromItems = new List<FieldSchema>();
            imageFields = new List<FieldSchema>();
            memoFields = new List<FieldSchema>();
            foreach (FieldSchema fld in dss.Fields)
            {
                if (fld.ExtendType == DataExtendType.ImageFile)
                    imageFields.Add(fld);
                else if (fld.ExtendType == DataExtendType.Html)
                {
                    memoFields.Add(fld);
                }
                else if (fld.IsInForm)
                    fromItems.Add(fld);

            }
        }

        public string draw(string dsName)
        {
            if (!string.IsNullOrEmpty(dsName))
                this._dsName = dsName;

            DataSource ds = getDataSourse();
            if (ds == null)
                throw new Exception(Lang.DataSourceNameIsNull);


            DataSourceSchema dss = ds.getSchema();

            StringBuilder sb = new StringBuilder();
            //<form method="post" enctype="multipart/form-data" id="frmTransfer" action="">
            sb.Append(@"<form method=""post"" enctype=""multipart/form-data"" action=""DataForm.");
            sb.Append(name);
            sb.Append(@".post.wbo"" ");
            sb.Append(@" id=""");
            sb.Append(name);
            sb.Append(@""" name=""");
            sb.Append(name);
            sb.Append(@""">");
            sb.Append("<table border='0'>");
            sb.Append("<tr>");
            sb.Append("<td colspan='");
            sb.Append(_rowFieldCount * 2);
            sb.Append("'>");
            sb.Append(dss.Title);
            sb.Append("</td>");
            sb.Append("</tr>");
            getFormItems(dss);
            int i = 0;
            while (i < fromItems.Count)
            {
                sb.Append("<tr>");
                for (int c = 0; c < this._rowFieldCount; c++)
                {
                    FieldSchema fss = null;
                    if (i < fromItems.Count)
                        fss = fromItems[i];


                    string value = "";
                    if (fss != null && _row != null && _row.Count > 0)
                        if (_row.ContainsKey(fss.Id))
                            value = _row[fss.Id];

                    sb.Append(@"<td style=""padding:1px;text-align: left;"">");
                    if (fss != null)
                    {
                        sb.Append("<label>");
                        string title = string.IsNullOrEmpty(fss.Title) ? fss.Id : fss.Title;
                        sb.Append(title);
                        sb.Append(":</label>");
                    }
                    else
                    {
                        sb.Append("&nbsp;");
                    }

                    sb.Append(@"</td><td style=""padding:1px 10px 1px 1px"">");

                    if (fss != null)
                    {
                        sb.Append(@"<input type=""text""");
                        sb.Append(@" name=""");
                        sb.Append(fss.Id);
                        sb.Append(@"""");
                        sb.Append(@" value=""");
                        sb.Append(value);
                        sb.Append(@""" />");
                    }
                    else
                    {
                        sb.Append("&nbsp;");
                    }
                    sb.Append("</td>");
                    i++;
                }
                sb.Append("</tr>");
            }

            //添加备注
            i = 0;
            while (i < memoFields.Count)
            {
                sb.Append("<tr>");
                sb.Append("<td colspan='");
                sb.Append(_rowFieldCount * 2);
                sb.Append("'>");
                FieldSchema fld = memoFields[i];
                string value = _row == null ? "" : _row[fld.Id];
                string title = string.IsNullOrEmpty(fld.Title) ? fld.Id : fld.Title;
                sb.Append(title);
                sb.Append("<br/>");
                sb.Append(@"<textarea style='width:");
                sb.Append(_rowFieldCount * _colWidth);
                sb.Append("px;height:");
                sb.Append(80);
                sb.Append("px;'");
                sb.Append(@" name=""");
                sb.Append(fld.Id);
                sb.Append(@""">");
                sb.Append(value);
                sb.Append(@"</textarea> ");
                sb.Append("</td>");
                sb.Append("</tr>");
                i++;
            }

            //添加图像
            i = 0;
            while (i < imageFields.Count)
            {
                sb.Append("<tr>");
                for (int c = 0; c < _rowFieldCount; c++)
                {
                    sb.Append("<td colspan='2'>");
                    if (i < imageFields.Count)
                    {
                        FieldSchema fld = imageFields[i];
                        string value = _row == null ? "" : _row[fld.Id];
                        string title = string.IsNullOrEmpty(fld.Title) ? fld.Id : fld.Title;
                        sb.Append(title);
                        sb.Append(@"<input type=""file"" ");
                        sb.Append(@" class=""easyui-filebox"" ");
                        sb.Append(@" name=""file_");
                        sb.Append(fld.Id);
                        sb.Append(@""" value=""""/> ");
                        sb.Append("<br/>");
                        sb.Append(@"<img src=""");
                        sb.Append(value);
                        sb.Append(@""" width=""200px""  /> ");

                        sb.Append(@"<input type=""hidden"" ");
                        sb.Append(@" name=""");
                        sb.Append(fld.Id);
                        sb.Append(@""" value=""");
                        sb.Append(value);
                        sb.Append(@""" />");
                    }
                    else
                        sb.Append("&nbsp");

                    sb.Append("</td>");
                    i++;
                }
                sb.Append("</tr>");
            }

            //sb.Append("<tr>");
            //sb.Append("<td colspan='");
            //sb.Append(_rowFieldCount * 2);
            //sb.Append("'>");
            //sb.Append(@"<input type=""button"" ");
            //sb.Append(@" onclick=""");
            //sb.Append(@"$('#");
            //sb.Append(name);
            //sb.Append(@"').submit()"" ");
            //sb.Append(@" value=""保存""/> ");
            //sb.Append("</td>");
            //sb.Append("</tr>");
            sb.Append("</table>");
            foreach (string fld in dss.PrimaryKeys)
            {
                //fld = XSqlBuilder.OLD_VERSION_PIX + fld;
                string value = _row == null || _row.Count < 1 ? "" : _row[XSqlBuilder.OLD_VERSION_PIX + fld];
                sb.Append(@"<input type=""hidden""  name=""");
                sb.Append(XSqlBuilder.OLD_VERSION_PIX);
                sb.Append(fld);
                sb.Append(@""" value=""");
                sb.Append(value);
                sb.Append(@"""/>");
            }
            sb.Append("</form>");

            //            sb.Append(@"<script language=""javascript"" type=""text/javascript"">");
            //            sb.Append(@"$('");
            //            sb.Append(name);
            //            sb.Append(@"').({
            //                success:function(data){
            //                  alert('记录保存')
            //                }
            //            })");
            //sb.Append("</script>");
            return sb.ToString();

        }

        public string edit(Dictionary<string, string> pk)
        {
            DataSource ds = getDataSourse();
            _row = ds.row(pk);
            return draw(null);
        }

        public int RowFieldCount
        {
            get { return _rowFieldCount; }
            set { _rowFieldCount = value; }
        }



        public string post()
        {
            ListDataRow row = new ListDataRow();
            foreach (string fld in this.Request.Form.AllKeys)
            {
                row.Add(fld, Request.Form[fld]);
            }

            DataSource ds = getDataSourse();

            //  bool isNew = DataSource.isNewRow(row);
            //  ds.updateRow(row);

            DataSourceSchema dss = ds.getSchema();
            foreach (string fld in Request.Files.Keys)
            {

                HttpPostedFile file = Request.Files[fld];
                if (file != null && file.ContentLength > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);
                    fileName = ds.getFieldFolder(fld) + fileName;
                    file.SaveAs(fileName);
                    string virPath = XSite.DataFileVirPath + fileName.Remove(0, XSite.DataFilePath.Length).Replace("\\", "/");
                    //row.Add(fld, virPath);
                    string fname = fld;
                    if (fname.StartsWith("file_"))
                        fname = fld.Remove(0, 5);
                    row[fname] = virPath;
                }
            }
            //  if (isNew)
            ds.updateRow(row);
            _row = row;

            return draw(null);
        }

    }


}













