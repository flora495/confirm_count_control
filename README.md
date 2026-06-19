# ConfirmCountControl

Jump Kingのmodです。ポーズメニューの **Save & Exit**・**Restart**・**Give Up** を選んだときに出る「Are you sure?」の確認画面の回数を、それぞれ個別にカスタマイズできるようにします。バニラでは常に1回ですが、このmodでは0〜5回の間で自由に変更できます。

- `0` — 確認画面を一切出さず即座に実行
- `1` — バニラと同じ動作（デフォルト）
- `2`〜`5` — 指定した回数だけ「Are you sure?」画面が連続して表示される（各画面に `(1/3)` のように残り回数が表示される）。どの段階で「No」を選んでもポーズメニューまで一発で戻る

## 使い方

メインメニュー（ホーム画面）→ **Mods** → **ConfirmCountControl** を開き、

1. **Enabled** にチェックを入れる
2. **Save & Exit**・**Restart**・**Give Up** それぞれの回数を左右キーで設定する

設定はメインメニューからのみ行えます（ゲーム中のポーズメニューには設定項目は表示されません）。

設定はmod自身のdllと同じフォルダに `F.ConfirmCountControl.Settings.xml` として保存されます。

## 必須環境

Harmonyを使用していますが、`0Harmony.dll`はこのmodに同梱されているため、別途サブスクライブする必要はありません。

## プロジェクト構成

| ファイル | 役割 |
|---|---|
| `ModEntry.cs` | modのエントリーポイント（`[JumpKingMod]`）。設定の読み込み/保存と、メインメニューへの設定項目登録を行う |
| `Settings.cs` | 有効/無効フラグと各アクションの確認回数を保持する設定データ。XMLでの読み込み/保存 |
| `MenuFactoryPatches.cs` | `MenuFactory.CreateExitToMenu/CreateRestart/CreateGiveUp` へのHarmonyパッチ。設定された回数に応じて確認ポップアップの連鎖を再構築する |
| `InstantConfirmSelector.cs` | 回数が0のときに使われる、ポップアップなしで即座にアクションを実行するノード |
| `ConfirmCountOption.cs` | メニュー上で0〜5の回数を左右キーで選ぶオプション部品 |
| `EnabledToggle.cs` | mod自体の有効/無効を切り替えるチェックボックス部品 |
