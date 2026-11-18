using UnityEngine;
using static SoundEffectManager;

public class TouchToPlaySE : MonoBehaviour
{
    [Header("SEManagerを指定")]
    [SerializeField] private SoundEffectManager seManager;

    [Header("SEの名前を指定")]
    [SerializeField] private SoundEffectName soundEffectName;

    private bool hasPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed)
        {
            return;
        }

        if (!other.CompareTag("Steam"))
        {
            return;
        }

        if (seManager == null)
        {
            Debug.LogWarning("SoundEffectManagerが設定されていません。", this);
            return;
        }

        hasPlayed = true;
        seManager.OnPlayOneShot(soundEffectName);
    }
}   
