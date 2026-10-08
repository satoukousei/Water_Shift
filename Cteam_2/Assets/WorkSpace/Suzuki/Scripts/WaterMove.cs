using UnityEngine;
using UnityEngine.InputSystem;

public class WaterMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float upJumpPower;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;// 地面(すり抜けない床)のレイヤー
    

    private Rigidbody rb;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded) //スペースキーが押されたとき＆接地してる時
        {
            //ジャンプの方向を上向きのベクトルに設定
            Vector3 jump_vector = Vector3.up;
            //ジャンプの速度を計算
            Vector3 jump_velocity = jump_vector * upJumpPower;

            //上向きの速度を設定
            rb.linearVelocity = jump_velocity;
        }
    }
}
