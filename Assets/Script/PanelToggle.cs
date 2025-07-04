using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    // Inspectorから設定する
    public GameObject panelToShowAndHide;
    public GameObject hamburgerButton; // ★ハンバーガーボタンの参照を追加

    void Start()
    {
        // 初期状態ではパネルを非表示、ハンバーガーボタンを表示
        if (panelToShowAndHide != null)
        {
            panelToShowAndHide.SetActive(false);
        }
        if (hamburgerButton != null)
        {
            hamburgerButton.SetActive(true);
        }
    }

    // パネルを表示するメソッド（ハンバーガーボタンから呼び出す）
    public void ShowPanel()
    {
        if (panelToShowAndHide != null)
        {
            panelToShowAndHide.SetActive(true);
        }
        if (hamburgerButton != null)
        {
            hamburgerButton.SetActive(false);
        }
    }

    // パネルを非表示にするメソッド（パネル内の×ボタンから呼び出す）
    public void HidePanel()
    {
        if (panelToShowAndHide != null)
        {
            panelToShowAndHide.SetActive(false);
        }
        if (hamburgerButton != null)
        {
            hamburgerButton.SetActive(true);
        }
    }
}