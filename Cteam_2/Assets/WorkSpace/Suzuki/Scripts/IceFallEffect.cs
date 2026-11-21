using UnityEngine;
using static EnumList_Player;

/// <summary>
/// 【使い方】
/// ・プレイヤーまたは氷状態制御オブジェクトにアタッチして使用します。
/// ・インスペクターから「落下中エフェクトPrefab」「着地エフェクトPrefab」「足元位置」「地面レイヤー」「EnumList_Player」を設定してください。
/// 
/// 【目的】
/// ・プレイヤーが氷状態（Ice）のときのみ、落下中エフェクトを広がった状態で一時停止・維持し、着地時に地面エフェクトを再生するためにあります。
/// </summary>
public class IceFallEffect : MonoBehaviour
{
    private const float HexColorMax = 255.0f;

    [Header("エフェクトPrefab設定")]
    [SerializeField] private GameObject fallingEffectPrefab = null;
    [SerializeField] private GameObject landingEffectPrefab = null;

    [Header("状態・判定設定")]
    [SerializeField] private EnumList_Player playerStateHolder = null;
    [SerializeField] private Transform footPoint = null;
    [SerializeField] private Rigidbody targetRigidbody = null;
    [SerializeField] private float groundCheckDistance = 0.05f;
    [SerializeField] private LayerMask groundLayer = 0;
    [SerializeField] private bool showFallingOnlyWhileFalling = true;
    [SerializeField] private float fallingVelocityThreshold = -0.05f;
    [SerializeField] private bool keepUntilGrounded = true;

    [Header("落下エフェクト調整")]
    [SerializeField] private Vector3 fallingEffectScale = new Vector3(1.0f, 0.35f, 1.0f);
    [SerializeField] private Vector3 footOffset = Vector3.zero;
    [Tooltip("エフェクトが広がりきって一時停止するまでの秒数")]
    [SerializeField] private float expandDuration = 0.25f;

    [Header("エフェクト色設定")]
    [SerializeField] private Color fallingEffectColor = new Color(255.0f / HexColorMax, 216.0f / HexColorMax, 74.0f / HexColorMax, 1.0f);
    [SerializeField] private Color landingEffectColor = new Color(255.0f / HexColorMax, 212.0f / HexColorMax, 0.0f / HexColorMax, 1.0f);

    private GameObject activeFallingEffect = null;
    private bool wasGrounded = true;
    private bool isLatchedFalling = false;
    private float fallingEffectTimer = 0.0f;
    private bool isFallingEffectPaused = false;

    /// <summary>
    /// 初期参照を取得します
    /// </summary>
    private void Awake()
    {
        
    }

    /// <summary>
    /// 毎フレームの氷状態・接地状態を確認し、エフェクトを更新します
    /// </summary>
    private void Update()
    {
        bool isGrounded = CheckGrounded();

        if (!IsIceState())
        {
            ResetFallingState();
            DestroyFallingEffect();
            wasGrounded = isGrounded;
            return;
        }

        bool isFalling = IsFalling(isGrounded);

        if (isFalling)
        {
            SpawnOrUpdateFallingEffect();
        }
        else
        {
            if (isGrounded && !wasGrounded)
            {
                SpawnLandingEffect();
            }

            DestroyFallingEffect();
        }

        wasGrounded = isGrounded;
    }

    /// <summary>
    /// オブジェクト無効化時に状態をリセットし、生成中の落下エフェクトを破棄します
    /// </summary>
    private void OnDisable()
    {
        ResetFallingState();
        DestroyFallingEffect();
    }

    /// <summary>
    /// 現在のプレイヤー状態が氷（Ice）かどうかを返します
    /// </summary>
    /// <returns>氷状態ならtrue</returns>
    private bool IsIceState()
    {
        if (playerStateHolder == null)
        {
            return false;
        }

        return playerStateHolder.CurrentPlayerState == PlayerState.Ice;
    }

    /// <summary>
    /// 落下判定の内部状態をリセットします
    /// </summary>
    private void ResetFallingState()
    {
        isLatchedFalling = false;
        fallingEffectTimer = 0.0f;
        isFallingEffectPaused = false;
    }

