
using UnityEngine;

public class ControllStop : MonoBehaviour
{
    [Header("ÉvÉåÉCÉÑÅ[ÇäÑÇËìñÇƒ")]
    [SerializeField] GameObject player;

    private bool isPlayerStop = false;
    private Vector3 stopPosition;
    private Rigidbody rb;


    private void Start()
    {
        rb = player.GetComponent<Rigidbody>();
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
