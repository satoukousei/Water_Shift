using Unity.VisualScripting;
using UnityEngine;

public class breakblock : MonoBehaviour
{
    [SerializeField] private float breakSpeed;     //壊すために必要な落下速度

    [SerializeField] private PlayerMove playerMove;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Block") && playerMove.rb.linearVelocity.magnitude > breakSpeed && playerMove.isIce)
        {
            Destroy(other.gameObject);
        }
    }
}
