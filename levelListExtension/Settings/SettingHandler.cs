using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using Steamworks;
using TMPro;

namespace levelListExtension.Settings
{
    class SettingsHandler : PersistentSingleton<SettingsHandler>
    {
        [UIValue("Enable")]
        public bool Enable
        {
            get => Configuration.Instance.Enable;
            set
            {
                Configuration.Instance.Enable = value;
            }
        }
        [UIAction("onRefresh")]
        public void onRefresh()
        {
            Plugin.Log.Info("count:" + Settings.Configuration.Instance.count.ToString());
            //debug
            Plugin.GetSongStats(Settings.Configuration.Instance.count,refreshText);
        }
        public int selectDiff
        {
            get => Configuration.Instance.selectDiff;
            set
            {
                Configuration.Instance.selectDiff = value;
            }
        }

        [UIComponent("refreshText")]
        public TextMeshProUGUI refreshText = null;
    }
}
