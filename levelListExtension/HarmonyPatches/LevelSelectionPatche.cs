
using BeatSaberMarkupLanguage;
using HarmonyLib;
using levelListExtension;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using IPALogger = IPA.Logging.Logger;

namespace levelListExtension.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelListTableCell))]
    [HarmonyPatch("SetDataFromLevelAsync", MethodType.Normal)]
    internal class LevelList
    {
        public static Dictionary<string, PlayerScore> plScore = new Dictionary<string, PlayerScore>();
        private static void Postfix(LevelListTableCell __instance, IPreviewBeatmapLevel level, bool isFavorite, ref UnityEngine.UI.Image ____favoritesBadgeImage,
            TextMeshProUGUI ____songBpmText, TextMeshProUGUI ____songAuthorText)
        {
            //return not custom level or mod disabled
            if (level.levelID.IndexOf("custom_level") == -1 || !Settings.Configuration.Instance.Enable) return;

            string levelID = level.levelID.Substring(13);
            ____songBpmText.text = "";

            __instance.transform.name = "test selectlist";

            string diffRaw = "";
            int selectDiff = Settings.Configuration.Instance.selectDiff;

            for (; selectDiff != -1; selectDiff--)
            {
                switch (selectDiff)
                {
                    case 4:
                        diffRaw = "_ExpertPlus_SoloStandard";
                        break;
                    case 3:
                        diffRaw = "_Expert_SoloStandard";
                        break;
                    case 2:
                        diffRaw = "_Hard_SoloStandard";
                        break;
                    case 1:
                        diffRaw = "_Normal_SoloStandard";
                        break;
                    case 0:
                        diffRaw = "_Easy_SoloStandard";
                        break;
                }

                if (plScore.ContainsKey(levelID + diffRaw))break;
            }
            
            if (plScore.ContainsKey(levelID+ diffRaw))
            {
                //____songAuthorText.text = (((float)plScore[levelID].Score.BaseScore / plScore[levelID].Leaderboard.MaxScore)*100F).ToString("F"); 

                //setText
                //string diff = plScore[levelID].Leaderboard.Difficulty.DifficultyRaw;
                string diff = "";
                string diffColor = "#FFFFFF";
                switch (selectDiff)
                {
                    case 4:
                        diff = "Ex+";
                        diffColor = "#00FFFF";
                        break;
                    case 3:
                        diff = "Ex";
                        diffColor = "#00FF00";
                        break;
                    case 2:
                        diff = "H";
                        diffColor = "#FF8000";
                        break;
                    case 1:
                        diff = "N";
                        diffColor = "#808080";
                        break;
                    case 0:
                        diff = "E";
                        diffColor = "#808080";
                        break;
                }
                
                float acc = ((float)plScore[levelID+diffRaw].Score.BaseScore / plScore[levelID + diffRaw].Leaderboard.MaxScore) * 100;
                string accText = "";
                string accColor = "#FFFFFF";

                //set rank acc
                if (acc == 100) { accText = "SSS"; accColor = "#00FFFF"; }
                else if (acc >= 90.0F) { accText = "SS"; accColor = "#00FFFF"; }
                else if (acc >= 80.0F) { accText = "S"; accColor = "#00FF00"; }
                else if (acc >= 65.0F) { accText = "A"; accColor = "#00FF00"; }
                else if (acc >= 50.0F) { accText = "B"; accColor = "#FF8000"; }
                else if (acc >= 35.0F) { accText = "C"; accColor = "#FF8000"; }
                else if (acc >= 20.0F) { accText = "D"; accColor = "#FF0000"; }
                else if (acc < 20.0F) { accText = "E"; accColor = "#FF0000"; }

                ____songBpmText.text = $"<color={diffColor}>{diff}</color>(<color=#FFCC4E>★</color>{plScore[levelID + diffRaw].Leaderboard.Stars.ToString("F1")}) " +
                $"<color={accColor}>{accText}</color>(<color={accColor}>{acc.ToString("F1")}</color>%) " +
                $"<color=#FF0000>x</color>(<color=#FF0000>{(plScore[levelID + diffRaw].Score.MissedNotes + plScore[levelID + diffRaw].Score.BadCuts)}</color>)" +
                $" - <color=#00FF00>{plScore[levelID + diffRaw].Score.Pp.ToString("F1")}</color>pp";

                //set ui
                ____songAuthorText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5F, 0.5F);
                var bpmIcon = ____songAuthorText.transform.parent.Find("BpmIcon");
                if (bpmIcon != null) bpmIcon.gameObject.SetActive(false);
            }
            else
            {
                ____songAuthorText.GetComponent<RectTransform>().anchorMax = new Vector2(1F, 0.5F);
                var bpmIcon = ____songAuthorText.transform.parent.Find("BpmIcon");
                if (bpmIcon != null) bpmIcon.gameObject.SetActive(true);

            }
        }
    }
}
