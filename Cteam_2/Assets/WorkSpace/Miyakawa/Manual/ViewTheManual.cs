using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ViewTheManual : MonoBehaviour
{
    [Tooltip("ステージの初めに説明をつけるかどうか")]
    [SerializeField] private bool isFirstStage = true;
    [Tooltip("説明画像")]
    [SerializeField] private Image manualImage;
    [Tooltip("Inspector に直接 InputAction を割り当て（InputActionReference を推奨）")]
    [SerializeField] private InputActionReference manualActionRef;

    bool isOpen = false;
    Events events;

   public void FirstViewManual()
    {
        if (isFirstStage)
        {
            isOpen = true;

            if (manualImage != null)
            {
                manualImage.gameObject.SetActive(true);
            }

            isFirstStage = false;
        }
    }

    private void Awake()
    {
        if (manualImage != null)
        {
            manualImage.gameObject.SetActive(false);
        }

        isOpen = false;
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (manualActionRef == null)
        {
            return;
        }

        var action = manualActionRef.action;

            action.performed += OnChangePerformed;

        action.Enable();
    }

    private void OnDisable()
    {
        if (manualActionRef == null)
        {
            return;
        }
        var action = manualActionRef.action;

            action.performed -= OnChangePerformed;

        action.Disable();
    }

    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        isOpen = !isOpen;

        if (manualImage != null)
        {
            manualImage.gameObject.SetActive(isOpen);
        }
    }
}

