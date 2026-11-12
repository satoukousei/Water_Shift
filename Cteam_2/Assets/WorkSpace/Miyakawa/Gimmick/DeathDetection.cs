using UnityEngine;
using static EnumList_Player;

public class DeathDetection : MonoBehaviour
{
    [Header("プレイヤー名を記入")]
    [SerializeField]private string PlayerParentName;
    [Header("状態を設定")]
    [SerializeField]
    private PlayerState playerState = PlayerState.Water; 
    [Header("EnumListを指定")]
    [SerializeField] private EnumList_Player changeStatePlayer;
    [Header("Eventsを指定")]
    [SerializeField] private Events events;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
     if(other.gameObject.name == PlayerParentName)
        {
            if(changeStatePlayer.CurrentPlayerState == playerState)
            {
                events.PlayerDeath.Invoke();

                Debug.Log("死亡判定");
                //ここに死亡時の処理を追加
            }
        }
    }
}
