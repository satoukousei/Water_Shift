using UnityEngine;
using UnityEngine.InputSystem;

public class WaterMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float upJumpPower;

    public bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }
        
    }
}
