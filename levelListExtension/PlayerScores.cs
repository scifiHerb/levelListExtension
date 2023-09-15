using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelListExtension
{
    using System;
    using Newtonsoft.Json;

    public class Difficulty
    {
        public int LeaderboardId { get; set; }
        public int Diff { get; set; }
        public string GameMode { get; set; }
        public string DifficultyRaw { get; set; }
    }

    public class Leaderboard
    {
        public int Id { get; set; }
        public string SongHash { get; set; }
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public Difficulty Difficulty { get; set; }
        public int MaxScore { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RankedDate { get; set; }
        public DateTime? QualifiedDate { get; set; }
        public DateTime? LovedDate { get; set; }
        public bool Ranked { get; set; }
        public bool Qualified { get; set; }
        public bool Loved { get; set; }
        public int MaxPP { get; set; }
        public double Stars { get; set; }
        public int Plays { get; set; }
        public int DailyPlays { get; set; }
        public bool PositiveModifiers { get; set; }
        public object PlayerScore { get; set; }
        public string CoverImage { get; set; }
        public object Difficulties { get; set; }
    }

    public class Score
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public int BaseScore { get; set; }
        public int ModifiedScore { get; set; }
        public double Pp { get; set; }
        public float Weight { get; set; }
        public string Modifiers { get; set; }
        public float Multiplier { get; set; }
        public int BadCuts { get; set; }
        public int MissedNotes { get; set; }
        public int MaxCombo { get; set; }
        public bool FullCombo { get; set; }
        public int Hmd { get; set; }
        public DateTime TimeSet { get; set; }
        public bool HasReplay { get; set; }
        public Leaderboard Leaderboard { get; set; }
    }

    public class PlayerScore
    {
        public Score Score { get; set; }
        public Leaderboard Leaderboard { get; set; }
        public bool? isScoreSaber { get; set; }
    }

    public class Metadata
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
    }

    public class PlayerScoresInfo
    {
        public List<PlayerScore> PlayerScores { get; set; }
        public Metadata Metadata { get; set; }
    }
}
