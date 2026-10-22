using UnityEngine;

/// <summary>
/// 指定したタグのキャラクターだけ通り抜けられる床
/// 床オブジェクトに「通常のCollider」と「IsTriggerをオンにしたCollider」の2つを付けて使います。
/// </summary>
public class TagPassFloor : MonoBehaviour
{
    private const string water_Tag = "Water";
    private const string steam_Tag = "Steam";

    [Tooltip("通り抜けできるタグ")]
    [SerializeField] private string[] passableTags = { water_Tag, steam_Tag };

    // 実際に踏める方のCollider
    private Collider solidCollider = null;

    /// <summary>
    /// IsTriggerがオフのColliderを床本体として取得する
    /// </summary>
    private void Awake()
    {
        foreach (Collider currentCollider in GetComponents<Collider>())
        {
            if (!currentCollider.isTrigger)
            {
                solidCollider = currentCollider;
                break;
            }
        }

        if (solidCollider == null)
        {
            Debug.LogWarning("IsTriggerがオフのColliderが見つかりません", this);
        }
    }

    /// <summary>
    /// トリガー範囲内にいる間、毎フレーム現在のタグを確認する
    /// （範囲内で形態が変わっても対応できる）
    /// </summary>
    /// <param name="other">範囲内にいるCollider</param>
    private void OnTriggerStay(Collider other)
    {
        if (solidCollider == null)
        {
            return;
        }

        Physics.IgnoreCollision(solidCollider, other, IsPassable(other));
    }

    /// <summary>
    /// 範囲外に出たら衝突判定を元に戻す
    /// </summary>
    /// <param name="other">範囲外に出たCollider</param>
    private void OnTriggerExit(Collider other)
    {
        if (solidCollider == null)
        {
            return;
        }

        Physics.IgnoreCollision(solidCollider, other, false);
    }

    /// <summary>
    /// 通り抜けできるタグかどうかを判定する
    /// </summary>
    /// <param name="other">判定対象のCollider</param>
    /// <returns>通り抜け可能ならtrue</returns>
    private bool IsPassable(Collider other)
    {
        foreach (string passableTag in passableTags)
        {
            if (other.CompareTag(passableTag))
            {
                return true;
            }
        }

        return false;
    }
}
