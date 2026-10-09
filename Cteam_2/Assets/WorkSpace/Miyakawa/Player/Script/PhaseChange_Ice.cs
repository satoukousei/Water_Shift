//キーを離した時に状態を水にする場合は、
//コメントアウトしているコードを元に戻してください。

using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Ice : MonoBehaviour
{
    [Tooltip("Inspector に直接 InputAction を割り当て（InputActionReference を推奨）")]
    [SerializeField] private InputActionReference iceActionRef;

    [Tooltip("状態を変更する EnumList_Player を Inspector で割当て")]
    [SerializeField] private EnumList_Player changeStatePlayer;

    [Tooltip("モデルを変更するModelChangeを割り当て")]
    [SerializeField] private ModelChanger modelChanger;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (iceActionRef == null)
        {
            Debug.LogWarning("iceActionRef が未設定です。", this);
            return;
        }

        var action = iceActionRef.action;
        action.performed += OnChangePerformed;
     //   action.canceled += OnChangeCanceled;
        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (iceActionRef == null) return;

        var action = iceActionRef.action;
        action.performed -= OnChangePerformed;
 //       action.canceled -= OnChangeCanceled;
        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }

    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_ice();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Ice);
        Debug.Log("現在の状態:固体");
    }

    private void OnChangeCanceled(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_water();
        changeStatePlayer.SetState(EnumList_Player.PlayerState.Water);
        Debug.Log("現在の状態:液体");
    }
}
