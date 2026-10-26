using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [Header("メニュー項目")]
    [SerializeField] private RectTransform[] menuItems;

    [Header("選択中の枠(Imageを持つオブジェクト)")]
    [SerializeField] private RectTransform selectionFrame;

    [Header("枠の余白")]
    [SerializeField] private Vector2 framePadding = new Vector2(20f, 10f);

    [Header("遷移先シーン名")]
    [SerializeField] private string newGameSceneName = "Stage1";
    [SerializeField] private string stageSelectSceneName = "StageSelect";

    private int currentIndex = 0;

    private void Start()
    {
        UpdateFrame();
    }

    private void Update()
    {
        // Wキーで上へ
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = menuItems.Length - 1;
            UpdateFrame();
        }

        // Sキーで下へ
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            currentIndex++;
            if (currentIndex >= menuItems.Length) currentIndex = 0;
            UpdateFrame();
        }

        // 決定(Enter or Space)
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Decide();
        }
    }

    // 枠を選択中の項目に重ねる
    private void UpdateFrame()
    {
        RectTransform target = menuItems[currentIndex];
        selectionFrame.position = target.position;
        selectionFrame.sizeDelta = target.sizeDelta + framePadding;
    }

    private void Decide()
    {
        switch (currentIndex)
        {
            case 0:
                SceneManager.LoadScene(newGameSceneName);
                break;
            case 1:
                SceneManager.LoadScene(stageSelectSceneName);
                break;
        }
    }
}