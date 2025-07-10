// 変更前
// using UnityEngine.UI;
// public List<Text> tabs;

// 変更後
using UnityEngine;
// using UnityEngine.UI; // ←これはもう不要かもしれません
using TMPro; // ← これを追加！
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    // public List<Text> tabs; // ← この行をコメントアウトするか削除
    public List<TextMeshProUGUI> tabs; // ← この行に変更！

    public Color selectedColor = Color.white;
    public Color defaultColor = Color.gray;

    void Start()
    {
        OnTabSelected(0);
    }

    public void OnTabSelected(int selectedIndex)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == selectedIndex)
            {
                tabs[i].color = selectedColor;
            }
            else
            {
                tabs[i].color = defaultColor;
            }
        }
    }
}