//キーを離した時に状態を水にする場合は、
//コメントアウトしているコードを元に戻してください。

using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseChange_Water : MonoBehaviour
{
    [Tooltip("InputActionを割り当て")]
    public InputActionReference waterActionRef;

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player changeStatePlayer;

    [Tooltip("ModelChangerを割り当て")]
    [SerializeField] private ModelChanger modelChanger;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (waterActionRef == null)
        {
            Debug.LogWarning("waterActionRefが未設定です。");
        }

        var action = waterActionRef.action;
        action.performed += OnChangePerformed;
      //action.canceled += OnChangeCanceled;
        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (waterActionRef == null) return;

        var action = waterActionRef.action;
        action.performed -= OnChangePerformed;
      //action.canceled -= OnChangeCanceled;
        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }
    private void OnChangePerformed(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_water();
        Debug.Log("現在の状態:液体");
    }

    private void OnChangeCanceled(InputAction.CallbackContext ctx)
    {
        modelChanger.ChangeModel_water();   
        Debug.Log("現在の状態:液体");
    }
}
