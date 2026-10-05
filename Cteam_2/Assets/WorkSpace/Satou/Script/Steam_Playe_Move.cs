using UnityEngine;

public class Steam_Playe_Move : MonoBehaviour
{
    [SerializeField] private float upperMoveSpeed = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        // 物理演算を通して上に移動
        rb.MovePosition(rb.position + Vector3.up * upperMoveSpeed * Time.fixedDeltaTime);
    }

    // WireMeshはTriggerなので、通過した瞬間にこちらが呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WireMesh"))
        {
            // WireMeshを通過した時の処理
        }
    }

    // WireMesh以外との通常の衝突
    private void OnCollisionEnter(Collision collision)
    {
        // 必要なら壁などに当たった時の処理
    }
}