using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBDataImporter
{
    public class ProfessionInserter
    {
        public static void InsertData(SqlConnection sqlConn, List<Person> personList)
        {

            //Adding data to profession table

            HashSet<string> professions = new HashSet<string>();


            Dictionary<string, int> professionDict = new Dictionary<string, int>();

            //Adding profession to HashSet

            foreach (var person in personList)
            {
                foreach (var profession in person.primaryProfession)
                {
                    professions.Add(profession);
                }
            }

            foreach (string profession in professions)
            {
                SqlCommand sqlCommand = new SqlCommand(
                    "INSERT INTO Professions(Profession)" +
                    "OUTPUT INSERTED.ProfessionID " +
                    "VALUES ('" + profession + "')", sqlConn);

                try
                {
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        int newID = (int)reader["ProfessionID"];
                        professionDict.Add(profession, newID);
                    }

                    reader.Close();
                } catch (Exception ex)
                {
                    throw new Exception(sqlCommand.CommandText, ex);
                }
            }

            foreach (Person myPerson in personList)
            {
                foreach (string profession in myPerson.primaryProfession)
                {
                    SqlCommand sqlCommand = new SqlCommand(
                        "INSERT INTO PersonProfessions (Nconst, ProfessionID)" +
                        " VALUES " +
                        "('" + myPerson.nconst + "', '"
                        + professionDict[profession] + "')", sqlConn);

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
