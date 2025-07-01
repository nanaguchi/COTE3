using UnityEngine;
using UnityEngine.EventSystems;

public class CelestialBody : MonoBehaviour
{
    public PlanetData planetData;

    void OnMouseEnter()
    {   
        if (planetData != null)
    {
        TooltipManager.Instance.ShowTooltip(planetData, this.transform); // 自分自身を渡す
    }
}

    void OnMouseExit()
    {
        TooltipManager.Instance.HideTooltip();  
    }
}