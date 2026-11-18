using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static ButtonSelector;

public class OpenTheMenu : MonoBehaviour
{
    [Header("ButtonSelectorを指定")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("InputActionReferenceのShowMenuを指定")]
    [SerializeField] public InputActionReference manualActionRef;

    [Header("メニュー用のBGImageを指定")]
    [SerializeField] private GameObject menuBackground;

    [Header("InputActionAssetControllerを指定")]
    [SerializeField] private InputActionAssetController inputActionAssetController;

    [Header("InputActionReferenceControllerを指定")]
    [SerializeField] private InputActionReferenceController inputActionReferenceController;

    public bool isMenuImageOpen = false;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (manualActionRef == null)
        {
            Debug.LogWarning("iceActionRef が未設定です。", this);
            return;
        }

        var action = manualActionRef.action;

        action.performed += OnChangePerformed;

        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (manualActionRef == null) return;
        var action = manualActionRef.action;

        action.performed -= OnChangePerformed;

        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }

    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
     if(isMenuImageOpen == false)
        {
            isMenuImageOpen = true;
            ShowMenu();
        }
        else
        {
            isMenuImageOpen = false;
            CloseMenu();
        }
    }

    private void ShowMenu()
    {
        if (menuBackground != null)
        {
            menuBackground.SetActive(true);
        }

        if (buttonSelector != null)
        {
            buttonSelector.HideManualImage();
            buttonSelector.ResetSelection();
        }

        inputActionAssetController.DisableActionAsset();
        inputActionReferenceController.DisableActionReference();
    }
    
    private void CloseMenu()
    {
        if (buttonSelector != null)
        {
            buttonSelector.ResetSelection();
            buttonSelector.HideManualImage();
        }

        if (menuBackground != null)
        {
            menuBackground.SetActive(false);
        }

        inputActionAssetController.EnableActionAsset();
        inputActionReferenceController.EnableActionReference();
    }
}
