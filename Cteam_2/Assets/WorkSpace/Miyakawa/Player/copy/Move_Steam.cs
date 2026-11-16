using UnityEngine;
using UnityEngine.InputSystem;

public class Move_Steam : MonoBehaviour
{
    [Header("InputActionを割り当て")]
    public InputActionReference playerActionRef;

    [Header("上昇スピード")]
    [SerializeField] private float upperMoveSpeed = 5f;

    [Header("EnumListを割り当て")]
    [SerializeField] private EnumList_Player playerState;

    [Header("Playerを割り当て")]
    [SerializeField] private GameObject playerModel;

    void FixedUpdate()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        // 物理演算を通して上に移動
        playerModel.transform.position += new Vector3(0f, upperMoveSpeed * Time.deltaTime, 0f);
    }
}
