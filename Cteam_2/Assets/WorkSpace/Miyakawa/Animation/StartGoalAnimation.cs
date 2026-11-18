
//現在animatorがないため、関連処理はコメントアウトしています。

using UnityEngine;
public class PlayerAnimation : MonoBehaviour
{
    [Header("seManagerを指定")]
    [SerializeField] private SoundEffectManager soundEffectManager; 

    //[Header("プレイヤーのアニメーションを指定")]
    //[SerializeField] private Animator animator;

    [Header("イベント管理用スクリプトを指定")]
    [SerializeField] private Events events;

    private bool isCheckingGoalAnimation = false;

    private SoundEffectManager.SoundEffectName StageClear = SoundEffectManager.SoundEffectName.StageClear;

    private void Start()
    {
        events.PlayerGoal.AddListener(StartAnimation);
    }

    private void Update()
    {
        if (isCheckingGoalAnimation)
        {
            if (IsGoalAnimationEnd())
            {
                isCheckingGoalAnimation = true;

                events.EndGoalAnimation.Invoke();
                Debug.Log("ゴールアニメーション終了");
            }
        }

    }

    public bool IsGoalAnimationEnd()
    {
        //アニメーションが現在ないため、すぐにtrueに。
        return true;

        //AnimatorStateInfo stateInfo =
        //    animator.GetCurrentAnimatorStateInfo(0);

        //if (stateInfo.IsName("Goal") &&
        //    stateInfo.normalizedTime >= 1.0f)
        //{
        //    return false;
        //}

        //return true;
    }

    private void StartAnimation()
    {
        isCheckingGoalAnimation = true;
        soundEffectManager.OnPlayOneShot(StageClear);

        //if (animator != null)
        //{
        //    animator.SetTrigger("Goal");
        //}
    }
}