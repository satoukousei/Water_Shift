using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【使い方】
/// ・水状態のプレイヤーオブジェクトにアタッチして使用します。
/// ・インスペクターから移動速度、ジャンプ力、レイの長さ、地面レイヤー、Rigidbody等を設定してください。
/// 
/// 【目的】
/// ・水状態のプレイヤーの移動、接地判定に基づくジャンプ、空中での移動制限、およびジャンプ中の慣性移動を制御するためにあります。
/// </summary>
public class WaterMove : MonoBehaviour
{
    [Header("移動・ジャンプ設定")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float upJumpPower = 7.0f;
    [SerializeField] private float rayLength = 1.1f;
    [SerializeField] private LayerMask[] groundLayer; //地面(すり抜けない床)のレイヤー


    [Header("コンポーネント設定")]
    [SerializeField] private Rigidbody rb = null;

    [Header("判定設定")]
    [SerializeField] private Vector3 jumpDirection = Vector3.zero;

    private bool isGrounded = false;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        Vector3 rayOrigin = transform.parent != null ? transform.parent.position : transform.position;
        for (int LayerCount = 0; LayerCount < groundLayer.Length; LayerCount++)
        {
            isGrounded = Physics.Raycast(rayOrigin + jumpDirection, Vector3.down, rayLength, groundLayer[LayerCount]);
            if (isGrounded)
            {
                break;
            }
        }
        Debug.DrawRay(rayOrigin, Vector3.down, Color.blue, rayLength);

        Movement();
        Jump();
    }

    private void Movement()
    {
        // 空中（接地していない）の時は移動入力を受け付けない
        if (!isGrounded || rb == null)
        {
            return;
        }

        float horizontalVelocity = 0.0f;

        // 右移動
        if (Keyboard.current.dKey.isPressed)
        {
            horizontalVelocity = moveSpeed;
        }
        // 左移動
        else if (Keyboard.current.aKey.isPressed)
        {
            horizontalVelocity = -moveSpeed;
        }

        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x = horizontalVelocity;
        rb.linearVelocity = currentVelocity;
    }

    private void Jump()
    {
        if (rb == null)
        {
            return;
        }

        // スペースキーが押されたとき＆接地している時
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            Vector3 jumpVector = Vector3.up;
            Vector3 jumpVelocity = jumpVector * upJumpPower;

            // 現在のX軸方向の慣性（速度）を保持したまま、上向きの速度を設定する
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = jumpVelocity.y;
            rb.linearVelocity = currentVelocity;
        }
    }

    public void WaterRbSwitch()
    {
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}