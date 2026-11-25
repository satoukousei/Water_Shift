using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("レイ設定")]
    [SerializeField] private float rayStartOffset = 0.5f;   // 足元から上にずらす量
    [SerializeField] private float rayDistance = 2f;        // 下に飛ばす長さ
    [SerializeField] private LayerMask groundLayer = ~0;    // 地面のレイヤー
    [SerializeField] private Rigidbody rigidbodyParent;

    [Header("復帰位置の補正")]
    [SerializeField] private float footOffset = 0f;         // 地面の上面からのオフセット(Pivotが足元なら0)

    [Header("落下判定")]
    [SerializeField] private float deathHeight = -10f;

    [Header("判定用タグ")]
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private string deathZoneTag = "DeathZone";

    private Vector3 returnPosition;
    private Quaternion returnRotation;

    private void Awake()
    {

        returnPosition = transform.position;
        returnRotation = transform.rotation;
    }

    private void Update()
    {
        SaveGroundCenter();

        if (transform.position.y < deathHeight)
        {
            ReturnToGroundCenter();
        }
    }

    private void SaveGroundCenter()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayStartOffset;
        float distance = rayStartOffset + rayDistance;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, distance,
                            groundLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                returnPosition = new Vector3(
                    hit.collider.bounds.center.x,
                    hit.point.y + footOffset,
                    transform.position.z);

                returnRotation = transform.rotation;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(deathZoneTag))
        {
            ReturnToGroundCenter();
        }
    }

    private void ReturnToGroundCenter()
    {
        transform.position = returnPosition;
        transform.rotation = returnRotation;

        if (rigidbodyParent != null)
        {
            rigidbodyParent.linearVelocity = Vector3.zero;
            rigidbodyParent.angularVelocity = Vector3.zero;
        }
    }
}