using UnityEngine;
using TMPro; // TextMeshProを扱うために必要

public class TooltipManager : MonoBehaviour
{
    // シングルトン：どこからでもこのマネージャーにアクセスできるようにする
    public static TooltipManager Instance;

    [Header("UI要素")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private RectTransform panelRectTransform;

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        panelRectTransform = tooltipPanel.GetComponent<RectTransform>();
        tooltipPanel.SetActive(false); // 念のため非表示にしておく
    }

    void Update()
    {
        // ツールチップが表示されている間、マウスカーソルに追従させる
        if (tooltipPanel.activeSelf)
        {
            // マウス位置をUIのローカル座標に変換して、パネルの位置を更新
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// ツールチップを表示する
    /// </summary>
    /// <param name="title">表示するタイトル</param>
    /// <param name="description">表示する説明文</param>
    public void ShowTooltip(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
        tooltipPanel.SetActive(true);
    }

    /// <summary>
    /// ツールチップを非表示にする
    /// </summary>
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}