    /// <summary>
    /// 地面に接しているかをRaycastで確認します
    /// </summary>
    /// <returns>接地していればtrue</returns>
    private bool CheckGrounded()
    {
        Vector3 origin = footPoint != null ? footPoint.position : transform.position;
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundLayer);
    }

    /// <summary>
    /// 落下・空中判定を行います
    /// </summary>
    /// <param name="isGrounded">接地中かどうか</param>
    /// <returns>落下中エフェクトを表示する状態ならtrue</returns>
    private bool IsFalling(bool isGrounded)
    {
        if (isGrounded)
        {
            isLatchedFalling = false;
            return false;
        }

        if (!showFallingOnlyWhileFalling)
        {
            isLatchedFalling = true;
            return true;
        }

        bool meetsVelocity = targetRigidbody == null || targetRigidbody.linearVelocity.y < fallingVelocityThreshold;
        if (meetsVelocity)
        {
            isLatchedFalling = true;
        }

        if (keepUntilGrounded && isLatchedFalling)
        {
            return true;
        }

        return meetsVelocity;
    }

    /// <summary>
    /// 落下中エフェクトを生成し、広がりきったタイミングで一時停止して維持します
    /// </summary>
    private void SpawnOrUpdateFallingEffect()
    {
        if (fallingEffectPrefab == null || footPoint == null)
        {
            return;
        }

        if (activeFallingEffect == null)
        {
            activeFallingEffect = Instantiate(
                fallingEffectPrefab,
                footPoint.position + footOffset,
                fallingEffectPrefab.transform.rotation
            );
            ApplyColor(activeFallingEffect, fallingEffectColor);
            PlayFromStart(activeFallingEffect);

            fallingEffectTimer = 0.0f;
            isFallingEffectPaused = false;
        }

        activeFallingEffect.transform.position = footPoint.position + footOffset;
        activeFallingEffect.transform.localScale = fallingEffectScale;

        // 指定秒数（広がりきった状態）になったら一時停止して着地まで維持
        if (!isFallingEffectPaused)
        {
            fallingEffectTimer += Time.deltaTime;
            if (fallingEffectTimer >= expandDuration)
            {
                PauseEffect(activeFallingEffect);
                isFallingEffectPaused = true;
            }
        }
    }

    /// <summary>
    /// ループを無効化して最初から再生します
    /// </summary>
    /// <param name="targetObject">対象のエフェクトオブジェクト</param>
    private void PlayFromStart(GameObject targetObject)
    {
        if (targetObject == null)
        {
            return;
        }

        ParticleSystem[] particleSystems = targetObject.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in particleSystems)
        {
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.loop = false;
            ps.Clear(true);
            ps.Play(true);
        }
    }

    /// <summary>
    /// エフェクトを現在の見た目（広がった状態）のまま一時停止します
    /// </summary>
    /// <param name="targetObject">対象のエフェクトオブジェクト</param>
    private void PauseEffect(GameObject targetObject)
    {
        if (targetObject == null)
        {
            return;
        }

        ParticleSystem[] particleSystems = targetObject.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Pause(true);
        }
    }

    /// <summary>
    /// 落下中エフェクトを破棄し、関連タイマーを初期化します
    /// </summary>
    private void DestroyFallingEffect()
    {
        if (activeFallingEffect != null)
        {
            Destroy(activeFallingEffect);
            activeFallingEffect = null;
        }

        fallingEffectTimer = 0.0f;
        isFallingEffectPaused = false;
    }

    /// <summary>
    /// 着地時に地面エフェクトを生成します
    /// </summary>
    private void SpawnLandingEffect()
    {
        if (landingEffectPrefab == null || footPoint == null)
        {
            return;
        }

        GameObject landingEffect = Instantiate(
            landingEffectPrefab,
            footPoint.position + footOffset,
            landingEffectPrefab.transform.rotation
        );
        ApplyColor(landingEffect, landingEffectColor);
    }

    /// <summary>
    /// 子階層を含むParticleSystemに指定色を適用します
    /// </summary>
    /// <param name="targetObject">対象オブジェクト</param>
    /// <param name="targetColor">適用する色</param>
    private void ApplyColor(GameObject targetObject, Color targetColor)
    {
        if (targetObject == null)
        {
            return;
        }

        ParticleSystem[] particleSystems = targetObject.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in particleSystems)
        {
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.startColor = targetColor;
        }
    }
}