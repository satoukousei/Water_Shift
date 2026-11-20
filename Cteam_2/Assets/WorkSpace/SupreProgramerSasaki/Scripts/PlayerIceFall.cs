using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerIceFall : MonoBehaviour
{
    [Header("SEManagerを指定")]
    [SerializeField] 
    private SoundEffectManager seManager;
    [SerializeField]
    private bool isGrounded = false;
    [SerializeField]
    private bool isPlayFallSound = false;

    private void Update()
    {
        FallSoundPlay();
    }

    /// <summary>
    /// 落下時のサウンドを流すための条件監視とサウンド再生
    /// </summary>
    private void FallSoundPlay()
    {
        if (isGrounded && !isPlayFallSound)
        {
            seManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.IceFalling);
            isPlayFallSound = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            isPlayFallSound = false;
        }
    }
}
