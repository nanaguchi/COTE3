using UnityEngine;

public class UIToggler : MonoBehaviour
{
    // Inspectorから、表示/非表示を切り替えたいUIの親オブジェクトを設定する
    public GameObject uiGroup;

    // ボタンのOnClickイベントからこの関数を呼び出す
    public void ToggleUIVisibility()
    {
        // uiGroupが設定されているか念のため確認
        if (uiGroup != null)
        {
            // 現在のアクティブ状態を取得し、その逆の状態を設定する
            // (もしアクティブなら非アクティブに、非アクティブならアクティブにする)
            bool isActive = uiGroup.activeSelf;
            uiGroup.SetActive(!isActive);
        }
    }
}