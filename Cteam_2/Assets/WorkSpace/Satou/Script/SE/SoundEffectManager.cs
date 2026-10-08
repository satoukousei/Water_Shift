using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class SoundEffectManager : MonoBehaviour
{
    private AudioSource[] m_audioSources;

    [SerializeField]
    private AudioClip[] m_audioClips;

    [SerializeField]
    private int m_audioSourceCount = 10;

    private int m_currentIndex = 0;

    public enum SoundEffectName
    {
        PlayerJump,
        PlayerMove,
        
    }

    // 各SEごとの音量を管理
    [Range(0f, 1f)]
    [SerializeField]
    private float[] m_seVolumes = new float[8] { 1, 1, 1, 1, 1, 1, 1, 1 };

    private void Awake()
    {
        m_audioSources = new AudioSource[m_audioSourceCount];
        for (int i = 0; i < m_audioSourceCount; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            m_audioSources[i] = source;
        }
    }

    public void OnPlayOneShot(SoundEffectName seNum)
    {
        var source = GetFreeAudioSource();

        // 個別音量で再生
        float volume = m_seVolumes[(int)seNum];
        source.PlayOneShot(m_audioClips[(int)seNum], volume);

        m_currentIndex++;
        if (m_currentIndex >= m_audioSources.Length)
        {
            m_currentIndex = 0;
        }
    }

    // 使っていないAudioSourceを選んでいます。
    private AudioSource GetFreeAudioSource()
    {
        int startIndex = m_currentIndex;

        while (m_audioSources[m_currentIndex].isPlaying)
        {
            m_currentIndex = (m_currentIndex + 1) % m_audioSources.Length;
            if (m_currentIndex == startIndex)
                break;
        }

        AudioSource source = m_audioSources[m_currentIndex];
        m_currentIndex = (m_currentIndex + 1) % m_audioSources.Length;
        return source;
    }

    // Inspectorで配列サイズを自動調整
    private void OnValidate()
    {
        if (m_seVolumes == null || m_seVolumes.Length != System.Enum.GetValues(typeof(SoundEffectName)).Length)
        {
            int len = System.Enum.GetValues(typeof(SoundEffectName)).Length;
            float[] newVolumes = new float[len];
            for (int i = 0; i < len; i++)
            {
                newVolumes[i] = (m_seVolumes != null && i < m_seVolumes.Length) ? m_seVolumes[i] : 1f;
            }
            m_seVolumes = newVolumes;
        }
    }
}
