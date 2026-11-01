using UnityEngine;
using static EnumList_Player;

/// <summary>
/// 氷状態(Ice)のプレイヤーのみ押すことができるスイッチ。
///
/// ・スイッチオブジェクトにアタッチ
/// ・Colliderの Is Trigger を ON
/// ・プレイヤーに EnumList_Player を付ける
///
/// ・Ice状態で乗るとスイッチON
/// ・離れるとスイッチOFF
/// ・IsPressed から現在状態を取得可能
/// </summary>
public class IceSwitchGimmick : MonoBehaviour
{
    [Header("現在の押下状態(読み取り専用)")]
    public bool IsPressed = false;

    /// <summary>
    /// プレイヤーがスイッチに乗った
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        EnumList_Player player = other.GetComponentInParent<EnumList_Player>();

        if (player == null)
            return;

        if (player.CurrentPlayerState == PlayerState.Ice)
        {
            IsPressed = true;

            Debug.Log("IceSwitch ON");
        }
    }

    /// <summary>
    /// プレイヤーがスイッチから降りた
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        EnumList_Player player = other.GetComponentInParent<EnumList_Player>();

        if (player == null)
            return;

        if (player.CurrentPlayerState == PlayerState.Ice)
        {
            IsPressed = false;

            Debug.Log("IceSwitch OFF");
        }
    }
}
