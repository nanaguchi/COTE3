using UnityEngine;

// このスクリプトが機能するには、ColliderとPlanetDataコンポーネントが必要
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlanetData))]
public class PlanetTooltipTrigger : MonoBehaviour
{
    private PlanetData planetData;

    void Awake()
    {
        // 自身が持つPlanetDataコンポーネントを取得
        planetData = GetComponent<PlanetData>();
    }

    // マウスカーソルがこのオブジェクトのコライダーに乗った時に呼ばれる
    void OnMouseEnter()
    {
        // TooltipManagerに、自身の情報を渡して表示を依頼する
        if (planetData != null)
        {
            TooltipManager.Instance.ShowTooltip(planetData.planetName, planetData.brief_info);
        }
    }

    // マウスカーソルがこのオブジェクトのコライダーから外れた時に呼ばれる
    void OnMouseExit()
    {
        // TooltipManagerに非表示を依頼する
        TooltipManager.Instance.HideTooltip();
    }
}