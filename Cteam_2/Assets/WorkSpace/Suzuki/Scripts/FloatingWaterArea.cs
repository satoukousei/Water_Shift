using UnityEngine;
using static EnumList_Player;

public class FloatingWaterArea : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("浮力の設定")]
    [SerializeField] private float buoyancyForce = 15f;

    private void OnTriggerStay(Collider other)
    {
        ApplyBuoyancy(other);
    }

    /// <summary>
    /// 接触対象のプレイヤー状態を確認し、氷状態であれば上向きの浮力を加える
    /// </summary>
    /// <param name="other">接触したコライダー</param>
    private void ApplyBuoyancy(Collider other)
    {
        EnumList_Player playerEnum = other.GetComponentInParent<EnumList_Player>();
        if (playerEnum == null)
        {
            return;
        }

        // プレイヤーが氷状態の場合のみ浮力を適用
        if (playerEnum.CurrentPlayerState == PlayerState.Ice)
        {
            Rigidbody targetRigidbody = other.attachedRigidbody;
            if (targetRigidbody != null)
            {
                targetRigidbody.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
            }
        }
    }
}
