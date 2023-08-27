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

        [OnStart]
        public void OnApplicationStart()
        {
            _harmony = new Harmony("com.scifiHerb.BeatSaber.levelListExtension");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            Log.Debug("OnApplicationStart");
            new GameObject("levelListExtensionController").AddComponent<levelListExtensionController>();
            BSEvents.menuSceneActive += OnMenuSceneActive;
            //https://scoresaber.com/api/player/76561199194622414/scores


            string songFileName = Directory.GetCurrentDirectory() + "\\UserData\\levelListExtension_Songs.json";
            if (!File.Exists(songFileName))File.Create(songFileName);
            string str = File.ReadAllText(songFileName);
            if(str == "")LevelList.plScore = new Dictionary<string, PlayerScore>();
            else
            {
                LevelList.plScore = JsonConvert.DeserializeObject<Dictionary<string, PlayerScore>>(str);
            }
            
            //GetSongStats($"https://scoresaber.com/api/player/{SteamUser.GetSteamID()}",100);
        }

        private void OnMenuSceneActive()
        {
            Plugin.Log.Info("menu scene active");
            GetSongStats(1);
        }

        private static int songcount = 0;
        private string songFileName = "\\UserData\\levelListExtension_Songs.json";
        public static async void GetSongStats(int count,TextMeshProUGUI text = null)
        {
            string url = $"https://scoresaber.com/api/player/{SteamUser.GetSteamID()}";
            var httpClient = new HttpClient();
            songcount = 0;

            for (int i = 0; i < count; i++)
            {
                var response = await httpClient.GetAsync(url + "/scores?sort=recent&page="+i); // 非同期にWebリクエストを送信する
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync(); // 非同期にレスポンスを読み込む
                    if (result == null) return;

                    var data = JsonConvert.DeserializeObject<PlayerScoresInfo>(result);

                    foreach (var l in data.PlayerScores)
                    {
                        LevelList.plScore.Remove(l.Leaderboard.SongHash + l.Leaderboard.Difficulty.DifficultyRaw);
                        LevelList.plScore.Add(l.Leaderboard.SongHash + l.Leaderboard.Difficulty.DifficultyRaw, l);

                        songcount++;
                        if(text != null)text.text = $"({songcount.ToString()}/{(count*8).ToString()}) " + $"Loaded:<color=#00FF00>{l.Leaderboard.SongName}</color>";
                    }
                }
                if (i != 0 && i % 10 == 0) await Task.Delay(1000);
            }
            if (text != null) text.text += " - Completed";
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

            File.WriteAllText(Directory.GetCurrentDirectory() + songFileName, JsonConvert.SerializeObject(LevelList.plScore));
        }
    }
}
