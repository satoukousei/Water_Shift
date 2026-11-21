using UnityEngine;

/// <summary>
/// 氷状態のプレイヤーが、上から箱に接触したとき箱を壊すスクリプト。
/// (押して動かす処理は ItemMove が担当)
/// </summary>
public class PushBox : MonoBehaviour
{
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private GameObject CrateBox;
    [SerializeField] private Collider PlayerIceCollider;
    [SerializeField] private Collider CrateCollider;
    [SerializeField] private Rigidbody PlayerRigidbody;

    private bool isPush = false;
    private bool canBreak = false;

    private void Update()
    {
        if (PlayerIceCollider == null ||
            CrateCollider == null ||
            PlayerRigidbody == null ||
            enumList_Player == null)
        {
            canBreak = false;
            return;
        }

        // 箱の上面からプレイヤーの底面までの高さ
        float height =
            PlayerIceCollider.bounds.min.y - CrateCollider.bounds.max.y;

        Vector3 rayOrigin = new Vector3(
            PlayerIceCollider.bounds.center.x,
            PlayerIceCollider.bounds.min.y + 0.05f,
            PlayerIceCollider.bounds.center.z
        );

        bool isFirstCrateBelow =
            Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out RaycastHit hit,
                10f
            ) &&
            hit.collider.gameObject == CrateCollider.gameObject;

        bool isIce =
            enumList_Player.CurrentPlayerState ==
            EnumList_Player.PlayerState.Ice;

        if (!isPush &&
            isFirstCrateBelow &&
            height >= 1.5f &&
            isIce)
        {
            canBreak = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayerCollider(other))
            return;

        if (canBreak &&
            !isPush &&
            enumList_Player != null &&
            enumList_Player.CurrentPlayerState ==
            EnumList_Player.PlayerState.Ice)
        {
            isPush = true;
            canBreak = false;

            if (CrateBox != null)
            {
                CrateBox.SetActive(false);
            }
        }
        else
        {
            canBreak = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayerCollider(other))
        {
            canBreak = false;
        }
    }

    private bool IsPlayerCollider(Collider other)
    {
        if (other == PlayerIceCollider)
            return true;

        return PlayerRigidbody != null &&
               other.attachedRigidbody == PlayerRigidbody;
    }
}