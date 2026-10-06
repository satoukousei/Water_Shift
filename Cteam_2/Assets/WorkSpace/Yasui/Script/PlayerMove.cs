using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    public Rigidbody rb;

    public bool isIce = true;     //氷状態であるかどうかのトリガー(あとで移動させる)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.dKey.isPressed)     // 右
        {
            transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
        }

        if (Keyboard.current.aKey.isPressed)     //左
        {
            transform.position += new Vector3(-MoveSpeed * Time.deltaTime, 0, 0);
        }

        //後で消す

        if (Keyboard.current.wKey.isPressed)     // 前
        {
            transform.position += new Vector3(0, 0, MoveSpeed * Time.deltaTime);
        }

        if (Keyboard.current.sKey.isPressed)     //後ろ
        {
            transform.position += new Vector3(0, 0, -MoveSpeed * Time.deltaTime);
        }
    }
}
