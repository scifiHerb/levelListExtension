using System.Collections.Generic;

namespace levelListExtension.BL
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public object myScore { get; set; }
        public Leaderboard leaderboard { get; set; }
        public double? weight { get; set; }
        public double? accLeft { get; set; }
        public double? accRight { get; set; }
        public double? id { get; set; }
        public double? baseScore { get; set; }
        public double? modifiedScore { get; set; }
        public double? accuracy { get; set; }
        public string playerId { get; set; }
        public double? pp { get; set; }
        public double? bonusPp { get; set; }
        public double? passPP { get; set; }
        public double? accPP { get; set; }
        public double? techPP { get; set; }
        public double? rank { get; set; }
        public string country { get; set; }
        public double? fcAccuracy { get; set; }
        public double? fcPp { get; set; }
        public string replay { get; set; }
        public string modifiers { get; set; }
        public double? badCuts { get; set; }
        public double? missedNotes { get; set; }
        public double? bombCuts { get; set; }
        public double? wallsHit { get; set; }
        public double? pauses { get; set; }
        public bool fullCombo { get; set; }
        public string platform { get; set; }
        public double? maxCombo { get; set; }
        public double? maxStreak { get; set; }
        public double? hmd { get; set; }
        public double? controller { get; set; }
        public string leaderboardId { get; set; }
        public string timeset { get; set; }
        public double? timepost { get; set; }
        public double? replaysWatched { get; set; }
        public double? playCount { get; set; }
        public double? priority { get; set; }
        public object player { get; set; }
        public ScoreImprovement scoreImprovement { get; set; }
        public object rankVoting { get; set; }
        public object metadata { get; set; }
        public Offsets offsets { get; set; }
    }

    public class Difficulty
    {
        public double? id { get; set; }
        public double? value { get; set; }
        public double? mode { get; set; }
        public string difficultyName { get; set; }
        public string modeName { get; set; }
        public double? status { get; set; }
        public ModifierValues modifierValues { get; set; }
        public object modifiersRating { get; set; }
        public double? nominatedTime { get; set; }
        public double? qualifiedTime { get; set; }
        public double? rankedTime { get; set; }
        public object stars { get; set; }
        public double? predictedAcc { get; set; }
        public object passRating { get; set; }
        public object accRating { get; set; }
        public object techRating { get; set; }
        public double? type { get; set; }
        public double? njs { get; set; }
        public double? nps { get; set; }
        public double? notes { get; set; }
        public double? bombs { get; set; }
        public double? walls { get; set; }
        public double? maxScore { get; set; }
        public double? duration { get; set; }
        public double? requirements { get; set; }
    }

    public class Difficulty2
    {
        public double? id { get; set; }
        public double? value { get; set; }
        public double? mode { get; set; }
        public string difficultyName { get; set; }
        public string modeName { get; set; }
        public double? status { get; set; }
        public ModifierValues modifierValues { get; set; }
        public object modifiersRating { get; set; }
        public double? nominatedTime { get; set; }
        public double? qualifiedTime { get; set; }
        public double? rankedTime { get; set; }
        public object stars { get; set; }
        public double? predictedAcc { get; set; }
        public object passRating { get; set; }
        public object accRating { get; set; }
        public object techRating { get; set; }
        public double? type { get; set; }
        public double? njs { get; set; }
        public double? nps { get; set; }
        public double? notes { get; set; }
        public double? bombs { get; set; }
        public double? walls { get; set; }
        public double? maxScore { get; set; }
        public double? duration { get; set; }
        public double? requirements { get; set; }
    }

    public class Leaderboard
    {
        public string id { get; set; }
        public Song song { get; set; }
        public Difficulty difficulty { get; set; }
        public object scores { get; set; }
        public object changes { get; set; }
        public object qualification { get; set; }
        public object reweight { get; set; }
        public object leaderboardGroup { get; set; }
        public double? plays { get; set; }
    }

    public class Metadata
    {
        public double? itemsPerPage { get; set; }
        public double? page { get; set; }
        public double? total { get; set; }
    }

    public class ModifierValues
    {
        public double? modifierId { get; set; }
        public double? da { get; set; }
        public double? fs { get; set; }
        public double? sf { get; set; }
        public double? ss { get; set; }
        public double? gn { get; set; }
        public double? na { get; set; }
        public double? nb { get; set; }
        public double? nf { get; set; }
        public double? no { get; set; }
        public double? pm { get; set; }
        public double? sc { get; set; }
        public double? sa { get; set; }
        public double? op { get; set; }
    }

    public class Offsets
    {
        public double? id { get; set; }
        public double? frames { get; set; }
        public double? notes { get; set; }
        public double? walls { get; set; }
        public double? heights { get; set; }
        public double? pauses { get; set; }
    }

    public class Root
    {
        public Metadata metadata { get; set; }
        public List<Datum> data { get; set; }
    }

    public class ScoreImprovement
    {
        public double? id { get; set; }
        public string timeset { get; set; }
        public double? score { get; set; }
        public double? accuracy { get; set; }
        public double? pp { get; set; }
        public double? bonusPp { get; set; }
        public double? rank { get; set; }
        public double? accRight { get; set; }
        public double? accLeft { get; set; }
        public double? averageRankedAccuracy { get; set; }
        public double? totalPp { get; set; }
        public double? totalRank { get; set; }
        public double? badCuts { get; set; }
        public double? missedNotes { get; set; }
        public double? bombCuts { get; set; }
        public double? wallsHit { get; set; }
        public double? pauses { get; set; }
    }

    public class Song
    {
        public string id { get; set; }
        public string hash { get; set; }
        public string name { get; set; }
        public string subName { get; set; }
        public string author { get; set; }
        public string mapper { get; set; }
        public double? mapperId { get; set; }
        public string coverImage { get; set; }
        public string fullCoverImage { get; set; }
        public string downloadUrl { get; set; }
        public double? bpm { get; set; }
        public double? duration { get; set; }
        public string tags { get; set; }
        public double? uploadTime { get; set; }
        public List<Difficulty> difficulties { get; set; }
    }


}
