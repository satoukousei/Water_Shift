using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【使い方】
/// ・水状態のプレイヤーオブジェクトにアタッチして使用します。
/// ・インスペクターから移動速度、ジャンプ力、レイの長さ、地面レイヤー、Rigidbody等を設定してください。
/// 
/// 【目的】
/// ・水状態のプレイヤーの移動、接地判定に基づくジャンプ、および移動停止時の滑り（慣性）を防止する制御を行うためにあります。
/// </summary>
public class WaterMove : MonoBehaviour
{
    [Header("移動・ジャンプ設定")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float upJumpPower = 7.0f;
    [SerializeField] private float rayLength = 1.1f;
    [SerializeField] private LayerMask[] groundLayer = null; // 地面(すり抜けない床)のレイヤー

    [Header("コンポーネント設定")]
    [SerializeField] private Rigidbody rb = null;

    [Header("判定設定")]
    [SerializeField] private Vector3 jumpDirection = Vector3.zero;

    private bool isGrounded = false;

    /// <summary>
    /// コンポーネントの初期参照取得を行います
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// 毎フレームの接地判定および移動・ジャンプ処理を呼び出します
    /// </summary>
    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        Vector3 rayOrigin = transform.parent != null ? transform.parent.position : transform.position;

        isGrounded = false;
        for (int layerCount = 0; layerCount < groundLayer.Length; layerCount++)
        {
            if (Physics.Raycast(rayOrigin + jumpDirection, Vector3.down, rayLength, groundLayer[layerCount]))
            {
                isGrounded = true;
                break;
            }
        }

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.blue);

        Movement();
        Jump();
    }

    /// <summary>
    /// プレイヤーの移動処理を行います
    /// </summary>
    private void Movement()
    {
        Transform targetTransform = transform.parent != null ? transform.parent : transform;

        float horizontalInput = 0.0f;

        // 右移動の入力
        if (Keyboard.current.dKey.isPressed)
        {
            horizontalInput = moveSpeed;
        }
        // 左移動の入力
        else if (Keyboard.current.aKey.isPressed)
        {
            horizontalInput = -moveSpeed;
        }

        // キー入力がある場合：位置を動かす
        if (horizontalInput != 0.0f)
        {
            targetTransform.position += new Vector3(horizontalInput * Time.deltaTime, 0.0f, 0.0f);

            if (rb != null)
            {
                Vector3 currentVelocity = rb.linearVelocity;
                currentVelocity.x = 0.0f;
                rb.linearVelocity = currentVelocity;
            }
        }
        else
        {
            if (rb != null)
            {
                Vector3 currentVelocity = rb.linearVelocity;
                currentVelocity.x = 0.0f;
                rb.linearVelocity = currentVelocity;
            }
        }
    }

    /// <summary>
    /// ジャンプ処理を行います
    /// </summary>
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

            // Y軸方向のみ上向きの速度を設定する
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = jumpVelocity.y;
            rb.linearVelocity = currentVelocity;
        }
    }

    /// <summary>
    /// 重みを有効化するスイッチ関数
    /// </summary>
    public void WaterRbSwitch()
    {
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}