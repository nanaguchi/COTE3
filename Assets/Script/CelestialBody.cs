using UnityEngine;
using UnityEngine.EventSystems;

public class CelestialBody : MonoBehaviour
{
    public PlanetData planetData;

    void OnMouseEnter()
    {   
        if (planetData != null)
        {
            TooltipManager.Instance.ShowTooltip(
                planetData.planetName,       // タイトル
                planetData.brief_info,       // 説明
                this.transform               // 自分のTransform
            );
        }
    }

    void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();  
    }
}