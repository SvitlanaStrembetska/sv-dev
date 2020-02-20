using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Svbase.Helpers
{
    public static class ConvertDataHelper
    {
        public static DataTable ConvertXslXtoDataTable(string strFilePath, string connString, int showTableRowsCount, ref List<string> errorsList)
        {
            var oleDbConnection = new OleDbConnection(connString);
            var dataTable = new DataTable();

            try
            {
                oleDbConnection.Open();

                var dtExcelSchema = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null)
                {
                    var sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                    string getRowsCount;
                    if (showTableRowsCount == -1)
                        getRowsCount = " * ";
                    else getRowsCount = " TOP " + showTableRowsCount + " * ";

                    using (
                        var oleDbCommand = new OleDbCommand("SELECT" + getRowsCount + "From [" + sheetName + "]",
                            oleDbConnection))
                    {
                        var oleDbDataAdapter = new OleDbDataAdapter { SelectCommand = oleDbCommand };
                        var dataSet = new DataSet();
                        oleDbDataAdapter.Fill(dataSet);

                        dataTable = dataSet.Tables[0];
                    }
                }
            }
            catch (OleDbException exception)
            {
                errorsList.Add(exception.Message);
                return null;
            }
            catch (InvalidOperationException exception)
            {
                errorsList.Add(exception.Message);
                return null;
            }
            finally
            {
                oleDbConnection.Close();
            }

            return dataTable;
        }

    }
}