
using UnityEngine;

public class ControllStop : MonoBehaviour
{
    [Header("プレイヤーを指定")]
    [SerializeField] private GameObject player;

    [Header("Eventsを指定")]
    [SerializeField] private Events events;

    [Header("InputActionAssetを指定")]
    [SerializeField] private InputActionAssetController inputActionAssetController;

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
        DisableMovement();
    }
    private void LateUpdate()
    {
        DisableMovement();
    }


    public void EnableMovement()
    {
        isPlayerStop = false;
        inputActionAssetController.EnableActionAsset();
    }

    public void DisableMovement()
    {
        isPlayerStop = true;
        inputActionAssetController.DisableActionAsset();
        player.transform.position = stopPosition;
        rb.isKinematic = false;
    }
}
