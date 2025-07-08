using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // シーン管理のために必要
using System.Collections; // コルーチン(WaitForSeconds)のために必要

public class SoundEffectPlayer : MonoBehaviour // クラス名とファイル名が一致していることを確認
{
    [SerializeField] private Slider bgmSlider;    // BGM用スライダー
    [SerializeField] private AudioSource bgmAudio; // BGMのAudioSource

    [SerializeField] private Slider seSlider;     // SE用スライダー
    [SerializeField] private AudioSource seAudio;  // SEのAudioSource (音量調整用)

    [SerializeField] private AudioClip clickSound; // リセットボタンなどの一般的なクリック音
    [SerializeField] private AudioClip titleButtonSound; // タイトルボタン専用の音
    [SerializeField] private string titleSceneName = "Title"; // 遷移先のタイトルシーン名 (Inspectorで設定)

    void Start()
    {
        // BGMスライダーのイベントリスナー設定
        if (bgmSlider != null && bgmAudio != null)
        {
            bgmSlider.onValueChanged.AddListener(UpdateBGMVolume);
            UpdateBGMVolume(bgmSlider.value);
        }
        else
        {
            Debug.LogWarning("BGM Slider or AudioSource not assigned for BGM in SoundEffectPlayer.", this);
        }

        // SEスライダーのイベントリスナー設定
        if (seSlider != null && seAudio != null)
        {
            seSlider.onValueChanged.AddListener(UpdateSEVolume);
            UpdateSEVolume(seSlider.value);
        }
        else
        {
            Debug.LogWarning("SE Slider or AudioSource not assigned for SE in SoundEffectPlayer.", this);
        }

        // seAudioがnullの場合の警告 (音量調整はできないが、シーン遷移時のSEは鳴らせる)
        if (seAudio == null)
        {
            Debug.LogError("SE AudioSource (seAudio) is not assigned in SoundEffectPlayer. SE volume will not be controllable.", this);
        }
    }

    void UpdateBGMVolume(float value)
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = value;
        }
    }

    void UpdateSEVolume(float value)
    {
        if (seAudio != null)
        {
            seAudio.volume = value;
        }
    }

    public void PlayClickSE()
    {
        if (seAudio != null && clickSound != null)
        {
            seAudio.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("SE AudioSource or Click Sound not assigned for PlayClickSE.", this);
        }
    }

    // タイトルへ戻るSEを再生し、その後シーンをロードするメソッド
    public void PlayTitleSEAndLoadScene()
    {
        if (titleButtonSound == null)
        {
            Debug.LogWarning("Title Button Sound not assigned. Loading scene immediately.", this);
            SceneManager.LoadScene(titleSceneName); // SEがなければ即座にロード
            return;
        }

        // 一時的なGameObjectを作成し、そこにAudioSourceを追加してSEを再生する
        // これにより、現在のシーンが破棄されてもこのAudioSourceは残る
        GameObject tempAudioGameObject = new GameObject("TempOneShotAudio");
        AudioSource tempAudioSource = tempAudioGameObject.AddComponent<AudioSource>();

        // SEスライダーで設定された音量を適用
        if (seAudio != null)
        {
            tempAudioSource.volume = seAudio.volume; // SEの現在の音量設定を適用
        }
        else
        {
            tempAudioSource.volume = 1.0f; // SE AudioSourceが未設定ならフルボリューム
            Debug.LogWarning("SE AudioSource (seAudio) is not assigned for volume control. Using full volume for TempOneShotAudio.", this);
        }

        tempAudioSource.clip = titleButtonSound; // 再生するオーディオクリップをセット
        tempAudioSource.Play(); // 再生開始

        // シーン遷移時にこの一時的なGameObjectを破棄しないようにする
        DontDestroyOnLoad(tempAudioGameObject);

        // SEの再生時間だけ待ってからシーンをロードするコルーチンを開始
        StartCoroutine(LoadSceneAfterSoundAndCleanup(tempAudioGameObject, titleButtonSound.length));
    }

    // 指定された時間待ってからシーンをロードし、一時的なGameObjectを破棄するコルーチン
    private IEnumerator LoadSceneAfterSoundAndCleanup(GameObject audioGameObject, float delay)
    {
        yield return new WaitForSeconds(delay); // 指定された秒数待機

        // シーンをロード
        SceneManager.LoadScene(titleSceneName);

        // 新しいシーンが完全にロードされるのを待つため、次のフレームまで待機
        yield return null;

        // シーンロード後、一時的なGameObjectを破棄する
        if (audioGameObject != null)
        {
            Destroy(audioGameObject);
        }
    }
}