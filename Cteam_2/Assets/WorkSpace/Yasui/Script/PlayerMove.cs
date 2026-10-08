using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;

    [SerializeField] public Rigidbody rb;

    public bool isIce = true; 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.dKey.isPressed)     // 右
        {
            transform.parent.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
        }

        if (Keyboard.current.aKey.isPressed)     //左
        {
            transform.parent.position += new Vector3(-MoveSpeed * Time.deltaTime, 0, 0);
        }
    }

    public void IceRbSwitch()
    {
        rb.useGravity = true;
    }
}
