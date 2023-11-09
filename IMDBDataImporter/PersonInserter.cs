using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class PersonInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Person> personsList)
        {

            //Adding Data to person table

            DataTable personTable = new DataTable("Persons");

            personTable.Columns.Add("Nconst", typeof(string));
            personTable.Columns.Add("PrimaryName", typeof(string));
            personTable.Columns.Add("BirthYear", typeof(int));
            personTable.Columns.Add("DeathYear", typeof(int));

            foreach (Person person in personsList)
            {
                DataRow personRow = personTable.NewRow();
                FillParameter(personRow, "Nconst", person.nconst);
                FillParameter(personRow, "PrimaryName", person.primaryName);
                FillParameter(personRow, "BirthYear", person.birthYear);
                FillParameter(personRow, "DeathYear", person.deathYear);
                personTable.Rows.Add(personRow);
            }
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn,
                SqlBulkCopyOptions.KeepNulls, null);
            bulkCopy.DestinationTableName = "Persons";
            bulkCopy.BulkCopyTimeout = 0;
            bulkCopy.WriteToServer(personTable);

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
