using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    [Header("プレイヤー名を記入")]
    [SerializeField] private string PlayerParentName;
    [Tooltip("Eventsがコンポーネントされているオブジェクトを指定")]
    [SerializeField]
    Events events;
    [Tooltip("フェード用イメージを指定")]
    [SerializeField]
    FadeEffect fadeEffect;
    [Tooltip("ステージ番号を記入(1始まり)")]
    [SerializeField]
    private int stageNumber = 1;
    [SerializeField]
    DoNotDestroy doNotDestroy;

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
