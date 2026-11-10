using UnityEngine;

/// <summary>
/// 【目的】
/// プレイヤーが氷状態の時、箱を壊すためのスクリプトです。
/// 【使い方】
/// 箱を壊すことでcanMoveをtrueにし位置の解除をします。
/// </summary>

public class PushBox : MonoBehaviour
{
    [SerializeField] private float breakBoxSpeed;
    [SerializeField] private float changeScale;
    [SerializeField] private PlayerMove playerMove;

    public bool isPush = false; //箱を押したかどうか
    [SerializeField] private GameObject normalBox;
    public bool canMove = false;

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

    }

    private void OnTriggerEnter(Collider other)
    {
        var playerVelocity = GetComponent<Rigidbody>();
        if(playerVelocity == null)
        {
            return;
        }
        //親オブジェクトを消して子オブジェクトに画像を変更する
        if (other.gameObject.CompareTag("Player") &&
           !isPush &&
           Mathf.Abs(playerVelocity.linearVelocity.y) < breakBoxSpeed
           && playerMove.IsIce)
        {
            
            Destroy(normalBox);
            canMove = true;
            ////箱自体のスケールを小さくするやり方

            //Transform boxTransform = transform;

            //Vector3 scale = boxTransform.localScale;
            //scale.y *= changeScale;
            //boxTransform.localScale = scale;
            //normalBoxCollider.size = scale;
            //isPush = true;

            //// 普通の箱を非表示にする
            //normalBox.SetActive(false);
            //normalBoxCollider.SetActive(false);

            //// 押された箱を表示する
            //pressedBox.SetActive(true);

            //isPush = true;
            //boxPosition.ChangeBox();     //★
        }
    }
}
