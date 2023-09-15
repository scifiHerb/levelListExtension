
using BeatSaberMarkupLanguage;
using HarmonyLib;
using levelListExtension;
using levelListExtension.UI;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
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
        private static TextMeshProUGUI packTitle = null;

        public static Dictionary<string, PlayerScore> plScore = new Dictionary<string, PlayerScore>();
        private static void Postfix(LevelListTableCell __instance, IPreviewBeatmapLevel level, bool isFavorite, ref UnityEngine.UI.Image ____favoritesBadgeImage,
            TextMeshProUGUI ____songBpmText, TextMeshProUGUI ____songAuthorText)
        {
            //get packTitle
            if (packTitle == null)
            {
                var levelsTableView = GameObject.Find("LevelsTableView");
                if (levelsTableView != null)
                {
                    var b = levelsTableView.transform.Find("TableView/Viewport/Content/LevelPackHeaderTableCell/PackName");
                    if (b != null) packTitle = b.GetComponent<TextMeshProUGUI>();
                }
            }
            bool isPlaylist = false;
            if (packTitle && packTitle.text != "Custom Levels") isPlaylist = true;
            //-----------------------------------
            var resultsView = Resources.FindObjectsOfTypeAll<LevelSelectionNavigationController>().FirstOrDefault();
            selectUI.instance.Create(resultsView);

            //return not custom level or mod disabled
            if (level.levelID.IndexOf("custom_level") == -1 || !Settings.Configuration.Instance.enable) return;

            string levelID = level.levelID.Substring(13);
            ____songBpmText.text = "";

            string diffRaw = "";
            int selectDiff = Settings.Configuration.Instance.selectDiff;

            //get song highScore
            //set difficulty name
            if (Plugin.playlistDiff.ContainsKey(levelID) && Settings.Configuration.Instance.priorityPlaylist && isPlaylist)
            {
                diffRaw = Plugin.playlistDiff[levelID];
            }
            else
            {
                selectDiff = Settings.Configuration.Instance.selectDiff;
                for (; selectDiff != -1; selectDiff--)
                {
                    switch (selectDiff)
                    {
                        case 4:
                            diffRaw = "_ExpertPlus_SoloLawless";
                            break;
                        case 3:
                            diffRaw = "_Expert_SoloLawless";
                            break;
                        case 2:
                            diffRaw = "_Hard_SoloLawless";
                            break;
                        case 1:
                            diffRaw = "_Normal_SoloLawless";
                            break;
                        case 0:
                            diffRaw = "_Easy_SoloLawless";
                            break;
                    }

                    if (plScore.ContainsKey(levelID + diffRaw)) goto endSelectDiff;
                }
                selectDiff = Settings.Configuration.Instance.selectDiff;
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

                    if (plScore.ContainsKey(levelID + diffRaw)) goto endSelectDiff;
                }

                selectDiff = Settings.Configuration.Instance.selectDiff;
                for (; selectDiff != -1; selectDiff--)
                {
                    switch (selectDiff)
                    {
                        case 4:
                            diffRaw = "_ExpertPlus_SoloOneSaber";
                            break;
                        case 3:
                            diffRaw = "_Expert_SoloOneSaber";
                            break;
                        case 2:
                            diffRaw = "_Hard_SoloOneSaber";
                            break;
                        case 1:
                            diffRaw = "_Normal_SoloOneSaber";
                            break;
                        case 0:
                            diffRaw = "_Easy_SoloOneSaber";
                            break;
                    }

                    if (plScore.ContainsKey(levelID + diffRaw)) goto endSelectDiff;
                }

                selectDiff = Settings.Configuration.Instance.selectDiff;
                for (; selectDiff != -1; selectDiff--)
                {
                    switch (selectDiff)
                    {
                        case 4:
                            diffRaw = "_ExpertPlus_Solo360Degree";
                            break;
                        case 3:
                            diffRaw = "_Expert_Solo360Degree";
                            break;
                        case 2:
                            diffRaw = "_Hard_Solo360Degree";
                            break;
                        case 1:
                            diffRaw = "_Normal_Solo360Degree";
                            break;
                        case 0:
                            diffRaw = "_Easy_Solo360Degree";
                            break;
                    }

                    if (plScore.ContainsKey(levelID + diffRaw)) goto endSelectDiff;
                }
            }
            endSelectDiff:

            if (plScore.ContainsKey(levelID+ diffRaw))
            {
                //setText
                //string diff = plScore[levelID].Leaderboard.Difficulty.DifficultyRaw;
                string diff = "";
                string diffColor = "#FFFFFF";
                switch (diffRaw)
                {
                    case "_ExpertPlus_SoloStandard":
                        diff = "Ex+";
                        diffColor = Settings.Configuration.Instance.Difficulty_ExPlus_Color;
                        break;
                    case "_Expert_SoloStandard":
                        diff = "Ex";
                        diffColor = Settings.Configuration.Instance.Difficulty_Ex_Color;
                        break;
                    case "_Hard_SoloStandard":
                        diff = "H";
                        diffColor = Settings.Configuration.Instance.Difficulty_Hard_Color;
                        break;
                    case "_Normal_SoloStandard":
                        diff = "N";
                        diffColor = Settings.Configuration.Instance.Difficulty_Normal_Color;
                        break;
                    case "_Easy_SoloStandard":
                        diff = "E";
                        diffColor = Settings.Configuration.Instance.Difficulty_Easy_Color;
                        break;
                }
                
                float acc = ((float)plScore[levelID+diffRaw].Score.ModifiedScore / plScore[levelID + diffRaw].Leaderboard.MaxScore) * 100;
                string accText = "";
                string accColor = "#FFFFFF";

                //set rank acc
                if (acc >= Settings.Configuration.Instance.Rank_SSS) { accText = "SSS"; accColor = Settings.Configuration.Instance.Rank_SSS_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_SSPlus) { accText = "SS+"; accColor = Settings.Configuration.Instance.Rank_SSPlus_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_SS) { accText = "SS"; accColor = Settings.Configuration.Instance.Rank_SS_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_SPlus) { accText = "S+"; accColor = Settings.Configuration.Instance.Rank_SPlus_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_S) { accText = "S"; accColor = Settings.Configuration.Instance.Rank_S_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_A) { accText = "A"; accColor = Settings.Configuration.Instance.Rank_A_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_B) { accText = "B"; accColor = Settings.Configuration.Instance.Rank_B_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_C) { accText = "C"; accColor = Settings.Configuration.Instance.Rank_C_Color; }
                else if (acc >= Settings.Configuration.Instance.Rank_D) { accText = "D"; accColor = Settings.Configuration.Instance.Rank_D_Color; }
                else if (acc < Settings.Configuration.Instance.Rank_D) { accText = "E"; accColor = Settings.Configuration.Instance.Rank_E_Color; }

                //star color
                string starColor = "";
                if (plScore[levelID + diffRaw].isScoreSaber == true) starColor = "#FFCC4E";
                else starColor = "#FF00FF";

                ____songBpmText.text = $"<color={diffColor}>{diff}</color>(<color={starColor}>★</color>{plScore[levelID + diffRaw].Leaderboard.Stars.ToString("F1")}) " +
                $"<color={accColor}>{accText}</color>(<color={accColor}>{acc.ToString("F1")}</color>%) ";

                //add modifiers
                if(plScore[levelID + diffRaw].Score.Modifiers!= "")____songBpmText.text += $"[{plScore[levelID + diffRaw].Score.Modifiers}]";
                //add missed notes,pp
                ____songBpmText.text += $"<color=#FF0000>x</color>(<color=#FF0000>{(plScore[levelID + diffRaw].Score.MissedNotes + plScore[levelID + diffRaw].Score.BadCuts)}</color>)" +
                $"<color=#00FF00>{plScore[levelID + diffRaw].Score.Pp.ToString("F1")}</color>pp";

                //add text
                if(Plugin.playlistDiff.ContainsKey(levelID))____songBpmText.text += "*";

                //set ui
                ____songAuthorText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5F, 0.5F);
                var bpmIcon = ____songAuthorText.transform.parent.Find("BpmIcon");
                if (bpmIcon != null) bpmIcon.gameObject.SetActive(false);

                var songAuthorText = ____songAuthorText.transform.parent.Find("SongAuthor");
                if (songAuthorText != null) songAuthorText.GetComponent<TextMeshProUGUI>().text = 
                        $"[<color=#00FF00>{plScore[levelID + diffRaw].Leaderboard.LevelAuthorName}</color>]";

                //debug
                /*Plugin.Log.Info($"pMod:{plScore[levelID + diffRaw].Leaderboard.PositiveModifiers}" +
                    $"modscore:{plScore[levelID + diffRaw].Score.ModifiedScore}" +
                    $"mod:{plScore[levelID + diffRaw].Score.Modifiers}");*/
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
