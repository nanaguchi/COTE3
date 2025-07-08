using UnityEngine;
using UnityEngine.UI;

public class SoundEffectPlayer : MonoBehaviour // クラス名がファイル名と一致していることを確認
{
    [SerializeField] private Slider bgmSlider;    // BGM用スライダー
    [SerializeField] private AudioSource bgmAudio; // BGMのAudioSource

    [SerializeField] private Slider seSlider;     // SE用スライダー
    [SerializeField] private AudioSource seAudio;  // SEのAudioSource

    // クリック音用のAudioClipを追加
    [SerializeField] private AudioClip clickSound; // リセットボタンのクリック音など

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

    // SEを再生するパブリックメソッド (追加)
    public void PlayClickSE()
    {
        if (seAudio != null && clickSound != null)
        {
            seAudio.PlayOneShot(clickSound); // SEを一度だけ再生
        }
        else
        {
            Debug.LogWarning("SE AudioSource or Click Sound not assigned for PlayClickSE.", this);
        }
    }
}