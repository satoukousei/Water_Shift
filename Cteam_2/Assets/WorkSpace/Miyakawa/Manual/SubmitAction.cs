using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SubmitAction : MonoBehaviour
{
    [Header("メニューのBG画像を指定")]
    [SerializeField] private GameObject menuBGImage;

    [Header("操作説明の画像を指定")]
    [SerializeField] private GameObject manualImage;

    [Header("ButtonSelectorを指定")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("InputActionを割り当て")]
    public InputActionReference menuActionRef;

    private bool isManualImageActive = false;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (menuActionRef == null)
        {
            Debug.LogWarning("playerActionRefが未設定です。");
            return;
        }

        var action = menuActionRef.action;
        action.started += OnMoveStarted;
        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (menuActionRef == null) return;

        var action = menuActionRef.action;
        action.started -= OnMoveStarted;
        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        if (menuBGImage != null && menuBGImage.activeSelf)
        {
            switch (buttonSelector.menucurrentState)
            {
                case ButtonSelector.MenuButtonState.Retry:
                    Debug.Log("リトライボタンが押されました。");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case ButtonSelector.MenuButtonState.SelectStage:
                    SceneManager.LoadScene("StageSelectScene");
                    break;
                case ButtonSelector.MenuButtonState.Manual:
                    if (manualImage != null)
                    {
                        buttonSelector.ActiveManualImage();
                    }
                    break;
            }
        }
    }
}
