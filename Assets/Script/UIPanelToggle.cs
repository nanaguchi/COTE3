using UnityEngine;

public class UIPanelToggle : MonoBehaviour
{
    // Inspectorから設定パネルを割り当てる
    public GameObject panelToShowAndHide;

    // パネルを表示するメソッド（ハンバーガーボタンから呼び出す）
    public void ShowPanel()
    {
        if (panelToShowAndHide != null)
        {
            panelToShowAndHide.SetActive(true);
        }
    }

    // パネルを非表示にするメソッド（×ボタンから呼び出す）
    public void HidePanel()
    {
        if (panelToShowAndHide != null)
        {
            panelToShowAndHide.SetActive(false);
        }
    }
}