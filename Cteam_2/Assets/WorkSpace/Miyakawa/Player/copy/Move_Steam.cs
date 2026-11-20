using UnityEngine;

public class Move_Steam : MonoBehaviour
{
    [Header("上昇スピード")]
    [SerializeField] private float upperMoveSpeed = 5f;

    [Header("EnumListを割り当て")]
    [SerializeField] private EnumList_Player playerState;

    [Header("Playerを割り当て")]
    [SerializeField] private GameObject playerModel;

    private void FixedUpdate()
    {
        UpperMove();
    }

    private void UpperMove()
    {
        if (playerState == null || playerModel == null)
        {
            return;
        }

        if (playerState.CurrentPlayerState == EnumList_Player.PlayerState.Steam)
        {
            playerModel.transform.position +=
                new Vector3(0f, upperMoveSpeed * Time.fixedDeltaTime, 0f);
        }
    }
}
