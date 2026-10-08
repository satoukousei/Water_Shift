using UnityEngine;

public class Steam_Playe_Move : MonoBehaviour
{
    [SerializeField] private float upperMoveSpeed = 5f;

    [SerializeField] private Rigidbody rigidBodyParent;



    void Awake()
    {
        rigidBodyParent.useGravity = false;
        rigidBodyParent.freezeRotation = true;
    }

    void FixedUpdate()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        // 物理演算を通して上に移動
        rigidBodyParent.MovePosition(rigidBodyParent.position + Vector3.up * upperMoveSpeed * Time.fixedDeltaTime);
    }

    public void SteamRbSwitch()
    {
        rigidBodyParent.useGravity = false;
    }
}