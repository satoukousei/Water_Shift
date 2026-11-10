using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【目的】
/// プレイヤーが水状態または水蒸気状態のときに鍵を通りにけるようにするためのスクリプトです。
/// 【使い方】
/// なし
/// </summary>

public class KeyAction : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider keyCollider;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ここで作っておくと便利
        bool ignore = playerMove.IsWater || playerMove.IsSteam;
        //水状態、水蒸気状態のときは重なるようにする
        Physics.IgnoreCollision
        //レイヤー取得
        (playerCollider,
        keyCollider,
        ignore
        );
    }
}
