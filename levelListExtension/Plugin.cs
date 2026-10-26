using IPA;
using IPA.Config.Stores;
using System;
using System.Collections.Generic;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using Steamworks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using BeatSaberMarkupLanguage.Settings;
using levelListExtension.Settings;
using HarmonyLib;
using System.Reflection;
using levelListExtension.HarmonyPatches;
using BS_Utils.Utilities;
using System.Threading.Tasks;
using TMPro;
using BeatSaberPlaylistsLib.Types;

namespace levelListExtension
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private Harmony _harmony;

        [Init]
        public void Init(IPALogger logger,IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            Configuration.Instance = conf.Generated<Configuration>();
            BeatSaberMarkupLanguage.Util.MainMenuAwaiter.MainMenuInitializing += MainMenuInit;
        }

        public void MainMenuInit()
        {
            BSMLSettings.Instance.AddSettingsMenu("levelListExtension", "levelListExtension.Settings.settings.bsml", SettingsHandler.instance);
        }

        //both APIs reject page sizes above 100 and page numbers below 1
        private const int PageSize = 100;
        private static readonly HttpClient httpClient = new HttpClient();
        private string songFileName = Path.Combine(Directory.GetCurrentDirectory(), "UserData", "levelListExtension_Songs.json");
        [OnStart]
        public void OnApplicationStart()
        {
            _harmony = new Harmony("com.scifiHerb.BeatSaber.levelListExtension");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            new GameObject("levelListExtensionController").AddComponent<levelListExtensionController>();
            BSEvents.levelSelected += OnLevelSelected;

            //load songs
            Dictionary<string, PlayerScore> saved = null;
            if (File.Exists(songFileName)) saved = JsonConvert.DeserializeObject<Dictionary<string, PlayerScore>>(File.ReadAllText(songFileName));
            LevelList.plScore = saved ?? new Dictionary<string, PlayerScore>();

            PlaylistManager.Utilities.Events.playlistSelected += onPlaylistSelected;
            BSEvents.levelPackSelected += OnLevelPackSelected;
        }

        //playlistDiff only applies to the playlist it was loaded from
        private void OnLevelPackSelected(LevelSelectionNavigationController controller, BeatmapLevelPack pack)
        {
            if (!(pack is PlaylistLevelPack)) playlistDiff.Clear();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            File.WriteAllText(songFileName, JsonConvert.SerializeObject(LevelList.plScore));
        }

        private void onPlaylistSelected(IPlaylist p, BeatSaberPlaylistsLib.PlaylistManager pm)
        {
            if (pm == null) return;
            string filePath = Path.Combine(pm.PlaylistPath, p.Filename);

            //load playlist data
            string playlistDataText = "";
            if (File.Exists(filePath + ".bplist")) playlistDataText = File.ReadAllText(filePath + ".bplist");
            else if (File.Exists(filePath + ".json")) playlistDataText = File.ReadAllText(filePath + ".json");

            var playlistData = JsonConvert.DeserializeObject<PlaylistData.Root>(playlistDataText);

            loadPlaylist(playlistData);
        }

        public static Dictionary<string, string> playlistDiff = new Dictionary<string,string>();
        private void loadPlaylist(PlaylistData.Root playlistData)
        {
            playlistDiff.Clear();
            if (playlistData == null) { Log.Info("loadPlaylist data null"); return; }
            if (playlistData.songs == null) { Log.Info("loadPlaylist song null"); return; }

            foreach (var song in playlistData.songs)
            {
                if (song.hash == null || song.difficulties == null || song.difficulties.Count == 0) continue;

                var difficulty = song.difficulties[0];
                string name;
                if (!playlistDiffNames.TryGetValue(difficulty.name ?? "", out name)) name = difficulty.name;

                playlistDiff[song.hash.ToUpper()] = "_" + name + "_Solo" + difficulty.characteristic;
            }
        }

        private static readonly Dictionary<string, string> playlistDiffNames = new Dictionary<string, string>
        {
            { "easy", "Easy" },
            { "normal", "Normal" },
            { "hard", "Hard" },
            { "expert", "Expert" },
            { "expertPlus", "ExpertPlus" },
        };

        private void OnLevelSelected(LevelCollectionViewController lc, BeatmapLevel lv)
        {
            if (!Configuration.Instance.refresh)
            {
                if (Configuration.Instance.listChoice == "Score Saber") GetSongStats(1);
                else if (Configuration.Instance.listChoice == "Beat Leader") GetSongStatsBl(1);
            }
        }
        public static async void GetSongStats(int count, TextMeshProUGUI text = null)
        {
            string url = $"https://scoresaber.com/api/player/{SteamUser.GetSteamID()}/scores?sort=recent";
            int limit = count * 8;
            int pageSize = Math.Min(PageSize, limit);
            int loaded = 0;

            try
            {
                for (int page = 1; loaded < limit; page++)
                {
                    var response = await httpClient.GetAsync($"{url}&limit={pageSize}&page={page}");
                    if (!response.IsSuccessStatusCode) break;

                    var data = JsonConvert.DeserializeObject<PlayerScoresInfo>(await response.Content.ReadAsStringAsync());
                    if (data?.PlayerScores == null || data.PlayerScores.Count == 0) break;
                    if (data.Metadata != null && limit > data.Metadata.Total) limit = data.Metadata.Total;

                    foreach (var l in data.PlayerScores)
                    {
                        if (loaded >= limit) break;

                        //priority
                        string key = l.Leaderboard.SongHash + l.Leaderboard.Difficulty.DifficultyRaw;
                        PlayerScore existing;
                        if (!LevelList.plScore.TryGetValue(key, out existing) || Configuration.Instance.listChoice == "Score Saber" || existing.isScoreSaber == true)
                        {
                            l.isScoreSaber = true;
                            LevelList.plScore[key] = l;
                        }

                        loaded++;
                        if (text != null) text.text = $"({loaded}/{limit}) Loaded <color=#00FF00>{l.Leaderboard.SongName}</color>";
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Failed to load ScoreSaber scores: {e.Message}");
            }

            await ShowCompleted(text, 2000);
        }

        public static async void GetSongStatsBl(int count, TextMeshProUGUI text = null)
        {
            string url = $"https://api.beatleader.com/player/{SteamUser.GetSteamID()}/scores?sortBy=date";
            int limit = count * 8;
            int pageSize = Math.Min(PageSize, limit);
            int loaded = 0;

            try
            {
                for (int page = 1; loaded < limit; page++)
                {
                    var response = await httpClient.GetAsync($"{url}&count={pageSize}&page={page}");
                    if (!response.IsSuccessStatusCode) break;

                    var data = JsonConvert.DeserializeObject<BL.Root>(await response.Content.ReadAsStringAsync());
                    if (data?.data == null || data.data.Count == 0) break;
                    if (data.metadata?.total != null && limit > data.metadata.total) limit = (int)data.metadata.total;

                    foreach (var l in data.data)
                    {
                        if (loaded >= limit) break;

                        var song = l.leaderboard.song;
                        var difficulty = l.leaderboard.difficulty;
                        var plScore = new PlayerScore
                        {
                            isScoreSaber = false,
                            Score = new Score
                            {
                                Leaderboard = new Leaderboard(),
                                ModifiedScore = (int)(l.modifiedScore ?? 0),
                                MissedNotes = (int)(l.missedNotes ?? 0),
                                BadCuts = (int)(l.badCuts ?? 0),
                                Pp = l.pp ?? 0,
                                Modifiers = l.modifiers ?? "",
                            },
                            Leaderboard = new Leaderboard
                            {
                                SongHash = song.hash,
                                MaxScore = (int)(difficulty.maxScore ?? 0),
                                LevelAuthorName = song.mapper,
                                SongAuthorName = song.author,
                                SongName = song.name,
                                SongSubName = song.subName,
                                //stars is an untyped JSON number, so it can be a boxed long or double
                                Stars = difficulty.stars != null ? Convert.ToDouble(difficulty.stars) : 0,
                            },
                        };

                        string key = $"{song.hash.ToUpper()}_{difficulty.difficultyName}_Solo{difficulty.modeName}";

                        //priority
                        PlayerScore existing;
                        if (!LevelList.plScore.TryGetValue(key, out existing) || Configuration.Instance.listChoice == "Beat Leader" || existing.isScoreSaber != true)
                        {
                            LevelList.plScore[key] = plScore;
                        }

                        loaded++;
                        if (text != null) text.text = $"({loaded}/{limit}) Loaded <color=#00FF00>{song.name}</color>";
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"Failed to load BeatLeader scores: {e.Message}");
            }

            await ShowCompleted(text, 1000);
        }

        private static async Task ShowCompleted(TextMeshProUGUI text, int delay)
        {
            if (text == null) return;
            text.text += " - Completed";
            await Task.Delay(delay);
            text.text = "";
        }
    }
}
