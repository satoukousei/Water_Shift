
using UnityEngine;

public class ControllStop : MonoBehaviour
{


    [Header("Events‚ðŽw’è")]
    [SerializeField] private Events events;

    [Header("InputActionAsset‚ðŽw’è")]
    [SerializeField] private InputActionAssetController inputActionAssetController;

    private bool isPlayerStop = false;
    private Vector3 stopPosition;
    private Rigidbody rb;


    private void Start()
    {
        events.PlayerGoal.AddListener(DisableMovement);
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
    }
}
