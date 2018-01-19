using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Collections;
using System.Web;
using xbase.umc.attributes;
using xbase.umc;
using xbase.local;
using xbase.utility;
using xbase.data.db;
using System.Text.RegularExpressions;
using System.Data;
using xbase.data.easyui;
using xbase.Exceptions;
using System.Transactions;
using System.Globalization;





namespace xbase.data.transfer
{
    [WboAttr(Description = "Excel文件传输", Title = "Excel文件传输", LifeCycle = LifeCycle.Session)]
    public class ExcelTransfer : HttpWbo
    {
        public const string ExcelVirPath = "/ExcelTransferFile/";
        private const int TYPE_TEST_COUNT = 100;

        private List<ColumnDef> _columnMaps = new List<ColumnDef>();
        private string _fileName;
        private string _connName;
        private string _tableName;
        private int _headRowNum = 0;
        private int _dataRowNum = 1;

        private ISheet _sheet;
        private HSSFWorkbook hssfworkbook;
        private int _page;
        private int _pageSize;
        private int _total;


        public string tableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
        public string connName
        {
            get { return _connName; }
            set { _connName = value; }
        }

        public int dataRowNo
        {
            get { return _dataRowNum; }
            set { _dataRowNum = value; }
        }

        public int headRowNo
        {
            get { return _headRowNum; }
            set { _headRowNum = value; }
        }

        public string fileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public List<ColumnDef> ColumnMaps
        {
            get { return _columnMaps; }
            set { _columnMaps = value; }
        }

        public void importFile()
        {
            uploadFile();
            openFile();
            import();
        }

        public void import()
        {
            using (TransactionScope ts = new TransactionScope())
            {
                DatabaseAdmin dba = DatabaseAdmin.getInstance(_connName);
                if (!dba.containsTableName(_tableName))
                {
                    TableDef tabDef = getTableDef();

                    dba.createTable(tabDef);
                }

                eachReadRow(0, 0, delegate(ListDataRow row)
                {
                    if (!row.isBlank())
                        dba.compareUpdate(_tableName, row);
                });

                ts.Complete();
            }
        }

        private TableDef getTableDef()
        {
            TableDef tabDef = new TableDef();
            tabDef.Name = _tableName;
            tabDef.Title = Path.GetFileNameWithoutExtension(_fileName);
            tabDef.Description = _fileName;
            tabDef.FieldDefs = new List<FieldDef>();
            foreach (ColumnDef cdf in ColumnMaps)
            {
                tabDef.FieldDefs.Add(cdf);
            }
            return tabDef;
        }

        public EasyUiGridData grid()
        {
            openFile();
            List<FieldDef> fields = new List<FieldDef>();
            foreach (ColumnDef field in ColumnMaps)
            {
                fields.Add(field);
            }
            EasyUiGridData ret = EUGridUtils.getGrid(_connName, fileName, fields);
            ret.url = "ExcelTransfer." + name + ".data.wbo";
            ret.filterInputs = null;
            return ret;
        }

        public delegate void RowHander(ListDataRow row);

        public void eachReadRow(int pageNo, int pageSize, RowHander rowHandler)
        {
            IEnumerator rowEnumerator = _sheet.GetRowEnumerator();
            moveToDataRow(rowEnumerator);

            int r = 0;

            if (pageNo < 1) pageNo = 1;

            while (pageSize > 0 && r < (pageSize * (pageNo - 1)) && rowEnumerator.MoveNext())
                r++;

            r = 0;

            do
            {
                IRow xlRow = (HSSFRow)rowEnumerator.Current;
                ListDataRow row = readRow(xlRow);
                rowHandler(row);
                r++;
            } while ((r < pageSize || pageSize <= 0) && rowEnumerator.MoveNext());

        }


        public List<ListDataRow> getRows()
        {
            List<ListDataRow> rows = new List<ListDataRow>();
            //eachReadRow(0, 0, delegate(ListDataRow row)//测试返回所有行
            eachReadRow(_page, _pageSize, delegate(ListDataRow row)
            {
                if (!row.isBlank())
                    rows.Add(row);
            });
            _total = (_sheet.LastRowNum + 1) - _dataRowNum;
            return rows;
        }

