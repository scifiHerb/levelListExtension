using HarmonyLib;
using levelListExtension.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace levelListExtension.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelListTableCell))]
    [HarmonyPatch("SetDataFromLevelAsync", MethodType.Normal)]
    internal class LevelList
    {
        private static TextMeshProUGUI packTitle = null;

        public static Dictionary<string, PlayerScore> plScore = new Dictionary<string, PlayerScore>();

        //lookup order: characteristic first, then from the selected difficulty down to Easy
        private static readonly string[] characteristics = { "SoloLawless", "SoloStandard", "SoloOneSaber", "Solo360Degree" };
        private static readonly string[] difficulties = { "Easy", "Normal", "Hard", "Expert", "ExpertPlus" };
        private static readonly string[][] diffKeys = characteristics
            .Select(c => difficulties.Select(d => "_" + d + "_" + c).ToArray())
            .ToArray();

        //vanilla y positions of the cell texts, from the LevelListTableCell prefab (all anchored to the vertical center)
        private const float NameY = 1.2F;
        private const float AuthorY = -1.6F;
        //how far everything moves up to make room for the score line, half of LevelListCellSize.ExtraHeight
        private const float ScoredOffsetY = LevelListCellSize.ExtraHeight / 2;
        private const string ScoreTextName = "levelListExtensionScore";
        //score line column start positions, in percent of the line width
        private const string ColumnDiff = "0";
        //fixed slot after the widest difficulty label (Ex+) so the stars line up whatever the difficulty
        private const string ColumnStars = "7";
        private const string ColumnAcc = "22.5";
        private const string ColumnPp = "39";
        private const string ColumnMisses = "58.5";

        private static void Postfix(LevelListTableCell __instance, BeatmapLevel beatmapLevel, bool isFavorite, ref Image ____favoritesBadgeImage,
            ref TextMeshProUGUI ____songNameText, ref TextMeshProUGUI ____songDurationText,
            ref TextMeshProUGUI ____songBpmText, ref TextMeshProUGUI ____songAuthorText)
        {
            var config = Settings.Configuration.Instance;

            //cells are reused, so start from the vanilla layout every time
            var scoreText = GetScoreText(__instance, ____songBpmText);
            SetLayout(scoreText, ____songNameText, ____songDurationText, ____songAuthorText, ____songBpmText, false);

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
            bool isPlaylist = packTitle && packTitle.text != "Custom Levels";
            //-----------------------------------
            if (!selectUI.instance.IsCreated)
            {
                var resultsView = Resources.FindObjectsOfTypeAll<LevelSelectionNavigationController>().FirstOrDefault();
                selectUI.instance.Create(resultsView);
            }

            //return not custom level or mod disabled
            if (!config.enable || !beatmapLevel.levelID.StartsWith("custom_level", System.StringComparison.Ordinal)) return;

            string levelID = beatmapLevel.levelID.Substring(13);

            //get song highScore
            //set difficulty name
            string diffRaw = null;
            PlayerScore score = null;
            if (config.priorityPlaylist && isPlaylist && Plugin.playlistDiff.TryGetValue(levelID, out diffRaw))
            {
                plScore.TryGetValue(levelID + diffRaw, out score);
            }
            else
            {
                int maxDiff = Mathf.Clamp(config.selectDiff, 0, difficulties.Length - 1);
                foreach (var keys in diffKeys)
                {
                    for (int d = maxDiff; d >= 0; d--)
                    {
                        if (plScore.TryGetValue(levelID + keys[d], out score))
                        {
                            diffRaw = keys[d];
                            goto endSelectDiff;
                        }
                    }
                }
            }
            endSelectDiff:

            if (score != null)
            {
                //setText
                string diff = "";
                string diffColor = "#FFFFFF";
                switch (diffRaw)
                {
                    case "_ExpertPlus_SoloStandard":
                        diff = "Ex+";
                        diffColor = config.Difficulty_ExPlus_Color;
                        break;
                    case "_Expert_SoloStandard":
                        diff = "Ex";
                        diffColor = config.Difficulty_Ex_Color;
                        break;
                    case "_Hard_SoloStandard":
                        diff = "H";
                        diffColor = config.Difficulty_Hard_Color;
                        break;
                    case "_Normal_SoloStandard":
                        diff = "N";
                        diffColor = config.Difficulty_Normal_Color;
                        break;
                    case "_Easy_SoloStandard":
                        diff = "E";
                        diffColor = config.Difficulty_Easy_Color;
                        break;
                }

                float acc = ((float)score.Score.ModifiedScore / score.Leaderboard.MaxScore) * 100;
                string accColor;

                //accuracy color from the rank thresholds
                if (acc >= config.Rank_SSS) accColor = config.Rank_SSS_Color;
                else if (acc >= config.Rank_SSPlus) accColor = config.Rank_SSPlus_Color;
                else if (acc >= config.Rank_SS) accColor = config.Rank_SS_Color;
                else if (acc >= config.Rank_SPlus) accColor = config.Rank_SPlus_Color;
                else if (acc >= config.Rank_S) accColor = config.Rank_S_Color;
                else if (acc >= config.Rank_A) accColor = config.Rank_A_Color;
                else if (acc >= config.Rank_B) accColor = config.Rank_B_Color;
                else if (acc >= config.Rank_C) accColor = config.Rank_C_Color;
                else if (acc >= config.Rank_D) accColor = config.Rank_D_Color;
                else accColor = config.Rank_E_Color;

                //star color
                string starColor = score.isScoreSaber == true ? "#FFCC4E" : "#FF00FF";

                //build the whole line at once so TMP only re-parses it once
                //each column starts at a fixed fraction of the line width so they line up across rows
                var sb = new System.Text.StringBuilder(256);
                sb.Append($"<pos={ColumnDiff}%><color={diffColor}>{diff}</color><pos={ColumnStars}%>(<color={starColor}>★</color>{score.Leaderboard.Stars.ToString("F1")})");
                sb.Append($"<pos={ColumnAcc}%><color={accColor}>{acc.ToString("F2")}%</color>");
                //add pp, then missed notes with modifiers and the playlist marker after them
                sb.Append($"<pos={ColumnPp}%><color=#00FF00>{score.Score.Pp.ToString("F2")}</color>pp");
                sb.Append($"<pos={ColumnMisses}%><color=#FF0000>x</color>(<color=#FF0000>{score.Score.MissedNotes + score.Score.BadCuts}</color>)");
                if (!string.IsNullOrEmpty(score.Score.Modifiers)) sb.Append($" [{score.Score.Modifiers}]");
                if (isPlaylist && Plugin.playlistDiff.ContainsKey(levelID)) sb.Append('*');
                scoreText.text = sb.ToString();

                //artist and mapper share the second line, the score gets its own line below
                //built from the level instead of the current text, since the game skips resetting it when the same level is set twice
                ____songAuthorText.text = $"{beatmapLevel.songAuthorName} [<color=#00FF00>{score.Leaderboard.LevelAuthorName}</color>]";
                SetLayout(scoreText, ____songNameText, ____songDurationText, ____songAuthorText, ____songBpmText, true);
            }
        }

        private static TextMeshProUGUI GetScoreText(LevelListTableCell cell, TextMeshProUGUI songBpmText)
        {
            var existing = cell.transform.Find(ScoreTextName);
            if (existing != null) return existing.GetComponent<TextMeshProUGUI>();

            //clone the bpm text so the new line uses the same font and material
            var scoreText = Object.Instantiate(songBpmText, songBpmText.transform.parent, false);
            scoreText.name = ScoreTextName;
            scoreText.alignment = TextAlignmentOptions.MidlineLeft;
            scoreText.overflowMode = TextOverflowModes.Ellipsis;

            //full width under the author, from the author's left edge to the cell's right edge
            var rect = scoreText.rectTransform;
            var authorRect = cell.transform.Find("SongAuthor") as RectTransform;
            float left = authorRect != null ? authorRect.anchoredPosition.x : 9.28F;
            rect.anchorMin = new Vector2(0F, 0.5F);
            rect.anchorMax = new Vector2(1F, 0.5F);
            rect.pivot = new Vector2(0F, 0.5F);
            //same height as the author text, Ellipsis hides the whole line if it is taller than the rect
            rect.sizeDelta = new Vector2(-left - 2F, authorRect != null ? authorRect.sizeDelta.y : 6F);
            rect.anchoredPosition = new Vector2(left, AuthorY + ScoredOffsetY - LevelListCellSize.ExtraHeight);
            return scoreText;
        }

        private static void SetLayout(TextMeshProUGUI scoreText, TextMeshProUGUI songNameText, TextMeshProUGUI songDurationText,
            TextMeshProUGUI songAuthorText, TextMeshProUGUI songBpmText, bool scored)
        {
            float offset = scored ? ScoredOffsetY : 0F;
            SetY(songNameText.rectTransform, NameY + offset);
            SetY(songDurationText.rectTransform, NameY + offset);
            SetY(songAuthorText.rectTransform, AuthorY + offset);
            SetY(songBpmText.rectTransform, AuthorY + offset);
            var bpmIcon = songBpmText.transform.parent.Find("BpmIcon") as RectTransform;
            if (bpmIcon != null) SetY(bpmIcon, AuthorY + offset);

            scoreText.gameObject.SetActive(scored);
            if (!scored) scoreText.text = "";
        }

        private static void SetY(RectTransform rect, float y)
        {
            var pos = rect.anchoredPosition;
            if (pos.y != y) rect.anchoredPosition = new Vector2(pos.x, y);
        }
    }

    //makes song rows one text line taller so the score fits under the artist/mapper line
    [HarmonyPatch(typeof(LevelCollectionTableView), nameof(LevelCollectionTableView.CellSize))]
    internal class LevelListCellSize
    {
        //same spacing as between the song name and author lines
        public const float ExtraHeight = 2.8F;

        private static void Postfix(int idx, bool ____showLevelPackHeader, ref float __result)
        {
            if (__result <= 0F || !Settings.Configuration.Instance.enable) return;
            if (idx == 0 && ____showLevelPackHeader) return;
            __result += ExtraHeight;
        }
    }
}
