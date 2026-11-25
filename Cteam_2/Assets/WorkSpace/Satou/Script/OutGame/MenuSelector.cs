using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    [Header("シーン名")]
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string stageSelectSceneName = "StageSelectScene";

    [Header("メニュー項目")]
    [SerializeField] private RectTransform[] menuItems;

    [Header("各項目を決定したときの遷移先シーン名")]
    [Tooltip("ゲームを終了したい項目には endGameKeyword と同じ文字列を入れる")]
    [SerializeField] private string[] destinationScenes;

    [Header("ゲーム終了")]
    [SerializeField] private string endGameKeyword = "EndGame";

    [Header("戻る")]
    [Tooltip("ステージセレクトで戻るボタンを押したときに戻るシーン名")]
    [SerializeField] private string backSceneName = "TitleScene";

    [Header("選択中の枠")]
    [SerializeField] private RectTransform selectionFrame;
    [SerializeField] private Vector2 framePadding = new Vector2(20f, 10f);

    [Header("コントローラー")]
    [Tooltip("左スティックがこの値を超えたら入力とみなす")]
    [SerializeField] private float stickThreshold = 0.6f;

    private enum MoveMode { Vertical, Horizontal }
    private MoveMode moveMode;
    private int currentIndex = 0;

    private int enumIndex = 1;

    // スティックの入力を1回の倒しにつき1回だけ反応させるためのフラグ
    private bool stickNeutral = true;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == stageSelectSceneName)
        {
            moveMode = MoveMode.Horizontal;   // ステージセレクト:A/D・左右
        }
        else
        {
            moveMode = MoveMode.Vertical;     // タイトル:W/S・上下
        }

        UpdateFrame();
    }

    private void Update()
    {
        // ---- 移動 ----
        int moveInput = GetKeyboardMove();
        int padMove = GetGamepadMove();
        if (padMove != 0)
        {
            moveInput = padMove;
        }
        if (moveInput != 0)
        {
            MenuMove(moveInput);
        }

        // ---- 決定 ----
        if (IsKeyboardDecide() || IsGamepadDecide())
        {
            Decide();
            return; // シーン遷移後に戻る処理が走らないようにする
        }

        // ---- 戻る(ステージセレクトのみ) ----
        if (SceneManager.GetActiveScene().name == stageSelectSceneName)
        {
            if (IsKeyboardBack() || IsGamepadBack())
            {
                Back();
            }
        }
    }

    //  キーボード入力


    /// <summary>キーボードの移動入力。戻り値: -1=前 / 0=なし / 1=次</summary>
    private int GetKeyboardMove()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return 0;
        }

        int move = 0;

        if (moveMode == MoveMode.Vertical)
        {
            if (keyboard.wKey.wasPressedThisFrame)
            {
                move = -enumIndex; // 上
            }
            if (keyboard.sKey.wasPressedThisFrame)
            {
                move = enumIndex;  // 下
            }
        }
        else
        {
            if (keyboard.aKey.wasPressedThisFrame)
            {
                move = -enumIndex; // 左
            }
                if (keyboard.dKey.wasPressedThisFrame)
                {
                    move = enumIndex;  // 右
                }
        }

        return move;
    }

    /// <summary>キーボードの決定入力(Enter / Space)</summary>
    private bool IsKeyboardDecide()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) 
        { 
            return false;
        }
            return keyboard.enterKey.wasPressedThisFrame
            || keyboard.spaceKey.wasPressedThisFrame;
    }

    /// <summary>キーボードの戻る入力(Esc)</summary>
    private bool IsKeyboardBack()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return false;
        }
        return keyboard.escapeKey.wasPressedThisFrame;
    }

    //  コントローラー入力


    /// <summary>コントローラーの移動入力(十字キー + 左スティック)。戻り値: -1=前 / 0=なし / 1=次</summary>
    private int GetGamepadMove()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return 0;
        }
        int move = 0;

        // 十字キー
        if (moveMode == MoveMode.Vertical)
        {
            if (gamepad.dpad.up.wasPressedThisFrame)
            {
                move = -enumIndex;
            }
            if (gamepad.dpad.down.wasPressedThisFrame)
            {
                move = enumIndex;
            }
        }
        else
        {
            if (gamepad.dpad.left.wasPressedThisFrame)
            {
                move = -enumIndex;
            }
            if (gamepad.dpad.right.wasPressedThisFrame)
            {
                move = enumIndex;
            }
        }

        // 左スティック
        int stickMove = GetStickMove(gamepad);
        if (stickMove != 0)
        {
            move = stickMove;
        }
        return move;
    }

    /// <summary>左スティックの移動入力(1回の倒しにつき1回だけ反応)</summary>
    private int GetStickMove(Gamepad gamepad)
    {
        Vector2 stick = gamepad.leftStick.ReadValue();
        float axis = (moveMode == MoveMode.Vertical) ? stick.y : stick.x;

        int move = 0;

        if (stickNeutral)
        {
            if (axis > stickThreshold)
            {
                // 縦:上に倒す=前の項目 / 横:右に倒す=次の項目
                move = (moveMode == MoveMode.Vertical) ? -enumIndex : enumIndex;
                stickNeutral = false;
            }
            else if (axis < -stickThreshold)
            {
                move = (moveMode == MoveMode.Vertical) ? enumIndex : -enumIndex;
                stickNeutral = false;
            }
        }
        else if (Mathf.Abs(axis) < stickThreshold * 0.5f)
        {
            // スティックが中央付近に戻ったら再入力を受け付ける
            stickNeutral = true;
        }

        return move;
    }

    /// <summary>コントローラーの決定入力(Xボタン)</summary>
    private bool IsGamepadDecide()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return false;
        }
        return gamepad.buttonWest.wasPressedThisFrame;
    }

    /// <summary>コントローラーの戻る入力(Aボタン)</summary>
    private bool IsGamepadBack()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            return false;
        }
        return gamepad.buttonSouth.wasPressedThisFrame;
    }
    //  メニュー動作

    private void MenuMove(int direction)
    {
        currentIndex += direction;

        // 端まで行ったら反対側へループ
        if (currentIndex < 0)
        {
            currentIndex = menuItems.Length - 1;
        }
        if (currentIndex >= menuItems.Length)
        {
            currentIndex = 0;
        }

        UpdateFrame();
    }

    private void UpdateFrame()
    {
        RectTransform target = menuItems[currentIndex];
        selectionFrame.position = target.position;
        selectionFrame.sizeDelta = target.sizeDelta + framePadding;
    }

    private void Decide()
    {
        if (currentIndex >= destinationScenes.Length)
        {
            return;
        }
        string destination = destinationScenes[currentIndex];

        // "EndGame" が指定されていたらゲームを終了
        if (destination == endGameKeyword)
        {
            EndGame();
            return;
        }

        SceneManager.LoadScene(destination);
    }

    private void Back()
    {
        SceneManager.LoadScene(backSceneName);
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        // エディタ上では再生モードを停止
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後はアプリケーションを終了
        Application.Quit();
#endif
    }
}