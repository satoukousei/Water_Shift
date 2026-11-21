using UnityEngine;

/// <summary>
/// 隣接物(上下左右)がある間は位置を固定し、
/// 隣接物がない状態で「氷状態のプレイヤー」に横から押されたときだけ固定を解除する。
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ItemMove : MonoBehaviour
{
    [SerializeField] private EnumList_Player enumList_Player;  // プレイヤーの状態
    [SerializeField] private string neighborTag = "box";
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float checkMargin = 0.05f;
    [SerializeField] private float pushNormalThreshold = 0.5f;


    private static readonly Vector3[] CheckDirs =
    {
        Vector3.left, Vector3.right, Vector3.up, Vector3.down
    };

    private Collider col;
    private Vector3 fixedPosition;
    private bool isLocked = true;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        fixedPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (HasNeighbor())
        {
            Lock();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isLocked) return;
        if (!IsPlayer(collision)) return;

        // 氷状態のプレイヤーだけが動かせる
        if (enumList_Player == null ||
            enumList_Player.CurrentPlayerState != EnumList_Player.PlayerState.Ice)
        {
            return;
        }

        // 横方向(X/Z)から押されているか
        bool pushedFromSide = false;
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Mathf.Abs(contact.normal.x) > pushNormalThreshold ||
                Mathf.Abs(contact.normal.z) > pushNormalThreshold)
            {
                pushedFromSide = true;
                break;
            }
        }
        if (!pushedFromSide) return;

        // 隣に箱があれば固定のまま
        if (HasNeighbor()) return;

        isLocked = false;

    }

    private void LateUpdate()
    {
        if (isLocked)
        {
            transform.position = fixedPosition;
        }
    }

    private void Lock()
    {
        if (!isLocked)
        {
            isLocked = true;
            fixedPosition = transform.position;

        }
    }

    private bool IsPlayer(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag)) return true;
        if (collision.transform.CompareTag(playerTag)) return true;
        if (collision.transform.root.CompareTag(playerTag)) return true;
        return false;
    }

    private bool HasNeighbor()
    {
        Bounds b = col.bounds;

        foreach (Vector3 dir in CheckDirs)
        {
            float extent = Mathf.Abs(Vector3.Dot(b.extents, dir));
            float distance = extent + checkMargin;



            RaycastHit[] hits = Physics.RaycastAll(
                b.center, dir, distance,
                ~0, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == col) continue;
                if (hit.collider.transform.IsChildOf(transform)) continue;

                if (hit.collider.CompareTag(neighborTag))
                {

                    return true;
                }
            }
        }
        return false;
    }
}