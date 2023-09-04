using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using Steamworks;
using HMUI;
using System.IO;

using System.Net.Http;
using Newtonsoft.Json;
using BeatSaberMarkupLanguage.Settings;
using levelListExtension.Settings;
using HarmonyLib;
using System.Reflection;
using levelListExtension.HarmonyPatches;
using BS_Utils.Utilities;
using IPA.Utilities;
using System.Threading.Tasks;
using TMPro;
using System.Security.Policy;
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
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger,IPA.Config.Config conf)
        {
            Instance = this;
            Log = logger;
            Log.Info("levelListExtension initialized.");
            Configuration.Instance = conf.Generated<Configuration>();
            BSMLSettings.instance.AddSettingsMenu("levelListExtension", "levelListExtension.Settings.settings.bsml", SettingsHandler.instance);
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion


        private static int songcount = 0;
        private string settingsName = Directory.GetCurrentDirectory() + "\\UserData\\levelListExtension.json";
        private string songFileName = Directory.GetCurrentDirectory() + "\\UserData\\levelListExtension_Songs.json";
        [OnStart]
        public void OnApplicationStart()
        {
            _harmony = new Harmony("com.scifiHerb.BeatSaber.levelListExtension");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Debug("OnApplicationStart");
            new GameObject("levelListExtensionController").AddComponent<levelListExtensionController>();
            BSEvents.levelSelected += OnLevelSelected;

            //load songs
            string songsText = "";
            LevelList.plScore = null;


            if (File.Exists(songFileName)) songsText = File.ReadAllText(songFileName);
            else songsText = "";
            LevelList.plScore = JsonConvert.DeserializeObject<Dictionary<string, PlayerScore>>(songsText);

            if (LevelList.plScore == null)
                LevelList.plScore = LevelList.plScore = new Dictionary<string, PlayerScore>();

            PlaylistManager.Utilities.Events.playlistSelected += onPlaylistSelected;
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

            File.WriteAllText(songFileName, JsonConvert.SerializeObject(LevelList.plScore));

        }
        public static TextMeshProUGUI packTitle = null;
        private void onPlaylistSelected(IPlaylist p, BeatSaberPlaylistsLib.PlaylistManager pm)
        {
            if (pm == null) return;
            string filePath = pm.PlaylistPath + "\\" + p.Filename;

            //load playlist data
            string playlistDataText = "";
            if (File.Exists(filePath + ".bplist")) playlistDataText = File.ReadAllText(filePath + ".bplist");
            else if (File.Exists(filePath + ".json")) playlistDataText = File.ReadAllText(filePath + ".json");
            else playlistDataText = "";

            var playlistData = JsonConvert.DeserializeObject<PlaylistData.Root>(playlistDataText);

            loadPlaylist(playlistData);
        }

        public static Dictionary<string, string> playlistDiff = new Dictionary<string,string>();
        private void loadPlaylist(PlaylistData.Root playlistData)
        {
            if (playlistData == null) { Plugin.Log.Info("loadPlaylist data null"); return; }
            if (playlistData.songs == null) { Plugin.Log.Info("loadPlaylist song null"); return; }
            playlistDiff = new Dictionary<string, string>();

            foreach (var song in playlistData.songs)
            {
                if (song.difficulties != null && song.difficulties != null && song.difficulties.Count >= 1)
                {
                    switch (song.difficulties[0].name)
                    {
                        case "easy":
                            song.difficulties[0].name = "Easy";
                            break;
                        case "normal":
                            song.difficulties[0].name = "Normal";
                            break;
                        case "hard":
                            song.difficulties[0].name = "Hard";
                            break;
                        case "expert":
                            song.difficulties[0].name = "Expert";
                            break;
                        case "expertPlus":
                            song.difficulties[0].name = "ExpertPlus";
                            break;
                    }
                    song.difficulties[0].characteristic = "Solo" + song.difficulties[0].characteristic;

                    playlistDiff.Remove(song.hash.ToUpper());
                    playlistDiff.Add(song.hash.ToUpper(), "_" + song.difficulties[0].name + "_" + song.difficulties[0].characteristic);
                }
                
            }

        }

        private void OnLevelSelected(LevelCollectionViewController lc,IPreviewBeatmapLevel lv)
        {
            if(!Settings.Configuration.Instance.refresh) GetSongStats(1);
        }
        public static async void GetSongStats(int count,TextMeshProUGUI text = null)
        {
            string url = $"https://scoresaber.com/api/player/{SteamUser.GetSteamID()}";
            var httpClient = new HttpClient();
            if(Settings.Configuration.Instance.refresh) songcount = 0;
            
            for (int i = 0; i < count; i++)
            {
                var response = await httpClient.GetAsync(url + "/scores?sort=recent&page="+i); // 非同期にWebリクエストを送信する
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync(); // 非同期にレスポンスを読み込む
                    if (result == null)return;

                    var data = JsonConvert.DeserializeObject<PlayerScoresInfo>(result);
                    var limit = count * 8;
                    if (limit > data.Metadata.Total) limit = data.Metadata.Total;

                    foreach (var l in data.PlayerScores)
                    {
                        LevelList.plScore.Remove(l.Leaderboard.SongHash + l.Leaderboard.Difficulty.DifficultyRaw);
                        LevelList.plScore.Add(l.Leaderboard.SongHash + l.Leaderboard.Difficulty.DifficultyRaw, l);

                        songcount++;

                        if(text != null)text.text = $"({songcount.ToString()}/{(limit).ToString()}) " + $"Loaded <color=#00FF00>{l.Leaderboard.SongName}</color>";
                        //Plugin.Log.Info($"({songcount.ToString()}/{limit.ToString()}) " + $"Loaded <color=#00FF00>{l.Leaderboard.SongName}</color>");

                        if (songcount >= limit)
                        {
                            if (text != null)
                            {
                                text.text += " - Completed";
                                await Task.Delay(2000);
                                text.text = "";
                                return;
                            }
                        }
                    }
                }
                if (i != 0 && i % 30 == 0) await Task.Delay(3000);
            }
        }
    }
}
