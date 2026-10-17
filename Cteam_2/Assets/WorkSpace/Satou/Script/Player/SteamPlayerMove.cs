using UnityEngine;

/// <summary>
/// 蒸気状態のプレイヤーを上方向へ移動させる
/// </summary>
public class SteamPlayerMove : MonoBehaviour
{
    [SerializeField] private float upperMoveSpeed = 5.0f;

    [SerializeField] private Rigidbody rigidBodyParent = null;

    /// <summary>
    /// 重力と回転を無効にする
    /// </summary>
    private void Awake()
    {
        rigidBodyParent.useGravity = false;
        rigidBodyParent.freezeRotation = true;
    }

    /// <summary>
    /// 物理演算の更新ごとに上へ移動する
    /// </summary>
    private void FixedUpdate()
    {
        UpperMove();
    }

    /// <summary>
    /// 物理演算を通して上に移動する
    /// </summary>
    private void UpperMove()
    {
        rigidBodyParent.MovePosition(rigidBodyParent.position + Vector3.up * upperMoveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 蒸気状態用に重力を無効にする
    /// </summary>
    public void SteamRbSwitch()
    {
        rigidBodyParent.useGravity = false;
    }
}
