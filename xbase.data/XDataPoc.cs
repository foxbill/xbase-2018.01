using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace xbase.data
{
    public class XDataPoc
    {
        public DataSet OpenData()
        {
            DataSet ds = new DataSet("CustomerOrders");


            return ds;
        }

        public void InsertRow(string myConnectionString)
        {
            // If the connection string is null, use a default.
            if (myConnectionString == "")
            {
                myConnectionString = "Initial Catalog=Northwind;Data Source=localhost;Integrated Security=SSPI;";
            }
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            string myInsertQuery = "INSERT INTO Customers (CustomerID, CompanyName) Values('NWIND', 'Northwind Traders')";
            SqlCommand myCommand = new SqlCommand(myInsertQuery);
            myCommand.Connection = myConnection;
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myCommand.Connection.Close();
        }


        public void FullDataSet()
        {
            SqlConnection nwindConn = new SqlConnection(@"Data Source=
                        localhost;Integrated Security=SSPI;Initial Catalog=northwind");

            SqlCommand selectCMD = new SqlCommand("SELECT CustomerID, CompanyName FROM Customers", nwindConn);
            selectCMD.CommandTimeout = 30;

            SqlDataAdapter catDA = new SqlDataAdapter();
            catDA.SelectCommand = selectCMD;

            nwindConn.Open();

            DataSet catDS = new DataSet();
            catDA.Fill(catDS, "Customers");

            nwindConn.Close();




        }

        public void ColumnsMap()
        {
            SqlDataAdapter custDA = new SqlDataAdapter();
            DataSet custDS = new DataSet();

            DataTableMapping custMap = custDA.TableMappings.Add("Table", "NorthwindCustomers");
            custMap.ColumnMappings.Add("CompanyName", "Company");
            custMap.ColumnMappings.Add("ContactName", "Contact");
            custMap.ColumnMappings.Add("PostalCode", "ZIPCode");

            custDA.Fill(custDS);

        }

        public void FullSchema()
        {
            //注意 如果数据源中的某列被标识为自动递增列，则 MissingSchemaAction 为 AddWithKey 的 FillSchema 方法或 Fill 方法将创建一个 AutoIncrement 属性设置为 true 的 DataColumn。不过，您将需要手动设置 AutoIncrementStep 和 AutoIncrementSeed 值。有关自动递增列的更多信息，请参阅创建 AutoIncrement 列。 
            //当使用 FillSchema 或将 MissingSchemaAction 设置为 AddWithKey 时，将需要在数据源中进行额外的处理来确定主键列信息。这一额外的处理可能会降低性能。如果主键信息在设计时已知，为了实现最佳性能，建议显式指定一个或多个主键列。有关为表显式设置主键信息的信息，请参阅为表定义主键。

            //以下代码示例显示如何使用 FillSchema 向 DataSet 添加架构信息。

            SqlDataAdapter custDA = new SqlDataAdapter();
            DataSet custDS = new DataSet();

            custDA.FillSchema(custDS, SchemaType.Source, "Customers");
            custDA.Fill(custDS, "Customers");

            //以下代码示例显示如何使用 Fill 方法的 MissingSchemaAction.AddWithKey 属性向 DataSet 添加架构信息。



            custDA.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            custDA.Fill(custDS, "Customers");

            //多个结果集

            //如果 DataAdapter 遇到从 SelectCommand 中返回的多个结果集，它将在 DataSet 中创建多个表。将向这些表提供递增的默认名称 TableN，以表示 Table0 的“Table”为第一个表名。如果以参数形式向 FillSchema 方法传递表名称，则将向这些表提供递增的默认名称 TableNameN，这些表名称以表示 TableName0 的“TableName”为起始。

            //注意 如果对返回多个结果集的命令调用 OleDbDataAdapter 对象的 FillSchema 方法，则将只返回第一个结果集中的架构信息。当使用 OleDbDataAdapter 为多个结果集返回架构信息时，建议在调用 Fill 方法时将 MissingSchemaAction 指定为 AddWithKey 并获取架构信息。        
        }


        /**********   并发控制解决方案***************
         * 
         * 
         ********************************************/
        public void UpdateDataSetByVersion()
        {
            //以下代码示例定义一个 UPDATE 语句，在该语句中，CustomerID 列用作以下两个参数的
            //SourceColumn：@CustomerID (SET CustomerID = @CustomerID) 
            //和 @OldCustomerID (WHERE CustomerID = @OldCustomerID)。
            //@CustomerID 参数用于将 CustomerID 列更新为 DataRow 中的当前值。因此，将使用 SourceVersion 为 Current 的 CustomerID SourceColumn。@OldCustomerID 参数用于标识数据源中的当前行。由于在该行的 Original 版本中找到了匹配列值，所以将使用 SourceVersion 为 Original 的相同 SourceColumn (CustomerID)。

            SqlDataAdapter custDA = new SqlDataAdapter();

            custDA.UpdateCommand.CommandText = @"Update customers (SET CustomerID = @CustomerID)  (WHERE CustomerID = @OldCustomerID)";
            custDA.UpdateCommand.Parameters.Add("@CustomerID", SqlDbType.NChar, 5, "CustomerID");

            custDA.UpdateCommand.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 40, "CompanyName");

            SqlParameter myParm = custDA.UpdateCommand.Parameters.Add("@OldCustomerID", SqlDbType.NChar, 5, "CustomerID");
            myParm.SourceVersion = DataRowVersion.Original;


            //UpdatedRowSource
            //可以使用 Command 对象的 UpdatedRowSource 属性控制从数据源中返回的值如何映射回 DataSet。通过将 UpdatedRowSource 属性设置为 UpdateRowSource 枚举值之一，可以控制是忽略 DataAdapter 命令返回的参数还是将其应用于 DataSet 中已更改的行。还可以指定是否将返回的第一个行（如果存在）应用于 DataSet 中已更改的行。
            custDA.UpdateCommand.UpdatedRowSource = UpdateRowSource.Both;

            //下表描述 UpdateRowSource 枚举的不同值，并说明它们如何影响与 DataAdapter 一起使用的命令的行为。

            //UpdateRowSource 说明 
            //Both 输出参数和返回的结果集的第一行都可以映射到 DataSet 中已更改的行。 
            //FirstReturnedRecord 只有返回的结果集第一行中的数据才可以映射到 DataSet 中已更改的行。 
            //None 将忽略任何输出参数或返回的结果集的行。 
            //OutputParameters 只有输出参数才可以映射到 DataSet 中已更改的行。 

        }

        /**********************************
         * 
         * 返回行数，处理自动ID，映射自动ID，的解决方案
         * 
         *********************************/
        public void Insert()
        {
            SqlConnection nwindConn = new SqlConnection("Data Source=localhost;Integrated Security=SSPI;Initial Catalog=northwind");

            SqlDataAdapter catDA = new SqlDataAdapter("SELECT CategoryID, CategoryName FROM Categories", nwindConn);

            catDA.InsertCommand = new SqlCommand("InsertCategory", nwindConn);
            catDA.InsertCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter myParm = catDA.InsertCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            myParm.Direction = ParameterDirection.ReturnValue;

            catDA.InsertCommand.Parameters.Add("@CategoryName", SqlDbType.NChar, 15, "CategoryName");

            myParm = catDA.InsertCommand.Parameters.Add("@Identity", SqlDbType.Int, 0, "CategoryID");
            myParm.Direction = ParameterDirection.Output;

            DataSet catDS = new DataSet();
            catDA.Fill(catDS, "Categories");

            DataRow newRow = catDS.Tables["Categories"].NewRow();
            newRow["CategoryName"] = "New Category";
            catDS.Tables["Categories"].Rows.Add(newRow);

            catDA.Update(catDS, "Categories");

            Int32 rowCount = (Int32)catDA.InsertCommand.Parameters["@RowCount"].Value;

        }
        /**
         * 自动产生SQl命令
         * 
         * 
         * */
        public void BuildSqlCommand()
        {
            SqlConnection nwindConn = new SqlConnection("Data Source=localhost;Integrated Security=SSPI;Initial Catalog=northwind");

            SqlDataAdapter custDA = new SqlDataAdapter("SELECT * FROM Customers", nwindConn);
            SqlCommandBuilder custCB = new SqlCommandBuilder(custDA);
            Console.WriteLine(custCB.GetUpdateCommand().CommandText);

            DataSet custDS = new DataSet();

            nwindConn.Open();
            custDA.Fill(custDS, "Customers");


            // Code to modify data in the DataSet here.

            // Without the SqlCommandBuilder, this line would fail.
            custDA.Update(custDS, "Customers");
            nwindConn.Close();

        }

        public void UpdateDataSet()
        {

            SqlConnection nwindConn = new SqlConnection(@"Data Source=
                        localhost;Integrated Security=SSPI;Initial Catalog=northwind");


            SqlDataAdapter catDA = new SqlDataAdapter("SELECT CategoryID, CategoryName FROM Categories", nwindConn);

            catDA.UpdateCommand = new SqlCommand("UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID", nwindConn);

            catDA.UpdateCommand.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 15, "CategoryName");

            SqlParameter workParm = catDA.UpdateCommand.Parameters.Add("@CategoryID", SqlDbType.Int);
            workParm.SourceColumn = "CategoryID";
            workParm.SourceVersion = DataRowVersion.Original;

            DataSet catDS = new DataSet();
            catDA.Fill(catDS, "Categories");

            DataRow cRow = catDS.Tables["Categories"].Rows[0];
            cRow["CategoryName"] = "New Category";

            catDA.Update(catDS);



            //例如，以下代码确保首先处理表中已删除的行，然后处理已更新的行，然后处理已插入的行。

            DataTable updTable = catDS.Tables["Customers"];

            // First process deletes.
            catDA.Update(updTable.Select(null, null, DataViewRowState.Deleted));

            // Next process updates.
            catDA.Update(updTable.Select(null, null, DataViewRowState.ModifiedCurrent));

            // Finally, process inserts.
            catDA.Update(updTable.Select(null, null, DataViewRowState.Added));



        }

    }
}
