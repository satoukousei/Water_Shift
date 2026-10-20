using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayDirection;
    [SerializeField] private float slideSpeed;
    public Rigidbody rb;

    public bool isIce = true; // 氷状態かどうか

    private Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 前後左右の回転を防ぐ
        rb.constraints |= RigidbodyConstraints.FreezeRotationX;
        rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
    }

    // 氷状態に切り替える
    public void IceRbSwitch()
    {
        isIce = true;

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    void Update()
    {

        moveDirection = Vector3.zero;

        if (Keyboard.current == null) return;

        if (Keyboard.current.dKey.isPressed) // 右
        {
            moveDirection.x += 1f;
        }

        if (Keyboard.current.aKey.isPressed) // 左
        {
            moveDirection.x -= 1f;
        }

        if (Keyboard.current.wKey.isPressed) // 前
        {
            moveDirection.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed) // 後ろ
        {
            moveDirection.y -= 1f;
        }


        //プレイヤーから下方向に線をとばす
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray,out RaycastHit hit, rayDirection))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;


            Vector3 forward = transform.forward;

            //線をとばした方向にプレイヤーの向きを合わせる
            Vector3 slopeForward = Vector3.ProjectOnPlane(forward, groundNormal).normalized;

            //坂の角度に合わせてプレイヤーを回転
            if (slopeForward.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(
                    slopeForward,
                    groundNormal
                    );
            }


            //坂に向かって滑らせる
            Vector3 gravity = Physics.gravity;

            Vector3 slideDirection = Vector3.ProjectOnPlane(gravity,groundNormal).normalized;

            transform.position += slideDirection * slideSpeed * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        // Y方向の速度は維持し、重力による落下を妨げない
        rb.linearVelocity = velocity;
    }
}