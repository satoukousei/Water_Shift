using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [Header("レイの設定")]
    [SerializeField] private float rayDistance = 2f;

    [Header("落下判定")]
    [SerializeField] private float deathHeight = -10f;

    [Header("判定用タグ")]
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private string deathZoneTag = "DeathZone";

    private Vector3 returnPosition;
    private Quaternion returnRotation;
    private Rigidbody playerRigidbody;
    private Collider playerCollider;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        // Groundブロックが見つかる前の初期復帰地点
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
        Vector3 rayOrigin = transform.position;

        if (playerCollider != null)
        {
            rayOrigin = new Vector3(
                transform.position.x,
                playerCollider.bounds.min.y + 0.05f,
                transform.position.z);
        }

        if (Physics.Raycast(
            rayOrigin,
            Vector3.down,
            out RaycastHit hit,
            rayDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                Vector3 blockCenter = hit.collider.bounds.center;

                float playerHalfHeight = 0f;

                if (playerCollider != null)
                {
                    playerHalfHeight = playerCollider.bounds.extents.y;
                }

                returnPosition = new Vector3(
                    blockCenter.x,
                    hit.collider.bounds.max.y + playerHalfHeight,
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

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 rayOrigin = transform.position;

        if (playerCollider != null)
        {
            rayOrigin = new Vector3(
                transform.position.x,
                playerCollider.bounds.min.y + 0.05f,
                transform.position.z);
        }

        Gizmos.DrawLine(
            rayOrigin,
            rayOrigin + Vector3.down * rayDistance);
    }
}