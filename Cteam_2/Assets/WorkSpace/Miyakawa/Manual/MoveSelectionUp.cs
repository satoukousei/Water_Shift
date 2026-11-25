using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSelectionUp : MonoBehaviour

{
    [Header("OpenTheMenu‚ðŽw’è")]
    [SerializeField] private OpenTheMenu openTheMenu;

    [Header("ButtonSelecter‚ðŽw’è")]
    [SerializeField] private ButtonSelector buttonSelector;

    [Header("InputAction‚ðŠ„‚è“–‚Ä")]
    [SerializeField] private InputActionReference menuActionRef;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (menuActionRef == null)
        {
            return;
        }

        var action = menuActionRef.action;
        action.started += OnMoveStarted;
        action.Enable();
    }

    private void OnDisable()
    {
        if (menuActionRef == null)
        {
            return;
        }
        var action = menuActionRef.action;
        action.started -= OnMoveStarted;
        action.Disable();

    }

    private void OnMoveStarted (InputAction.CallbackContext context)
    {
        if (openTheMenu.isMenuImageOpen == true)
        {
            buttonSelector.CurrentUp();
        }
    }
}
