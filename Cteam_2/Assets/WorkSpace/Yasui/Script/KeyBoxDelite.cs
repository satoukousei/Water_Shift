using UnityEngine;
/// <summary>
/// 【目的】
/// 鍵箱を消すためのスクリプトです。
/// 【使い方】
/// 鍵と箱が衝突したときに箱を削除します。
/// </summary>

public class KeyBoxDelite : MonoBehaviour
{
    [SerializeField] private Collider KeyCollider;
    [SerializeField] private GameObject KeyBox;
    [SerializeField] private GameObject Key;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == KeyCollider)
        {
            Destroy(Key);
            Destroy(KeyBox);
        } 
    }
}
