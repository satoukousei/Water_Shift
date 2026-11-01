using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyAction : MonoBehaviour
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Collider keyCollider;
    bool hasKey = false;
    bool ishit = false;
    bool isKeyHold = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //水状態のときは重なるようにする
        //このコライダーだけ動かないようにする
        Physics.IgnoreCollision
        //レイヤー取得
        (playerCollider,     
        keyCollider,
        playerMove.IsWater
        );
        

        //水状態のときに氷状態(仮で下キーを押したときに)にすると鍵を内包(子オブ)にする
        if (hasKey && playerMove.IsWater && !isKeyHold &&
            Keyboard.current != null &&     //キーボード入力チェック
            Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            Debug.Log("子オブジェクトになりました");
            //transform.SetParentで親オブジェクトを設定、playerMove.transformで親オブジェクト元を指定
            transform.SetParent(playerMove.transform);
            transform.position = playerMove.transform.position;

            isKeyHold = true;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        //プレイヤーにあったっているかつ水状態かつ鍵を持っていない時
        if (collision.gameObject.CompareTag("Player") && playerMove.IsWater && !isKeyHold)
        {
            //鍵を取得できるようにする
            Debug.Log("鍵を取得できます");
            hasKey = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isKeyHold)
        {
            Debug.Log("鍵を離しました");
            hasKey = false;
        }
    }
}
