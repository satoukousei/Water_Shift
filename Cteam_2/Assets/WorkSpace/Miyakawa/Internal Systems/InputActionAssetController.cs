
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionAssetController : MonoBehaviour
{
    [Header("~‚ß‚½‚¢InputActionAsset‚ğ‘S‚Äİ’è")]
    [SerializeField]
    private List<InputActionAsset> inputAsset;

    public void DisableActionAsset()
    {
        foreach (InputActionAsset asset in inputAsset)
        {
            asset.Disable();
        }
    }

    public void EnableActionAsset()
    {
        foreach (InputActionAsset asset in inputAsset)
        {
            asset.Enable();
        }
    }
}
