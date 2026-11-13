using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("プレイヤーのアニメーションを割り当て")]
    [SerializeField] private Animator animator;

    [Header("イベント管理用スクリプトを割り当て")]
    [SerializeField] private Events events;

    private bool isCheckingGoalAnimation = false;

    private void Update()
    {
        if (isCheckingGoalAnimation)
        {
            if (IsGoalAnimationEnd())
            {
                isCheckingGoalAnimation = false;

                events.EndGoalAnimation.Invoke();
                Debug.Log("ゴールアニメーション終了");
            }
        }

    }

    public bool IsGoalAnimationEnd()
    {
        AnimatorStateInfo stateInfo =
            animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Goal") &&
            stateInfo.normalizedTime >= 1.0f)
        {
            return true;
        }

        return false;
    }

    private void isCheckingGoalAnimationStart()
    {
               isCheckingGoalAnimation = true;
    }
}