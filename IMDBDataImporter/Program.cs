using IMDBDataImporter;
using IMDBDataImporter.Models;
using System.Collections.Immutable;
using System.Data.SqlClient;

string ConnString = "server=localhost;database=IMDB;" +
            "user id=SA;password=Wame123456789;TrustServerCertificate=True";

List<Person> persons = new List<Person>();
List<Title> titles = new List<Title>();
List<Crew> crews = new List<Crew>();

foreach (string line in
    System.IO.File.ReadLines
    (@"/Users/wamegassama/Documents/IMDB/data-4.tsv")
    .Skip(100001).Take(99000000))
{
    string[] values = line.Split("\t");
    if (values.Length == 6)
    {
        persons.Add(new Person(values[0], values[1], ConvertToInt(values[2]), ConvertToInt(values[3]), values[4], values[5]));
    }
}

foreach (string line in System.IO.File.ReadLines(@"/Users/wamegassama/Documents/IMDB/data-2.tsv").Skip(1).Take(100000))
{
    string[] values = line.Split("\t");
    if(values.Length == 9)
    {
        titles.Add(new Title(values[0], values[1], values[2], values[3],
            ConvertToBool(values[4]), ConvertToInt(values[5]),
            ConvertToInt(values[6]), ConvertToInt(values[7]),
            values[8]));
    }
}

foreach (string line in System.IO.File.ReadLines(@"/Users/wamegassama/Documents/IMDB/data-3.tsv").Skip(1).Take(100000))
{
    string[] values = line.Split("\t");
    if (values.Length == 3)
    {
        crews.Add(new Crew(values[0], values[1], values[2]));
    }
}

Console.WriteLine("Hvad vil du?");
Console.WriteLine("1 for delete all");
Console.WriteLine("2 for titles bulk insert");
Console.WriteLine("3 for persons bulk insert");
Console.WriteLine("4 for professions (inkl. PersonProfessions) bulk insert");
Console.WriteLine("5 for genre (inkl. TitleGenres) bulk insert");
Console.WriteLine("6 for knownForTitles (inkl. PersonknownForTitles) bulk insert");
Console.WriteLine("7 for writers (inkl. TitleWriters) bulk insert");
Console.WriteLine("8 for directors (inkl. TitleDirectors) bulk insert");

string input = Console.ReadLine();

DateTime before = DateTime.Now;

SqlConnection sqlConn = new SqlConnection(ConnString);
sqlConn.Open();

switch (input)
{
    case "1":
        SqlCommand cmd = new SqlCommand(
            "DELETE FROM Genres DELETE FROM PersonKnownForTitles DELETE FROM PersonProfessions DELETE FROM Persons DELETE FROM Professions DELETE FROM TitleGenres DELETE FROM Titles DELETE FROM KnownForTitles DELETE FROM Directors DELETE FROM Writers DELETE FROM TitleDirectors DELETE FROM TitleWriters", sqlConn);
        cmd.ExecuteNonQuery();
        break;
    case "2":
        TitleInserter.InsertData(sqlConn, titles);
        break;
    case "3":
        PersonInserter.InsertData(sqlConn, persons);
        break;
    case "4":
        ProfessionInserter.InsertData(sqlConn, persons);
        break;
    case "5":
        GenreInserter.InsertData(sqlConn, titles);
        break;
    case "6":
        PersonKnownForTitleInserter.InsertData(sqlConn, persons);
        break;
    case "7":
        WriterInserter.InsertData(sqlConn, crews);
        break;
    case "8":
        DirectorInserter.InsertData(sqlConn, crews);
        break;
}

sqlConn.Close();

DateTime after = DateTime.Now;

Console.WriteLine("Tid: " + (after - before));


bool ConvertToBool(string input)
{
    if (input == "0")
    {
        return false;
    }
    else if (input == "1")
    {
        return true;
    }
    throw new ArgumentException(
        "Kolonne er ikke 0 eller 1, men " + input);
}

int? ConvertToInt(string input)
{
    if (input.ToLower() == @"\n")
    {
        return null;
    }
    else
    {
        return int.Parse(input);
    }

}