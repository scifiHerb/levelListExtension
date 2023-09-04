using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;

namespace levelListExtension.Settings
{
    class SettingsHandler : PersistentSingleton<SettingsHandler>
    {
        [UIValue("enable")]
        public bool enable
        {
            get => Configuration.Instance.enable;
            set
            {
                Configuration.Instance.enable = value;
            }
        }
        [UIValue("refresh")]
        public bool refresh
        {
            get => Configuration.Instance.refresh;
            set
            {
                Configuration.Instance.refresh = value;
            }
        }
        [UIValue("priorityPlaylist")]
        public bool priorityPlaylist
        {
            get => Configuration.Instance.priorityPlaylist;
            set
            {
                Configuration.Instance.priorityPlaylist = value;
            }
        }
    }
}
