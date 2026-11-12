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
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider keyCollider;
    // Update is called once per frame
    void Update()
    {
        //ここで作っておくと便利
        bool ignore = enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Water ||
                                 enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Steam;


        //水状態、水蒸気状態のときは重なるようにする
        Physics.IgnoreCollision
            //レイヤー取得
            (playerCollider,
            keyCollider,
            ignore
            );
    }
}
