using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public Camera cam;
    public GameObject tooltipPanel;
    public Text tooltipText;

    private Transform currentTarget;

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var planetInfo = hit.collider.GetComponent<PlanetInfoHolder>();
            if (planetInfo != null)
            {
                // ツールチップ表示
                tooltipPanel.SetActive(true);

                // マウス位置から少し上に表示
                Vector3 screenPos = Input.mousePosition;
                tooltipPanel.transform.position = screenPos + new Vector3(10f, 20f, 0f);

                // brief_infoを表示
                tooltipText.text = planetInfo.planetData.brief_info;

                return;
            }
        }

        // 当たってなければ非表示
        tooltipPanel.SetActive(false);
    }
}