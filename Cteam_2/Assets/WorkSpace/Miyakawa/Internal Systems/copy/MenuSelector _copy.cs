using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuSelector_copy : MonoBehaviour
{
    [Header("シーン名")]
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string stageSelectSceneName = "StageSelect";

    [Header("メニュー項目")]
    [SerializeField] private RectTransform[] menuItems;

    [Header("各項目を決定したときの遷移先シーン名")]
    [SerializeField] private string[] destinationScenes;

    [Header("選択中の枠")]
    [SerializeField] private RectTransform selectionFrame;
    [SerializeField] private Vector2 framePadding = new Vector2(20f, 10f);

    [Tooltip("Don'tDestroyを割り当て")]
    [SerializeField]
    DoNotDestroy doNotDestroy;

    private enum MoveMode { Vertical, Horizontal }
    private MoveMode moveMode;
    private int currentIndex = 0;

    private int enumIndex = 1;
    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == stageSelectSceneName)
        {
            moveMode = MoveMode.Horizontal;   // ステージセレクト:A/D
        }
        else
        {
            moveMode = MoveMode.Vertical;     // タイトル:W/S
        }

        UpdateFrame();
    }

    private void Update()
    {
        if (moveMode == MoveMode.Vertical)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                Move(-enumIndex);  // 上
            }
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                Move(enumIndex);   // 下
            }   
        }
        else
        {
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                Move(-enumIndex);  // 左
            }
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                Move(enumIndex);   // 右
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        // 決定ボタン
        {
            Decide();
        }
    }

    private void Move(int direction)
    {
        currentIndex += direction;

        // 端まで行ったら反対側へループ
        if (currentIndex < 0)
        {
            currentIndex = menuItems.Length - 1;
        }
        if (currentIndex >= menuItems.Length)
        {
            currentIndex = 0;
        }

        UpdateFrame();
    }

    private void UpdateFrame()
    {
        RectTransform target = menuItems[currentIndex];
        selectionFrame.position = target.position;
        selectionFrame.sizeDelta = target.sizeDelta + framePadding;
    }

    private void Decide()
    {
        if (doNotDestroy.isStageClear[currentIndex + 1] == true)
        {
            if (currentIndex < destinationScenes.Length)
            {
                SceneManager.LoadScene(destinationScenes[currentIndex]);
            }
        }
    }
}