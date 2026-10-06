using Unity.VisualScripting;
using UnityEngine;

public class breakblock : MonoBehaviour
{
    [SerializeField] private float breakSpeed;     //壊すために必要な落下速度

    [SerializeField] private PlayerMove playerMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && playerMove.rb.linearVelocity.magnitude > breakSpeed && playerMove.isIce)
        {
            Destroy(gameObject);
        }
    }
}
