using System.Collections.Generic;

namespace levelListExtension.PlaylistData
{
    public class Difficulty
    {
        public string characteristic { get; set; }
        public string name { get; set; }
    }

    public class Song
    {
        public string songName { get; set; }
        public string levelAuthorName { get; set; }
        public string hash { get; set; }
        public string levelid { get; set; }
        public List<Difficulty> difficulties { get; set; }
    }
    public class CustomData
    {
        public string syncURL { get; set; }
    }

    public class Root
    {
        public string playlistTitle { get; set; }
        public string playlistAuthor { get; set; }
        public CustomData customData { get; set; }
        public string image { get; set; }
        public List<Song> songs { get; set; }
    }
}
