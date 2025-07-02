using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必須

public class TitleSceneManager : MonoBehaviour
{
    // ボタンから呼び出すための公開メソッド
    public void StartGame()
    {
        // "GameScene"という名前のシーンを読み込む
        // ★あなたのシーン名が違う場合は、ここの名前を書き換えてください
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); 
    }
}