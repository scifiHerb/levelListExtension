using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using HMUI;
using System.Reflection;
using BeatSaberMarkupLanguage;
using Newtonsoft.Json;
using static IPA.Logging.Logger;
using levelListExtension.UI;
using System.Linq;

namespace levelListExtension.HarmonyPatches
{
    [HarmonyPatch(typeof(StandardLevelDetailView))]
    [HarmonyPatch("SetContent", MethodType.Normal)]
    internal class LevelListTableCellSetDataFromLevel
    {
        public static IBeatmapLevel selectedLevel = null;
        public static Transform button = null;

        private static void Postfix(IBeatmapLevel level, BeatmapCharacteristicSO defaultBeatmapCharacteristic,
            PlayerData playerData, TextMeshProUGUI ____actionButtonText, StandardLevelDetailView __instance, IDifficultyBeatmap ____selectedDifficultyBeatmap)
        {
            var resultsView = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().FirstOrDefault();
            Plugin.Log.Info("resultsView"+resultsView.ToString());
            selectUI.instance.Create(resultsView);
            
        }
    }
}
