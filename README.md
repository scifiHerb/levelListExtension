# levelListExtension
ビートセイバーの譜面リスト拡張MODです、指定した難易度のハイスコア(存在しない場合下の難易度）をリストに表示します.

# 使い方


・ModSettingsからlevelListExtensionのrefreshボタンで過去にプレイした譜面の一括取得ができます。

・一括取得の際履歴の最後まで読み込むとその時点で停止します、ボタンを押した後はSettingから抜けて大丈夫です。

・難易度は譜面リスト右上のボタンから順送りで指定して、指定した難易度のデータがない場合はその下の難易度から取得します。

対応バージョンは1.29.1です、最新版では動作しません。

ScoreSaberからの取得なので新ノーツ（V3)譜面はデータが無いため取得できません。

# 設定項目
{
  "Enable": true,    MODのOn Off
  "selectDiff": 4,   指定難易度（0=easy,4=expert plus)
  "count": 100       取得譜面数（一度に8譜面取得するのでcount*8譜面取得します)
}

![Beat Saber 2023_08_28 16_51_44](https://github.com/scifiHerb/levelListExtension/assets/109839172/c679eb60-e465-4cf1-bbfb-8fbf755a0b6c)


