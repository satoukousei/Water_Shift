using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【目的】
///  プレイヤーの行動をまとめています。
/// 【使い方】
///  プレイヤーの移動速度をmoveSpeedで変更することができます。
///  キーボード(A/D)、ゲームパッド(左スティック/十字キー)で左右移動できます。
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
    private const float SlideAngleTolerance = 1f;
    private const float SlopeHitTieTolerance = 0.05f;
    private const float GroundRayStartOffset = 0.25f;
    private const float SlopeContactDistance = 0.4f;
    [SerializeField] public Rigidbody rb;
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float inertiaMove;
    [SerializeField] private float rayDirection;
    [SerializeField] private float slideMaxSpeed;
    [SerializeField, Min(0f)] private float slideAcceleration = 4f;
    [SerializeField] private float rotateSpeed;
    [SerializeField, Range(0f, 89f)] private float slideAngle = 15f;
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float maxAngle = 90.0f;//マジックナンバーだったので直しました。
    [Header("InputActionAssetControllerを指定")]
    [SerializeField] private InputActionAssetController inputActionAssetController;
    [Header("SE")]
    [SerializeField] private SoundEffectManager soundEffectManager;
    [SerializeField] private float slideSeInterval = 1.4f;
    // Block transitions and brief raycast misses should not cancel sliding immediately.
    [SerializeField, Min(0f)] private float slideDetectionGracePeriod = 0.35f;
    private float slideSeTimer;
    private float slideGraceTimer;
    private float slideVelocityLogTimer;
    private const float InputDeadzone = 0.2f;
    private bool wasSliding;
    private bool wasGrounded;
    private Vector3 slideDirection;
    private Vector3 slideSurfaceNormal = Vector3.up;
    private Quaternion initialVisualRotation;

    public bool isSlide = false;
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

    private RaycastHit currentGroundHit;
    private bool hasGroundHit;
    private Collider[] playerColliders;
    private bool previousSlideState;
    private bool hasLoggedSlideState;
    private Collider previousHitCollider;
    private bool hasLoggedMissingReference;

    private void Awake()
    {
        if (rb == null) rb = GetComponentInParent<Rigidbody>();
        if (enumList_Player == null) enumList_Player = GetComponentInParent<EnumList_Player>();
        // Ground checks should stay centered on the physics body, not the tilting visual child.
        if (playerRoot == null) playerRoot = rb != null ? rb.transform : transform;
        if (visualRoot == null) visualRoot = transform.Find("model_ice");
        if (visualRoot == null) visualRoot = transform;
        initialVisualRotation = visualRoot.rotation;
        // PlayerMove が Rigidbody の子に付いている構成では、Collider は兄弟や別の子にある。
        // ルート配下すべてを自分のColliderとしてRaycast対象から外す。
        Transform playerHierarchyRoot = rb != null ? rb.transform : transform.root;
        playerColliders = playerHierarchyRoot.GetComponentsInChildren<Collider>(true);
    }

    private void Update()
    {
        if (inputActionAssetController != null)
        {
            inputActionAssetController.EnableActionAsset();
        }

        moveDirection = Vector3.zero;
        if (rb == null || playerRoot == null || enumList_Player == null)
        {
            isSlide = false;
            wasSliding = false;
            if (!hasLoggedMissingReference)
            {
                Debug.LogError($"[PlayerMove] Sliding check skipped: Rigidbody={(rb == null ? "missing" : "ok")}, playerRoot={(playerRoot == null ? "missing" : "ok")}, EnumList_Player={(enumList_Player == null ? "missing" : "ok")}.", this);
                hasLoggedMissingReference = true;
            }
            return;
        }
        hasGroundHit = TryGetGroundHit(out currentGroundHit);
        Vector3 groundRayOrigin = playerRoot.position + Vector3.up * GroundRayStartOffset;
        Debug.DrawRay(groundRayOrigin, Vector3.down * (rayDirection + GroundRayStartOffset), hasGroundHit ? Color.green : Color.red);
        bool isGrounded = hasGroundHit && currentGroundHit.collider.CompareTag("Ground");
        if (enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice && isGrounded && !wasGrounded && soundEffectManager != null)
            soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.IceFalling);
        wasGrounded = isGrounded;
        bool detectedSlope = hasGroundHit && currentGroundHit.distance <= SlopeContactDistance &&
                             currentGroundHit.collider.CompareTag("Slope") &&
                             Vector3.Angle(currentGroundHit.normal, Vector3.up) >= Mathf.Max(0f, slideAngle - SlideAngleTolerance) &&
                             enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice;
        if (detectedSlope)
        {
            slideSurfaceNormal = currentGroundHit.normal;
            slideGraceTimer = slideDetectionGracePeriod;
            isSlide = true;
        }
        else if (!hasGroundHit && wasSliding && enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice && slideGraceTimer > 0f)
        {
            slideGraceTimer -= Time.deltaTime;
            isSlide = true;
        }
        else
        {
            isSlide = false;
            slideGraceTimer = 0f;
        }
        Collider hitCollider = hasGroundHit ? currentGroundHit.collider : null;
        if (!hasLoggedSlideState || isSlide != previousSlideState || hitCollider != previousHitCollider)
        {
            string hitName = hasGroundHit ? currentGroundHit.collider.name : "<no hit>";
            string hitTag = hasGroundHit ? currentGroundHit.collider.tag : "<none>";
            float hitAngle = hasGroundHit ? Vector3.Angle(currentGroundHit.normal, Vector3.up) : -1f;
            float hitDistance = hasGroundHit ? currentGroundHit.distance : -1f;
            Debug.Log($"[PlayerMove] isSlide={isSlide}; hit={hitName}; tag={hitTag}; angle={hitAngle:F1}; distance={hitDistance:F2}; threshold={slideAngle:F1}; state={enumList_Player.CurrentPlayerState}; ray={rayDirection + GroundRayStartOffset:F2}; velocity={rb.linearVelocity}", this);
            previousSlideState = isSlide;
            previousHitCollider = hitCollider;
            hasLoggedSlideState = true;
        }
        if (isSlide)
        {
            UpdateSlideSe();
            if (inputActionAssetController != null) inputActionAssetController.DisableActionAsset();
            RotateVisualToSurface(slideSurfaceNormal);
        }
        else
        {
            wasSliding = false;
            if (hasGroundHit && currentGroundHit.collider.CompareTag("Slope"))
                RotateVisualToSurface(currentGroundHit.normal);
            else
                visualRoot.rotation = Quaternion.Slerp(visualRoot.rotation, initialVisualRotation, rotateSpeed * Time.deltaTime);
        }

        if (isSlide) return;

        // キーボード(A/D)・ゲームパッド(左スティック/十字キー)の左右入力
        float horizontal = GetHorizontalInput();
        if (Mathf.Abs(horizontal) > 0f)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.x += inertiaMove * horizontal;
            velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
            rb.linearVelocity = velocity;
        }
    }

    /// <summary>
    /// キーボード(A/D)とゲームパッド(左スティック/十字キー)の横入力を -1〜1 で返します。
    /// </summary>
    private float GetHorizontalInput()
    {
        float horizontal = 0f;

        // キーボード
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.dKey.isPressed) horizontal += 1f;
            if (keyboard.aKey.isPressed) horizontal -= 1f;
        }

        // ゲームパッド
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            float stick = gamepad.leftStick.x.ReadValue();
            if (Mathf.Abs(stick) >= InputDeadzone) horizontal += stick;

            horizontal += gamepad.dpad.x.ReadValue();
        }

        return Mathf.Clamp(horizontal, -1f, 1f);
    }

    /// <summary>
    /// Rigidbody本体を回さず、PlayerMoveが付いた見た目側だけを接地面に合わせます。
    /// </summary>
    private void RotateVisualToSurface(Vector3 surfaceNormal)
    {
        if (surfaceNormal.sqrMagnitude < 0.001f) return;
        Quaternion slopeTilt = Quaternion.FromToRotation(Vector3.up, surfaceNormal.normalized);
        visualRoot.rotation = slopeTilt * initialVisualRotation;
    }

    private bool TryGetGroundHit(out RaycastHit groundHit)
    {
        groundHit = default;
        if (rayDirection <= 0f) return false;

        Vector3 rayOrigin = playerRoot.position + Vector3.up * GroundRayStartOffset;
        RaycastHit[] hits = Physics.RaycastAll(
            rayOrigin,
            Vector3.down,
            rayDirection + GroundRayStartOffset,
            ~0,
            QueryTriggerInteraction.Ignore);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        RaycastHit nearestNonPlayerHit = default;
        RaycastHit nearestSlopeHit = default;
        bool foundNonPlayerHit = false;
        bool foundSlopeHit = false;
        foreach (RaycastHit hit in hits)
        {
            bool isPlayerCollider = false;
            if (playerColliders != null)
            {
                foreach (Collider playerCollider in playerColliders)
                {
                    if (playerCollider == hit.collider)
                    {
                        isPlayerCollider = true;
                        break;
                    }
                }
            }

            if (isPlayerCollider) continue;

            if (hit.collider.CompareTag("Slope"))
            {
                if (!foundSlopeHit)
                {
                    nearestSlopeHit = hit;
                    foundSlopeHit = true;
                }
                continue;
            }

            if (!foundNonPlayerHit)
            {
                nearestNonPlayerHit = hit;
                foundNonPlayerHit = true;
            }
        }

        if (foundSlopeHit && (!foundNonPlayerHit || nearestSlopeHit.distance <= nearestNonPlayerHit.distance + SlopeHitTieTolerance))
        {
            groundHit = nearestSlopeHit;
            return true;
        }
        if (!foundNonPlayerHit) return false;
        groundHit = nearestNonPlayerHit;
        return true;
    }

    /// <summary>
    /// 氷状態で坂を滑っている間、一定間隔で滑走SEを再生します。
    /// </summary>
    private void UpdateSlideSe()
    {
        isSlide = true;

        if (!wasSliding)
        {
            slideSeTimer = 0f;
        }

        slideSeTimer -= Time.deltaTime;
        if (slideSeTimer <= 0f)
        {
            if (soundEffectManager != null)
            {
                soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.IceSlip);
            }

            slideSeTimer = slideSeInterval;
        }

        wasSliding = true;
    }

    /// <summary>
    /// 坂の下方向へ加速し、滑走速度を上限内に保ちます。
    /// </summary>
    private void FixedUpdate()
    {
        if (rb == null || playerRoot == null || enumList_Player == null) return;
        if (!isSlide) return;

        // Update and FixedUpdate can run at different rates. Recheck the slope here
        // so a stale Update hit cannot keep projecting velocity after leaving the slope.
        bool hasFixedGroundHit = TryGetGroundHit(out RaycastHit fixedGroundHit);
        bool isOnDetectedSlope = hasFixedGroundHit &&
                                 fixedGroundHit.collider.CompareTag("Slope") &&
                                 fixedGroundHit.distance <= SlopeContactDistance &&
                                 Vector3.Angle(fixedGroundHit.normal, Vector3.up) >= Mathf.Max(0f, slideAngle - SlideAngleTolerance) &&
                                 enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice;
        Vector3 velocity = rb.linearVelocity;
        // Keep the grace period for brief ray misses, but do not apply slope forces in midair.
        if (!isOnDetectedSlope)
        {
            Vector3 lastSlideDirection = Vector3.ProjectOnPlane(Vector3.down, slideSurfaceNormal).normalized;
            float airborneSpeed = lastSlideDirection.sqrMagnitude > 0.001f ? Vector3.Dot(velocity, lastSlideDirection) : 0f;
            LogSlideVelocity(false, airborneSpeed, hasFixedGroundHit ? fixedGroundHit.distance : -1f);
            return;
        }

        slideSurfaceNormal = fixedGroundHit.normal;
        Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, slideSurfaceNormal).normalized;
        if (slideDirection.sqrMagnitude < 0.001f) return;

        velocity = Vector3.ProjectOnPlane(velocity, fixedGroundHit.normal);
        velocity += slideDirection * slideAcceleration * Time.fixedDeltaTime;
        float speed = Vector3.Dot(velocity, slideDirection);
        if (slideMaxSpeed > 0f && speed > slideMaxSpeed) velocity -= slideDirection * (speed - slideMaxSpeed);
        rb.linearVelocity = velocity;

        LogSlideVelocity(true, speed, fixedGroundHit.distance);
    }

    private void LogSlideVelocity(bool isOnDetectedSlope, float speed, float groundDistance)
    {
        slideVelocityLogTimer -= Time.fixedDeltaTime;
        if (slideVelocityLogTimer <= 0f)
        {
            Debug.Log($"[PlayerMove] sliding speed={speed:F2}; onSlope={isOnDetectedSlope}; groundDistance={groundDistance:F2}; gravity={rb.useGravity}; velocity={rb.linearVelocity}; acceleration={slideAcceleration:F2}; maxSpeed={slideMaxSpeed:F2}", this);
            slideVelocityLogTimer = 0.25f;
        }
    }
}
