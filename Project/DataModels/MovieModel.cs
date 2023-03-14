using System.Text.Json.Serialization;


class MovieModel
{
 [JsonPropertyName("id")]
 public int Id { get; set; }

 [JsonPropertyName("title")]
 public string Title { get; set; }

 [JsonPropertyName("release_date")]
 public DateTime ReleaseDate { get; set; }

 [JsonPropertyName("director")]
 public string Director { get; set; }

 public MovieModel(int id, string title, DateTime releaseDate, string director)
 {
  Id = id;
  Title = title;
  ReleaseDate = releaseDate;
  Director = director;
 }

}




