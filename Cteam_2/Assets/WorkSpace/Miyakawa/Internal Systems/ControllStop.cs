
using UnityEngine;

public class ControllStop : MonoBehaviour
{
    [Header("ƒvƒŒƒCƒ„[‚ğŠ„‚è“–‚Ä")]
    [SerializeField] GameObject player;

    [Header("Events‚ğŠ„‚è“–‚Ä")]
    [SerializeField] Events events;

    private bool isPlayerStop = false;
    private Vector3 stopPosition;
    private Rigidbody rb;


    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
        events.PlayerGoal.AddListener(DisableMovement);
    }

    private void Update()
    {
        PlayerStop();
    }
    private void LateUpdate()
    {
        PlayerStop();
    }


    private void PlayerStop()
    {
        if (isPlayerStop)
        {
            player.transform.position = stopPosition;
            rb.isKinematic = false;
        }
    }

    public void EnableMovement()
    {
        isPlayerStop = false;
    }

    public void DisableMovement()
    {
        isPlayerStop = true;
        stopPosition = player.transform.position;
    }
}
