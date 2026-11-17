
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionReferenceController : MonoBehaviour
{

    [Header("í‚é~Ç∑ÇÈInputAction")]
    [SerializeField]

    private List<InputActionReference> inputActions;

    public void DisableActionReference()
    {
        foreach (InputActionReference action in inputActions)
        {
            action.action.Disable();
        }
    }

    public void EnableActionReference()
    {
        foreach (InputActionReference action in inputActions)
        {
            action.action.Enable();
        }
    }

}
