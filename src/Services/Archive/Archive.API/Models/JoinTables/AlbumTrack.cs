namespace Archive.API.Models.JoinTables
{
    public class AlbumTrack
    {
        public Guid AlbumId { get; set; }

        public Album Album { get; set;  }

        public Guid TrackId { get; set;  }
        public Track Track { get; set; }
    }
}
