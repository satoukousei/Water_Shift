using UnityEngine;
using RayFire;

/// <summary>
/// 手前に置いたトリガーColliderで氷の速度を測り、
/// 既定値以上ならブロックの破片を Sleeping から目覚めさせて崩す。
/// </summary>
public class BlockBreakSensor : MonoBehaviour
{
    [Header("参照(Inspectorで設定)")]
    [SerializeField] private RayfireRigid blockRigid;       // 壊したいブロック(親)
    [SerializeField] private Transform playerTransform;     // 速度を測る対象(PlayerParent)
    [SerializeField] private EnumList_Player stateHolder;   // 氷のときだけ有効にする

    [Header("設定")]
    [SerializeField] private float breakSpeed = 4.0f;          // これ以上の速度で壊せる
    [SerializeField] private float playerCheckDistance = 2.0f; // センサー内のColliderがプレイヤーかを判定する距離
    [SerializeField] private bool demolishAfterRelease = true; // 解放後にDemolish()も呼ぶか

    private Vector3 lastPosition;
    private float currentSpeed;
    private bool isBroken = false;

    private void Start()
    {
        lastPosition = playerTransform.position;

        // 最初は子(破片)を Sleeping にして、動かない状態で待機させる
        blockRigid.simulationType = SimType.Sleeping;
        blockRigid.Initialize();
    }

    // 毎フレーム、位置の変化量から速度を計算
    private void Update()
    {
        if (Time.deltaTime <= 0f) return;

        Vector3 delta = playerTransform.position - lastPosition;
        currentSpeed = delta.magnitude / Time.deltaTime;
        lastPosition = playerTransform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBroken) return;
        if (stateHolder.CurrentPlayerState != EnumList_Player.PlayerState.Ice) return;
        if (currentSpeed < breakSpeed) return;

        // センサーに入ったのがプレイヤーかを距離で確認
        float sqrLimit = playerCheckDistance * playerCheckDistance;
        if ((other.transform.position - playerTransform.position).sqrMagnitude > sqrLimit) return;

        Release();
    }

    private void Release()
    {
        isBroken = true;
        Debug.Log("速度 " + currentSpeed + " で解放しました");

        // 1. Dynamicに変更
        blockRigid.simulationType = SimType.Sleeping;

        // 2. リセットして、変更をセットアップし直す
        blockRigid.ResetRigid();

        // 3. 初期化(ResetRigidで再初期化されない場合のみ必要)
        blockRigid.Initialize();

        if (demolishAfterRelease)
        {
            blockRigid.Demolish();
        }
    }
}
