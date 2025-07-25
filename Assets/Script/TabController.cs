
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TabController : MonoBehaviour
{
    public List<TextMeshProUGUI> tabs; 

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