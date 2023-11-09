using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class GenreInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Title> titleList)
        {
            HashSet<string> genres = new HashSet<string>();

            Dictionary<string, int> genreDict = new Dictionary<string, int>();

            foreach (var title in titleList)
            {
                foreach (var genre in title.genre)
                {
                    genres.Add(genre);
                }
            }

            foreach (string genre in genres)
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "INSERT INTO Genres(Genre)" +
                    "OUTPUT INSERTED.GenreID " +
                    "VALUES ('" + genre + "')", sqlConn);

                try
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        int newID = (int)reader["GenreID"];
                        genreDict.Add(genre, newID);
                    }

                    reader.Close();
                } catch (Exception ex)
                {
                    throw new Exception(sqlCommand.CommandText, ex);
                }
            }

            foreach (Title myTitle in titleList)
            {
                foreach (string genre in myTitle.genre)
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO TitleGenres (Tconst, GenreID)" +
                        " VALUES " +
                        "('" + myTitle.tconst + "', '"
                        + genreDict[genre] + "')", sqlConn);

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    } catch (Exception ex)
                    {
                        throw new Exception(sqlCommand.CommandText, ex);
                    }
                }
            }
        }
    }
}
