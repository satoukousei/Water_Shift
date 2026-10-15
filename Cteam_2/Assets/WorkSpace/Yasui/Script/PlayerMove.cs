using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] public Rigidbody rb;
    [SerializeField] private EnumList_Player stateHolder; // プレイヤーの状態を持つスクリプト

    // 状態から自動で決まる(直接代入はしない)
    public bool isIce => stateHolder.CurrentPlayerState == EnumList_Player.PlayerState.Ice;

    void Update()
    {
        if (Keyboard.current.dKey.isPressed)     // 右
        {
            transform.parent.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
        }

        if (Keyboard.current.aKey.isPressed)     // 左
        {
            transform.parent.position += new Vector3(-MoveSpeed * Time.deltaTime, 0, 0);
        }
    }

    public void IceRbSwitch()
    {
        rb.useGravity = true;
    }
}