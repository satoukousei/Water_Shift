using UnityEngine;

/// <summary>
/// 効果音（SE）の再生を管理する
/// 複数のAudioSourceを順番に使い回して同時再生に対応する
/// </summary>
public class SoundEffectManager : MonoBehaviour
{
    private const int audioSourceCount = 10;
    private const float minVolume = 0f;
    private const float maxVolume = 1f;
    private const float defaultVolume = 1f;

    /// <summary>
    /// 再生できるSEの種類
    /// </summary>
    public enum SoundEffectName
    {
        Player_Jump,
        Player_Move
    }

    private AudioSource[] audioSources = System.Array.Empty<AudioSource>();

    [SerializeField]
    private AudioClip[] audioClips = System.Array.Empty<AudioClip>();

    private int currentIndex = 0;

    // 各SEごとの音量を管理
    [Range(minVolume, maxVolume)]
    [SerializeField]
    private float[] seVolumes = System.Array.Empty<float>();

    /// <summary>
    /// 再生用のAudioSourceを必要な数だけ生成する
    /// </summary>
    private void Awake()
    {
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSources[i] = source;
        }
    }

    /// <summary>
    /// 指定したSEを一度だけ再生する
    /// </summary>
    /// <param name="seNum">再生するSEの種類</param>
    public void OnPlayOneShot(SoundEffectName seNum)
    {
        AudioSource source = GetFreeAudioSource();

        // 個別音量で再生
        float volume = seVolumes[(int)seNum];
        source.PlayOneShot(audioClips[(int)seNum], volume);

        currentIndex++;
        if (currentIndex >= audioSources.Length)
        {
            currentIndex = 0;
        }
    }

    /// <summary>
    /// 使われていないAudioSourceを選んで返す
    /// </summary>
    /// <returns>再生に使うAudioSource</returns>
    private AudioSource GetFreeAudioSource()
    {
        int startIndex = currentIndex;

        while (audioSources[currentIndex].isPlaying)
        {
            currentIndex = (currentIndex + 1) % audioSources.Length;
            if (currentIndex == startIndex)
            {
                break;
            }
        }

        AudioSource source = audioSources[currentIndex];
        currentIndex = (currentIndex + 1) % audioSources.Length;
        return source;
    }

    /// <summary>
    /// Inspectorで配列サイズを自動調整する
    /// </summary>
    private void OnValidate()
    {
        int enumLength = System.Enum.GetValues(typeof(SoundEffectName)).Length;
        if (seVolumes != null && seVolumes.Length == enumLength)
        {
            return;
        }

        float[] newVolumes = new float[enumLength];
        for (int i = 0; i < enumLength; i++)
        {
            newVolumes[i] = (seVolumes != null && i < seVolumes.Length) ? seVolumes[i] : defaultVolume;
        }

        seVolumes = newVolumes;
    }
}
