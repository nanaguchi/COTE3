using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TooltipLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Camera mainCamera;
    private RectTransform tooltipRect;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void SetTooltipTarget(RectTransform tooltip)
    {
        tooltipRect = tooltip;
    }

    public void ClearTooltip()
    {
        tooltipRect = null;
    }

    void Update()
    {
        if (tooltipRect == null) return;

        Vector3 start = transform.position;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            tooltipRect,
            tooltipRect.position,
            mainCamera,
            out Vector3 end
        );

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        Debug.Log("Start: " + start + " | End: " + end);
    }
}