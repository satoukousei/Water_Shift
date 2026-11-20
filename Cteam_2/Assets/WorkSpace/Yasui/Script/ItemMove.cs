using UnityEngine;

/// <summary>
/// 【目的】
///  アイテム(箱や鍵)どうしが当たったときにそのアイテムの位置を固定するためにあります。
///  
/// 【使い方】
/// プロト段階では木箱を壊すこと(canMoveをtrueにすること)で位置固定を解除します。
/// 完成版ではbool追加で位置解除を増やし余計な位置固定を防いでください。
/// </summary>
public class ItemMove : MonoBehaviour
{
    //private bool positionLocked;                  //互いにぶつかったときに位置を固定させるためのトリガー
    private Vector3 fixedPosition;                //ぶつかったときの位置を記録する

    [SerializeField] private PushBox pushBox;     //位置を固定させる物体(基本はつけている物体を入れる)

    private void Start()
    {
        fixedPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        //一定のタグでは位置を固定しない
        if (other.CompareTag("Player") ||
            other.CompareTag("Ground") ||
            other.CompareTag("Slope"))
        {
            return;
        }


        if(other.CompareTag("box"))
        {
            //位置記録、固定
            fixedPosition = transform.position;
            //positionLocked = true;
            pushBox.CanMove = false;
        }
    }

    private void LateUpdate()
    {
        ////位置を固定する
        //if (positionLocked)
        //{
        //    transform.position = fixedPosition;
        //}
        ////壊すと動かせるようになる
        //if (pushBox.CanMove)
        //{
        //    positionLocked = false;
        //}

        if (!pushBox.CanMove)
        {
            transform.position = fixedPosition;
        }
        else
        {
            fixedPosition = transform.position;
        }
    }
}
