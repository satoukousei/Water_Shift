using UnityEngine;
using UnityEngine.LowLevel;

/// <summary>
/// 【目的】
/// プレイヤーが氷状態の時、箱を壊すためのスクリプトです。
/// 【使い方】
/// 箱を壊すことでcanMoveをtrueにし位置の解除をします。
/// </summary>

public class PushBox : MonoBehaviour
{ 
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private GameObject CrateBox;      //壊す箱をとる
    [SerializeField] private Collider PlayerIceCollider;      //壊す箱をとる
    [SerializeField] private Collider CrateCollider;
    public bool isPush = false;                         //箱を押したかどうか
    public bool canMove = false;                        //くっついて動かせない時に壊して動かせるようにするためのトリガー
    public bool canBreak = false;

    void Update()
    {
        // 箱の上面からプレイヤーの底面までの高さを測る
        float height = PlayerIceCollider.bounds.min.y - CrateCollider.bounds.max.y;

        Debug.Log(height);

        // 2m以上の高さから落下していて、氷状態なら箱を壊す
        if (height >= 1.5f &&
            enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice)
        {
            canBreak = true;
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (canBreak && enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice)
        {
            isPush = true;
            canMove = true;
            CrateBox.SetActive(false);
        }
        else canBreak = false;
    }
}