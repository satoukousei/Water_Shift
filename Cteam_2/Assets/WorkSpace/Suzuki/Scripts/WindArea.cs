using UnityEngine;
using static EnumList_Player;

/// <summary>
/// 風の向きを定義する列挙型
/// </summary>
public enum WindDirection
{
    Right,
    Left,
    Up,
    Down
}

/// <summary>
/// 【使い方】
/// ・風を起こしたいエリア（空間）のオブジェクトにアタッチして使用します。
/// ・オブジェクトには「IsTriggerをオンにしたCollider」を設定し、インスペクターから風の向きと風速を調整してください。
/// 
/// 【目的】
/// ・プレイヤーが気体（Steam）状態のときのみ風の力を受けて、指定した方向へ押し流されるようにするためにあります。
/// </summary>
public class WindArea : MonoBehaviour
{
    [Header("風の設定")]
    [SerializeField] private WindDirection windDirection = WindDirection.Right;
    [SerializeField] private float windPower = 10.0f;

    /// <summary>
    /// トリガー範囲内にいる間、気体状態のプレイヤーに風力を与え続ける
    /// </summary>
    /// <param name="other">範囲内にいるコライダー</param>
    private void OnTriggerStay(Collider other)
    {
        ApplyWindForce(other);
    }

    /// <summary>
    /// プレイヤーが気体（Steam）状態の場合、指定した方向へ風の力を加えます
    /// </summary>
    /// <param name="other">接触したコライダー</param>
    private void ApplyWindForce(Collider other)
    {
        EnumList_Player playerEnum = other.GetComponentInParent<EnumList_Player>();
        if (playerEnum == null)
        {
            return;
        }

        if (playerEnum.CurrentPlayerState == PlayerState.Steam)
        {
            Rigidbody targetRigidbody = other.attachedRigidbody;
            if (targetRigidbody != null)
            {
                Vector3 directionVector = GetDirectionVector(windDirection);
                targetRigidbody.AddForce(directionVector * windPower, ForceMode.Acceleration);
            }
        }
    }

    /// <summary>
    /// 設定された風の向きに対応する方向ベクトルを返します
    /// </summary>
    /// <param name="direction">風の向き</param>
    /// <returns>方向ベクトル</returns>
    private Vector3 GetDirectionVector(WindDirection direction)
    {
        switch (direction)
        {
            case WindDirection.Right:
                return Vector3.right;
            case WindDirection.Left:
                return Vector3.left;
            case WindDirection.Up:
                return Vector3.up;
            case WindDirection.Down:
                return Vector3.down;
            default:
                return Vector3.zero;
        }
    }
}