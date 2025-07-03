using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionOnDoubleClick : MonoBehaviour
{
    private float lastClickTime = 0f;
    private const float doubleClickThreshold = 0.25f;

    void OnMouseDown()
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick <= doubleClickThreshold)
        {
            // ダブルクリック検知 → 遷移先シーン名を生成
            string sceneName = gameObject.name + "_celestialscreen";

            if (IsSceneAvailable(sceneName))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); 
            }
            else
            {
                Debug.LogWarning("シーン '" + sceneName + "' は Build Settings に存在しません。");
            }
        }

        lastClickTime = Time.time;
    }

    private bool IsSceneAvailable(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneFileName == sceneName)
                return true;
        }
        return false;
    }
}