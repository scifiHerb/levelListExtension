using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace levelListExtension.Settings
{
    public class Configuration
    {
        public static Configuration Instance { get; set; } = null;
        public virtual bool enable { get; set; } = true;
        public virtual bool refresh { get; set; } = true;
        public virtual bool priorityPlaylist { get; set; } = true;
        public int selectDiff =4;
        public int count = 200;
        public float Rank_SSS = 100.0F;
        public float Rank_SS = 90.0F;
        public float Rank_S = 80.0F;
        public float Rank_A = 65.0F;
        public float Rank_B = 50.0F;
        public float Rank_C = 35.0F;
        public float Rank_D = 20.0F;
        public float Rank_E = 0.0F;

        public string Rank_SSS_Color = "#00FFFF";
        public string Rank_SS_Color = "#00FFFF";
        public string Rank_S_Color = "#00FF00";
        public string Rank_A_Color = "#00FF00";
        public string Rank_B_Color = "#FF8000";
        public string Rank_C_Color = "#FF8000";
        public string Rank_D_Color = "#FF0000";
        public string Rank_E_Color = "#FF0000";
    }
}
