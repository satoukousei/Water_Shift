using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSelectionUp : MonoBehaviour

{

    [Header("ButtonSelecter‚ğw’è")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("InputAction‚ğŠ„‚è“–‚Ä")]
    public InputActionReference menuActionRef;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (menuActionRef == null)
        {
            Debug.LogWarning("playerActionRef‚ª–¢İ’è‚Å‚·B");
            return;
        }

        var action = menuActionRef.action;
        action.started += OnMoveStarted;
        action.Enable();

        Debug.Log($"OnEnable “o˜^: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (menuActionRef == null) return;

        var action = menuActionRef.action;
        action.started -= OnMoveStarted;
        action.Disable();

        Debug.Log($"OnDisable ‰ğœ: {name} action={action.name}", this);
    }

    private void OnMoveStarted (InputAction.CallbackContext context)
    {
        buttonSelector.CurrentUp();
    }
}
