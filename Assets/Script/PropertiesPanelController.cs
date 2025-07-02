using UnityEngine;
using UnityEngine.UI; // Buttonを扱うために必要

public class PropertiesPanelController : MonoBehaviour
{
    [Header("タブ設定")]
    [Tooltip("重力、温度などのタブボタンを順番に設定")]
    public Button[] tabButtons;
    [Tooltip("各タブのアンダーラインのGameObjectを順番に設定")]
    public GameObject[] underlines;

    [Header("パネル設定")]
    [Tooltip("スライダーなどが入っているパネル本体")]
    public GameObject propertiesPanel; // スライダー群の親パネル

    void Start()
    {
        // 最初は0番目のタブが選択されている状態にする
        SelectTab(0);
    }

    // タブを選択するメソッド
    public void SelectTab(int tabIndex)
    {
        // 全てのアンダーラインを一旦非表示にする
        for (int i = 0; i < underlines.Length; i++)
        {
            underlines[i].SetActive(false);
        }

        // 指定されたタブのアンダーラインだけを表示する
        if (tabIndex < underlines.Length)
        {
            underlines[tabIndex].SetActive(true);
        }
        
        // TODO: ここに、将来的にスライダーの機能を切り替える処理を追加します
    }

    // パネルの表示・非表示を切り替えるメソッド
    public void TogglePanelVisibility()
    {
        if (propertiesPanel != null)
        {
            // 現在の状態と逆の状態にする（表示なら非表示に、非表示なら表示に）
            propertiesPanel.SetActive(!propertiesPanel.activeSelf);
        }
    }
}