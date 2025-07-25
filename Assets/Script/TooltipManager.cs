using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    private Camera mainCamera;
    private Transform targetTransform;
    private RectTransform panelRectTransform;

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
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);

        if (screenPos.z < 0f)
        {
            tooltipPanel.SetActive(false); 
            return;
        }

        screenPos.y += 100;
        screenPos.x += 100;

        float clampedX = Mathf.Clamp(screenPos.x, 0, Screen.width - panelRectTransform.rect.width);
        float clampedY = Mathf.Clamp(screenPos.y, 0, Screen.height - panelRectTransform.rect.height);

        panelRectTransform.position = new Vector3(clampedX, clampedY, 0);
    }
}

    
    public void ShowTooltip(string title, string description, Transform target)
{
    tooltipPanel.SetActive(true);
    titleText.text = title;
    descriptionText.text = description;
    targetTransform = target;
}

    public void HideTooltip()
{
    tooltipPanel.SetActive(false);
    targetTransform = null;
}
}