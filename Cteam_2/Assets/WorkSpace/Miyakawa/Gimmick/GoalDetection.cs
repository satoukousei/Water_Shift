using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    [Header("プレイヤー名を記入")]
    [SerializeField] private string PlayerParentName;

    [Header("Eventsがコンポーネントされているオブジェクトを指定")]
    [SerializeField] Events events;

    [Header("フェード用スクリプトを指定")]
    [SerializeField] FadeEffect fadeEffect;

    [Header("プレイヤー停止用スクリプトを指定")]
    [SerializeField] ControllStop controllStop;

    [Header("Don'tDestroy用のスクリプトを指定")]
    [SerializeField] DoNotDestroy doNotDestroy;

    [Header("ステージ番号を記入(1始まり)")]
    [SerializeField] private int stageNumber = 1;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == PlayerParentName)
        {
            events.PlayerGoal.Invoke();
            Debug.Log("ゴール");
        }
    }

    public void GoalEvent()
    {
        doNotDestroy.isStageClear[stageNumber] = true;
        fadeEffect.StartFadeOut();
    }
}
