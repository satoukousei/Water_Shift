using UnityEngine;
using UnityEngine.InputSystem;

public class ControllStop : MonoBehaviour
{
    [SerializeField] private InputActionReference targetAction;
    [SerializeField] public bool inputEnabled = true;

    private void OnEnable()
    {
        ApplyInputState();
    }

    private void OnDisable()
    {
        if (targetAction != null)
            targetAction.action.Disable();
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
        ApplyInputState();
    }

    private void ApplyInputState()
    {
        if (targetAction == null)
            return;

        if (inputEnabled)
            targetAction.action.Enable();
        else
            targetAction.action.Disable();
    }
}