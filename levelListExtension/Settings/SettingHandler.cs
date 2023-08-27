using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;

namespace levelListExtension.Settings
{
    class SettingsHandler : PersistentSingleton<SettingsHandler>
    {
        [UIValue("showGood")]
        public bool showGood
        {
            get => Configuration.Instance.showGood;
            set
            {
                Configuration.Instance.showGood = value;
            }
        }
        [UIValue("showBad")]
        public bool showBad
        {
            get => Configuration.Instance.showBad;
            set
            {
                Configuration.Instance.showBad = value;
            }
        }
        [UIValue("showBSR")]
        public bool showBSR
        {
            get => Configuration.Instance.showBSR;
            set
            {
                Configuration.Instance.showBSR = value;
            }
        }
        public int selectDiff
        {
            get => Configuration.Instance.selectDiff;
            set
            {
                Configuration.Instance.selectDiff = value;
            }
        }
    }
}
