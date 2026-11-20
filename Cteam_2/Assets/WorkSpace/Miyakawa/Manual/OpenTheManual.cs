using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OpenTheManual : MonoBehaviour
{
    [Header("ButtonSelectorを指定")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("OpenTheManualを指定")]
    [SerializeField] private OpenTheMenu openTheMenu;

    [Header("InputActionRefを指定")]
    [SerializeField] private InputActionReference menuActionRef;

    [Header("ManualImageを指定")]
    [SerializeField] private Image manualImage;

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
        if (openTheMenu.isMenuImageOpen == true)
        {
            Color color = manualImage.color;
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            color.a = 1f;
            manualImage.color = color;
            openTheMenu.isMenuImageOpen = false;
        }
        else
        {
            Color color = manualImage.color;
            color.r = 1f;
            color.g = 1f;
            color.b = 1f;
            color.a = 0f;
            manualImage.color = color;
            openTheMenu.isMenuImageOpen = true;
            buttonSelector.HideManualImage();
        }
    }
}
