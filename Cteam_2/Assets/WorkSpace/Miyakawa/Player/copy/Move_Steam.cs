using UnityEngine;

public class Move_Steam : MonoBehaviour
{
    [SerializeField] private float upperMoveSpeed = 5f;

    [SerializeField] private Rigidbody rigidBodyParent;

    void FixedUpdate()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        // •¨—‰‰Z‚ğ’Ê‚µ‚Äã‚ÉˆÚ“®
        rigidBodyParent.MovePosition(rigidBodyParent.position + Vector3.up * upperMoveSpeed * Time.fixedDeltaTime);
    }

    void FixidRigidbody()
    {
        rigidBodyParent.useGravity = false;
        rigidBodyParent.freezeRotation = true;
    }
}
