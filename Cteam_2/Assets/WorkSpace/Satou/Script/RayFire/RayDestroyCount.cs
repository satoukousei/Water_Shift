using UnityEngine;
using RayFire;

public class RayDestroyCount : MonoBehaviour
{
    [SerializeField] private float removeDelay = 3f; // コライダーを抜くまでの秒数

    private RayfireRigid rayFireRigid;

    void Start()
    {
        rayFireRigid = GetComponent<RayfireRigid>();

        // 開始から removeDelay 秒後に実行
        Invoke(nameof(RemoveColliders), removeDelay);
    }

    void RemoveColliders()
    {
        // 自分と子オブジェクトのコライダー
        RemoveCollidersFrom(gameObject);

        if (rayFireRigid == null) return;

        // 分割で生成された破片のコライダー
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

    // 指定オブジェクトと子のコライダーを全て外す
    void RemoveCollidersFrom(GameObject target)
    {
        foreach (Collider col in target.GetComponentsInChildren<Collider>(true))
        {
            Destroy(col);
        }
    }
}