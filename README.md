# levelListExtension

![Beat Saber 2023_08_29 20_02_03](https://github.com/scifiHerb/levelListExtension/assets/109839172/453351c5-9490-4263-9be2-8c712733be32)
![Beat Saber 2023_09_16 17_59_56](https://github.com/scifiHerb/levelListExtension/assets/109839172/68ce7f66-6102-4f5a-ad65-08479f3cf6b0)

ビートセイバーの譜面リスト拡張MODです、指定した難易度のハイスコア(存在しない場合下の難易度）をリストに表示します.

対応バージョンはBeatsaberの1.29.1、SteamVR版のみの対応です。

# 使い方
起動後初回の譜面選択画面で曲の読み込みをします。  
読み込まれた譜面情報は随時取得し、ファイルに保存しているので二度目以降、譜面のクリア後には必要ありません  

# 設定項目 (\Beat Saber\UserData\levelListExtension.json)  
{  
  "Enable": true,    MODのOn Off  
  "refresh":譜面リストの再読み込みをするかどうかです、読み込み後自動的にfalseになります  
  "priorityPlaylist":プレイリストに難易度が設定されている場合selectDiffに関わらず設定されている難易度を読みに行きます  
  "listChoice":ScoreSaber優先かBeatLeader優先か選べます  

  "selectDiff": 4,   指定難易度（0=easy,4=expert plus)  
  
  "count": 200       取得譜面数（一度に8譜面取得するのでcount*8譜面取得します)  
  "Rank~":各ランクの精度値を設定できます  
  "Rank_*Color~":ランクの色をそれぞれ設定できます  
  "Difficulty~":各難易度の色設定が出来ます。  
}

# English Translate
This is an extension mod for Beat Saber, which displays the high score (or the score from a lower difficulty if it doesn't exist) for a specified difficulty in the song list.  

The supported version is Beat Saber 1.29.1, and it's compatible with the SteamVR version only.  

# Usage
Upon launching, you should load a song on the song selection screen. The loaded song information is continuously retrieved and saved in a file, so you won't need to do it again after the first time.  

# Configuration (\Beat Saber\UserData\levelListExtension.json)  
{  
"Enable": true, : Turn the MOD on or off.  
"refresh" : Determines whether to reload the song list. It automatically sets to false after loading.   
"priorityPlaylist" : When a playlist has a difficulty set, it will prioritize the set difficulty, regardless of selectDiff.  
"listChoice" : Choose between ScoreSaber or BeatLeader as a priority.  
"selectDiff": 4, : The specified difficulty (0=easy, 4=expert plus).  
"count": 200, : Number of songs to retrieve (8 songs are retrieved at once, so it's count8 songs in total).  
"Rank~" : You can set accuracy values for each rank.  
"Rank_Color~" : You can set colors for each rank.  
"Difficulty~" : You can set colors for each difficulty.  
}



