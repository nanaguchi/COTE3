using UnityEngine;

// AudioSourceが必須であることを示す
[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    // Inspectorから再生したい音を設定する
    public AudioClip soundToPlay;
    
    private AudioSource audioSource;

    void Awake()
    {
        // このオブジェクトに付いているAudioSourceを取得
        audioSource = GetComponent<AudioSource>();
    }

    // ゲーム開始時に一度だけ呼ばれる
    void Start()
    {
        // soundToPlayが設定されていれば、一度だけ再生する
        if (soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }
    }
}