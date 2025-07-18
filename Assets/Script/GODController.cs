using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    // Inspectorから設定する項目
    [SerializeField] private ToggleGroup toggleGroup; // ラジオボタンのグループ
    [SerializeField] private VideoPlayer videoPlayer;  // 動画プレイヤー
    [SerializeField] private GameObject videoScreen;  // 動画を再生するRawImageのGameObject

    // 動画が再生中かどうかを管理するフラグ
    private bool isPlaying = false;

    void Start()
    {
        // 最初は動画スクリーンを非表示にしておく
        videoScreen.SetActive(false);
    }

    void Update()
    {
        // Enterキーが押された瞬間を検出
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // もし動画が再生中なら
            if (isPlaying)
            {
                // 動画を停止する
                StopVideo();
            }
            // まだ再生されておらず、かつラジオボタンが選択されているなら
            else if (toggleGroup.AnyTogglesOn())
            {
                // 動画を再生する
                PlayVideo();
            }
        }
    }

    void PlayVideo()
    {
        // 再生中フラグを立てる
        isPlaying = true;

        // 動画スクリーンを表示する
        videoScreen.SetActive(true);

        // 動画を再生する
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    void StopVideo()
    {
        // 再生中フラグを下ろす
        isPlaying = false;

        // 動画スクリーンを非表示にする
        videoScreen.SetActive(false);

        // 動画を停止する
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }
}