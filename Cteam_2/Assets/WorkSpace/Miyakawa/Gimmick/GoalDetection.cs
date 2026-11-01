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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == PlayerParentName)
        {
            events.Goal.Invoke();
            Debug.Log("ゴール");
        }
    }

    public void GoalEvent()
    {
        fadeEffect.StartFadeOut();
    }
}
