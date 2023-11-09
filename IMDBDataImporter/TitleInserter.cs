using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class TitleInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Title> titles)
        {

            //Adding Data to titles table

            DataTable titleTable = new DataTable("Titles");

            titleTable.Columns.Add("Tconst", typeof(string));
            titleTable.Columns.Add("TitleType", typeof(string));
            titleTable.Columns.Add("PrimaryTitle", typeof(string));
            titleTable.Columns.Add("OriginalTitle", typeof(string));
            titleTable.Columns.Add("IsAdult", typeof(bool));
            titleTable.Columns.Add("StartYear", typeof(int));
            titleTable.Columns.Add("EndYear", typeof(int));
            titleTable.Columns.Add("RuntimeMinutes", typeof(int));

            foreach (Title title in titles)
            {
                DataRow titleRow = titleTable.NewRow();
                FillParameter(titleRow, "Tconst", title.tconst);
                FillParameter(titleRow, "TitleType", title.titleType);
                FillParameter(titleRow, "PrimaryTitle", title.primaryTitle);
                FillParameter(titleRow, "OriginalTitle", title.originalTitle);
                FillParameter(titleRow, "IsAdult", title.isAdult);
                FillParameter(titleRow, "StartYear", title.startYear);
                FillParameter(titleRow, "EndYear", title.endYear);
                FillParameter(titleRow, "RuntimeMinutes", title.runtimeMinutes);
                titleTable.Rows.Add(titleRow);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.DestinationTableName = "Titles";
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.WriteToServer(titleTable);

            void FillParameter(DataRow row,
            string columnName,
            object? value)
            {
                if (value != null)
                {
                    row[columnName] = value;
                }
                else
                {
                    row[columnName] = DBNull.Value;
                }
            }
        }
    }
}
