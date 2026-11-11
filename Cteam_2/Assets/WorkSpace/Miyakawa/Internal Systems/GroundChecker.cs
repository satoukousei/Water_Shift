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

    private Vector3 safePosition;
    private Quaternion safeRotation;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        safePosition = transform.position;
        safeRotation = transform.rotation;

        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SaveSafePositionIfGrounded();

        if (transform.position.y < deathHeight)
        {
            ReturnToSafePosition();
        }
    }

    private void SaveSafePositionIfGrounded()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(
            rayOrigin,
            Vector3.down,
            out RaycastHit hit,
            rayDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                safePosition = transform.position;
                safeRotation = transform.rotation;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(deathZoneTag))
        {
            ReturnToSafePosition();
        }
    }

    private void ReturnToSafePosition()
    {
        transform.position = safePosition;
        transform.rotation = safeRotation;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(
            rayOrigin,
            rayOrigin + Vector3.down * rayDistance);
    }
}