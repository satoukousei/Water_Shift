using UnityEngine;

/// <summary>
/// プレイヤーが触れるとゴール処理を行うフラグ
/// </summary>
public class GoalFlag : MonoBehaviour
{
    private const string player_Layer_Name = "Player";

    /// <summary>
    /// トリガーに何かが入ったときに呼ばれる
    /// </summary>
    /// <param name="other">接触したCollider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(player_Layer_Name))
        {
            // プレイヤーがゴールフラグに触れたときの処理
            // ここにゴール時の処理を追加する
        }
    }
}
