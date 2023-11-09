using System;
using IMDBDataImporter.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class WriterInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Crew> crewList)
        {

            HashSet<string> writers = new HashSet<string>();

            Dictionary<string, int> writerDict = new Dictionary<string, int>();

            foreach (var crew in crewList)
            {
                foreach (var writer in crew.writers)
                {
                    writers.Add(writer);
                }
            }

            foreach (string writer in writers)
            {

                if (writer == @"\N")
                {

                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO Writers(Nconst)" +
                        "OUTPUT INSERTED.WriterID " +
                        "VALUES (@Col1)", sqlConn);

                    sqlCommand.Parameters.AddWithValue("@Col1", DBNull.Value);

                    try
                    {
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            int newID = (int)reader["WriterID"];
                            writerDict.Add(writer, newID);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(sqlCommand.CommandText, ex);
                    }
                } else
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO Writers(Nconst)" +
                        "OUTPUT INSERTED.WriterID " +
                        "VALUES ('" + writer + "')", sqlConn);

                    try
                    {
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            int newID = (int)reader["WriterID"];
                            writerDict.Add(writer, newID);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(sqlCommand.CommandText, ex);
                    }
                }
            }

            foreach (Crew myCrew in crewList)
            {
                foreach (string writer in myCrew.writers)
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO TitleWriters (Tconst, WriterID)" +
                        " VALUES " +
                        "('" + myCrew.tconst + "', '"
                        + writerDict[writer] + "')", sqlConn);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(sqlCommand.CommandText, ex);
                    }
                }
            }
        }
    }
}
