using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 【目的】
/// プレイヤーが氷状態の時、箱を壊すためのスクリプトです。
/// 【使い方】
/// 箱を壊すことでcanMoveをtrueにし位置の解除をします。
/// </summary>

public class PushBox : MonoBehaviour
{ 
    [SerializeField] private EnumList_Player enumList_Player;
    [SerializeField] private GameObject CrateBox;      //壊す箱をとる
    [SerializeField] private Collider PlayerIceCollider;      //壊す箱をとる
    [SerializeField] private Collider CrateCollider;
    [SerializeField] private SoundEffectManager soundEffectManager;
    //～●privateに変更しました●～
    private bool isPush = false;                         //箱を押したかどうか
    private bool canMove = false;                        //くっついて動かせない時に壊して動かせるようにするためのトリガー
    private bool canBreak = false;
    //～●ここまで●～
    public bool CanMove { get => canMove; set => canMove = value; }
    void Update()
    {

        if (enumList_Player.CurrentPlayerState != EnumList_Player.PlayerState.Ice)
        {
            isPush = false;
            canMove = false;
            canBreak = false;
        }

        // 箱の上面からプレイヤーの底面までの高さを測る
        float height = PlayerIceCollider.bounds.min.y - CrateCollider.bounds.max.y;

        // 2m以上の高さから落下していて、氷状態なら箱を壊す
        if (height >= 1.5f &&
            enumList_Player.CurrentPlayerState == EnumList_Player.PlayerState.Ice)
        {
            canBreak = true;
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ice"))
        {
            if (canBreak)
            {
                isPush = true;
                canMove = true;
                CrateBox.SetActive(false);
            }
            else
            {
                canMove = false;
                isPush = false;
            }
            canBreak = false;
            if (soundEffectManager != null)
            {
                soundEffectManager.OnPlayOneShot(SoundEffectManager.SoundEffectName.BreakWoodBox);
            }
            CrateBox.SetActive(false);
        }
        else
        {
            canMove = false;
            isPush = false;
            canBreak = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (enumList_Player.CurrentPlayerState != EnumList_Player.PlayerState.Ice)
            {
                isPush = false;
                canMove = false;
                return;
            }

            isPush = true;
            canMove = true;
        }
        else
        {
            canBreak = false;
        }
    }
}