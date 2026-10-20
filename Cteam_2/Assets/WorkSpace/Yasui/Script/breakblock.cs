using UnityEngine;

public class breakblock : MonoBehaviour
{
    [SerializeField] private float breakRockSpeed;       // 壊すために必要な速度
    [SerializeField] private PlayerMove playerMove;

    private void OnCollisionEnter(Collision collision)
    {
        if (!playerMove.isIce)
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