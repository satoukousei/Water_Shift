using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Eventsを指定")]
    [SerializeField] private Events events;

    [Header("最後のステージの番号を記入")]
    [SerializeField] private int finalStageNumber = 0;

    [Header("デバッグ用にboolをインスぺクターでいじれるようにする")]
    [SerializeField] private bool isFinalStage = false;


    private int currentStageNumber = 0;

    private void Start()
    {
        currentStageNumber = SceneManager.GetActiveScene().buildIndex;
        events.EndFadein.AddListener(LoadNextScene);
    }

    public void LoadNextScene()
    {
        if (isFinalStage)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentIndex + 1);
        }

    }
}
