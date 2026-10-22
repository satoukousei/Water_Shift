using UnityEngine;
using RayFire;

public class RayDestroyCount : MonoBehaviour
{
    [SerializeField] private float removeDelay = 3f; // 衝突してからコライダーを抜くまでの秒数

    private RayfireRigid rayFireRigid;
    private bool triggered = false; // 二重実行防止

    void Start()
    {
        rayFireRigid = GetComponent<RayfireRigid>();
    }

    // 何かが衝突したときに呼ばれる
    void OnCollisionEnter(Collision collision)
    {
        if (triggered)
        {
            return;
        }
        // 相手のRigidbodyがKinematicのときだけ処理
        Rigidbody other = collision.rigidbody;
        if (other != null && other.isKinematic)
        {
            triggered = true;

            // 衝突から removeDelay 秒後にコライダーを抜く
            Invoke(nameof(RemoveColliders), removeDelay);
        }
    }

    void RemoveColliders()
    {
        // 自分と子オブジェクトのコライダー
        RemoveCollidersFrom(gameObject);

        if (rayFireRigid == null)
        {
            return;
        }
        // 分割(Demolish)で生成された破片のコライダー
        if (rayFireRigid.fragments != null)
        {
            foreach (RayfireRigid frag in rayFireRigid.fragments)
            {
                if (frag != null)
                    RemoveCollidersFrom(frag.gameObject);
            }
        }

        // 破片のルートオブジェクト配下のコライダー
        if (rayFireRigid.rootChild != null)
            RemoveCollidersFrom(rayFireRigid.rootChild.gameObject);
    }

    // 指定オブジェクトと子孫のコライダーを全て削除
    void RemoveCollidersFrom(GameObject target)
    {
        foreach (Collider col in target.GetComponentsInChildren<Collider>(true))
        {
            Destroy(col);
        }
    }
}