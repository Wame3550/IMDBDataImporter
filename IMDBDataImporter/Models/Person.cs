using System;

namespace IMDBDataImporter
{
	public class Person
	{
		public string nconst { get; set; }
		public string primaryName { get; set; }
		public int? birthYear { get; set; }
		public int? deathYear { get; set; }
		public List<string> primaryProfession { get; set; }
		public List<string> knownForTitles { get; set; }

		public Person(string nconst, string primaryName, int? birthYear, int? deathYear, string primaryProfession, string knownForTitles)
		{
			this.nconst = nconst;
			this.primaryName = primaryName;
			this.birthYear = birthYear;
			this.deathYear = deathYear;
			this.primaryProfession = primaryProfession.Split(',').ToList();
			this.knownForTitles = knownForTitles.Split(',').ToList();
		}
	}
}

