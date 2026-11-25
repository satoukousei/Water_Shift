using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【使い方】
/// ・水状態のプレイヤーオブジェクトにアタッチして使用します。
/// ・インスペクターから移動速度、ジャンプ力、レイの長さ、地面レイヤー、Rigidbody等を設定してください。
/// ・Xboxコントローラーの左スティック／十字キーで左右移動、Aボタンでジャンプします（キーボードのA/D、Spaceにも対応）。
/// 
/// 【目的】
/// ・水状態のプレイヤーの移動および接地判定に基づくジャンプを制御し、移動停止時や段差落下時の不要な滑りを防ぐためにあります。
/// </summary>
public class WaterMove : MonoBehaviour
{
    private const float InputDeadzone = 0.2f;

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

    private void Start()
    {

    }

    /// <summary>
    /// 毎フレームの接地判定および移動・ジャンプ処理を呼び出します
    /// </summary>
    private void Update()
    {
        Vector3 rayOrigin = transform.parent != null ? transform.parent.position : transform.position;

        isGrounded = false;
        if (groundLayer != null)
        {
            for (int layerCount = 0; layerCount < groundLayer.Length; layerCount++)
            {
                if (Physics.Raycast(rayOrigin + jumpDirection, Vector3.down, rayLength, groundLayer[layerCount]))
                {
                    isGrounded = true;
                    break;
                }
            }
        }


        Movement();
        Jump();
    }

    /// <summary>
    /// 左右の移動処理を行います（左スティック・十字キー・A/Dキーに対応）
    /// </summary>
    private void Movement()
    {
        Transform targetTransform = transform.parent != null ? transform.parent : transform;
        float horizontalAxis = GetHorizontalInput();

        if (Mathf.Abs(horizontalAxis) > InputDeadzone)
        {
            float moveAmount = horizontalAxis * moveSpeed * Time.deltaTime;
            targetTransform.position += new Vector3(moveAmount, 0.0f, 0.0f);
        }

        // 滑りを防ぐため、X軸のRigidbody速度をゼロに維持する
        if (rb != null)
        {
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.x = 0.0f;
            rb.linearVelocity = currentVelocity;
        }
    }

    /// <summary>
    /// コントローラーおよびキーボードから左右入力値を取得します
    /// </summary>
    /// <returns>-1.0f〜1.0fの左右入力値</returns>
    private float GetHorizontalInput()
    {
        float horizontalInput = 0.0f;

        if (Gamepad.current != null)
        {
            float stickX = Gamepad.current.leftStick.x.ReadValue();
            float dpadX = Gamepad.current.dpad.x.ReadValue();

            if (Mathf.Abs(stickX) > Mathf.Abs(dpadX))
            {
                horizontalInput = stickX;
            }
            else
            {
                horizontalInput = dpadX;
            }
        }

        if (Mathf.Abs(horizontalInput) <= InputDeadzone && Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed)
            {
                horizontalInput = 1.0f;
            }
            else if (Keyboard.current.aKey.isPressed)
            {
                horizontalInput = -1.0f;
            }
        }

        return Mathf.Clamp(horizontalInput, -1.0f, 1.0f);
    }

    /// <summary>
    /// ジャンプ処理を行います（Aボタン・Spaceキーに対応）
    /// </summary>
    private void Jump()
    {
        if (rb == null)
        {
            return;
        }

        if (IsJumpPressedThisFrame() && isGrounded)
        {
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = upJumpPower;
            rb.linearVelocity = currentVelocity;
        }
    }

    /// <summary>
    /// ジャンプボタンが押された瞬間かを判定します
    /// </summary>
    /// <returns>押されたフレームならtrue</returns>
    private bool IsJumpPressedThisFrame()
    {
        bool isGamepadJumpPressed = Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame;
        bool isKeyboardJumpPressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        return isGamepadJumpPressed || isKeyboardJumpPressed;
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