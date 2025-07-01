using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private Camera mainCamera;
    private Transform targetTransform; // 表示対象（天体）
    private RectTransform panelRectTransform;
    private TooltipLineRenderer tooltipLine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        panelRectTransform = tooltipPanel.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        tooltipPanel.SetActive(false);
    }

    void Update()
    {
        if (tooltipPanel.activeSelf && targetTransform != null)
        {
            // 天体のワールド座標 → スクリーン座標に変換
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);

            // ツールチップを少し上に表示（+Y方向）
            screenPos.y += 100;

            // UIパネル位置に変換
            panelRectTransform.position = screenPos;
        }
    }

    public void ShowTooltip(PlanetData planetData, Transform target)
{
    titleText.text = planetData.planetName;
    descriptionText.text = planetData.brief_info;

    targetTransform = target;
    tooltipPanel.SetActive(true);

    tooltipLine = target.GetComponent<TooltipLineRenderer>();
    if (tooltipLine != null)
    {
        tooltipLine.SetTooltipTarget(panelRectTransform);
    }
}

    public void HideTooltip()
{
    tooltipPanel.SetActive(false);
    targetTransform = null;

    if (tooltipLine != null)
    {
        tooltipLine.ClearTooltip();
        tooltipLine = null;
    }
}
}