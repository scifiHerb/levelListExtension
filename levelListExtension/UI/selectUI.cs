using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Parser;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using levelListExtension;

using static IPA.Logging.Logger;
using System.Configuration;
using levelListExtension.Settings;
using IPA.Utilities;
using System.IO;

namespace levelListExtension.UI
{
    internal class selectUI
    {
        static public selectUI instance = new selectUI();

        public void Create(StandardLevelDetailViewController resultsView)
        {
            if (root != null) return;
            BSMLParser.instance.Parse(
                Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"levelListExtension.UI.selectUI.bsml"),
                resultsView.gameObject, instance
            );
            resultsView.didActivateEvent += ResultsView_didActivateEvent;
            resultsView.didDeactivateEvent += ResultsView_didDeactivateEvent;

            root.SetParent(GameObject.Find("ScreenContainer").transform);
            root.localPosition = new Vector3(-7,20.5F,0);
            root.name = "selectButton";

            setDiffName();
        }

        private void setDiffName()
        {
            string result = "";
            switch (Settings.Configuration.Instance.selectDiff)
            {
                case 0:
                    result = "Easy";
                    break;
                case 1:
                    result = "Normal";
                    break;
                case 2:
                    result = "Hard";
                    break;
                case 3:
                    result = "Ex";
                    break;
                case 4:
                    result = "Ex+";
                    break;
            }
            selectButton.text = result;
        }
        private void ResultsView_didActivateEvent(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            //Plugin.Log.Info("Activate");
            root.gameObject.SetActive(true);

        }
        private void ResultsView_didDeactivateEvent(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            //Plugin.Log.Info("DEACTIVATE");
            root.gameObject.SetActive(false);
        }


        public void setText(string  text)
        {
        }
        [UIAction("onClick")]
        protected async Task onClick()
        {
            Settings.Configuration.Instance.selectDiff += 1;
            if (Settings.Configuration.Instance.selectDiff > 4) Settings.Configuration.Instance.selectDiff = 0;

            setDiffName();
        }

        [UIComponent("root")]
        protected RectTransform root = null;
        [UIComponent("selectButton")]
        protected TextMeshProUGUI selectButton = null;
    }
}
