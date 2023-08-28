# levelListExtension
ビートセイバーの譜面リスト拡張MODです、指定した難易度のハイスコア(存在しない場合下の難易度）をリストに表示します.
対応バージョンは1.29.1までなのと、新ノーツを含む譜面(V3)はScoreSaberにデータがないため取得できません。

# 使い方
ModSettingsからlevelListExtensionのrefreshボタンで過去のプレイ履歴を一括取得します、取得はバックグラウンドで行うので設定から抜けても問題ありません。
取得数はデフォルトで800ですが設定ファイルのCount数から指定できます。


# 設定項目 (\Beat Saber\UserData\levelListExtension.json)
{
  "Enable": true,    MODのOn Off

  "selectDiff": 4,   指定難易度（0=easy,4=expert plus)
  
  "count": 100       取得譜面数（一度に8譜面取得するのでcount*8譜面取得します)
}

![Beat Saber 2023_08_28 16_51_44](https://github.com/scifiHerb/levelListExtension/assets/109839172/c679eb60-e465-4cf1-bbfb-8fbf755a0b6c)


