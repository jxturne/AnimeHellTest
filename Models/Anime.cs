namespace AnimeHellTest.Models
{
    public class Anime
    {
        public long MalID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int ID { get; set; }
        public string? StudioName { get; set; }
        public int Episodes { get; set; }
        public int Seasons { get; set; }
        public bool IsCompleted { get; set; }
        public string? ImageUrl { get; internal set; }
    }
}

