using UnityEngine;

public class breakblock : MonoBehaviour
{
    [SerializeField] private float breakRockSpeed;       // 壊すために必要な速度
    [SerializeField] private EnumList_Player enumList_Player;

    private void OnCollisionEnter(Collision collision)
    {
        if (enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Water|| 
            enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Steam)
        {
            return;                                   // 氷のときだけ
        }

        if (!collision.gameObject.CompareTag("Block"))
        {
            return;           // Blockタグだけ
        }
        if (collision.relativeVelocity.magnitude < breakRockSpeed)
        {
            return;   // 速度が足りない
        }
        Destroy(collision.gameObject);  
    }
}