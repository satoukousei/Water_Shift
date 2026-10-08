using UnityEngine;

/// <summary>
/// 指定したタグのキャラクターだけ通り抜けられる床
/// 床オブジェクトに「通常のCollider」と「IsTriggerをオンにしたCollider」の2つを付けて使います。
/// </summary>
public class TagPassFloor : MonoBehaviour
{
    [Tooltip("通り抜けできるタグ")]
    [SerializeField] private string[] passableTags = { "Water", "Steam" };

    private Collider solidCollider; // 実際に踏める方のCollider

    private void Awake()
    {
        // IsTriggerがオフのColliderを「床本体」として取得
        foreach (var c in GetComponents<Collider>())
        {
            if (!c.isTrigger)
            {
                solidCollider = c;
                break;
            }
        }

        if (solidCollider == null)
            Debug.LogWarning("IsTriggerがオフのColliderが見つかりません", this);
    }

    // トリガー範囲内にいる間、毎フレーム現在のタグを確認する
    // （範囲内で形態が変わっても対応できる）
    private void OnTriggerStay(Collider other)
    {
        if (solidCollider == null) return;
        Physics.IgnoreCollision(solidCollider, other, IsPassable(other));
    }

    // 範囲外に出たら衝突判定を元に戻す
    private void OnTriggerExit(Collider other)
    {
        if (solidCollider == null) return;
        Physics.IgnoreCollision(solidCollider, other, false);
    }

    private bool IsPassable(Collider other)
    {
        foreach (var tag in passableTags)
        {
            if (other.CompareTag(tag)) return true;
        }
        return false;
    }
}
