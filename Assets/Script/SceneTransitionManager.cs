using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    // ボタンのOnClickイベントから、このメソッドを呼び出す
    // sceneNameには、Inspectorから遷移したいシーンの名前を指定する
    public void GoToScene(string sceneName)
    {
        if (!string.IsNullOrEmpty("godmodescreen"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("godmodescreen");
        }
        else
        {
            Debug.LogError("シーン名が指定されていません！");
        }
    }

    public void GoToScene1(string sceneName)
    {
        if (!string.IsNullOrEmpty("title2"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("title2");
        }
        else
        {
            Debug.LogError("シーン名が指定されていません！");
        }
    }

    public void GoToSampleScene(string sceneName)
    {
        if (!string.IsNullOrEmpty("title2"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.LogError("シーン名が指定されていません！");
        }
    }

}