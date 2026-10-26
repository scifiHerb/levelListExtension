using System.Collections.Generic;
using System.Linq;

namespace levelListExtension.Settings
{
    public class Configuration
    {
        public static Configuration Instance { get; set; } = null;
        public virtual bool enable { get; set; } = true;
        public virtual bool refresh { get; set; } = true;
        public virtual bool priorityPlaylist { get; set; } = true;

        public List<object> options = new object[] { "Score Saber", "Beat Leader"}.ToList();

        public string listChoice = "Score Saber";


        public int selectDiff =4;
        public int count = 200;
        public float Rank_SSS = 100.0F;
        public float Rank_SSPlus = 95.0F;
        public float Rank_SS = 90.0F;
        public float Rank_SPlus = 85.0F;
        public float Rank_S = 80.0F;
        public float Rank_A = 65.0F;
        public float Rank_B = 50.0F;
        public float Rank_C = 35.0F;
        public float Rank_D = 20.0F;
        public float Rank_E = 0.0F;

        public string Rank_SSS_Color = "#00FFFF";
        public string Rank_SSPlus_Color = "#00FFFF";
        public string Rank_SS_Color = "#00FFFF";
        public string Rank_SPlus_Color = "#00FF00";
        public string Rank_S_Color = "#00FF00";
        public string Rank_A_Color = "#00FF00";
        public string Rank_B_Color = "#FF8000";
        public string Rank_C_Color = "#FF8000";
        public string Rank_D_Color = "#FF0000";
        public string Rank_E_Color = "#FF0000";

        public string Difficulty_ExPlus_Color = "#00FFFF";
        public string Difficulty_Ex_Color = "#00FF00";
        public string Difficulty_Hard_Color  = "#FF8000";
        public string Difficulty_Normal_Color = "#808080";
        public string Difficulty_Easy_Color = "#808080";
    }
}
