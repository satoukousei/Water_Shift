using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    [Header("プレイヤー名を記入")]
    [SerializeField] private string PlayerParentName;

    [Header("Eventsがコンポーネントされているオブジェクトを指定")]
    [SerializeField] private Events events;

    [Header("フェード用スクリプトを指定")]
    [SerializeField] private FadeEffect fadeEffect;

    //ステージ選択等結滞したときにコメントアウトを外す

    //[Header("Don'tDestroy用のスクリプトを指定")]
    //[SerializeField] private DoNotDestroy doNotDestroy;

    //[Header("ステージ番号を記入(1始まり)")]
    //[SerializeField] private int stageNumber = 1;

    private void Start()
    {
      events.PlayerGoal.AddListener(GoalEvent);
    }

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
      //ステージ選択等決定したときにコメントアウトを外す
      //  doNotDestroy.isStageClear[stageNumber] = true;
        fadeEffect.StartFadeOut();
    }
}
