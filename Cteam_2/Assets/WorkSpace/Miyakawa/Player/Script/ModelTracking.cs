using UnityEngine;

public class ModelTracking : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    public void TransferModel()
    {
        transform.position = Player.transform.position;
    }
}