        private ListDataRow readRow(IRow xlRow)
        {
            ListDataRow row = new ListDataRow();

            //            for (int i = 0; i < xlRow.LastCellNum; i++)
            foreach (ColumnDef colDef in _columnMaps)
            {
                if (colDef.ExcelColNum < 0)
                {
                    row.Add(colDef.Name, null);
                    continue;
                }
                ICell cell = xlRow.GetCell(colDef.ExcelColNum);
                string value = cell == null ? null : cell.ToString();
                if (colDef.Type.Equals("datetime", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime d;
                    if (DateTime.TryParse(value, out d))
                        value = d.ToString();
                    else if (DateTime.TryParse(value, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out d))
                        value = d.ToString();
                }
                if (colDef.Type.Equals("numeric", StringComparison.OrdinalIgnoreCase))
                {
                    double d = 0;
                    double.TryParse(value, out d);
                    value = "0";
                }

                row.Add(colDef.Name, value);
                if (colDef.IsPriKey)
                    row.Add(XSqlBuilder.OLD_VERSION_PIX + colDef.Name, value);
            }
            return row;
        }

        private void moveToDataRow(IEnumerator rowEnumerator)
        {
            int i = 0;
            while (i <= _dataRowNum)
            {
                i++;
                rowEnumerator.MoveNext();
                IRow xlRow = (HSSFRow)rowEnumerator.Current;
                if (xlRow.RowNum == _dataRowNum)
                    return;
            }
        }

        public void openFile(string filePath)
        {
            this._fileName = filePath;
            openFile();
        }

        private string getExcelPath()
        {
            string filePath = Server.MapPath(ExcelVirPath);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath += this._fileName;
            return filePath;
        }

        public void openFile()
        {
            if (_sheet != null) return;
            if (string.IsNullOrEmpty(_fileName))
                throw new Exception(Lang.NoUploadFile);

            using (FileStream file = new FileStream(getExcelPath(), FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
                int sheetNo = 0;
                if (string.IsNullOrWhiteSpace(sheetName) || int.TryParse(sheetName, out sheetNo))
                    _sheet = hssfworkbook.GetSheetAt(sheetNo);
                else
                    _sheet = hssfworkbook.GetSheet(sheetName);

                file.Close();
            }
        }

        private void readFieldDef(int colId, FieldDef fieldDef)
        {
            IEnumerator rowEnumerator = _sheet.GetRowEnumerator();
            int sn = 0;
            int len = 0;
            bool isUnicode = false;
            bool isString = false;

            TypeCheckFn ckDate = delegate(string s)
            {
                DateTime d; return DateTime.TryParse(s, out d)
|| DateTime.TryParse(s, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.AssumeLocal, out d) || string.IsNullOrEmpty(s);
            };
            TypeCheckFn ckBool = delegate(string s) { bool d; return bool.TryParse(s, out d) || string.IsNullOrEmpty(s); };
            TypeCheckFn ckInt = delegate(string s) { int d; return int.TryParse(s, out d) || string.IsNullOrEmpty(s); };
            TypeCheckFn ckNum = delegate(string s) { double d; return double.TryParse(s, out d) || string.IsNullOrEmpty(s); };
            TypeCheckFn ckStr = delegate(string s) { return true; };


            if (eachValueInCol(colId, out len, out sn, out isUnicode, ckDate))
            {
                fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDotNetType(typeof(DateTime));
                len = 0;
            }
            else if (eachValueInCol(colId, out len, out sn, out isUnicode, ckBool))
            {
                fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDotNetType(typeof(bool));
                len = 0;
            }
            else if (eachValueInCol(colId, out len, out sn, out isUnicode, ckInt))
            {
                fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDotNetType(typeof(int));
                len = 0;
            }
            else if (eachValueInCol(colId, out len, out sn, out isUnicode, ckNum))
            {
                fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDotNetType(typeof(double));
                len = 24;
            }
            else
            {
                isString = true;
                eachValueInCol(colId, out len, out sn, out isUnicode, ckStr);
                if (isUnicode)
                    fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDbType(DbType.String);
                else
                    fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDbType(DbType.AnsiString);
            }

            if (isString && len < 1)
            {
                len = 255;
                fieldDef.Type = DatabaseAdmin.getInstance(_connName).typeOfDbType(DbType.String);
            }

            fieldDef.Length = len;
            fieldDef.Procesion = sn;


        }

        private delegate bool TypeCheckFn(string checkValue);

        private bool eachValueInCol(int colNo, out int len, out int sn, out bool isUnicode, TypeCheckFn checkCell)
        {
            len = 0;
            isUnicode = false;
            sn = 0;

            IEnumerator rowEnumerator = _sheet.GetRowEnumerator();
            moveToDataRow(rowEnumerator);

            bool hasValue = false;
            int i = 0;
            do
            {
                IRow xlRow = (HSSFRow)rowEnumerator.Current;
                ICell cell = xlRow.GetCell(colNo);
                string cValue = (cell == null) ? null : cell.ToString();

                if (string.IsNullOrEmpty(cValue))
                    cValue = null;
                else
                    hasValue = true;

                if (!checkCell(cValue))
                    return false;
                if (cell != null)
                {
                    string s = cell.ToString();
                    if (Regex.IsMatch(s, @"[\u007F-\uFFFD]+"))//匹配中文 @"[\u4e00-\u9fa5]+"
                        isUnicode = true;
                    int cLen = s.Length;
                    if (cell.CellType.Equals(CellType.NUMERIC) && s.IndexOf(".") >= 0)
                    {
                        cLen = cell.CellType.Equals(CellType.STRING) ? s.Length : s.IndexOf(".");
                        int cSn = cell.CellType.Equals(CellType.STRING) ? 0 : s.Length - s.LastIndexOf(".");
                        if (cSn > sn) sn = cSn;
                    }
                    if (cLen > len)
                        len = cLen;
                }
                i++;
            } while ((i < TYPE_TEST_COUNT) && rowEnumerator.MoveNext());

            if (i < 1)
                throw new XUserException(Lang.UploadFileNoData);

            return hasValue;
            //return true;
        }


        public ListData data(int page, int rows)
        {
            if (page < 1) page = 1;
            if (rows < 1) rows = 15;
            _page = page;
            _pageSize = rows;

            ListData ret = new ListData();
            ret.rows = this.getRows();
            ret.total = _total;
            return ret;
        }

        private void buildColumns()
        {
            _columnMaps = new List<ColumnDef>();
            IRow headRow = _sheet.GetRow(_headRowNum);
            for (int i = 0; i < headRow.LastCellNum; i++)
            {
                ColumnDef fldDef = new ColumnDef();
                fldDef.Alias = i.ToXlsColName();
                ICell cell = headRow.GetCell(i);
                string title = cell == null ? null : cell.ToString();
                if (string.IsNullOrEmpty(title))
                    title = fldDef.Alias;

                fldDef.Title = title;
                fldDef.Name = title.ToQuanPin();

                fldDef.Alias = fldDef.Name;
                // fldDef.Type = "varchar";
                //fldDef.Length = 200;
                readFieldDef(i, fldDef);
                fldDef.ExcelColNum = i;
                _columnMaps.Add(fldDef);
                //    }
            }
        }



        public void createTransfer(string sheetName, int headRowNo, int dataRowNo, string connName, string tableName)
        {
            if (this.Request.Files.Count <= 0)
                throw new Exception(Lang.NoUploadFile);

            uploadFile();


            this.sheetName = sheetName;
            this.headRowNo = headRowNo;
            this.dataRowNo = dataRowNo;
            this._connName = connName;
            if (string.IsNullOrEmpty(tableName))
                tableName = Path.GetFileNameWithoutExtension(_fileName).ToQuanPin();

            this.tableName = tableName;

            openFile();
            buildColumns();
        }

        private void uploadFile()
        {
            _sheet = null;
            HttpPostedFile file = this.Request.Files[0];
            if (file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                throw new Exception(Lang.NoUploadFile);

            this._fileName = file.FileName;
            file.SaveAs(getExcelPath());
        }

        public string sheetName { get; set; }

    }//end ExcelTransfer
}
