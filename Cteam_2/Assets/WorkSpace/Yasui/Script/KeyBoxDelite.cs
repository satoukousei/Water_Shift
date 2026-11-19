using UnityEngine;
/// <summary>
/// 【目的】
/// 鍵箱を消すためのスクリプトです。
/// 【使い方】
/// 鍵と箱が衝突したときに箱を削除します。
/// </summary>

public class KeyBoxDelite : MonoBehaviour
{
    [SerializeField] private Collider KeyCollider;     //鍵のコライダーをとる
    [SerializeField] private GameObject KeyBox;        //鍵箱をとる
    [SerializeField] private GameObject Key;           //鍵をとる
    private void OnCollisionEnter(Collision collision)
    {
        //鍵と鍵箱がぶつかったら互いを壊す
        if (collision.collider == KeyCollider)
        {
            Destroy(Key);
            Destroy(KeyBox);
        } 
    }
}
