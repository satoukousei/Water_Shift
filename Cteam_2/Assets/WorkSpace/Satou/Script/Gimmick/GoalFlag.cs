using UnityEngine;

public class GoalFlag : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // プレイヤーがゴールフラグに触れたときの処理
            Debug.Log("Goal!");
            // ここにゴール時の処理を追加することができます
        }
    }
}
