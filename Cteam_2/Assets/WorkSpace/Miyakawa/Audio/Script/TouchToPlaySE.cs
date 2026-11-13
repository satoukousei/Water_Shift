
using UnityEngine;
using static SoundEffectManager;

public class TouchToPlaySE : MonoBehaviour
{
    [Header("蒸気についているコライダーのみのゲームオブジェクトを指定")]
    [SerializeField] private GameObject steamColliderObject;

    [Header("SEManagerを指定")]
    [SerializeField] private SoundEffectManager seManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            seManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.CloudsTatchCeiling);
        }
    }
}

