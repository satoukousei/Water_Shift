//キーを離した時に状態を水にする場合は、
//コメントアウトしているコードを元に戻してください。

using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Water_Copy : MonoBehaviour
{
    [Tooltip("親のRigidBodyを割り当て")]
    [SerializeField] private Rigidbody rb;

    [Tooltip("InputActionを割り当て")]
    public InputActionReference waterActionRef;

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player changeStatePlayer;

    [Tooltip("ModelChangerを割り当て")]
    [SerializeField] private ModelChanger_Copy modelChanger_Copy;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        var action = waterActionRef.action;
        action.performed += OnChangePerformed;
        action.Enable();

    }

    private void OnDisable()
    {
        if (waterActionRef == null)
        {
            return;
        }
        var action = waterActionRef.action;
        action.performed -= OnChangePerformed;
        action.Disable();
    }
    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        modelChanger_Copy.ChangeModel_water();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Water);
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
