using UnityEngine;
using UnityEngine.InputSystem;

public class Move_Jump : MonoBehaviour
{
    private LayerMask groundLayer;
    private bool isGrounded;
    private bool isMoving;

    [SerializeField] private Rigidbody rb;

    [Tooltip("InputActionを割り当て")]
    public InputActionReference playerActionRef;

    [Tooltip("EnumListを割り当て")]
    [SerializeField] private EnumList_Player playerState;

    [Tooltip("Playerを割り当て")]
    [SerializeField] private GameObject playerModel;

    [Tooltip("ジャンプ力")]
    [SerializeField] private float jumpPower = 5f;

    [Tooltip("Rayの長さ")]
    [SerializeField] private float rayLength = 0.1f;

    private void Start()
    {
        groundLayer = LayerMask.GetMask("Ground"); // "Ground"レイヤーを取得
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        var action = playerActionRef.action;
        action.started += OnMoveStarted;
        action.canceled += OnMoveCanceled;
        action.Enable();
    }

    private void OnDisable()
    {
        if (playerActionRef == null)
        {
            return;
        }
        var action = playerActionRef.action;
        action.started -= OnMoveStarted;
        action.canceled -= OnMoveCanceled;
        action.Disable();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(playerModel.transform.position,
                     Vector3.down, rayLength, groundLayer);

        if (!isMoving)
        {
            return;
        }
        OnJump();
    }   

    private void OnMoveStarted(InputAction.CallbackContext ctx)
    {
        isMoving = true;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        isMoving = false;
    }

    private void OnJump()
    {
        var state = playerState.CurrentPlayerState;
        if (state != EnumList_Player.PlayerState.Water)
        {
            return;
        }
        if (!isGrounded)
        {
            return;
        }
        Vector3 jump_vector = Vector3.up;
        Vector3 jump_velocity = jump_vector * jumpPower;
        rb.linearVelocity = jump_velocity;
    }
}
