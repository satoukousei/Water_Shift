using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] private EnumList_Player stateHolder;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rayDirection;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideAcceleration;
    [SerializeField] private float afterSlideSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float stopSpeed;
    [SerializeField] private float slideAngle;
    public Rigidbody Rb;

    public bool IsWater = false;     //水状態かどうか
    public bool IsIce = true;        // 氷状態かどうか
    public bool IsSteam = false;     //蒸気状態かどうか

    public bool isSlide = false;
    //public bool isIce => stateHolder.CurrentPlayerState == EnumList_Player.PlayerState.Ice;

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
        IsIce = true;

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
            //transform.parent.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            moveDirection.x -= 1f;
        }

        if (Keyboard.current.wKey.isPressed) // 前
        {
            //transform.parent.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            moveDirection.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed) //後ろ
        {
            moveDirection.y -= 1f;
        }

        //状態変化(仮)
        if(Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            IsWater = true;
            IsIce = false;
            IsSteam = false;
        }
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            IsWater = false;
            IsIce = true;
            IsSteam = false;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            IsWater = false;
            IsIce = true;
            IsSteam = false;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            IsWater = false;
            IsIce = false;
            IsSteam = true;
        }



        //プレイヤーから下方向に線をとばす
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDirection))     //坂を下る
        {

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;

            float n = Mathf.Abs(Mathf.Acos(Vector3.Dot(groundNormal, Vector3.up)) * Mathf.Rad2Deg);
            Debug.Log(n);

            if (n > slideAngle && !isSlide)     //一定の角度以上で滑るようにする
            {
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

                Vector3 slideDirection = Vector3.ProjectOnPlane(gravity, groundNormal).normalized;

                //Mathf.MoveTowards(変化させたい変数、目標値(上限)、変化量)
                slideSpeed = Mathf.MoveTowards(
                    slideSpeed,
                    20f,
                    slideAcceleration * Time.deltaTime);

                transform.position += slideDirection * slideSpeed * Time.deltaTime;

            }
            else if (hit.collider.CompareTag("Floor") && !isSlide)
            {
                isSlide = true;
                afterSlideSpeed = 6f;     //初期数値を入れる
                slideSpeed = 8f;     //初期数値を入れる
            }
        }

        if (isSlide)
        {

            Debug.Log("床に当たりました");

            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;

            float n = Mathf.Abs(Mathf.Acos(Vector3.Dot(groundNormal, Vector3.up)) * Mathf.Rad2Deg);
            Debug.Log(n);

            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.identity,
            rotateSpeed * Time.deltaTime
            );

            transform.position -= Vector3.right * afterSlideSpeed * Time.deltaTime;

            afterSlideSpeed = Mathf.MoveTowards(     
                afterSlideSpeed,
                0.0f,
                stopSpeed * Time.deltaTime);

            if (afterSlideSpeed <= 0 && n > slideAngle)
            {
                isSlide = false;
            }
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