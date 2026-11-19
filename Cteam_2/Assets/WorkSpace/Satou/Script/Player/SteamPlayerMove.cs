using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// 蒸気状態のプレイヤーを上方向へ移動させる
/// </summary>
public class SteamPlayerMove : MonoBehaviour
{
    [SerializeField] private float startUpMoveSpeed = 3.0f;
    [SerializeField] private float maxSpeed = 7.0f;
    [SerializeField] private float upAcceleration = 1.0f;
    [SerializeField] private Rigidbody rigidBodyParent = null;
    private float currentSpeed = 0f;
    /// <summary>
    /// 重力と回転を無効にする
    /// </summary>
    private void Awake()
    {
        VelocityReset();
        rigidBodyParent.useGravity = false;
        rigidBodyParent.freezeRotation = true;
    }
    private void Update()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        rigidBodyParent.linearVelocity = Vector3.up * currentSpeed;
        currentSpeed += upAcceleration * Time.deltaTime;

        if(currentSpeed >= maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

    }



    public void VelocityReset()
    {
        currentSpeed = startUpMoveSpeed;
        rigidBodyParent.linearVelocity = Vector3.zero;
    }



    public void SteamRbSwitch()
    {
        rigidBodyParent.useGravity = false;
    }
}
