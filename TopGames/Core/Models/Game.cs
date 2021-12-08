using System;

namespace TopGames.Core.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReviewCount { get; set; }
        public long InstallCount { get; set; }
        public string CurrentVersion { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Size { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
