//キーを離した時に状態を水にする場合は、
//コメントアウトしているコードを元に戻してください。

using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Steam : MonoBehaviour
{
    [Tooltip("InputActionを割り当て")]
    public InputActionReference steamActionRef; 

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player changeStatePlayer;

    [Tooltip("ModelChangerを割り当て")]
    [SerializeField] private ModelChanger modelChanger;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        var action = steamActionRef.action;
        action.performed += OnChangePerformed;
        action.Enable();

    }

    private void OnDisable()
    {
        if (steamActionRef == null) return;

        var action = steamActionRef.action;
        action.performed -= OnChangePerformed;
        action.Disable();

    }

    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_steam();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Steam);
    }

    private void OnChangeCanceled(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_water();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Water);
    }
}
