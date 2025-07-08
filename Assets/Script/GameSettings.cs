using UnityEngine;
using UnityEngine.UI; // UIを使うために必須
using System.Linq; // Linqを使うために必須

public class GameSettings : MonoBehaviour
{
    // InspectorからToggle Groupをアタッチする
    public ToggleGroup meteoriteSizeGroup;

    // ボタンが押されたときに呼び出される関数
    public void OnSizeChanged()
    {
        // 現在選択されているToggleのラベル名を取得して表示する
        Toggle activeToggle = meteoriteSizeGroup.GetFirstActiveToggle();
        if (activeToggle != null)
        {
            // Toggleの子にあるLabelコンポーネントのテキストを取得
            string selectedLabel = activeToggle.GetComponentInChildren<Text>().text;
            Debug.Log("選択されたサイズ: " + selectedLabel);
        }
    }
}