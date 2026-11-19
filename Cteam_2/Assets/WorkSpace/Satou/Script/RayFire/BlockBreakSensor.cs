using UnityEngine;
using RayFire;

/// <summary>
/// 手前に置いたトリガーColliderで氷の速度を測り、
/// 既定値以上ならブロックの破片を Sleeping から目覚めさせて崩し、
/// 破片に Fragment レイヤーを付与して右方向へ吹き飛ばす。
/// 吹き飛ばし後は Player レイヤーと Fragment レイヤーの衝突を無効にし、
/// 規定の時間が過ぎたら破片を Destroy する。
/// </summary>
public class BlockBreakSensor : MonoBehaviour
{
    [Header("参照(Inspectorで設定)")]
    [SerializeField] private RayfireRigid blockRigid;       // 壊したいブロック(親)
    [SerializeField] private Transform playerTransform;     // 速度を測る対象(PlayerParent)
    [SerializeField] private EnumList_Player stateHolder;   // 氷のときだけ有効にする
    [SerializeField] private GameObject[] fragments;       // 親の下についている破片(子)

    [Header("設定")]
    [SerializeField] private float breakSpeed = 4.0f;          // これ以上の速度で壊せる
    [SerializeField] private bool demolishAfterRelease = false; // 解放後にDemolish()も呼ぶか

    [Header("レイヤー")]
    [SerializeField] private LayerMask playerLayer;             // プレイヤー判定用(Playerレイヤー)
    [SerializeField] private string playerLayerName = "Player";     // 衝突を切る相手のレイヤー名
    [SerializeField] private string fragmentLayerName = "Fragment"; // 破片に付与するレイヤー名

    [Header("吹き飛ばし")]
    [SerializeField] private float blowForce = 10.0f;          // Impulseの強さ

    [Header("消去")]
    [SerializeField] private float destroyDelay = 5.0f;        // 壊れてからDestroyするまでの秒数
    [SerializeField] private bool destroyBlockRoot = true;     // ブロック(親)本体もDestroyするか

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
        if (Time.deltaTime <= 0f)
        {
            return;
        }
        Vector3 delta = playerTransform.position - lastPosition;
        currentSpeed = delta.magnitude / Time.deltaTime;
        lastPosition = playerTransform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBroken)
        {
            return;
        }
        if (stateHolder.CurrentPlayerState != EnumList_Player.PlayerState.Ice)
        {
            return;
        }
        if (currentSpeed < breakSpeed)
        {
            return;
        }
        // センサーに入ったのがPlayerレイヤーかを確認
        if ((playerLayer.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        Release();
    }

    private void Release()
    {
        isBroken = true;
        Debug.Log("速度 " + currentSpeed + " で解放しました");

        // 吹き飛ばす方向(プレイヤーの右方向)を先に確定
        Vector3 blowDirection = playerTransform.right;

        // 1. Dynamicに変更
        blockRigid.simulationType = SimType.Dynamic;

        // 2. リセットして、変更をセットアップし直す
        blockRigid.ResetRigid();

        // 3. 初期化(ResetRigidで再初期化されない場合のみ必要)
        blockRigid.Initialize();

        if (demolishAfterRelease)
        {
            blockRigid.Demolish();
        }

        // 4. 破片にレイヤーを付与して右方向へ吹き飛ばす
        BlowFragments(blowDirection);

        // 5. 規定時間後に破片を消す
        ScheduleDestroy();
    }

    private void BlowFragments(Vector3 direction)
    {
        int fragmentLayer = LayerMask.NameToLayer(fragmentLayerName);
        if (fragmentLayer < 0)
        {
            Debug.LogWarning("レイヤー '" + fragmentLayerName + "' が存在しません。Project Settings > Tags and Layers で追加してください");
        }

        foreach (GameObject fragment in fragments)
        {
            if (fragment == null)
            {
                continue;
            }

            // レイヤー付与
            if (fragmentLayer >= 0)
            {
                fragment.layer = fragmentLayer;
            }

            // Rigidbodyがなければ追加し、物理を有効化
            if (!fragment.TryGetComponent(out Rigidbody rb))
            {
                rb = fragment.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false;
            rb.WakeUp();

            // 右方向へImpulseで吹き飛ばす
            rb.AddForce(direction * blowForce, ForceMode.Impulse);
        }

        // 吹き飛ばし後、PlayerレイヤーとFragmentレイヤーを当たらなくする
        DisablePlayerFragmentCollision(fragmentLayer);
    }

    private void DisablePlayerFragmentCollision(int fragmentLayer)
    {
        int playerLayerIndex = LayerMask.NameToLayer(playerLayerName);

        if (playerLayerIndex < 0 || fragmentLayer < 0)
        {
            Debug.LogWarning("Player / Fragment のレイヤーが見つからないため、衝突設定を変更できません");
            return;
        }

        // true = 無視する(当たらない)
        Physics.IgnoreLayerCollision(playerLayerIndex, fragmentLayer, true);
    }

    // 規定時間後に破片(とブロック本体)をDestroyする
    private void ScheduleDestroy()
    {
        foreach (GameObject fragment in fragments)
        {
            if (fragment != null)
            {
                Destroy(fragment, destroyDelay);
            }
        }

        if (destroyBlockRoot && blockRigid != null)
        {
            Destroy(blockRigid.gameObject, destroyDelay);
        }
    }
}