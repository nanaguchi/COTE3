using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlanetData))]
public class PlanetDoubleClickHandler : MonoBehaviour
{
    private float lastClickTime = -1f;
    private readonly float doubleClickThreshold = 0.3f; 

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            HandleDoubleClick();
        }
        else
        {
            lastClickTime = Time.time;
        }
    }

    private void HandleDoubleClick()
    {
        // 1. この惑星のPlanetDataを取得
        PlanetData myData = GetComponent<PlanetData>();

        // 2. PlanetDataに設定されているシーン名が空でないか確認
        if (!string.IsNullOrEmpty(myData.detailSceneName))
        {
            Debug.Log(this.gameObject.name + " がダブルクリックされました。シーン「" + myData.detailSceneName + "」に遷移します。");
            
            // 3. PlanetDataに設定された専用のシーンをロードする
            UnityEngine.SceneManagement.SceneManager.LoadScene(myData.detailSceneName);
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " に遷移先のシーン名が設定されていません。");
        }
    }
}