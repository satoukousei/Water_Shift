using UnityEngine;
using UnityEngine.InputSystem;

public class Move_Left : MonoBehaviour
{
    [Tooltip("InputActionを割り当て")]
    public InputActionReference playerActionRef;

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player playerState;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (playerActionRef == null)
        {
            Debug.LogWarning("playerActionRefが未設定です。");
        }

        var action = playerActionRef.action;
        action.performed += MovePerformed;
        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (playerActionRef == null) return;

        var action = playerActionRef.action;
        action.performed -= MovePerformed;
        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }

    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        var state = playerState.CurrentPlayerState;
        if (state == EnumList_Player.PlayerState.Water || state == EnumList_Player.PlayerState.Ice)
        {
            transform.parent.position += new Vector3(-1f * Time.deltaTime, 0, 0);
        }
    }


}
