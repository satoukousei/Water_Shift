using UnityEngine;
using UnityEngine.InputSystem;

public class Move_Left : MonoBehaviour
{
    [Tooltip("InputActionを割り当て")]
    public InputActionReference playerActionRef;

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player playerState;

    [Tooltip("移動速度")]
    [SerializeField] private float moveSpeed = 1f;

    [Tooltip("Playerを割り当て")]
    [SerializeField] private GameObject playerModel;

    private bool isMoving;

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        if (playerActionRef == null)
        {
            Debug.LogWarning("playerActionRefが未設定です。");
            return;
        }

        var action = playerActionRef.action;
        action.started += OnMoveStarted;
        action.canceled += OnMoveCanceled;
        action.Enable();

        Debug.Log($"OnEnable 登録: {name} action={action.name}", this);
    }

    private void OnDisable()
    {
        if (playerActionRef == null) return;

        var action = playerActionRef.action;
        action.started -= OnMoveStarted;
        action.canceled -= OnMoveCanceled;
        action.Disable();

        Debug.Log($"OnDisable 解除: {name} action={action.name}", this);
    }

    private void Update()
    {
        if (!isMoving) return;
        if (playerModel == null) return;

        var state = playerState.CurrentPlayerState;
        if (state == EnumList_Player.PlayerState.Water || state == EnumList_Player.PlayerState.Ice)
        {
            playerModel.transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    private void OnMoveStarted(InputAction.CallbackContext ctx)
    {
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        isMoving = false;
    }
}
