using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class PersonKnownForTitleInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Person> personsList)
        {
            HashSet<string> knownForTitles = new HashSet<string>();

            Dictionary<string, string> knownForTitleDict = new Dictionary<string, string>();

            foreach (var person in personsList)
            {
                foreach (var knownForTitle in person.knownForTitles)
                {
                    knownForTitles.Add(knownForTitle);
                }
            }

            foreach (string knownForTitle in knownForTitles)
            {
                knownForTitleDict.Add(knownForTitle, knownForTitle);
            }

            foreach (Person myPerson in personsList)
            {
                foreach (string knownForTitle in myPerson.knownForTitles)
                {
                    if (knownForTitleDict[knownForTitle] != @"\N")
                    {
                        //SqlCommand sqlCommand = new SqlCommand(
                        //"INSERT INTO PersonknownForTitles (Nconst, Tconst)" +
                        //" VALUES " +
                        //"('" + myPerson.nconst + "', @Col1)", sqlConn);

                        //sqlCommand.Parameters.AddWithValue("@Col1", DBNull.Value);

                        //try
                        //{
                        //    sqlCommand.ExecuteNonQuery();
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw new Exception(sqlCommand.CommandText, ex);
                        //}

                        SqlCommand sqlCommand = new SqlCommand(
                     "INSERT INTO PersonknownForTitles (Nconst, Tconst)" +
                     " VALUES " +
                     "('" + myPerson.nconst + "', '"
                     + knownForTitleDict[knownForTitle] + "')", sqlConn);

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
}
