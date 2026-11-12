using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【目的】
///  プレイヤーの行動をまとまています。
/// 【使い方】
///  プレイヤーの移動速度をmoveSpeedで変更することができます。
///  状態変化をさせることができます(仮)。
///  水状態でSpaceを押すことでジャンプできるようにしています(仮)。
///  坂を下ることができます。
///  〇坂の説明
///  ・slideSpeedでプレイヤーが当たったときの坂を下る初速です。
///  ・slideAccelerationでプレイヤーが坂にいる間毎秒その値だけ加速します。
///  ・rotateSpeedで坂から地面に着地したときに毎秒ゆっくり角度を変えられます(数値が少ないほどゆっくり)。
///  ・afterSlideSpeedで地面に着地したときに慣性をつけられます。
///  ・stopSpeedでafterSlideSpeedから毎秒減速します。
/// </summary>

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


    //public bool IsWater = false;     //水状態かどうか
    //public bool IsIce = true;        // 氷状態かどうか
    //public bool IsSteam = false;     //蒸気状態かどうか
    private bool hitOther = false;   //坂と床以外にぶつかっているかどうか
    public  bool isSlide = false;
    private bool isSlope = false;
    public bool isIce => stateHolder.CurrentPlayerState == EnumList_Player.PlayerState.Ice;

    private Vector3 moveDirection;



    void Awake()
    {
        // 前後左右の回転を防ぐ
        //rb.constraints |= RigidbodyConstraints.FreezeRotationX;
        //rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
    }

    // 氷状態に切り替える
    public void IceRbSwitch()
    {
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }

    void Update()
    {

        moveDirection = Vector3.zero;

        if (Keyboard.current == null) return;

        if (Keyboard.current.dKey.isPressed) // 右
        {
            transform.parent.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);

            moveDirection.x += moveSpeed;
        }

        if (Keyboard.current.aKey.isPressed) // 左
        {
            transform.parent.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            moveDirection.x -= moveSpeed;
        }

        if (Keyboard.current.wKey.isPressed) // 前
        {
            //transform.parent.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            moveDirection.z += moveSpeed;
        }

        if (Keyboard.current.sKey.isPressed) //後ろ
        {
            moveDirection.z -= moveSpeed;
        }

        //状態変化(仮)
        //if(Keyboard.current.downArrowKey.wasPressedThisFrame)
        //{
        //    IsWater = true;
        //    IsIce = false;
        //    IsSteam = false;
        //}
        //else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        //{
        //    IsWater = false;
        //    IsIce = true;
        //    IsSteam = false;
        //}
        //else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        //{
        //    IsWater = false;
        //    IsIce = false;
        //    IsSteam = true;
        //}


        //if (IsWater)
        //{
        //    if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //    {
        //        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        //    }
        //}


        //プレイヤーから下方向に線をとばす
        Ray ray = new Ray(transform.parent.position, Vector3.down);

        bool hasHit = Physics.Raycast(ray, out RaycastHit Hit, rayDirection);

        Debug.DrawRay(transform.parent.position, Vector3.down, Color.red, rayDirection);
        

        if (Physics.Raycast(ray, out RaycastHit hit, rayDirection) && hasHit)     //坂を下る
        {
            
            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;

            float n = Mathf.Abs(Mathf.Acos(Vector3.Dot(groundNormal, Vector3.up)) * Mathf.Rad2Deg);
            Debug.Log(n);

            

            //一定の角度以上かつ滑ってない状態かつ坂以外にぶつかっていない場合に坂を滑る
            if (hit.collider.CompareTag("Slope") && n >= slideAngle && !isSlide && !hitOther)     //一定の角度以上で滑るようにする
            {
                Vector3 forward = transform.parent.forward;

                //線をとばした方向にプレイヤーの向きを合わせる
                Vector3 slopeForward = Vector3.ProjectOnPlane(forward, groundNormal).normalized;

                //坂の角度に合わせてプレイヤーを回転
                if (slopeForward.sqrMagnitude > 0.001f)
                {
                    transform.parent.rotation = Quaternion.LookRotation(
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

                transform.parent.position += slideDirection * slideSpeed * Time.deltaTime;

            }
            //一定の角度以上かつ滑っている状態のときRayがあったっている物体に角度をあわせる
            else if (n <= slideAngle && isSlide)
            {

                Vector3 forward = transform.parent.forward;

                //線をとばした方向にプレイヤーの向きを合わせる
                Vector3 slopeForward = Vector3.ProjectOnPlane(forward, groundNormal).normalized;

                //坂の角度に合わせてプレイヤーを回転
                if (slopeForward.sqrMagnitude > 0.001f)
                {
                    transform.parent.rotation = Quaternion.LookRotation(
                        slopeForward,
                        groundNormal
                        );
                }
            }
            //Rayが床に当たっているかつ坂を下っていない場合
            else if (hit.collider.CompareTag("Ground") && !isSlide && !isSlope && !hitOther)
            {
                //滑るようにする
                isSlide = true;
                isSlope = true;
                hitOther = true;
                afterSlideSpeed = 6f;
                slideSpeed = 8f;
            }
        }

        //滑り終わった後に坂にいない場合
        if (isSlide && isSlope && hitOther)
        {

            Debug.Log("床に当たりました");

            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;

            float n = Mathf.Abs(Mathf.Acos(Vector3.Dot(groundNormal, Vector3.up)) * Mathf.Rad2Deg);
            Debug.Log(n);

            transform.parent.rotation = Quaternion.Slerp(
            transform.parent.rotation,
            Quaternion.identity,
            rotateSpeed * Time.deltaTime
            );

            transform.parent.position -= Vector3.right * afterSlideSpeed * Time.deltaTime;

            afterSlideSpeed = Mathf.MoveTowards(     
                afterSlideSpeed,
                0.0f,
                stopSpeed * Time.deltaTime);

            //Rayがあったっているとき
            if (hasHit)
            {
                //坂にあたったら
                if (hit.collider.CompareTag("Slope"))
                {
                    //坂を下れるようにする
                    isSlide = false;
                    isSlope = false;
                    hitOther = false;
                }
                //坂以外に当たった場合
                else
                {
                    //滑れないようにする
                    isSlide = true;
                    isSlope = true;
                    hitOther = true;
                }
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

    void OnCollisionEnter(Collision collision)
    {
        //坂に当たったときにしか戻らないようにする
        if (collision.gameObject.CompareTag("Ground"))
        {
            isSlide = true;
            isSlope = true;
            hitOther = true;
            Debug.Log("床に当たりました");
        }
        else if (collision.gameObject.CompareTag("Slope"))
        {
            isSlide = false;
            isSlope = false;
            hitOther = false;
            Debug.Log("坂に当たりました");
        }
    }
}