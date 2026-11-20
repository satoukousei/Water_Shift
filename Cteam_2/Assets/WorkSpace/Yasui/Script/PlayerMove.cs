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
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float inertiaMove;
    [SerializeField] private float rayDirection;
    [SerializeField] private float slideMaxSpeed;
    [SerializeField] private float slideAcceleration;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float slideAngle;
    [SerializeField] private Transform playerRoot;
    [SerializeField] private float maxAngle = 90.0f;//マジックナンバーだったので直しました。
    [Header("InputActionAssetControllerを指定")]
    [SerializeField] private InputActionAssetController inputActionAssetController;

    public bool  isSlide = false;
    public bool isIce => enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice;

    private Vector3 moveDirection;

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
        inputActionAssetController.EnableActionAsset();

        //移動
        moveDirection = Vector3.zero;
        if (Keyboard.current == null)
        {
            return;
        }
        if (Keyboard.current.dKey.isPressed) // 右
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.x += inertiaMove;

            if (velocity.x >= moveSpeed)
            {
                velocity.x = moveSpeed;
            }

            rb.linearVelocity = velocity;
        }
        if (Keyboard.current.aKey.isPressed) // 左
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.x -= inertiaMove;

            if (velocity.x <= -moveSpeed)
            {
                velocity.x = -moveSpeed;
            }

            rb.linearVelocity = velocity;
        }

        //プレイヤーから下方向に線をとばす
        Ray ray = new Ray(playerRoot.position, Vector3.down);

        bool hasHit = Physics.Raycast(ray, out RaycastHit Hit, rayDirection);     //Rayが当たっているかどうか判定するためのhasHitトリガー

        if (Physics.Raycast(ray, out RaycastHit hit, rayDirection) && hasHit)     //坂を下る
        {
            //坂の向き法線ベクトル
            Vector3 groundNormal = hit.normal;
            //Rayがあったった物体の角度をとる
            float currentAngle = Mathf.Abs(Mathf.Acos(Vector3.Dot(groundNormal, Vector3.up)) * Mathf.Rad2Deg);　
            //～●nという名前が分かりづらかったのでcurrentAngleに変更しました●～

            //一定の角度以上かつ滑ってない状態かつ坂以外にぶつかっていない場合に坂を滑る
            if (hit.collider.CompareTag("Slope") && currentAngle >= slideAngle && 
                enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice)     //一定の角度以上で滑るようにする
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

                //状態変化を止める
                inputActionAssetController.DisableActionAsset();

                //～▲変えました▲～
                if (currentAngle <= maxAngle) //現在の角度が
                {
                    Vector3 velocity = rb.linearVelocity;
                    velocity.x -= slideAcceleration;

                    if (velocity.x <= -slideMaxSpeed)
                    {
                        velocity.x = -slideMaxSpeed;
                    }
                    rb.linearVelocity = velocity;
                }
                else if (currentAngle >= maxAngle)
                {
                    Vector3 velocity = rb.linearVelocity;
                    velocity.x += slideAcceleration;

                    if (velocity.x >= slideMaxSpeed)
                    {
                        velocity.x = slideMaxSpeed;
                    }
                    rb.linearVelocity = velocity;
                }
            }
            //～▲ここまで▲～

            //一定角度以上のときプレイヤーの角度だけ変更する
            else if (hit.collider.CompareTag("Slope") && currentAngle <= slideAngle)
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

                //状態変化を止める
                inputActionAssetController.DisableActionAsset();
            }
            else if (hit.collider.CompareTag("Ground"))
            {
                transform.parent.rotation = Quaternion.Slerp(
                transform.parent.rotation,
                Quaternion.identity,
                rotateSpeed * Time.deltaTime
                );

                Vector3 velocity = rb.linearVelocity;
                velocity.x += slideAcceleration;
                if (velocity.x <= 0)
                {

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
        //rb.linearVelocity = velocity;
    }
}