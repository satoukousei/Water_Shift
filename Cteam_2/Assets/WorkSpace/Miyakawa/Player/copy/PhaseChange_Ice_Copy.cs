//キーを離した時に状態を水にする場合は、
//コメントアウトしているコードを元に戻してください。

using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Ice_Copy : MonoBehaviour
{
    [Tooltip("親のRigidBodyを割り当て")]
    [SerializeField] private Rigidbody rb;

    [Tooltip("Inspector に直接 InputAction を割り当て（InputActionReference を推奨）")]
    [SerializeField] private InputActionReference iceActionRef;

    [Tooltip("状態を変更する EnumList_Player を Inspector で割当て")]
    [SerializeField] private EnumList_Player changeStatePlayer;

    [Tooltip("モデルを変更するModelChangeを割り当て")]
    [SerializeField] private ModelChanger_Copy modelChanger_Copy;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (iceActionRef == null)
        {
            return;
        }

        var action = iceActionRef.action;
        action.performed += OnChangePerformed;
        action.Enable();
    }

    private void OnDisable()
    {
        if (iceActionRef == null) return;

        var action = iceActionRef.action;
        action.performed -= OnChangePerformed;
        action.Disable();

    }

    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        modelChanger_Copy.ChangeModel_ice();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Ice);
        RbSwitch();
    }

    private void OnChangeCanceled(InputAction.CallbackContext ctx)
    {
        modelChanger_Copy.ChangeModel_water();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Water);
        RbSwitch();
    }

    public void RbSwitch()
    {
        rb.useGravity = true;
    }
}
