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
    public class DirectorInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Crew> crewList)
        {
            HashSet<string> directors = new HashSet<string>();

            Dictionary<string, int> directorDict = new Dictionary<string, int>();

            foreach (var crew in crewList)
            {
                foreach (var director in crew.directors)
                {
                    directors.Add(director);
                }
            }

            foreach (string director in directors)
            {

                if (director == @"\N")
                {

                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO Directors(Nconst)" +
                        "OUTPUT INSERTED.DirectorID " +
                        "VALUES (@Col1)", sqlConn);

                    sqlCommand.Parameters.AddWithValue("@Col1", DBNull.Value);

                    try
                    {
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            int newID = (int)reader["DirectorID"];
                            directorDict.Add(director, newID);
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
                        "INSERT INTO Directors(Nconst)" +
                        "OUTPUT INSERTED.DirectorID " +
                        "VALUES ('" + director + "')", sqlConn);

                    try
                    {
                        SqlDataReader reader = sqlCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            int newID = (int)reader["DirectorID"];
                            directorDict.Add(director, newID);
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
                foreach (string director in myCrew.directors)
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO TitleDirectors (Tconst, DirectorID)" +
                        " VALUES " +
                        "('" + myCrew.tconst + "', '"
                        + directorDict[director] + "')", sqlConn);

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
