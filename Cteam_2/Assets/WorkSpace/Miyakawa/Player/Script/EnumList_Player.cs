using System;
using UnityEngine;

public class EnumList_Player : MonoBehaviour
{
    [Header("プレイヤーの初期状態")]
    [SerializeField]
    private PlayerState playerState = PlayerState.Water; // プレイヤーの状態を文字列で保持する変数

    public enum PlayerState { Water, Ice, Steam }

    public PlayerState CurrentPlayerState => playerState;

    public event Action<PlayerState> OnStateChanged;

    public void SetState(PlayerState newState)
    {
        if (playerState == newState) return;
        playerState = newState;
        // 状態変化に伴う処理をここに追加（アニメーション、エフェクト等）
        OnStateChanged?.Invoke(playerState);
    }

    private void Awake()
    {
        Debug.Log("初期状態:" + playerState);
    }
}
