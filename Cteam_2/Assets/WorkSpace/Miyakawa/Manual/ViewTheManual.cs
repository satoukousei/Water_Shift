using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


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

    void Start()
    {
        events = GetComponent<Events>();
    }

   public void FirstViewManual()
    {
        if (isFirstStage)
        {
            isOpen = true;
            manualImage.color = Color.white;
            isFirstStage = false;
        }
    }

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
        isOpen = !isOpen;

        manualImage.color = isOpen
        ? Color.white
            :new Color(1f, 1f, 1f, 0f);
    }
}

