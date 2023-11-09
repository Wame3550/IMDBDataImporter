using System;

namespace IMDBDataImporter.Models
{
	public class Crew
	{
		public string tconst { get; set; }
		public List<string> directors { get; set; }
		public List<string> writers { get; set; }

		public Crew(string tconst, string directors, string writers)
		{
			this.tconst = tconst;
			this.directors = directors.Split(',').ToList();
			this.writers = writers.Split(',').ToList();
		}
	}
}