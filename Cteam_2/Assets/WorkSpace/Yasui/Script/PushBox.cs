using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField] private float breakBoxSpeed;
    [SerializeField] private float changeScale;
    [SerializeField] private PlayerMove playerMove;

    public bool isPush = false; //箱を押したかどうか
    [SerializeField] private GameObject normalBox;
    [SerializeField] private GameObject pressedBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //箱自体のスケールを小さくするやり方
        //if (collision.gameObject.CompareTag("Player") && !isPush && collision.relativeVelocity.magnitude < breakBoxSpeed)
        //{
        //    Transform boxTransform = transform;

        //    Vector3 scale = boxTransform.localScale;
        //    scale.y *= changeScale;
        //    boxTransform.localScale = scale;
        //    isPush = true;
        //}

        //親オブジェクトを消して子オブジェクトに画像を変更する
        if (collision.gameObject.CompareTag("Player") &&
           !isPush &&
           collision.relativeVelocity.magnitude < breakBoxSpeed)
        {
            // 普通の箱を非表示にする
            normalBox.SetActive(false);

            // 押された箱を表示する
            pressedBox.SetActive(true);

            isPush = true;
        }
    }
}
